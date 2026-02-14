
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameEvents;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GameSceneManager : SceneLoadBase
{
    [SerializeField] public MyGameInstance MyGameInstance;
    [SerializeField] public CheckpointManager CheckpointManager;
    [SerializeField] public SceneLoadingData SceneLoadingData;
    [SerializeField] private string DeathScenePath;
    [SerializeField] private string WinScenePath;
    [SerializeField] private bool subscribeToPlayer = true;
    public static GameSceneManager Instance { get; private set; }

    public virtual void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning($"Duplicate singleton found! {gameObject.name}");
            Destroy(gameObject);
        }
        Instance = this;
    }
    public virtual void Start()
    {

        if (CheckpointManager != null)
        {
            CheckpointManager.GetCheckpointsFromScene();
        }
    }

    public void LoadDeathScene(object sender)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(DeathScenePath);
    }

    public void LoadWinScene(GameObject spawner)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(WinScenePath);
    }

    public virtual void DebugLoadScene(int checkpointIndex)
    {
        StartCoroutine(DebugLoadSceneSequence(checkpointIndex));
    }


    /// <summary>
    /// Load sub-level scene on checkpoint, auto activate
    /// </summary>
    /// <param name="checkpointIndex"></param>
    public virtual IEnumerator DebugLoadSceneSequence(int checkpointIndex)
    {
        CheckpointManager.SetCurrentIndex(checkpointIndex);
        List<string> sceneList = GetSceneListOnCheckpoint(checkpointIndex);// scene list to actually load based on checkpoint
        if (sceneList.Count == 0) yield break;
        yield return UnloadAllSubLevels(); // unload all the sub-level scenes
        yield return LoadScenes(sceneList.ToHashSet(),LoadSceneMode.Additive); //loading scenes, no duplicates
        yield return null;
        CheckpointManager.GetCheckpointsFromScene();
        SetPlayerToCheckpoint();
    }
    public void SetPlayerToCheckpoint()
    {
        if (MyGameInstance == null || MyGameInstance.PlayerGameObject == null) return;
        Rigidbody rb = MyGameInstance.PlayerGameObject.GetComponentInChildren<Rigidbody>();
        if(rb == null) return;
        Transform checkpointTrans = CheckpointManager.GetCurrentCheckpointTransform();
        MyGameInstance.PlayerTransform.SetPositionAndRotation(checkpointTrans.position, checkpointTrans.rotation);
        rb.position = MyGameInstance.PlayerTransform.position;
        rb.rotation = MyGameInstance.PlayerTransform.rotation;
    }



    /// <summary>
    /// Unload all sub levels that match the Checkpoint Scene Sets's scene name
    /// </summary>
    /// <returns></returns>
    public IEnumerator UnloadAllSubLevels()
    {
        yield return UnloadScenes(GetAllSubLevels());

    }


    /// <summary>
    /// Get the scene list based on the checkpoint index
    /// </summary>
    /// <param name="checkpointIndex"></param>
    /// <returns></returns>
    public virtual List<string> GetSceneListOnCheckpoint(int checkpointIndex)
    {
        CheckpointSceneSet sceneSet =
           SceneLoadingData.CheckpointSceneSets.FirstOrDefault(s => s.CheckpointIndex == checkpointIndex);
        var desired = sceneSet?.Scenes?
            .Where(sf => sf!= null && IsSceneNameVaild(sf.SceneName))
            .Select(sf => sf.SceneName)
            .ToHashSet()
            ?? new HashSet<string>();

        return desired.ToList();

    }
    /// <summary>
    /// Get all sub level names from the checkpoint scene sets, no duplicates
    /// </summary>
    /// <returns></returns>
    public HashSet<string> GetAllSubLevels()
    {
        var desired = SceneLoadingData.CheckpointSceneSets
            .Where(set => set != null)
            .SelectMany(set => set.Scenes)
            .Where(scene => scene != null)
            .Select(scene => scene.SceneName)
            .ToHashSet();
        return desired;
    }



}
