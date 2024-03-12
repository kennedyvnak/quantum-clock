using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

namespace QuantumClock {
    public class PaperManual : MonoBehaviour {
        [System.Serializable]
        public class PaperData {
            [TextArea] public string[] textData;
        }
        
        [SerializeField] private TextMeshProUGUI m_Text, m_PageText;
        [SerializeField] private Button m_ButtonLeft, m_ButtonRight, m_ButtonClose;
        [SerializeField] private string m_PageFormat;
        [SerializeField] private CanvasGroup m_Group;
        [SerializeField] private ContentSizeFitter m_SizeFitter;
        [SerializeField] private AudioSource m_PaperSound;
        [SerializeField] private PaperData[] m_Datas;

        public static PaperManual instance { get; private set; }

        private int _currentPage;
        private PaperData _currentData;
        private System.Action _onClose;

        private void Awake() {
            instance = this;
        }

        private void Start() {
            m_ButtonClose.onClick.AddListener(CloseScreen);
        }

        public static void ToggleGroup(CanvasGroup group, bool enabled) {
            group.alpha = enabled ? 1.0f : 0.0f;
            group.interactable = enabled;
            group.blocksRaycasts = enabled;
        }

        public void ShowData(int dataIdx, System.Action onClose) {
            var data = m_Datas[dataIdx];
            ShowData(data, onClose);
        }

        public void ShowData(PaperData data, System.Action onClose) {
            _currentPage = 0;
            _currentData = data;
            _onClose = onClose;

            UpdateText();

            m_ButtonLeft.onClick.RemoveAllListeners();
            m_ButtonRight.onClick.RemoveAllListeners();

            m_ButtonLeft.onClick.AddListener(() => MovePage(-1));
            m_ButtonRight.onClick.AddListener(() => MovePage(1));

            ToggleGroup(m_Group, true);
            GameManager.instance.TogglePlayerInput(false);
            m_PaperSound.Play();
        }

        public void MovePage(int idx) {
            _currentPage += idx;
            _currentPage = Mathf.Clamp(_currentPage, 0, _currentData.textData.Length - 1);
            m_PaperSound.Play();
            UpdateText();
        }

        public void CloseScreen() {
            ToggleGroup(m_Group, false);
            GameManager.instance.TogglePlayerInput(true);
            m_PaperSound.Play();
            _onClose?.Invoke();
        }

        private void UpdateText() {
            m_SizeFitter.enabled = _currentPage == 0;
            m_Text.text = _currentData.textData[_currentPage];
            m_PageText.text = string.Format(m_PageFormat, _currentPage + 1, _currentData.textData.Length);
            m_ButtonClose.gameObject.SetActive(_currentPage == _currentData.textData.Length - 1);
        }
    }
}
