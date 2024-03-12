using UnityEngine;
using UnityEngine.Events;

namespace QuantumClock {
    public class FollowerArea : MonoBehaviour {
        [SerializeField] private UnityEvent<bool> m_TargetToggled;

        private bool _entered;

        public UnityEvent<bool> targetToggled => m_TargetToggled;
        public bool entered => _entered;

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.transform.CompareTag("Player")) return;
            _entered = true;
            m_TargetToggled.Invoke(true);
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (!other.transform.CompareTag("Player")) return;
            _entered = false;
            m_TargetToggled.Invoke(false);
        }
    }
}

