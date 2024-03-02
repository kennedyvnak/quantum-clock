using System;
using UnityEngine;
using UnityEngine.Events;

namespace QuantumClock {
    public enum PassageDirection { Right, Down, Left, Up }

    public class QuantumObject : MonoBehaviour {
        [Flags] 
        public enum QuantumObserver { 
            None = 0, Lantern = 1 << 0, Camera = 1 << 1, Anchor = 1 << 2 
        }
        
        [SerializeField] protected UnityEvent<bool> m_ObserverToggled;

        protected Collider2D _collider { get; private set; }
        protected QuantumObserver _observerFlags { get; private set; } 
        
        protected virtual void Awake() {
            _collider = GetComponent<Collider2D>();
        }

        public void ToggleObserver(bool observerEnabled, QuantumObserver flag) {
            if (observerEnabled) _observerFlags |= flag;
            else _observerFlags &= ~flag; 
            
            EVENT_ObserverToggled(_observerFlags == QuantumObserver.None);
        }

        protected virtual void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Lantern")) ToggleObserver(true, QuantumObserver.Lantern);
            else if (other.CompareTag("CameraAnchor")) ToggleObserver(true, QuantumObserver.Camera);
        }

        protected virtual void OnTriggerExit2D(Collider2D other) {
            if (other.CompareTag("Lantern")) ToggleObserver(false, QuantumObserver.Lantern);
            else if (other.CompareTag("CameraAnchor")) ToggleObserver(false, QuantumObserver.Camera);
        }

        protected virtual void EVENT_ObserverToggled(bool quantumEnabled) {
            m_ObserverToggled.Invoke(quantumEnabled);
        }
    }
}
