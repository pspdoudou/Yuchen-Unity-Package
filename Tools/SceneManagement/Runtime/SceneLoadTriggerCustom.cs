using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(BoxCollider))]

public class SceneLoadTriggerCustom : MonoBehaviour
{   

    [Header("Config")]
    [SerializeField] private string _triggerObjectTag = "Player";
    [SerializeField] private SceneField[] _sceneToLoad;
    [SerializeField] private SceneField[] _sceneToUnload;

    [Header("Events")]
    public UnityEvent OnStart;
    public UnityEvent<string> OnLoad;
    public UnityEvent<string> OnUnload;

    private void OnValidate()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(_triggerObjectTag)) return;
        Application.backgroundLoadingPriority = ThreadPriority.Low;

        OnStart.Invoke();


        for (int i = 0; i < _sceneToLoad.Length; i++)
        {
            bool nextLoaded = CheckIfSceneLoaded(_sceneToLoad[i]);
            if (!string.IsNullOrEmpty(_sceneToLoad[i]) && !nextLoaded) StartCoroutine(LoadNext(_sceneToLoad[i]));
        }

        for (int i = 0; i < _sceneToUnload.Length; i++)
        {
            bool previousLoaded = CheckIfSceneLoaded(_sceneToUnload[i]);
            if (!string.IsNullOrEmpty(_sceneToUnload[i]) && previousLoaded) StartCoroutine(UnloadPrevious(_sceneToUnload[i]));
        }

        Application.backgroundLoadingPriority = ThreadPriority.High;
    }


    public void LoadAndUnloadScene()
    {
        Application.backgroundLoadingPriority = ThreadPriority.Normal;
        OnStart.Invoke();
        
        for (int i = 0; i < _sceneToLoad.Length; i++)
        {
            bool nextLoaded = CheckIfSceneLoaded(_sceneToLoad[i]);
            if (!string.IsNullOrEmpty(_sceneToLoad[i]) && !nextLoaded) StartCoroutine(LoadNext(_sceneToLoad[i]));
        }
        for (int i = 0; i < _sceneToUnload.Length; i++)
        {
            bool previousLoaded = CheckIfSceneLoaded(_sceneToUnload[i]);
            if (!string.IsNullOrEmpty(_sceneToUnload[i]) && previousLoaded) StartCoroutine(UnloadPrevious(_sceneToUnload[i]));
        }
        Application.backgroundLoadingPriority = ThreadPriority.High;
    }


    private IEnumerator LoadNext(string sceneName)
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        if (load == null)
        {
            yield break;
        }
        load.allowSceneActivation = false;
        while (load.progress < 0.9f)
        {
            yield return null;
        }
        load.allowSceneActivation = true;
        OnLoad.Invoke(sceneName);
    }

    private IEnumerator UnloadPrevious(string sceneName)
    {
        AsyncOperation unload = SceneManager.UnloadSceneAsync(sceneName);
        if (unload == null)
        {
            yield break;
        }
        while (!unload.isDone)
        {
            yield return null;
        }

        OnUnload.Invoke(sceneName);
    }

    private bool CheckIfSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name.Equals(sceneName)) return true;
        }

        return false;
    }
}

