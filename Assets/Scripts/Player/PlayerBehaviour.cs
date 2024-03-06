using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace QuantumClock { 
    public class PlayerBehaviour : MonoBehaviour {
        private static readonly int s_DirXKey = Animator.StringToHash("DirX");
        private static readonly int s_DirYKey = Animator.StringToHash("DirY");
        private static readonly int s_VelocityKey = Animator.StringToHash("Velocity");
        private static readonly int s_RunningKey = Animator.StringToHash("Running");

        [SerializeField] private float m_MoveSpeed;
        [SerializeField] private Transform m_LightTransform, m_PlayerLight;
        [SerializeField] private Animator m_Anim;
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

        private void Start() {
            ToggleLantern(true);
        }

        private void Update() {
            Vector3 dir = _mousePos - _mainCamera.WorldToScreenPoint(m_LightTransform.position);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            m_LightTransform.localRotation = Quaternion.AngleAxis(angle - 90.0f, Vector3.forward);

            if (dir.magnitude > 0.0f) {
                m_Anim.SetFloat(s_DirXKey, dir.x);
                m_Anim.SetFloat(s_DirYKey, dir.y);
            }

#if UNITY_EDITOR
            if (Keyboard.current.f6Key.wasPressedThisFrame) {
                ScreenCapture.CaptureScreenshot($"Recordings/GameCapture{System.DateTime.Now:yy-MM-dd HHmmss}.png");
            }
#endif
        }

        private void FixedUpdate() {
            rb.velocity = _moveInput * m_MoveSpeed;
            m_Anim.SetFloat(s_VelocityKey, rb.velocity.magnitude);
        }

        public void ToggleLantern(bool active) {
            m_LightTransform.gameObject.SetActive(active);
            m_PlayerLight.gameObject.SetActive(!active);
            _lanternActive = active;
        }

        public void Move(InputAction.CallbackContext ctx) {
            _moveInput = ctx.ReadValue<Vector2>();
        } 
        
        public void Point(InputAction.CallbackContext ctx) {
            _mousePos = ctx.ReadValue<Vector2>();
        }

        public void ToggleLantern(InputAction.CallbackContext ctx) {
            if (!ctx.performed) return;
            ToggleLantern(!_lanternActive);
        }

        public void Interact(InputAction.CallbackContext ctx) {
        } 
        
        public void CameraShot(InputAction.CallbackContext ctx) {
            if (!ctx.performed) return;
            m_QuantumCamera.TakeShot(_lanternActive);
        } 
        
        public void CameraClear(InputAction.CallbackContext ctx) {
            if (!ctx.performed) return;
            m_QuantumCamera.Clear();
        } 
    }
}