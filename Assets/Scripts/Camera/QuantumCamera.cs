using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace QuantumClock {
    public class QuantumCamera : MonoBehaviour {
        [SerializeField] private RawImage m_Image;
        [SerializeField] private UnityEvent<Texture2D> m_TakeShot;
        [SerializeField] private Camera m_Camera;
        [SerializeField] private RenderTexture m_ScreenTexture;
        [SerializeField] private Collider2D m_LightCollider;

        public Texture2D renderedTexture { get; private set; }

        private GameObject _cameraAnchorObj;

        private void Awake() {
            _cameraAnchorObj = Instantiate(m_LightCollider.gameObject, transform);    
            _cameraAnchorObj.SetActive(false);
            Destroy(_cameraAnchorObj.GetComponent<Light2D>());
            _cameraAnchorObj.tag = "CameraAnchor";
            _cameraAnchorObj.AddComponent<Rigidbody2D>().isKinematic = true;
        }

        public void TakeShot(bool lanternEnabled) {
            if (!renderedTexture) renderedTexture = new Texture2D(m_ScreenTexture.width, m_ScreenTexture.height, TextureFormat.ARGB32, false);
            m_Camera.gameObject.SetActive(true);
            m_Camera.Render();
            Graphics.CopyTexture(m_ScreenTexture, renderedTexture);
            m_Camera.gameObject.SetActive(false);
            m_TakeShot.Invoke(renderedTexture);
            m_Image.texture = renderedTexture;
            m_Image.enabled = true;

            _cameraAnchorObj.transform.SetPositionAndRotation(m_LightCollider.transform.position, m_LightCollider.transform.rotation);
            _cameraAnchorObj.SetActive(lanternEnabled);
        }

        public void Clear() {
            m_TakeShot.Invoke(null);
            m_Image.enabled = false;
            _cameraAnchorObj.SetActive(false);
        }
    }
}
