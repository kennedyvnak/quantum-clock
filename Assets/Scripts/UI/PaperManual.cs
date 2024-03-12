using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
        [SerializeField] private PaperData[] m_Datas;

        private int _currentPage;
        private PaperData _currentData;

        private void Start() {
            m_ButtonClose.onClick.AddListener(CloseScreen);
            ShowData(0);
        }

        public static void ToggleGroup(CanvasGroup group, bool enabled) {
            group.alpha = enabled ? 1.0f : 0.0f;
            group.interactable = enabled;
            group.blocksRaycasts = enabled;
        }

        public void ShowData(int dataIdx) {
            var data = m_Datas[dataIdx];
            ShowData(data);
        }

        public void ShowData(PaperData data) {
            _currentPage = 0;
            _currentData = data;

            UpdateText();

            m_ButtonLeft.onClick.RemoveAllListeners();
            m_ButtonRight.onClick.RemoveAllListeners();

            m_ButtonLeft.onClick.AddListener(() => MovePage(-1));
            m_ButtonRight.onClick.AddListener(() => MovePage(1));

            ToggleGroup(m_Group, true);
        }

        public void MovePage(int idx) {
            _currentPage += idx;
            _currentPage = Mathf.Clamp(_currentPage, 0, _currentData.textData.Length - 1);
            UpdateText();
        }

        public void CloseScreen() {
            ToggleGroup(m_Group, false);
        }

        private void UpdateText() {
            m_SizeFitter.enabled = _currentPage == 0;
            m_Text.text = _currentData.textData[_currentPage];
            m_PageText.text = string.Format(m_PageFormat, _currentPage + 1, _currentData.textData.Length);
        }
    }
}
