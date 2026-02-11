using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Scene = UnityEngine.SceneManagement.Scene;


[DefaultExecutionOrder(-10000)]
public class MainMenuSceneLoad : MonoBehaviour
{
    [Header("Main Menu Loading Bar")]
    [SerializeField] private GameObject _loadingBar;
    [SerializeField] private Slider _loadingBarSlider;

    [Header("Scenes To Load")]
    [SerializeField] private SceneLoadingData _sceneLoadingData;
    [SerializeField] private CheckpointManager _checkpointManager;
    

    [SerializeField] private CinematicsManager _cinematicsManager;
    
    [SerializeField] private GameObject _eventObject;
    private List<AsyncOperation> _sceneToLoad = new List<AsyncOperation>();

    private void Awake()
    {
        if(_loadingBar != null) _loadingBar.SetActive(false);
        DontDestroyOnLoad(gameObject);
    }
    public void StartGame()
    {
        Application.backgroundLoadingPriority = ThreadPriority.High;
        if (_loadingBar != null) _loadingBar.SetActive(true);
        
        /*foreach (var scene in _sceneLoadingData.PersistentScenes)
        {
            //SceneManager.LoadScene(scene);
            _sceneToLoad.Add(SceneManager.LoadSceneAsync(scene));
        }

        List<string> levelScenesToLoad = GetLevelScenes();

        foreach (var scene in levelScenesToLoad)
        {
             //SceneManager.LoadScene(scene, LoadSceneMode.Additive);
             _sceneToLoad.Add(SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive));
        }*/

        if (_loadingBar != null) StartCoroutine(SceneLoadingSequence());
        if (_loadingBar != null) StartCoroutine(ProgressLoadingBar());
    }

    private List<string> GetLevelScenes()
    {
        // Find current index to which scene to load
        var set = _sceneLoadingData.LevelSceneSets.FirstOrDefault(s => s.CheckpointIndex == _checkpointManager.CurrentIndex);
        var desired = (set?.Scenes ?? new List<SceneField>())
            .Where(sf => sf != null && !string.IsNullOrEmpty(sf.SceneName))
            .Select(sf => sf.SceneName)
            .ToHashSet();
        if (_cinematicsManager == null) return desired.ToList();
        
        if (_checkpointManager.CurrentIndex == 0 && _cinematicsManager.PlayerWaitForCinematics && _sceneLoadingData.StartCutScene != null)
        {
            desired.Add(_sceneLoadingData.StartCutScene);
        }
        
        // Scene to load
        return desired.ToList();
    }


    private IEnumerator ProgressLoadingBar()
    {
        float totalLoadingProgress = _sceneLoadingData.PersistentScenes.Count + GetLevelScenes().Count + 1;
        
        while (_sceneToLoad.Any(x => !x.isDone))
        {
            float loadingProgress = 0;
            for (int i = 0; i < _sceneToLoad.Count; i++)
            {
                loadingProgress += _sceneToLoad[i].progress;
            }
            _loadingBarSlider.value = loadingProgress / totalLoadingProgress;
            yield return null;
        }
    }


    private IEnumerator SceneLoadingSequence()
    {
        Destroy(_eventObject);
        Scene currentScene = SceneManager.GetActiveScene();
        
        foreach (var scene in _sceneLoadingData.PersistentScenes)
        {
            //SceneManager.LoadScene(scene);
            var op = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            if (op != null)op.allowSceneActivation = false;
            _sceneToLoad.Add(op);
            while (_sceneToLoad.Any(s => s.progress < 0.9f))
            {
                yield return null;
            }
        }
        
        List<string> levelScenesToLoad = GetLevelScenes();

        foreach (var scene in levelScenesToLoad)
        {
            //SceneManager.LoadScene(scene, LoadSceneMode.Additive);
            var op = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            if(op != null) op.allowSceneActivation = false;
            _sceneToLoad.Add(op);
            while (_sceneToLoad.Any(s => s.progress < 0.9f))
            {
                yield return null;
            }
        }
        
        foreach (var o in _sceneToLoad)
        {
            o.allowSceneActivation = true;
        }
        while (_sceneToLoad.Any(s => !s.isDone))
        {
            yield return null;
        }
        _checkpointManager.GetCheckpointsFromScene();
        
        //load player scene last
        var playerOp = SceneManager.LoadSceneAsync(_sceneLoadingData.PlayerScene, LoadSceneMode.Additive);
        if (playerOp != null) playerOp.allowSceneActivation = false;
        _sceneToLoad.Add(playerOp);
        
        while (_sceneToLoad.Any(s => s.progress < 0.9f))
        {
            yield return null;
        }
        
        foreach (var o in _sceneToLoad)
        {
            o.allowSceneActivation = true;
        }
        SceneManager.UnloadSceneAsync(currentScene);
        Destroy(gameObject);
    }


}
