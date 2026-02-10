using DebugMenu;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System.Collections;
using GameEvents;


public class PlayerController : Controller
{
    [HideInInspector] public Vector3 MoveInput { get; private set; }
    private Vector2 _lookInput;
    private bool _isPressingSAC;
    private bool _hasSlideAndCrouch;
    private Vector3 _aimPosition;
    private Vector3 _cameraRetativeInput;
    public bool InWeaponSwap;
    private PlayerInput PlayerInput;
    [SerializeField] private LayerMask aimLayer;
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private float MaskOnSpeedMultiplier = 0.5f;
    //[SerializeField] private Targetable _targetable;
    [SerializeField] private BoolEventAsset _isMaskOnBoolEvent;

    private bool _isMaskOn;

    public UnityEvent<bool> OnSlowMoStats;
    private float _speedMultiplier => _isMaskOn ? MaskOnSpeedMultiplier : 1f;

    protected override void Awake()
    {
        base.Awake();
        PlayerInput = GetComponent<PlayerInput>();
        _isMaskOn = false;
    }

    private void Start()
    {  

    }


    public void SetInputToDefault(bool gamePaused)
    { 
        if(gamePaused) return; // only reset the fire and ads input after resume the game
        if(_isPressingSAC) _isPressingSAC = false;
    }


    public void OnJump()
    {
        //if (!Health.IsAlive) return;
        // TryJump will check for the ground first
        Movement.TryJump();
       // Debug.Log("jump");

    }

    public void OnToggleMask()
    {
        switch (_isMaskOn)
        {
            case true: _playerAnimator.SetTrigger("MaskUp"); break;
            case false: _playerAnimator.SetTrigger("MaskDown"); break;

        }
        _isMaskOn = !_isMaskOn;
        //if (_targetable != null) _targetable.isTargetable = !_isMaskOn;
        if(_isMaskOnBoolEvent != null) _isMaskOnBoolEvent.Invoke(_isMaskOn);
        Movement.ApplySpeedMultiplier(_speedMultiplier);

        
    }


    public void OnSlideAndCrouch(InputValue inputValue)
    {
        _isPressingSAC = inputValue.isPressed;
        if (_isPressingSAC)
        {
            if (Movement.IsGrounded)
            {
                if (Movement.HorizontalSpeed <= Movement.Speed * 0.5f && !Movement.IsCrouching) Movement.StartCrouch(true);
                else if (!Movement.IsSliding)
                {
                    //if (Movement.IsDashing) return;
                    //if (Movement.IsSliding) Movement.StopSliding();
                    // if(!movement.IsGrounded) return;
                    //if(!movement.IsSliding) return;
                    Movement.StartSliding(_cameraRetativeInput);
                }
                _hasSlideAndCrouch = true;
            }
            else Movement.TryDiving(true);
        }
        else
        {
            if(Movement.IsCrouching) Movement.StartCrouch(false);
            if(Movement.IsSliding) Movement.StopSliding();
            Movement.TryDiving(false);
            _hasSlideAndCrouch = false;
        }
    }


    public void OnMove(InputValue inputValue)
    {
        // read the Vector2 input from our device
        Vector2 input = inputValue.Get<Vector2>();

        // remap to world direction
        MoveInput = new Vector3(input.x, 0f, input.y);

        // debug movement direction
        Debug.DrawRay(transform.position, MoveInput, Color.magenta, 1f);
    }

    public void OnLook(InputValue inputValue)
    {
        _lookInput = inputValue.Get<Vector2>();
        // Debug.Log(lookInput);

    }
    public void OnDash()
    {
        //if(Movement.IsSliding) return;
        if (Movement.IsSliding) Movement.StopSliding();
        Movement.StartDash(_cameraRetativeInput);

    }



    private IEnumerator WeaponSwapDelay()
    {
        InWeaponSwap = true;
        yield return new WaitForSeconds(0.2f);
        InWeaponSwap = false;
    }


    
    private void Update()
    {
        if (Movement == null) return;

        // find correct right/forward directions based on main camera rotation
        Vector3 up = Vector3.up;
        Vector3 right = Camera.main.transform.right;
        Vector3 forward = Vector3.Cross(right, up);
        _cameraRetativeInput = forward * MoveInput.z + right * MoveInput.x;
        Movement.SetLookDirection(Camera.main.transform.forward);
        Movement.SetMoveInput(_cameraRetativeInput);
        

        Ray cameraRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(cameraRay, out RaycastHit hitInfo, Mathf.Infinity, aimLayer))
        { 
            _aimPosition = hitInfo.point;
            //Debug.Log(hitInfo.transform.position);
        }
        else 
        {
            _aimPosition = Camera.main.transform.position + Camera.main.transform.forward * 10000f;
        }

    }
    
  


}
