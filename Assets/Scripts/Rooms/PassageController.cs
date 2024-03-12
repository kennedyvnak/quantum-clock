using UnityEngine;
using UnityEngine.Events;

namespace QuantumClock { 
    public class PassageController : QuantumObject {
        [SerializeField] private PassageController m_TransitTo;
        [SerializeField] private Collider2D m_DoorBlock;

        [SerializeField] private GameObject m_Rendererer;
        [SerializeField] private bool m_InitPassage;

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
            if (quantumEnabled && !isBlocking) 
                Transit();
        }
    }
}
