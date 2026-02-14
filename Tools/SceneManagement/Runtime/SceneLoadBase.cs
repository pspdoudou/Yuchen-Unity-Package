using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoadBase : MonoBehaviour
{

    public UnityEvent<string> OnLoad;
    public UnityEvent<string> OnUnload;

    /// <summary>
    /// If the scene name is not valid, return false; else return true
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="mode"></param>
    /// <param name="op"></param>
    /// <returns></returns>
    public static bool IsSceneNameVaild(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogError("Scene name is null or empty.");
            return false;
        }

        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError($"Scene '{sceneName}' not found in build settings.");
            return false;
        }
        return true;
    }

    /// <summary>
    /// Check if any scens loaded with the scene name.
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public bool IsSceneLoaded(string sceneName)
    {
        return GetLoadedScenesByName(sceneName).Count > 0;
    }

    /// <summary>
    /// Check if any scens loaded with the scene name, return matched scenes, including duplicates if any.
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="matches"></param>
    /// <returns></returns>
    public bool IsSceneLoaded(string sceneName, out List<Scene> matches)
    {
        matches = GetLoadedScenesByName(sceneName);
        return matches.Count > 0;
    }

    /// <summary>
    /// Load scene and auto activate when its done, only check if the scenename is valid, will check duplicates.
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public virtual IEnumerator LoadScene(string sceneName, LoadSceneMode mode)
    {
        if (IsSceneLoaded(sceneName)) yield break;
        if (!IsSceneNameVaild(sceneName)) yield break;
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, mode);
        if (op == null) yield break;
         yield return op;
        OnLoad?.Invoke(sceneName);
    }
    /// <summary>
    /// Load scenes one by one with list of scene names, auto activate, using hashset to avoid duplicates.
    /// </summary>
    /// <param name="sceneNames"></param>
    /// <param name="mode"></param>
    /// <returns></returns>
    public virtual IEnumerator LoadScenes(HashSet<string> sceneNames, LoadSceneMode mode)
    {
        if(sceneNames == null) yield break;
        if (sceneNames.Count == 0) yield break;
        foreach (var scene in sceneNames)
        {
           yield return StartCoroutine(LoadScene(scene, mode)); 
        }
    }

    /// <summary>
    /// Unload all the existing scenes that match the scene name, will check if scene is exist
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="mode"></param>
    /// <returns></returns>
    public virtual IEnumerator UnloadScene(string sceneName)
    {
        if (!IsSceneLoaded(sceneName,out List<Scene> matches)) yield break ;
        foreach (var scene in matches)
        { 
           yield return StartCoroutine(UnloadScene(scene));
        }
        OnUnload?.Invoke(sceneName);
    }

    /// <summary>
    /// Unload one scene, will check if the scene is exist
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="mode"></param>
    /// <returns></returns>
    public virtual IEnumerator UnloadScene(Scene scene)
    {
        if (!IsSceneLoaded(scene.name)) yield break;
        AsyncOperation op = SceneManager.UnloadSceneAsync(scene);
        if (op == null) yield break;
        yield return op;
        OnUnload?.Invoke(scene.name);
    }
    /// <summary>
    /// Unload all the existing scenes that match the scene name list
    /// </summary>
    /// <param name="sceneNames"></param>
    /// <returns></returns>
    public virtual IEnumerator UnloadScenes(HashSet<string> sceneNames)
    {
        if (sceneNames == null) yield break;
        if (sceneNames.Count == 0) yield break;
        List<Scene> matchScenes = GetLoadedScenesByNameList(sceneNames);
        if (matchScenes == null) yield break;   
        foreach (var scene in matchScenes)
        {
            yield return StartCoroutine(UnloadScene(scene));
        }
    }


    /// <summary>
    /// Get all loaded scenes including Duplicate
    /// </summary>
    /// <returns></returns>
    public List<Scene> GetAllLoadedScenes()
    {
        List<Scene> scenes = new List<Scene>();

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded)
            {
                scenes.Add(scene);
            }
        }
        return scenes;
    }

    /// <summary>
    /// Find all the scene including duplicates that matchs the scene names list, return null if none
    /// </summary>
    /// <param name="sceneNames"></param>
    /// <returns></returns>
    public List<Scene> GetLoadedScenesByNameList(HashSet<string> sceneNames)
    {
        List<Scene> loaded = GetAllLoadedScenes();
        List<Scene> matches = new List<Scene>();
        foreach (var s in loaded)
        {
            if (sceneNames.Contains(s.name))
            { 
                matches.Add(s);    
            }        
        }
        if (matches.Count == 0) return null;
        else return matches;
    }

    /// <summary>
    /// Find all the scene including duplicates that matchs the scene name
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public List<Scene> GetLoadedScenesByName(string sceneName)
    {
        List<Scene> matches = new();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);

            if (scene.isLoaded && scene.name == sceneName)
                matches.Add(scene);
        }
        return matches;
    }



    /// <summary>
    /// LoadScene AsyncOperation without activation
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="mode"></param>
    /// <returns></returns>
    public virtual AsyncOperation LoadSceneInBackground(string sceneName, LoadSceneMode mode)
    {
        if (!IsSceneNameVaild(sceneName)) return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, mode);
#if UNITY_EDITOR
        Debug.Log($"loading scene {sceneName} called by {gameObject.name}");
#endif
        if (op == null) return null;
        op.allowSceneActivation = false;
        return op;
    }


}
