using System;
using System.Collections;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DarkHavoc
{
    public class TransitionManager : Service<TransitionManager>
    {
        protected override bool DonDestroyOnLoad => true;

        private static readonly int Out = Animator.StringToHash("Out");
        private static readonly int In = Animator.StringToHash("In");
        private static readonly int Show = Animator.StringToHash("Show");
        private static readonly int Hide = Animator.StringToHash("Hide");

        private bool _transitionInProgress;
        private Animator _animator;

        protected override void Awake()
        {
            base.Awake();
            _animator = GetComponent<Animator>();
        }

        public void LoadLobbyScene() => StartCoroutine(LoadLobbySceneAsync());
        private IEnumerator LoadLobbySceneAsync()
        {
            // ResetTriggers();
            yield return SetTransitionPanel(true);
            yield return SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
            yield return SceneManager.LoadSceneAsync("Lobby", LoadSceneMode.Additive);
            yield return SetTransitionPanel(false);
            // ResetTriggers();
        }

        public void LoadBiomeScene(Biome biome) => StartCoroutine(LoadBiomeSceneAsync(biome));

        private IEnumerator LoadBiomeSceneAsync(Biome biome)
        {
            // ResetTriggers();
            yield return SetTransitionPanel(true);
            yield return SceneManager.LoadSceneAsync("HUD", LoadSceneMode.Single);
            yield return SceneManager.LoadSceneAsync(biome.ToString(), LoadSceneMode.Additive);
            yield return SetTransitionPanel(false);
            // ResetTriggers();
        }
        
        public IEnumerator SetTransitionPanel(bool state)
        {
            yield return new WaitUntil(() => _transitionInProgress != state);
            ResetTriggers();
            yield return null;
            _animator.SetTrigger(state ? In : Out);
            yield return new WaitForSeconds(1f);
            _transitionInProgress = state;
        }

        public IEnumerator SetMenuPanel(bool state, Action callback = null)
        {
            yield return new WaitUntil(() => _transitionInProgress != state);
            ResetTriggers();
            yield return null;
            _animator.SetTrigger(state ? Show : Hide);
            yield return new WaitForSeconds(.25f);
            callback?.Invoke();
            _transitionInProgress = state;
        }

        private void ResetTriggers()
        {
            _animator.ResetTrigger(Out);
            _animator.ResetTrigger(In);
            _animator.ResetTrigger(Show);
            _animator.ResetTrigger(Hide);
        }
    }
}