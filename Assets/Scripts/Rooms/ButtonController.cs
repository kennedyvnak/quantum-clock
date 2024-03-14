using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace QuantumClock {
    public class ButtonController : MonoBehaviour {
        [SerializeField] private SpriteRenderer m_Renderer;
        [SerializeField] private Sprite m_SpriteOn, m_SpriteOff;

        [SerializeField] private UnityEvent<bool> m_ButtonToggled;
        [SerializeField] private UnityEvent m_ButtonOn, m_ButtonOff;

        [SerializeField] private AudioSource m_Audio;

        public UnityEvent<bool> buttonToggled => m_ButtonToggled;

        private bool _buttonEnabled;
        private List<Collider2D> _entered = new List<Collider2D>();

        public void Toggle(bool buttonEnabled) {
            m_Renderer.sprite = buttonEnabled ? m_SpriteOn : m_SpriteOff;
            _buttonEnabled = buttonEnabled;

            m_ButtonToggled.Invoke(!buttonEnabled);
            if (buttonEnabled) m_ButtonOn.Invoke();
            else m_ButtonOff.Invoke();

            m_Audio.Play();
        }


        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag("Player") && !other.CompareTag("Entity")) return;

            _entered.Add(other);
            Toggle(true);
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (!other.CompareTag("Player") && !other.CompareTag("Entity")) return;

            _entered.Remove(other);
            Toggle(_entered.Count > 0);
        }
    }
}
