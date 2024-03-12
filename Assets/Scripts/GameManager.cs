using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace QuantumClock {
    public class GameManager : MonoBehaviour {
        [Header("Game Start")]
        [SerializeField] private SoundtrackManager m_SoundtrackManager;

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
        [SerializeField] private AudioSource m_GameOverSound;

        public static GameManager instance { get; private set; }

        public PlayerInput playerInput => m_PlayerInput;

        private Coroutine _chromaticCoroutine;

        private void Awake() {
            instance = this;
        }

        public void TogglePlayerInput(bool enabled) {
            m_PlayerInput.SwitchCurrentActionMap(enabled ? "Player" : "UI");
        }

        public void EnemyGameOver(Transform enemy) {
            m_PlayerInput.enabled = false;
            m_Pointer.transform.position = enemy.transform.position; 
            m_PlayerBehaviour.SetPointer(m_Pointer);
            m_PlayerBehaviour.ToggleLantern(true);
            m_GameOverSound.Play();

            StartCoroutine(GameOverCoroutine());
        }

        public void ChromaticAberration() {
            if (_chromaticCoroutine != null) StopCoroutine(_chromaticCoroutine);
            _chromaticCoroutine = StartCoroutine(ChromaticAberrationCoroutine());
        }

        private IEnumerator GameOverCoroutine() {
            if (_chromaticCoroutine != null) StopCoroutine(_chromaticCoroutine);
            if (!m_Volume.profile.TryGet<ChromaticAberration>(out var chromatic)) yield break;

            chromatic.intensity.Override(1.0f);
            yield return new WaitForSeconds(m_GameOverDelay);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            chromatic.intensity.Override(0.0f);
        }

        private IEnumerator ChromaticAberrationCoroutine() {
            if (!m_Volume.profile.TryGet<ChromaticAberration>(out var chromatic)) yield break;

            m_ChromaticSound.pitch = Random.Range(m_ChromaticSoundPitchRange.x, m_ChromaticSoundPitchRange.y);
            m_ChromaticSound.Play();

            chromatic.intensity.Override(1.0f);
            yield return new WaitForSeconds(m_ChromaticDuration);
            chromatic.intensity.Override(0.0f);
        }
    }
}
