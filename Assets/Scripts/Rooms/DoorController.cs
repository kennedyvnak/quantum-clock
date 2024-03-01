using UnityEngine;

namespace QuantumClock {
    public class DoorController : MonoBehaviour {
        private PassageController _currentPassage;

        public void SetPassage(PassageController passage) {
            _currentPassage = passage;    
            transform.SetParent(passage.transform);
            var rotation = (passage.direction == PassageDirection.Right || passage.direction == PassageDirection.Left) ? 90.0f : 0.0f;
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.Euler(0.0f, 0.0f, rotation));
            _currentPassage.SetDoor(this);
        }
    }
}