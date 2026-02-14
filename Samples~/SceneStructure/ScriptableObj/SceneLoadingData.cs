using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneLoadingData", menuName = "Scriptable Objects/SceneCheckpointSet")]
public class SceneLoadingData : ScriptableObject
{
    [field: SerializeField] public List<SceneField> PersistentScenes { get; private set; }
    [field: SerializeField] public List<SceneField> LevelScenes { get; private set; }
    [field: SerializeField] public List<SceneField> PlayerScene { get; private set; }
    [field: SerializeField] public SceneField StartCutScene { get; private set; }
    [field: SerializeField] public List<CheckpointSceneSet> CheckpointSceneSets { get; private set; }

}

[System.Serializable]
public class CheckpointSceneSet
{
    [Min(0)] public int CheckpointIndex;
    [Tooltip("sub levels to load for this check point")]
    public List<SceneField> Scenes = new();
}

