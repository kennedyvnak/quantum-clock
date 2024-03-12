using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace QuantumClock {
    public class GameManager : MonoBehaviour {
        [SerializeField] private PlayerBehaviour m_PlayerBehaviour;
        [SerializeField] private Transform m_Pointer;
        [SerializeField] private PlayerInput m_PlayerInput;
        [SerializeField] private float m_GameOverDelay;
        [SerializeField] private AudioSource m_GameOverSound;

        public static GameManager instance { get; private set; }

        public PlayerInput playerInput => m_PlayerInput;

        private void Awake() {
            instance = this;
        }

        public void EnemyGameOver(Transform enemy) {
            m_PlayerInput.enabled = false;
            m_Pointer.transform.position = enemy.transform.position; 
            m_PlayerBehaviour.SetPointer(m_Pointer);
            m_PlayerBehaviour.ToggleLantern(true);
            m_GameOverSound.Play();

            StartCoroutine(GameOverCoroutine());
        }

        private IEnumerator GameOverCoroutine() {
            yield return new WaitForSeconds(m_GameOverDelay);

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
