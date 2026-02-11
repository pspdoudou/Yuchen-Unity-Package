using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sirenix.OdinInspector;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    
    [field: SerializeField, HideInInlineEditors] public CheckpointManager CheckpointManager { get; private set; }

    [field: SerializeField] public int Index { get; private set; }
    [InfoBox("Duplicate Index in CheckpointManager or No CheckpointManager or CheckpointManager has not initialized properly!", InfoMessageType.Warning), ShowIf("@DuplicateIndex == true"), ShowInInspector]

    public bool DuplicateIndex
    {
        get
        {
            if (CheckpointManager == null) return true;
            if (CheckpointManager.CurrentCheckpoints.Count == 0) return true;
            HashSet<int> seen = new();
            seen.Add(Index);
            foreach (var c in CheckpointManager.CurrentCheckpoints)
            {
                if (c == null) return true;
                if (c.Index == Index && c != this) return true;
            }
            return false;
        }
    }

    public Vector3 Position => transform.position;
    public Quaternion Rotation => transform.rotation;

    public void SetCheckpoint()
    {
        CheckpointManager.SetCurrentIndex(Index);
    }

    [Button("Update Checkpoints")]
    public void UpdateCheckpoints()
    {
        CheckpointManager.GetCheckpointsFromScene();
    }
}
