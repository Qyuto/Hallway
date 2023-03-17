using Network;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseRotation : MonoBehaviour
{
    [SerializeField] private float mouseSense = 0.3f;
    [SerializeField] private Transform viewTransform;
    [SerializeField] private InputActionReference rotationReference;

    private Transform _bodyTransform;
    private Vector2 _deltaMouse;
    private float _yRotation;

    private void Start()
    {
        if (LocalPlayer.Player.isLocalPlayer)
        {
            _bodyTransform = LocalPlayer.Player.transform;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    private void OnEnable()
    {
        rotationReference.action.performed += Rotate;
        rotationReference.action.Enable();
    }

    private void Rotate(InputAction.CallbackContext obj)
    {
        _deltaMouse = obj.ReadValue<Vector2>();
        _yRotation -= _deltaMouse.y * mouseSense;
        _yRotation = Mathf.Clamp(_yRotation, -80, 80);
        viewTransform.localRotation = Quaternion.Euler(_yRotation, 0, 0);

        _bodyTransform.Rotate(Vector3.up, _deltaMouse.x * mouseSense);
    }
}