using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "CheckpointManager", menuName = "Scriptable Objects/CheckpointManager")]
public class CheckpointManager : ScriptableObject
{
    [SerializeField] public int CurrentIndex = 0;

    [InfoBox("0 is the fallback scene, make sure it always has a scene name.")]
    [field: SerializeField] public List<string> SceneIndexList { get; private set; } = new();
    [field: SerializeField, InlineEditor] public List<Checkpoint> CurrentCheckpoints { get; private set; } = new();

    [Button("Get Checkpoints")]
    public void GetCheckpointsFromScene()
    {
        CurrentCheckpoints.Clear();
        CurrentCheckpoints = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None).ToList();
        CurrentCheckpoints.Sort((x,y) => x.Index - y.Index); //sort ascendingly
    }
    
    public Vector3 GetCurrentTransform(out Quaternion rotation)
    {
        rotation = Quaternion.identity;
        if (CurrentCheckpoints.Count == 0) return Vector3.zero;
        if (CurrentCheckpoints.Exists(x => x.Index == CurrentIndex))
        {
            Checkpoint targetCheckpoint = CurrentCheckpoints.First(x => x.Index == CurrentIndex);
            rotation = targetCheckpoint.Rotation;
            return targetCheckpoint.Position;
        }
        rotation = CurrentCheckpoints[0].Rotation;
        return CurrentCheckpoints[0].Position;
    }
    
    
    public void SetCurrentIndex(int index)
    {
        CurrentIndex = Mathf.Max(0, index);
    }

    public string GetScene()
    {
        return SceneIndexList[CurrentIndex];
    }
}
