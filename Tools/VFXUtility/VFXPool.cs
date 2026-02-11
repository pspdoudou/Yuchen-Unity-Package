using System.Collections;
using MyObjectPooler;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.VFX;

public class VFXPool : PooledObject
{
    [SerializeField] private VisualEffect _vfx;
    [SerializeField] private bool _autoReturn = false;
   
    private Coroutine _waitCo;
    private void Awake()
    {
        if (!_vfx) _vfx = GetComponent<VisualEffect>();
    }

    public override void OnTake()
    {
        base.OnTake();
        _vfx.Play();
        if (_autoReturn)
        {
            if (_waitCo != null) { StopCoroutine(_waitCo); _waitCo = null; }
            _waitCo = _vfx.StartWaitAliveZero(this,  ReturnToPool);
        }
    }

    public override void OnReturn()
    {
        base.OnReturn();
        _vfx.Stop();
        if (_waitCo != null) { StopCoroutine(_waitCo); _waitCo = null; }
    }

}



public static class CheckVFXParticalExtension
{
    private static readonly WaitForSeconds s_FirstDelay = new WaitForSeconds(0.3f);
    private static readonly WaitForSeconds s_CheckEvery = new WaitForSeconds(0.5f);
    public static Coroutine StartWaitAliveZero(this VisualEffect vfx, MonoBehaviour runner, System.Action onDone)
    {

        if (vfx == null || runner == null) return null;
        return runner.StartCoroutine(WaitAndCallback(vfx, onDone));
    }

    private static IEnumerator WaitAndCallback(VisualEffect vfx, System.Action onDone)
    {
        yield return s_FirstDelay;

        while (vfx && vfx.aliveParticleCount > 0)
            yield return s_CheckEvery;
        onDone?.Invoke();
    }


}
