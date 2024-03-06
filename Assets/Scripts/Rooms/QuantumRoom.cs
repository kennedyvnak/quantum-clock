using UnityEngine;

namespace QuantumClock {
    public class QuantumRoom : MonoBehaviour {
        [SerializeField] private Transform m_TeleportTo;

        private PlayerBehaviour _currentPlayer;

        public void Teleport() {
            _currentPlayer.transform.position = m_TeleportTo.position;
            _currentPlayer.quantumCamera.vCam.ForceCameraPosition(_currentPlayer.transform.position, Quaternion.identity);
        }

        private void EVENT_LanternToggled(bool lanternEnabled) {
            if (!lanternEnabled) Teleport();
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.TryGetComponent<PlayerBehaviour>(out var player)) return;
            _currentPlayer = player;
            player.onLanternToggle.AddListener(EVENT_LanternToggled);
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (!other.TryGetComponent<PlayerBehaviour>(out var player) || player != _currentPlayer) return;
            player.onLanternToggle.RemoveListener(EVENT_LanternToggled);
            _currentPlayer = null;
        }
    }
}
