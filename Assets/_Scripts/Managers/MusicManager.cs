using System;
using System.Collections.Generic;
using DarkHavoc.ServiceLocatorComponents;
using UnityEngine;
using UnityEngine.Serialization;

namespace DarkHavoc.Managers
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : Service<MusicManager>
    {
        protected override bool DonDestroyOnLoad => true;

        [SerializeField] private AudioClip lobby;
        [SerializeField] private AudioClip forgottenCatacombs;
        [SerializeField] private AudioClip forgottenCatacombsBoss;
        [SerializeField] private AudioClip corruptedAbyss;
        [SerializeField] private AudioClip corruptedAbyssBoss;
        [SerializeField] private AudioClip theInfectionVessel;
        [SerializeField] private AudioClip theInfectionVesselBoss;

        private AudioSource _audioSource;
        private Dictionary<string, AudioClip> _audioClips;

        protected override void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
            _audioSource.loop = true;

            _audioClips = new Dictionary<string, AudioClip>()
            {
                { "MainMenu", lobby },
                { "ForgottenCatacombs", forgottenCatacombs },
                { "ForgottenCatacombsBoss", forgottenCatacombsBoss },
                { "CorruptedAbyss", corruptedAbyss },
                { "CorruptedAbyssBoss", corruptedAbyssBoss },
                { "TheInfectionVessel", theInfectionVessel },
                { "TheInfectionVesselBoss", theInfectionVesselBoss },
            };
        }

        private void Start()
        {
            TransitionManager.OnMainMenu += TransitionManagerOnMainMenu;
            TransitionManager.OnBiomeLoaded += TransitionManagerOnOnBiomeLoaded;
            TransitionManager.OnBossLoaded += TransitionManagerOnBossLoaded;
        }

        private void TransitionManagerOnMainMenu()
        {
            PlayMusic(lobby);
        }

        private void TransitionManagerOnOnBiomeLoaded(Biome biome)
        {
            if (!_audioClips.TryGetValue(biome.ToString(), out var clip))
                return;
            PlayMusic(clip);
        }

        private void TransitionManagerOnBossLoaded(Biome biome)
        {
            if (!_audioClips.TryGetValue(biome + "Boss", out var clip))
                return;
            PlayMusic(clip);
        }

        private void PlayMusic(AudioClip audioClip)
        {
            _audioSource.Stop();
            _audioSource.clip = audioClip;
            _audioSource.Play();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            TransitionManager.OnMainMenu -= TransitionManagerOnMainMenu;
            TransitionManager.OnBiomeLoaded -= TransitionManagerOnOnBiomeLoaded;
            TransitionManager.OnBossLoaded -= TransitionManagerOnBossLoaded;
        }
    }
}