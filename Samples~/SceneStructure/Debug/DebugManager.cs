using DebugMenu;
using GameEvents;
using UnityEngine;

public class DebugManager : GameManager
{
    [SerializeField] private MyGameInstance _myGamenInstance;
    [SerializeField] private Transform _TestGym;
    [SerializeField] private BoolEventAsset _endingCinematicEvent;
    [SerializeField] private BoolEventAsset _togglePlayerHUD;
    [SerializeField] private StringEventAsset _debugStats;

    string godModeString = "";
    string ghostModeString = "";
    private bool _IsPlayerHUDEnabled = true;


    private void Awake()
    {
        DebugMenuSystem.Instance.RegisterObject(this);

    }

    private void Start()
    {
        
    }

    public void OnHudLoaded()
    {
    }

    private void OnDisable()
    {
        DebugMenuSystem.Instance.DeregisterObject(this);

    }

    #region GodAndGhost
    private void DisplayDebugStats()
    {
        _debugStats?.Invoke(godModeString + "\n" + ghostModeString);
    }



    [DebugCommand("GodMode")]
    public void GodMode()
    {
       // _myGamenInstance.PlayerController.PlayerHealth.GodMode();
        //godModeString = _myGamenInstance.PlayerController.PlayerHealth.IsGodModeOn ? "God Mode Enable" : "";
        DisplayDebugStats();
    }


    [DebugCommand("GhostMode")]
    public void GhostMode()
    {
        //_myGamenInstance.PlayerController.OnGhostMode();
       // ghostModeString = _myGamenInstance.PlayerController.IsGhostModeOn ? "Ghost Mode Enable" : "";
        DisplayDebugStats();
    }
    #endregion


    #region LevelTeleport

    [DebugCommand("Start Test Gym")]
    public void StartTestGym()
    {
        GameObject testgym = _TestGym.gameObject;
        if(testgym.activeInHierarchy ==false) testgym.SetActive(true);
        Rigidbody _rb = _myGamenInstance.PlayerGameObject.GetComponent<Rigidbody>();
        //_referenceGetter.PlayerGameObject.transform.position = _levelStart[0].position;
        _rb.transform.position = _TestGym.position;
    }


    [DebugCommand("Checkpoint 0")]
    public void StartLevel0()
    {
        GameSceneManager.Instance.DebugLoadScene(0);
        
    }

    [DebugCommand("Checkpoint 1")]
    public void StartLevel1()
    {
        GameSceneManager.Instance.DebugLoadScene(1);
    }

    
    

    #endregion

    #region Cinematics

    [DebugCommand("Trigger Cinematic")]
    public void TriggerCinematic()
    {
        _endingCinematicEvent.Invoke(true);
    }

    #endregion

    #region ShowCase

    [DebugCommand("Toggle Player HUD")]
    public void TogglePlayerHUD()
    {
        if (_togglePlayerHUD == null) return;
        _IsPlayerHUDEnabled = !_IsPlayerHUDEnabled;
        _togglePlayerHUD?.Invoke(_IsPlayerHUDEnabled);
    }
    #endregion





}