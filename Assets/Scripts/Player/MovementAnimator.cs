using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementAnimator : MonoBehaviour
{
    [SerializeField] private InputActionReference moveReference;
    [SerializeField] private PlayerMove playerMove;

    private Animator _playerAnimator;
    private Vector2 _nextDelta;
    private Vector2 _currentDelta;
    private int _velocityHashY;
    private int _velocityHashX;
    private bool _isCrouching;

    private void Awake()
    {
        _velocityHashY = Animator.StringToHash("VelocityY");
        _velocityHashX = Animator.StringToHash("VelocityX");

        _playerAnimator = GetComponentInParent<Animator>();
        playerMove.OnCrouching.AddListener(OnCrouching);
    }

    private void OnCrouching(bool status)
    {
        _isCrouching = status;
        _playerAnimator.SetBool("IsCrouching", _isCrouching);
    }

    private void OnEnable()
    {
        moveReference.action.performed += UpdateDeltaValue;
        moveReference.action.canceled += UpdateDeltaValue;
        moveReference.action.Enable();
    }

    private void OnDisable()
    {
        moveReference.action.performed -= UpdateDeltaValue;
        moveReference.action.canceled -= UpdateDeltaValue;
        moveReference.action.Disable();
    }

    private void Update()
    {
        _currentDelta = Vector3.MoveTowards(_currentDelta, _nextDelta, Time.fixedDeltaTime * 3f);
        _playerAnimator.SetFloat(_velocityHashY, _currentDelta.y);
        _playerAnimator.SetFloat(_velocityHashX, _currentDelta.x);
    }

    private void UpdateDeltaValue(InputAction.CallbackContext obj)
    {
        _nextDelta = obj.ReadValue<Vector2>();
    }
}