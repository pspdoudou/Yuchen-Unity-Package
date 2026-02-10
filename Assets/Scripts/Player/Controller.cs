using CharacterMovement;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

// abstract prevetning us from using controller directly, we haave to inherit from it first 
//requireComponent automatically adds the component, and prevents removal
[RequireComponent(typeof(CustomCharacterMovement))]
public abstract class Controller : MonoBehaviour
{
    [field: SerializeField] protected CursorLockMode CursorMode { get; set; } = CursorLockMode.Locked;
    public CustomCharacterMovement Movement { get; private set; }
    public Animator Animator { get; private set; }
    //protected PlayerDash dash;
   /* public Health Health { get; private set; }
    public Targetable Targetable { get; private set; }
    public Vision Vision { get; private set; }
   */
   

    //Awake is called before start
    protected virtual void Awake()
    {
        Movement = GetComponent<CustomCharacterMovement>();
        Animator = GetComponent<Animator>();

        Cursor.lockState = CursorMode;

    }





}
