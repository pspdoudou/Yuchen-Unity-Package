using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// A simple Meter.
/// </summary>
[AddComponentMenu("UI/ShaderGraph Samples/Meter")]
[RequireComponent(typeof(Graphic))]
[DisallowMultipleComponent]
public class Meter : UIBehaviour
{
    [SerializeField, Range(0f, 1f)] private float _value = 0.5f;

    [SerializeField] private string _sliderValuePropertyName = "_MeterValue";

    private int? _meterValuePropertyId;

    private Graphic _graphic;
    private Material _runTimeMat;
    public Graphic Graphic
    {
        get
        {
            if (_graphic == null)
            {
                _graphic = GetComponent<Graphic>();
            }

            return _graphic;
        }
    }

    public Material RunTimeMat
    {
        get
        {
            if (_runTimeMat == null)
            {
                _runTimeMat = Instantiate(Graphic.material);
                Graphic.material = _runTimeMat;
            }
            return _runTimeMat;
        }
    }

    public float Value
    {
        get => _value;
        set
        {
            _value = value;
            SetMaterialValue(value);
        }
    }

    protected int MeterValuePropertyId
    {
        get
        {
            if (!_meterValuePropertyId.HasValue)
                _meterValuePropertyId = Shader.PropertyToID(_sliderValuePropertyName);
            return _meterValuePropertyId.Value;
        }
    }

    protected override void Start()
    {
        base.Start();
        Value = _value;
    }

    private void SetMaterialValue(float value)
    {
        var mat = RunTimeMat;
        if (mat != null && mat.HasFloat(MeterValuePropertyId))
            mat.SetFloat(MeterValuePropertyId, value);
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        Value = _value;
    }
#endif

}
