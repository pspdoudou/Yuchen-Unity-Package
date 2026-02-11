using UnityEngine;
using MyObjectPooler;
using UnityEngine.VFX;

public class VFXAttacher : MonoBehaviour
{
    [SerializeField] private GameObject _objToAttach;


    public void AttachVFX(VFXPool vfx)
    {
        if (_objToAttach == null) return;
        Transform parent = _objToAttach.transform;
        //Debug.Log(parent.position);
        vfx.transform.SetParent(parent, true);
  
    }

    public void AttachVFX(VisualEffect vfx)
    {
        if (!_objToAttach) return;
        Transform parent = _objToAttach.transform;
        //Debug.Log(parent.position);
        vfx.transform.SetParent(parent, true);

    }


}
