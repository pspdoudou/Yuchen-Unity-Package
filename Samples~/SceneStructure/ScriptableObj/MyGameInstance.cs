using System;
using Unity.Cinemachine;
using UnityEngine;

[CreateAssetMenu(fileName = "GameInstance", menuName = "Scriptable Objects/GameInstance")]
public class MyGameInstance : GameInstancesBase
{
    public PlayerController PlayerController;

    public int CurrentDifficultyLevel = 0;
    public event Action OnReferenceReady;

    public void SetDifficulty(int level)
    {
        CurrentDifficultyLevel = level;
    }

    public override void SetPlayer(GameObject playerGameObject)
    {
        base.SetPlayer(playerGameObject);
        PlayerController = playerGameObject.GetComponent<PlayerController>();

        if (PlayerGameObject && CinemachineCamera)
        {
            if (!Initialized) OnReferenceReady?.Invoke();
            Initialized = true;
        }
    }
    public override void SetcinemachineCamera(CinemachineCamera cinemachineCamera)
    {
        base.SetcinemachineCamera(cinemachineCamera);
        if (PlayerGameObject && CinemachineCamera)
        {
            if (!Initialized) OnReferenceReady?.Invoke();
            Initialized = true;

        }
    }
}
