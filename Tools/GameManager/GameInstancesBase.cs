
using Unity.Cinemachine;
using UnityEngine;

public class GameInstancesBase : ScriptableObject
{
    //References
    public GameObject PlayerGameObject;
    public Transform PlayerTransform;
    public CinemachineCamera CinemachineCamera;

    public bool Initialized { get; protected set; } = false;


    public virtual void OnEnable()
    {
        ClearReferences();
    }
    public virtual void ClearReferences()
    {
        PlayerGameObject = null;
        PlayerTransform = null;
        CinemachineCamera = null;
        Initialized = false;
    }

    public virtual void SetPlayer(GameObject playerGameObject)
    {
        PlayerGameObject = playerGameObject;
        PlayerTransform = playerGameObject.transform;
    }

    public virtual void SetcinemachineCamera(CinemachineCamera cinemachineCamera)
    {

        CinemachineCamera = cinemachineCamera;

    }
    



}
