using UnityEngine;
using UnityEngine.Events;

namespace QuantumClock {
    public class LeverController : MonoBehaviour {
        [SerializeField] private SpriteRenderer m_Renderer;
        [SerializeField] private Sprite m_SpriteOn, m_SpriteOff;

        [SerializeField] private UnityEvent<bool> m_LeverToggled;
        [SerializeField] private UnityEvent m_LeverOn, m_LeverOff;

        [SerializeField] private AudioSource m_Audio;

        public UnityEvent<bool> leverToggled => m_LeverToggled;

        private bool _leverEnabled;

        public void Toggle(bool leverEnabled) {
            m_Renderer.sprite = leverEnabled ? m_SpriteOn : m_SpriteOff;
            _leverEnabled = leverEnabled;

            m_LeverToggled?.Invoke(leverEnabled);
            if (leverEnabled) m_LeverOn.Invoke();
            else m_LeverOff.Invoke();

            m_Audio.Play();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag("Player")) return;

            Toggle(!_leverEnabled);
        }
    }
}
