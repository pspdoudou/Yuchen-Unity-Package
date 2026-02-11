using GameEvents;
using Sirenix.OdinInspector;
using UnityEngine;

public class CustomEventTrigger : TriggerVolume
{
    [SerializeField][TextArea, HideLabel, PropertySpace(8)] private string text;

    [SerializeField] private StringEventAsset stringEventAsset;
    [SerializeField] private BoolEventAsset boolEventAsset;
    [SerializeField] private IntEventAsset intEventAsset;


    public void InvokeTheStringEvent()
    {
        stringEventAsset?.Invoke(text);
    }


    public void InvokeTheEmptyText()
    { 
    
        stringEventAsset?.Invoke("");
    
    }

    public void InvokeTheBoolEvent(bool state)
    { 
    
        boolEventAsset?.Invoke(state);
    
    }

    public void InvokeTheIntEvent(int index)
    {

        intEventAsset?.Invoke(index);

    }



}
