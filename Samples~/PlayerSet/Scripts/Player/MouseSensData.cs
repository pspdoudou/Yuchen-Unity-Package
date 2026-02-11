using System;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/MouseSens Data")]
public class MouseSensData : ScriptableObject
{

    [field: Header("Mouse Sens Setting")]
    [field: SerializeField] public float HipSens { get; private set; } = 0.05f;

    public event Action OnDataChanged;

    public void SetMouseSensitivity(float v) { HipSens = v; OnDataChanged?.Invoke(); }
  
}
