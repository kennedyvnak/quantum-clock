using UnityEngine;
using UnityEngine.UI;

namespace QuantumClock {
    public class GameStarter : MonoBehaviour {
        [Header("Game Start")]
        [SerializeField] private ChangeSoundtrack m_SoundtrackManager;
        [SerializeField] private Button m_StartButton;
        [SerializeField] private Image m_BlackImage;

        private void Start() {
            m_StartButton.onClick.AddListener(StartGame);
        }

        private void StartGame() {
            m_SoundtrackManager.enabled = true;
            m_StartButton.gameObject.SetActive(false);
            PaperManual.instance.ShowData(0, ShowPlayer);
        }

        private void ShowPlayer() {
            m_BlackImage.enabled = false;
        }
    }
}
