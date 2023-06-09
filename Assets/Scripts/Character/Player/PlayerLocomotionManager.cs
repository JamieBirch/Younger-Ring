using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    private PlayerManager _playerManager;
    
    public float verticalMovement;
    public float horizontalMovement;
    public float moveAmount;

    private Vector3 moveDirection;
    private Vector3 _targetRotation;
    [SerializeField] private float walkingSpeed = 2;
    [SerializeField] private float runningSpeed = 5;
    [SerializeField] private float rotationSpeed = 15;


    protected override void Awake()
    {
        base.Awake();

        _playerManager = GetComponent<PlayerManager>();
    }

    protected override void Update()
    {
        base.Update();

        if (_playerManager.IsOwner)
        {
            _playerManager._characterNetworkManager.animatorVerticalParameter.Value = verticalMovement;
            _playerManager._characterNetworkManager.animatorHorizontalParameter.Value = horizontalMovement;
            _playerManager._characterNetworkManager.networkMoveAmountParameter.Value = moveAmount;
        }
        else
        {
            verticalMovement = _playerManager._characterNetworkManager.animatorVerticalParameter.Value;
            horizontalMovement = _playerManager._characterNetworkManager.animatorHorizontalParameter.Value;
            moveAmount = _playerManager._characterNetworkManager.networkMoveAmountParameter.Value;
            
            _playerManager.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount);
        }
    }

    public void HandleAllMovement()
    {
        HandleGroundedMovement();
        HandleRotation();
    }

    private void GetVerticalAndHorizontalInputs()
    {
        verticalMovement = PlayerInputManager.instance.verticalInput;
        horizontalMovement = PlayerInputManager.instance.horizontalInput;
        moveAmount = PlayerInputManager.instance.moveAmount;
    }

    private void HandleGroundedMovement()
    {
        GetVerticalAndHorizontalInputs();
        
        moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
        moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
        moveDirection.Normalize();
        moveDirection.y = 0;

        if (PlayerInputManager.instance.moveAmount > 0.5f)
        {
            _playerManager.CharacterController.Move(moveDirection * runningSpeed * Time.deltaTime);
        } 
        else if (PlayerInputManager.instance.moveAmount <= 0.5f)
        {
            _playerManager.CharacterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
        }
    }

    private void HandleRotation()
    {
        _targetRotation = Vector3.zero;
        _targetRotation = PlayerCamera.instance.Camera.transform.forward * verticalMovement;
        _targetRotation = _targetRotation + PlayerCamera.instance.Camera.transform.right * horizontalMovement;
        _targetRotation.Normalize();
        _targetRotation.y = 0;

        if (_targetRotation == Vector3.zero)
        {
            _targetRotation = transform.forward;
        }
        
        Quaternion newRotation = Quaternion.LookRotation(_targetRotation);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = targetRotation;
    }

}
