using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuantumClock {
    public class GameEnder : MonoBehaviour {
        [SerializeField] private string m_LoadSceneName;

        public void LoadScene() {
            SceneManager.LoadScene(m_LoadSceneName);
        }
    }
}
