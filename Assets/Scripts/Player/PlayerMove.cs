using Network;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private InputActionReference moveReference;
    [SerializeField] private InputActionReference crouchReference;

    [SerializeField] private LayerMask overlappedMask;
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float crouchSpeed = 2f;

    private CharacterController _characterController;

    private bool _isGrounded;
    private bool _isCrouching;

    private Vector2 _readValue;
    private Vector3 _nextDirection;
    private Vector3 _velocity;

    public bool IsMoving;
    public UnityEvent<bool> OnCrouching;

    private void Awake()
    {
        _characterController = GetComponentInParent<CharacterController>();
    }

    private void Start()
    {
        LocalPlayer.Player.OnPlayerDeath.AddListener(DisableMovement);
    }

    private void DisableMovement()
    {
        enabled = false;
    }

    private void StopCrouching(InputAction.CallbackContext obj)
    {
        _isCrouching = false;
        OnCrouching?.Invoke(_isCrouching);
    }

    private void StartCrouching(InputAction.CallbackContext obj)
    {
        _isCrouching = true;
        OnCrouching?.Invoke(_isCrouching);
    }

    private void UpdateMoveDirection(InputAction.CallbackContext obj)
    {
        _readValue = obj.ReadValue<Vector2>();
    }

    private void Update()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.2f, overlappedMask,
            QueryTriggerInteraction.Ignore);
        if (_isGrounded && _velocity.y < 0)
            _velocity.y = -2f;

        IsMoving = _nextDirection.sqrMagnitude != 0;
        _velocity.y += -9.81f * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        _nextDirection = transform.right * _readValue.x + transform.forward * _readValue.y;

        if (_isCrouching)
            _characterController.Move(_nextDirection.normalized * crouchSpeed * Time.fixedDeltaTime);
        else
            _characterController.Move(_nextDirection.normalized * moveSpeed * Time.fixedDeltaTime);

        _characterController.Move(_velocity * Time.deltaTime);
    }

    private void OnEnable()
    {
        moveReference.action.performed += UpdateMoveDirection;
        moveReference.action.canceled += UpdateMoveDirection;
        crouchReference.action.canceled += StopCrouching;
        crouchReference.action.performed += StartCrouching;

        crouchReference.action.Enable();
        moveReference.action.Enable();
    }

    private void OnDisable()
    {
        moveReference.action.performed -= UpdateMoveDirection;
        moveReference.action.canceled -= UpdateMoveDirection;
        crouchReference.action.canceled -= StopCrouching;
        crouchReference.action.performed -= StartCrouching;

        crouchReference.action.Disable();
        moveReference.action.Disable();
    }
}