using Pathfinding;
using UnityEngine;

namespace QuantumClock {
    public class FollowerBehaviour : QuantumObject {
        [SerializeField] private FollowerAI m_Ai;
        [SerializeField] private float m_MinFollowingTimeKill, m_MinDistanceKill;
        [SerializeField] private Transform m_Target;

        [Header("Sounds" )]
        [SerializeField] private AudioSource m_EmissionSource;
        [SerializeField] private AudioClip[] m_EmissionClips;
        [SerializeField] private Vector2 m_SoundsDelay;

        private bool moving => m_Ai.canMove;

        private float _soundDelay;
        private float _followingTime = 0.0f;
        private bool _hasTarget;
        private Transform _startPoint;
        private AIDestinationSetter _targetSetter;

        protected override void Awake() {
            base.Awake();
            m_Ai.reachedEnd.AddListener(EVENT_ReachedEnd);
            _soundDelay = Random.Range(m_SoundsDelay.x, m_SoundsDelay.y);
        }

        private void Start() {
            _startPoint = new GameObject("EntryPoint").transform;
            _startPoint.SetParent(m_Ai.transform.parent);
            _startPoint.position = transform.position;
            _targetSetter = m_Ai.GetComponent<AIDestinationSetter>();
            ToggleTarget(false);
        }

        private void Update() {
            if (!moving) return;
            _followingTime += Time.deltaTime;

            if (!_hasTarget) return;
            _soundDelay -= Time.deltaTime;
            if (_soundDelay <= 0.0f) {
                _soundDelay = Random.Range(m_SoundsDelay.x, m_SoundsDelay.y);
                m_EmissionSource.clip = m_EmissionClips[Random.Range(0, m_EmissionClips.Length)];
                m_EmissionSource.Play();
            }
        }

        public void ToggleTarget(bool hasTarget) {
            _hasTarget = hasTarget;
            _targetSetter.target = hasTarget ? m_Target : _startPoint;
        }

        private void EVENT_ReachedEnd() {
            if (moving && _followingTime >= m_MinFollowingTimeKill && _hasTarget && Vector2.Distance(transform.position, m_Target.position) <= m_MinDistanceKill) GameOver();
        }

        private void GameOver() {
            GameManager.instance.EnemyGameOver(transform);
        }

        protected override void EVENT_ObserverToggled(bool quantumEnabled) {
            base.EVENT_ObserverToggled(quantumEnabled);
            m_Ai.canMove = quantumEnabled;
            if (quantumEnabled) _soundDelay = 0.0f;
            else _followingTime = 0.0f;
            GameManager.instance.ChromaticAberration();
        }
    }
}
