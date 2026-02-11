using System;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;

[CreateAssetMenu(fileName = "GameInstance", menuName = "Scriptable Objects/GameInstance")]
public class GameInstances : ScriptableObject
{
    //References
    public int CurrentDifficultyLevel { get; private set; } = 0;
    public GameObject PlayerGameObject { get; private set; }
    //public PlayerController PlayerController { get; private set; }
    public Transform PlayerTransform { get; private set; }
    public CinemachineCamera CinemachineCamera { get; private set; }

    public event Action OnReferenceReady;
    public event Action OnCinemachineCamReady;
    
    private bool _initialized = false;


    private void OnEnable()
    {
        ClearReferences();
    }
    public void ClearReferences()
    {
        PlayerGameObject = null;
        //PlayerController = null;
        PlayerTransform = null;
        CinemachineCamera = null;
        _initialized = false;
    }
    
    public void SetPlayer(GameObject playerGameObject)
    {
        PlayerGameObject = playerGameObject;
        //PlayerController = playerGameObject.GetComponent<PlayerController>();
        PlayerTransform = playerGameObject.transform; 
        if (PlayerGameObject && CinemachineCamera)
        {
            if(!_initialized) OnReferenceReady?.Invoke();
            _initialized = true;
        }
    }

    public void SetcinemachineCamera(CinemachineCamera cinemachineCamera)
    {

        CinemachineCamera = cinemachineCamera;
        OnCinemachineCamReady?.Invoke();
        if (PlayerGameObject && CinemachineCamera)
        {
            if (!_initialized) OnReferenceReady?.Invoke();
            _initialized = true;

        }
    }
    

    public void SetDifficulty(int level)
    {
        CurrentDifficultyLevel = level;
    }

}
