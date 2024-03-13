using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace QuantumClock { 
    public class PlayerBehaviour : MonoBehaviour {
        private static readonly int s_DirXKey = Animator.StringToHash("DirX");
        private static readonly int s_DirYKey = Animator.StringToHash("DirY");
        private static readonly int s_VelocityKey = Animator.StringToHash("Velocity");
        private static readonly int s_RunningKey = Animator.StringToHash("Running");

        [SerializeField] private float m_MoveSpeed, m_RunSpeed;
        [SerializeField] private Transform m_LightTransform, m_PlayerLight;
        [SerializeField] private Animator m_Anim;
        [SerializeField] private QuantumCamera m_QuantumCamera;
        [SerializeField] private bool m_HasCamera;

        [SerializeField] private UnityEvent<bool> m_OnLanternToggle;

        public Rigidbody2D rb { get; private set; }

        public UnityEvent<bool> onLanternToggle => m_OnLanternToggle;
        public QuantumCamera quantumCamera => m_QuantumCamera;
        public Vector2 pointerPosition => _mousePos;

        private Vector2 _moveInput;
        private bool _running;
        private Vector3 _mousePos;
        private Camera _mainCamera;
        private Transform _pointer;
        private bool _hasCamera;

        private bool _lanternActive = true;

        private void Awake() {
            rb = GetComponent<Rigidbody2D>();
            _mainCamera = Camera.main;
            _hasCamera = m_HasCamera;
        }

        private void Start() {
            ToggleLantern(true);
        }

        private void Update() {
            if (_pointer) {
                Vector3 pdir = _pointer.transform.position - m_LightTransform.transform.position;
                float pangle = Mathf.Atan2(pdir.y, pdir.x) * Mathf.Rad2Deg;
                m_LightTransform.localRotation = Quaternion.AngleAxis(pangle - 90.0f, Vector3.forward);

                if (pdir.magnitude > 0.0f) {
                    m_Anim.SetFloat(s_DirXKey, pdir.x);
                    m_Anim.SetFloat(s_DirYKey, pdir.y);
                }
                return;
            }

            Vector3 dir = _mainCamera.ScreenToWorldPoint(_mousePos) - m_LightTransform.position;
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
            rb.velocity = _moveInput * (_running ? m_RunSpeed : m_MoveSpeed);
            m_Anim.SetFloat(s_VelocityKey, rb.velocity.magnitude);
        }

        public void ToggleLantern(bool active) {
            m_LightTransform.gameObject.SetActive(active);
            m_PlayerLight.gameObject.SetActive(!active);
            _lanternActive = active;
            m_OnLanternToggle.Invoke(active);
        }

        public void GetCamera() {
            _hasCamera = true;
        }

        public void SetPointer(Transform pointer) {
            _pointer = pointer;
        }

        public void Move(InputAction.CallbackContext ctx) {
            _moveInput = ctx.ReadValue<Vector2>();
        } 

        public void Run(InputAction.CallbackContext ctx) {
            if (ctx.performed) _running = true;
            else if (ctx.canceled) _running = false;
            m_Anim.SetBool(s_RunningKey, _running);
        }
        
        public void Point(InputAction.CallbackContext ctx) {
            _mousePos = ctx.ReadValue<Vector2>();
            // Beucase the screen is rendered in a 1920x1080 texture need to do this.
            _mousePos.x = _mousePos.x / Screen.width * 1920.0f;
            _mousePos.y = _mousePos.y / Screen.height * 1080.0f;
        }

        public void ToggleLantern(InputAction.CallbackContext ctx) {
            if (!ctx.performed) return;
            ToggleLantern(!_lanternActive);
        }

        public void CameraShot(InputAction.CallbackContext ctx) {
            if (!ctx.performed || !_hasCamera) return;
            m_QuantumCamera.TakeShot(_lanternActive);
        } 
        
        public void CameraClear(InputAction.CallbackContext ctx) {
            if (!ctx.performed || !_hasCamera) return; m_QuantumCamera.Clear(); 
        } 
    }
}
