using UnityEngine;

[CreateAssetMenu(fileName = "CinematicsManager", menuName = "Scriptable Objects/CinematicsManager")]
public class CinematicsManager : ScriptableObject
{
    [field: SerializeField] public bool PlayerWaitForCinematics { get; set; } = true;

    public void SetWait(bool wait)
    {
        PlayerWaitForCinematics = wait;
    }
}
