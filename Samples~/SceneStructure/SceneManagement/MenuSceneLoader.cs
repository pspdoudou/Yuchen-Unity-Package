using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSceneLoader : GameSceneManager
{
    public static MenuSceneLoader Instance { get; private set; }
    [Header("Main Menu Loading Bar")]
    [SerializeField] private GameObject _loadingBar;
    [SerializeField] private Slider _loadingBarSlider;
    [SerializeField] private CinematicsManager _cinematicsManager;

    private List<AsyncOperation> _sceneToLoadOp = new List<AsyncOperation>();

    public override void Awake()
    {
        if (_loadingBar != null) _loadingBar.SetActive(false);
        DontDestroyOnLoad(gameObject);
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        
    }
    public override void Start()
    {
        base.Start();
    }

    public void NewGameStart()
    {
        if (CheckpointManager != null) CheckpointManager.SetCurrentIndex(0); //reset the check point index
        StartGameOnCheckPoint();    
    }


    public void StartGameOnCheckPoint()
    {
        Application.backgroundLoadingPriority = ThreadPriority.High;
        if (_loadingBar != null) _loadingBar.SetActive(true);
        StartCoroutine(SceneLoadingSequence());
        if (_loadingBar != null) StartCoroutine(ProgressLoadingBar());
    }
    public List<string> GetSubLevelOnCheckpoint()
    {
        if(CheckpointManager == null) return null;
        // Find current index to which sublevel scene to load
        var desired = GetSceneListOnCheckpoint(CheckpointManager.CurrentIndex); 

        //If there is cinematic scene on start,add it to the list
        if (_cinematicsManager == null) return desired.ToList();
        if (CheckpointManager.CurrentIndex == 0 && _cinematicsManager.PlayerWaitForCinematics && SceneLoadingData.StartCutScene != null)
        {
            desired.Add(SceneLoadingData.StartCutScene);
        }
        // Actual Scene to load
        return desired.ToList();
    }


    private IEnumerator ProgressLoadingBar()
    {
        float totalLoadingProgress = SceneLoadingData.PersistentScenes.Count 
            + GetSubLevelOnCheckpoint().Count 
            + SceneLoadingData.PlayerScene.Count;

        while (_sceneToLoadOp.Any(x => !x.isDone))
        {
            float loadingProgress = 0;
            for (int i = 0; i < _sceneToLoadOp.Count; i++)
            {
                loadingProgress += _sceneToLoadOp[i].progress;
            }
            _loadingBarSlider.value = loadingProgress / totalLoadingProgress;
            yield return null;
        }
    }


    private IEnumerator SceneLoadingSequence()
    {
        List<Scene> PreOploadedScene = GetAllLoadedScenes(); // already loaded scenes
        List<string> loadingScene = new List<string>(); // current loading scenes
       
        foreach (var scene in SceneLoadingData.PersistentScenes)
        {
            if (loadingScene.Contains(scene)) continue;// check duplicate
            loadingScene.Add(scene);// tracking the loading scenes
        }
        foreach (var scene in GetSubLevelOnCheckpoint())
        {
            if (loadingScene.Contains(scene)) continue;// check duplicate
            loadingScene.Add(scene);// tracking the loading scenes
        }
        //load player scene last
        foreach (var scene in SceneLoadingData.PlayerScene)
        {
            if (loadingScene.Contains(scene)) continue;// check duplicate
            loadingScene.Add(scene);// tracking the loading scenes
        }
        yield return UnloadAllSubLevels();
        foreach (var scene in loadingScene)// loading the scene in order
        {
            var op = LoadSceneInBackground(scene, LoadSceneMode.Additive);
            if (op == null) continue;
            _sceneToLoadOp.Add(op); //add to the asyncOp list 
            while (_sceneToLoadOp.Any(s => s.progress < 0.9f))// wait for the op is at activation step, then go to next scene
            {
                yield return null;
            }
        }
        foreach (var o in _sceneToLoadOp) //activate the scene
        {
            o.allowSceneActivation = true;
        }
        while (_sceneToLoadOp.Any(s => !s.isDone)) // wait for the activation
        {
            yield return null;
        }

        CheckpointManager.GetCheckpointsFromScene();
        SetPlayerToCheckpoint();
        foreach (var scene in PreOploadedScene)// unload all the preExist scenes before the operation
        {
            yield return UnloadScene(scene);
        }
        Destroy(gameObject);

    }

}
