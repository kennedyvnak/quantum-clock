using UnityEngine;

namespace QuantumClock {
    public class TutorialTrigger : MonoBehaviour {
        [SerializeField, TextArea] private string m_Text;
        [SerializeField] private bool m_TriggerOnce, m_CheckCollisionEnter, m_CheckCollisionExit;

        [SerializeField] private QuantumObject m_QuantumObject;
        [SerializeField] private QuantumRoom[] m_QuantumRooms;

        private uint _id;

        private void Awake() {
            if (m_QuantumObject) {
                m_QuantumObject.observerToggled.AddListener(QuantumObjectToggled);
            }

            if (m_QuantumRooms.Length > 0) {
                foreach (var room in m_QuantumRooms) {
                    room.triggerToggled.AddListener(QuantumRoomToggled);
                }
            }
        }

        public void TriggerLabel() {
            if (m_TriggerOnce && _id != 0) return;
            _id = TutorialLabel.instance.DrawText(m_Text, _id);
        }

        public void DisableLabel() {
            TutorialLabel.instance.DisableText();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!m_CheckCollisionEnter || !other.CompareTag("Player")) return;
            TriggerLabel();
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (!m_CheckCollisionExit || !other.CompareTag("Player")) return;
            DisableLabel();
        }

        private void QuantumObjectToggled(bool quantumEnabled) {
            m_QuantumObject.observerToggled.RemoveListener(QuantumObjectToggled);
            TriggerLabel();
        }

        private void QuantumRoomToggled(bool triggerIn) {
            if (triggerIn) TriggerLabel();
            else DisableLabel();
        }
    }
}
