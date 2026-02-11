using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameEvents;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class LevelLoader : MonoBehaviour
{
    [FormerlySerializedAs("_referenceGetter")] [SerializeField] private GameInstances gameinstance;
    [SerializeField] private CheckpointManager checkpointManager;
   // [SerializeField] private SceneLoadingData _sceneLoadingData;
    [SerializeField] private string DeathScenePath;
    [SerializeField] private string WinScenePath;
    [SerializeField] private bool subscribeToPlayer = true;
    [SerializeField] private GameObjectEventAsset targetSpawnerEventAsset;
    [SerializeField] private GameObjectEventAsset playerSpawnEventAsset;
    

    
    private void Start()
    {
        playerSpawnEventAsset?.AddListener(OnPlayerStart);

        if (checkpointManager != null)
        {
            checkpointManager.GetCheckpointsFromScene();
           // LoadCheckpointPosition();
        }
    }

    private void OnDestroy()
    {
        playerSpawnEventAsset?.RemoveListener(OnPlayerStart);
       // if (gameinstance?.PlayerHealth != null){gameinstance.PlayerHealth.OnDeath -= LoadDeathScene;}
        
    }

    private void OnPlayerStart(GameObject arg0)
    {
        //arg0.GetComponent<Health>().OnDeath += LoadDeathScene;
        Vector3 location = checkpointManager.GetCurrentTransform(out Quaternion rotation);
        Rigidbody rb = arg0.GetComponent<Rigidbody>();
        arg0.transform.parent.rotation = rotation;
        rb.rotation = rotation;
        arg0.transform.position = location;
        rb.position = arg0.transform.position;
    }

    //External use set the current index and load
    
    public void LoadDeathScene(object sender)
    {
        SceneManager.LoadScene(DeathScenePath);
    }
    
    public void LoadWinScene(GameObject spawner)
    {
        SceneManager.LoadScene(WinScenePath);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    /*
    public void DebugLoadScene(int checkpointIndex)
    {
        CheckpointSceneSet sceneSet =
            _sceneLoadingData.LevelSceneSets.FirstOrDefault(s => s.CheckpointIndex == checkpointIndex);
        var desired = (sceneSet?.Scenes ?? new List<SceneField>())
            .Where(sf => sf != null && !string.IsNullOrEmpty(sf.SceneName))
            .Select(sf => sf.SceneName)
            .ToHashSet();

        List<string> sceneList = desired.ToList();
        if (sceneList.Count == 0) return;
        UnloadAllLevelScene();
        checkpointManager.SetCurrentIndex(checkpointIndex);
        foreach (var scene in sceneList)
        {
            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        }
        
        Vector3 location = checkpointManager.GetCurrentTransform(out Quaternion rotation);
        Rigidbody rb = gameinstance.PlayerController.GetComponent<Rigidbody>();
        gameinstance.PlayerTransform.parent.rotation = rotation;
        rb.rotation = rotation;
        gameinstance.PlayerTransform.position = location;
        rb.position = rb.transform.position;
        
    }
    

    public void LoadCheckpointPosition()
    {
        //Debug.Log("moveplayer");
        if (gameinstance == null || gameinstance.PlayerController == null) return;
        Rigidbody rb = gameinstance.PlayerController.GetComponent<Rigidbody>();
        gameinstance.PlayerTransform.position = checkpointManager.GetCurrentTransform(out Quaternion rotation);
        rb.position = gameinstance.PlayerTransform.position;
        gameinstance.PlayerTransform.rotation = rotation;
        rb.rotation = rotation;
    }

    private void UnloadAllLevelScene()
    {
        foreach (var scene in _sceneLoadingData.LevelScenes)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene activeScene = SceneManager.GetSceneAt(i);
                if (scene.SceneName.Equals(activeScene.name))
                {
                    SceneManager.UnloadSceneAsync(activeScene);
                }
            }
        }
    }*/
}
