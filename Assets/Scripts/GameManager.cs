using UnityEngine;
using UnityEngine.UI;

namespace QuantumClock {
    public class GameManager : MonoBehaviour {
        [SerializeField] private Image m_PointerImage;
        [SerializeField] private int m_ClockCount;
        [SerializeField] private float m_ClockFactor;
        [SerializeField] private AudioSource[] m_ClockSounds;

        public static GameManager instance { get; private set; }

        private int _clockSteps;
        private int _lastClockIndex;

        private void Awake() {
            instance = this;
        }

        public void AddClockCount() {
            _clockSteps++;
            var rot = m_PointerImage.transform.rotation.eulerAngles;
            rot.z = _clockSteps / (float)m_ClockCount * m_ClockFactor;
            m_PointerImage.transform.rotation = Quaternion.Euler(rot);

            if (_lastClockIndex + 1 >= m_ClockSounds.Length) _lastClockIndex = 0;
            m_ClockSounds[_lastClockIndex].Play();
            _lastClockIndex++;
        }
    }
}