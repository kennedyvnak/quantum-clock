using UnityEngine;

namespace QuantumClock {
    public class SoundtrackManager : MonoBehaviour {
        [SerializeField] private AudioClip[] m_Clips;
        [SerializeField] private AudioSource m_Source;
        [SerializeField] private Vector2 m_WaitNextSoundtrackTime;

        private float _waitTime;

        private void Start() {
            Play(m_Clips[Random.Range(0, m_Clips.Length)]);
        }

        public void Play(AudioClip music) {
            if (m_Source.isPlaying) m_Source.Stop();   

            m_Source.clip = music;
            m_Source.Play();
            _waitTime = music.length + Random.Range(m_WaitNextSoundtrackTime.x, m_WaitNextSoundtrackTime.y);
        }

        private void Update() {
            if (_waitTime <= 0.0f) return;

            _waitTime -= Time.deltaTime;
            if (_waitTime <= 0.0f) Play(m_Clips[Random.Range(0, m_Clips.Length)]);
        }
    }
}
