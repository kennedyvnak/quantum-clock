using Pathfinding;
using UnityEngine;
using UnityEngine.Events;

namespace QuantumClock {
    public class FollowerAI : AIPath {
        [SerializeField] private UnityEvent m_ReachedEnd;

        public UnityEvent reachedEnd => m_ReachedEnd;

        public override void OnTargetReached() {
            base.OnTargetReached();
            m_ReachedEnd.Invoke();
        }
    }
}