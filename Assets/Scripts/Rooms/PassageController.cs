using UnityEngine;
using UnityEngine.Events;

namespace QuantumClock { 
    public class PassageController : QuantumObject {
        [SerializeField] private PassageController m_TransitTo;
        [SerializeField] private Collider2D m_DoorBlock;

        [SerializeField] private GameObject m_Rendererer;
        [SerializeField] private bool m_InitPassage, m_ToggleBehaviour;

        [SerializeField] private UnityEvent<bool> m_OnBlockChanged;

        public bool isBlocking { get; private set; }
        public UnityEvent<bool> onBlockChanged => m_OnBlockChanged;

        private void Start() {
            SetBlocked(!m_InitPassage);
        }

        public void SetBlocked(bool blocked) {
            m_DoorBlock.enabled = blocked;
            m_Rendererer.SetActive(blocked);
            isBlocking = blocked;

            PathRescaner.Rescan(_collider);

            m_OnBlockChanged?.Invoke(isBlocking);
        }

        public void Transit() {
            m_TransitTo.SetBlocked(false);
            SetBlocked(true);
            GameManager.instance.ChromaticAberration();
        }

        protected override void EVENT_ObserverToggled(bool quantumEnabled) {
            base.EVENT_ObserverToggled(quantumEnabled);
            if (quantumEnabled && m_ToggleBehaviour) {
                SetBlocked(!isBlocking);
                GameManager.instance.ChromaticAberration(true);
                return;
            }

            if (quantumEnabled && !isBlocking && !m_TransitTo.isObserved) 
                Transit();
        }

        private void OnDrawGizmosSelected() {
            const int max = 15;

            PassageController passage = this;
            for (int i = 0; i < max; i++) {
                if (!passage.m_TransitTo) break;
                Gizmos.DrawLine(passage.transform.position, passage.m_TransitTo.transform.position);
                if (passage.m_InitPassage) Gizmos.color = Color.blue;
                Gizmos.DrawSphere(passage.transform.position, 0.5f - (i * 0.075f));
                Gizmos.color = Color.white;
                passage = passage.m_TransitTo;
                if (passage == this) break;
            }
        }
    }
}
