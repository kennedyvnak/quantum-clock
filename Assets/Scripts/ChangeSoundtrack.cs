using UnityEngine;

namespace QuantumClock {
    public class ChangeSoundtrack : MonoBehaviour {
        [SerializeField] private AudioClip m_Clip;
        [SerializeField] private bool m_PlayOnStart;
        
        public void Start() {
            var source = SoundtrackManager.instance.source;
            if (source.clip == m_Clip && source.isPlaying) return;

            if (source.isPlaying || m_PlayOnStart) {
                SoundtrackManager.instance.Play(m_Clip);
            } else {
                source.clip = m_Clip;
            }
        }
    }
}
