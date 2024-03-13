using UnityEngine;
using TMPro;
using System.Collections;

namespace QuantumClock {
    public class TutorialLabel : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI m_Text;
        [SerializeField] private float m_ScrollSpeed, m_CommaDelay, m_DotDelay;

        private Coroutine _textDrawCoroutine;
        private uint _currentLabelID = 1;

        public static TutorialLabel instance { get; private set; }

        private void Awake() {
            instance = this;
        }

        public uint DrawText(string text, uint prevId) {
            if (prevId != _currentLabelID) {
                _currentLabelID++; 
                if (_textDrawCoroutine != null) StopCoroutine(_textDrawCoroutine);
                m_Text.maxVisibleCharacters = 0;
                m_Text.text = text;
                _textDrawCoroutine = StartCoroutine(DrawText());
            }
            m_Text.enabled = true;
            return _currentLabelID;
        }

        public void DisableText() {
            m_Text.enabled = false;
        }

        private IEnumerator DrawText() {
            yield return null;

            float charCount = 0;
            int pauseChar = -1;

            while (charCount < m_Text.textInfo.characterCount) {
                if (!m_Text.enabled) yield return null;

                int curChar = (int)charCount;
                charCount += Time.deltaTime * m_ScrollSpeed;
                m_Text.maxVisibleCharacters = curChar;
                if (curChar > 0 && pauseChar != curChar) {
                    var prevCharText = m_Text.text[curChar - 1];
                    float time = prevCharText == ',' ? m_CommaDelay : (prevCharText == '.' ? m_DotDelay : -1.0f);
                    if (time > 0.0f) {
                        pauseChar = curChar;
                        yield return new WaitForSeconds(time);
                    }
                }
                yield return null;
            }

            m_Text.maxVisibleCharacters = m_Text.textInfo.characterCount;
        }
    }
}
