using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuantumClock {
    public class LevelChanger : MonoBehaviour {
        [SerializeField] private string m_LevelName;

        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag("Player")) return;

            SceneManager.LoadScene(m_LevelName);
        }
    }
}
