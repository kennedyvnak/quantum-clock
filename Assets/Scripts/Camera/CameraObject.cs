using UnityEngine;

namespace QuantumClock {
    public class CameraObject : MonoBehaviour {
        [SerializeField] private PlayerBehaviour m_Player;
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag("Player")) return;

            m_Player.GetCamera();
            PaperManual.instance.ShowData(1, null);
            gameObject.SetActive(false);
        }
    }
}
