using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace QuantumClock {
    public class GameManager : MonoBehaviour {
        [Header("Chromatic")]
        [SerializeField] private Volume m_Volume;
        [SerializeField] private float m_ChromaticDuration;
        [SerializeField] private AudioSource m_ChromaticSound;
        [SerializeField] private Vector2 m_ChromaticSoundPitchRange;

        [Header("GameOver")]
        [SerializeField] private PlayerBehaviour m_PlayerBehaviour;
        [SerializeField] private Transform m_Pointer;
        [SerializeField] private PlayerInput m_PlayerInput;
        [SerializeField] private float m_GameOverDelay;
        [SerializeField] private AudioSource m_GameOverSound, m_DeathSound;
        [SerializeField] private RawImage m_BlackImage;
        [SerializeField] private float m_DeathDelay;

        public static GameManager instance { get; private set; }

        public PlayerInput playerInput => m_PlayerInput;

        private Coroutine _chromaticCoroutine;
        private bool _gameOver;

        private void Awake() {
            instance = this;
        }

        public void TogglePlayerInput(bool enabled) {
            m_PlayerInput.SwitchCurrentActionMap(enabled ? "Player" : "UI");
        }

        public void EnemyGameOver(Transform enemy) {
            if (_gameOver) return;
            _gameOver = true;

            m_PlayerInput.enabled = false;
            m_Pointer.transform.position = enemy.transform.position; 
            m_PlayerBehaviour.SetPointer(m_Pointer);
            m_PlayerBehaviour.ToggleLantern(true);
            m_GameOverSound.Play();

            StartCoroutine(GameOverCoroutine());
        }

        public void ChromaticAberration(bool playSound = false) {
            if (_chromaticCoroutine != null) StopCoroutine(_chromaticCoroutine);
            _chromaticCoroutine = StartCoroutine(ChromaticAberrationCoroutine(playSound));
        }

        public void RestartGame() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void INPUT_RestartGame(InputAction.CallbackContext ctx) {
            if (ctx.performed) RestartGame();
        }

        private IEnumerator GameOverCoroutine() {
            if (_chromaticCoroutine != null) StopCoroutine(_chromaticCoroutine);
            if (!m_Volume.profile.TryGet<ChromaticAberration>(out var chromatic)) yield break;

            chromatic.intensity.Override(1.0f);
            yield return new WaitForSeconds(m_GameOverDelay);

            m_BlackImage.color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            m_DeathSound.Play();
            yield return new WaitForSeconds(m_DeathDelay);

            RestartGame();
        }

        private IEnumerator ChromaticAberrationCoroutine(bool playSound) {
            if (!m_Volume.profile.TryGet<ChromaticAberration>(out var chromatic)) yield break;

            m_ChromaticSound.pitch = Random.Range(m_ChromaticSoundPitchRange.x, m_ChromaticSoundPitchRange.y);
            if (playSound) m_ChromaticSound.Play();

            chromatic.intensity.Override(1.0f);
            yield return new WaitForSeconds(m_ChromaticDuration);
            chromatic.intensity.Override(0.0f);
        }
    }
}
