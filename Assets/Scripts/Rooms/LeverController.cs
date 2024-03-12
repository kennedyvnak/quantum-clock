using UnityEngine;
using UnityEngine.Events;

namespace QuantumClock {
    public class LeverController : MonoBehaviour {
        [SerializeField] private bool m_StartOn;
        [SerializeField] private SpriteRenderer m_Renderer;
        [SerializeField] private Sprite m_SpriteOn, m_SpriteOff;

        [SerializeField] private UnityEvent<bool> m_LeverToggled;

        public UnityEvent<bool> leverToggled => m_LeverToggled;

        private bool _leverEnabled;

        private void Start() {
            Toggle(m_StartOn, false);
        }

        public void Toggle(bool leverEnabled, bool raiseEvent) {
            m_Renderer.sprite = leverEnabled ? m_SpriteOn : m_SpriteOff;
            if (raiseEvent) m_LeverToggled?.Invoke(leverEnabled);
            _leverEnabled = leverEnabled;
        }

        public void ToggleWithoutRaising(bool leverEnabled) {
            Toggle(leverEnabled, false);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag("Player")) return;

            Toggle(!_leverEnabled, true);
        }
    }
}
