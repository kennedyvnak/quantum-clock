using UnityEngine;

namespace QuantumClock {
    public class LanternDeactivator : MonoBehaviour {
        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.TryGetComponent<PlayerBehaviour>(out var player)) return;
            player.ToggleLantern(false);
        }
    }
}
