using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(BoxCollider))]

public class SceneLoadTrigger : SceneLoadBase
{   

    [Header("Config")]
    [SerializeField] private string _triggerObjectTag = "Player";
    [SerializeField] private List<SceneField> _sceneToLoad;
    [SerializeField] private List<SceneField> _sceneToUnload;

    [Header("Events")]
    public UnityEvent OnStart;

    private HashSet<string> _sceneToLoadHash = new HashSet<string>();
    private HashSet<string> _sceneToUnloadHash = new HashSet<string>();

    private void Awake()
    {
        _sceneToLoadHash = _sceneToLoad.Where(s => s!=null).Select(s=>s.SceneName).ToHashSet();
        _sceneToUnloadHash = _sceneToUnload.Where(s => s != null).Select(s => s.SceneName).ToHashSet();
    }
    private void OnValidate()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(_triggerObjectTag)) return;
        LoadAndUnloadScene();
    }


    public void LoadAndUnloadScene()
    {
        Application.backgroundLoadingPriority = ThreadPriority.Normal;
        OnStart.Invoke();
        
        for (int i = 0; i < _sceneToLoad.Count; i++)
        {
            bool nextLoaded = CheckIfSceneLoaded(_sceneToLoad[i]);
            if (!nextLoaded) StartCoroutine(LoadScene(_sceneToLoad[i],LoadSceneMode.Additive));
        }

        StartCoroutine(UnloadScenes(_sceneToUnloadHash));
        Application.backgroundLoadingPriority = ThreadPriority.High;
    }





}

