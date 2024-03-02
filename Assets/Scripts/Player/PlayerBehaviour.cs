using UnityEngine;
using UnityEngine.InputSystem;

namespace QuantumClock { 
    public class PlayerBehaviour : MonoBehaviour {
        [SerializeField] private float m_MoveSpeed;
        [SerializeField] private Transform m_LightTransform;
        [SerializeField] private QuantumCamera m_QuantumCamera;

        public Rigidbody2D rb { get; private set; }

        private Vector2 _moveInput;
        private Vector3 _mousePos;
        private Camera _mainCamera;

        private bool _lanternActive = true;

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

        public void ToggleLantern(InputAction.CallbackContext ctx) {
            if (!ctx.performed) return;
            m_LightTransform.gameObject.SetActive(!_lanternActive);
            _lanternActive = !_lanternActive;
        }

        public void Interact(InputAction.CallbackContext ctx) {
        } 
        
        public void CameraShot(InputAction.CallbackContext ctx) {
            if (!ctx.performed) return;
            m_QuantumCamera.TakeShot();
        } 
        
        public void CameraClear(InputAction.CallbackContext ctx) {
            if (!ctx.performed) return;
            m_QuantumCamera.Clear();
        } 
    }
}