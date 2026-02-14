using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "CheckpointManager", menuName = "Scriptable Objects/CheckpointManager")]
public class CheckpointManager : ScriptableObject
{
    [SerializeField] public int CurrentIndex = 0;

    [InfoBox("0 is the fallback scene, make sure it always has a scene name.")]
    [field: SerializeField, InlineEditor] public List<Checkpoint> ExistingCheckpoints { get; private set; } = new();

    [Button("Get Checkpoints")]
    public void GetCheckpointsFromScene()
    {
        ExistingCheckpoints.Clear();
        ExistingCheckpoints = FindObjectsByType<Checkpoint>(FindObjectsSortMode.None).ToList();
        ExistingCheckpoints.Sort((x,y) => x.Index - y.Index); //sort ascendingly
    }
    
    public Transform GetCurrentCheckpointTransform()
    {
       
        if (ExistingCheckpoints.Count == 0) return null;
        if (ExistingCheckpoints.Exists(x => x.Index == CurrentIndex))
        {
            Checkpoint targetCheckpoint = ExistingCheckpoints.First(x => x.Index == CurrentIndex);
            return targetCheckpoint.transform;
        }
        return null;
    }
    
    
    public void SetCurrentIndex(int index)
    {
        CurrentIndex = Mathf.Max(0, index);
    }
}
