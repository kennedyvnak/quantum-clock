using UnityEngine;

namespace QuantumClock {
    public class QuantumTeleportObject : MonoBehaviour {
        [SerializeField] private Transform[] m_Anchors;
        [SerializeField] private QuantumObject m_TeleportObject;
        
        private int _currentAnchor;

        private void Start() {
            m_TeleportObject.observerToggled.AddListener(EVENT_ObserverToggled);
            _currentAnchor = -1;
            EVENT_ObserverToggled(true);
        }

        private void EVENT_ObserverToggled(bool quantumEnabled) {
            if (!quantumEnabled) return;

            _currentAnchor++;
            if (_currentAnchor >= m_Anchors.Length) _currentAnchor = 0;

            var anchor = m_Anchors[_currentAnchor];
            m_TeleportObject.transform.position = anchor.position;

            GameManager.instance.ChromaticAberration();
        }
    }
}
