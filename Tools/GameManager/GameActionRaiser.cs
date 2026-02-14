using GameEvents;
using UnityEngine;
using UnityEngine.Events;

public class GameActionRaiser : MonoBehaviour
{
    [SerializeField] private GameEventRaiseFlag _raiseEvent = GameEventRaiseFlag.Start;
    public UnityEvent UnityEvent;
    private void Awake()
    {
        if (HasEventFlag(GameEventRaiseFlag.Awake)) UnityEvent.Invoke();
    }
    private void OnEnable()
    {
        if (HasEventFlag(GameEventRaiseFlag.OnEnable)) UnityEvent.Invoke();
    }

    private void Start()
    {
        if (HasEventFlag(GameEventRaiseFlag.Start)) UnityEvent.Invoke();
    }

    private void FixedUpdate()
    {
        if (HasEventFlag(GameEventRaiseFlag.FixedUpdate)) UnityEvent.Invoke();
    }

    private void Update()
    {
        if (HasEventFlag(GameEventRaiseFlag.Update)) UnityEvent.Invoke();
    }

    private void LateUpdate()
    {
        if (HasEventFlag(GameEventRaiseFlag.LateUpdate)) UnityEvent.Invoke();
    }

    private void OnDisable()
    {
        if (HasEventFlag(GameEventRaiseFlag.OnDisable)) UnityEvent.Invoke();
    }

    private void OnDestroy()
    {
        if (HasEventFlag(GameEventRaiseFlag.OnDestroy)) UnityEvent.Invoke();
    }


    private bool HasEventFlag(GameEventRaiseFlag flag)
    {
        return (_raiseEvent & flag) == flag;
    }
    [System.Flags]
    public enum GameEventRaiseFlag
    {
        Awake = 1 << 1,
        OnEnable = 1 << 2,
        Start = 1 << 3,
        FixedUpdate = 1 << 4,
        Update = 1 << 5,
        LateUpdate = 1 << 6,
        OnDisable = 1 << 7,
        OnDestroy = 1 << 8
    }
}
