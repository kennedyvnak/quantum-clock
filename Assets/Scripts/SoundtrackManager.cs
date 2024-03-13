using UnityEngine;

namespace QuantumClock {
    public class SoundtrackManager : MonoBehaviour {
        [SerializeField] private AudioSource m_Source;
        [SerializeField] private Vector2 m_WaitNextSoundtrackTime;

        private float _waitTime;

        public AudioSource source => m_Source;

        public static SoundtrackManager instance { get; private set; }

        private void Awake() {
            if (instance) Destroy(gameObject);
            else {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void Play(AudioClip music) {
            if (m_Source.isPlaying) m_Source.Stop();   

            m_Source.clip = music;
            m_Source.Play();
            _waitTime = music.length + Random.Range(m_WaitNextSoundtrackTime.x, m_WaitNextSoundtrackTime.y);
        }

        private void Update() {
            if (_waitTime <= 0.0f || m_Source.isPlaying) return;

            _waitTime -= Time.deltaTime;
            if (_waitTime <= 0.0f) m_Source.Play();
        }
    }
}
