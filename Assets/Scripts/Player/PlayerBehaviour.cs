using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour {
    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private Transform m_LightTransform;

    public Rigidbody2D rb { get; private set; }

    private Vector2 _moveInput;
    private Vector3 _mousePos;
    private Camera _mainCamera;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
    }

    private void Update() {
        Vector3 dir = _mousePos - _mainCamera.WorldToScreenPoint(m_LightTransform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        m_LightTransform.localRotation = Quaternion.AngleAxis(angle - 90.0f, Vector3.forward);
    }   

    private void FixedUpdate() {
        rb.velocity = _moveInput * m_MoveSpeed;
    }

    public void Move(InputAction.CallbackContext ctx) {
        _moveInput = ctx.ReadValue<Vector2>();
    } 
    
    public void Point(InputAction.CallbackContext ctx) {
        _mousePos = ctx.ReadValue<Vector2>();
    }

    public void Interact(InputAction.CallbackContext ctx) {
    } 
    
    public void CameraShot(InputAction.CallbackContext ctx) {
    } 
    
    public void CameraClear(InputAction.CallbackContext ctx) {
    } 
}
