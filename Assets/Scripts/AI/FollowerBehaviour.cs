using UnityEngine;

namespace QuantumClock {
    public class FollowerBehaviour : QuantumObject {
        [SerializeField] private FollowerAI m_Ai;
        [SerializeField] private float m_MinFollowingTimeKill;

        [Header("Sounds" )]
        [SerializeField] private AudioSource m_EmissionSource;
        [SerializeField] private AudioClip[] m_EmissionClips;
        [SerializeField] private Vector2 m_SoundsDelay;

        private bool moving => m_Ai.canMove;

        private float _soundDelay;
        private float _followingTime = 0.0f;
        private bool _gameOver = false;

        protected override void Awake() {
            base.Awake();
            m_Ai.reachedEnd.AddListener(EVENT_ReachedEnd);
            _soundDelay = Random.Range(m_SoundsDelay.x, m_SoundsDelay.y);
        }

        private void Update() {
            if (!moving) return;
            _followingTime += Time.deltaTime;

            _soundDelay -= Time.deltaTime;
            if (_soundDelay <= 0.0f) {
                _soundDelay = Random.Range(m_SoundsDelay.x, m_SoundsDelay.y);
                m_EmissionSource.clip = m_EmissionClips[Random.Range(0, m_EmissionClips.Length)];
                m_EmissionSource.Play();
            }
        }

        private void EVENT_ReachedEnd() {
            if (moving && _followingTime >= m_MinFollowingTimeKill) GameOver();
        }

        private void GameOver() {
            if (_gameOver) return;
            _gameOver = true;

            Debug.Log("Game Over");
        }

        protected override void EVENT_ObserverToggled(bool quantumEnabled) {
            base.EVENT_ObserverToggled(quantumEnabled);
            m_Ai.canMove = quantumEnabled;
            if (!quantumEnabled) _followingTime = 0.0f;
        }
    }
}
