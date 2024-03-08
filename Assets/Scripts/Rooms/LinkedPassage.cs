using UnityEngine;

namespace QuantumClock {
    public class LinkedPassage : MonoBehaviour {
        [SerializeField] private PassageController m_ParentPassage;
        [SerializeField] private Collider2D m_DoorBlock;
        [SerializeField] private Collider2D m_Trigger;

        [SerializeField] private GameObject m_Rendererer;

        private void Start() {
            SetBlock(m_ParentPassage.isBlocking);
            m_ParentPassage.onBlockChanged.AddListener(SetBlock);
        }

        public void SetBlock(bool isBlocking) {
            m_DoorBlock.enabled = isBlocking;            
            m_Rendererer.SetActive(isBlocking);
            PathRescaner.Rescan(m_Trigger);
        }
    }
}
