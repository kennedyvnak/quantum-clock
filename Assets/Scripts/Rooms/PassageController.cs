using UnityEngine;

namespace QuantumClock { 
    public class PassageController : QuantumObject {
        [SerializeField] private PassageDirection m_Dir;
        [SerializeField] private PassageController m_TransitTo;

        [SerializeField] private SpriteRenderer m_Rendererer;
        [SerializeField] private DoorController m_DoorPrefab;
        [SerializeField] private bool m_InitWithDoor;

        private DoorController _currentDoor;

        public PassageDirection direction => m_Dir;

        private void Start() {
            if (!m_InitWithDoor) return;
            Instantiate(m_DoorPrefab).SetPassage(this);
        }

        public void SetDoor(DoorController door) {
            _currentDoor = door;
            _collider.enabled = false;            
            m_Rendererer.enabled = false;
        }

        public void Transit() {
            _currentDoor.SetPassage(m_TransitTo);
            _currentDoor = null;
            _collider.enabled = true;
            m_Rendererer.enabled = true;
        }

        protected override void EVENT_ObserverToggled(bool quantumEnabled) {
            base.EVENT_ObserverToggled(quantumEnabled);
            if (quantumEnabled && _currentDoor) 
                Transit();
        }
    }
}
