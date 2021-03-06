﻿using AmbientSounds.Models;
using Microsoft.Toolkit.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmbientSounds.Services
{
    /// <summary>
    /// Class for performing sound mix operations.
    /// </summary>
    public class SoundMixService : ISoundMixService
    {
        private readonly ISoundDataProvider _soundDataProvider;
        private readonly IMixMediaPlayerService _player;
        private readonly string[] _namePlaceholders = new string[] { "🎵", "🎼", "🎧", "🎶" };

        public SoundMixService(
            ISoundDataProvider soundDataProvider,
            IMixMediaPlayerService player)
        {
            Guard.IsNotNull(soundDataProvider, nameof(soundDataProvider));
            Guard.IsNotNull(player, nameof(player));

            _soundDataProvider = soundDataProvider;
            _player = player;
        }

        /// <inheritdoc/>
        public async Task<string> SaveMixAsync(IList<Sound> sounds, string name = "")
        {
            if (sounds == null || sounds.Count <= 1)
            {
                return "";
            }

            var mix = new Sound()
            {
                Id = Guid.NewGuid().ToString(),
                IsMix = true,
                Name = string.IsNullOrWhiteSpace(name) ? RandomName() : name.Trim(),
                SoundIds = sounds.Select(x => x.Id).ToArray(),
                ImagePaths = sounds.Select(x => x.ImagePath).ToArray()
            };

            await _soundDataProvider.AddLocalSoundAsync(mix);
            return mix.Id;
        }

        /// <inheritdoc/>
        public async Task<bool> LoadMixAsync(Sound mix)
        {
            if (mix?.SoundIds == null || !mix.IsMix) return false;

            // save instance of id
            // since RemoveAll will reset the id.
            var previousMixId = _player.CurrentMixId;
            _player.RemoveAll();

            // if the mix we're trying to play was
            // the same as the previous, return now
            // since we don't want to play it again.
            if (!string.IsNullOrWhiteSpace(previousMixId) && 
                previousMixId == mix.Id)
            {
                return false;
            }

            var sounds = await _soundDataProvider.GetSoundsAsync(soundIds: mix.SoundIds);
            if (sounds != null && sounds.Count == mix.SoundIds.Length)
            {
                foreach (var s in sounds)
                {
                    await _player.ToggleSoundAsync(s, parentMixId: mix.Id);
                }

                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public async Task ReconstructMixesAsync(IList<Sound> dehydratedMixes)
        {
            if (dehydratedMixes == null || dehydratedMixes.Count == 0)
            {
                return;
            }

            var allSounds = await _soundDataProvider.GetSoundsAsync();
            var allSoundIds = allSounds.Select(x => x.Id);

            foreach (var soundMix in dehydratedMixes)
            {
                if (allSoundIds.Contains(soundMix.Id) || 
                    soundMix.SoundIds == null || 
                    soundMix.SoundIds.Length == 0)
                {
                    continue;
                }

                var soundsForThisMix = allSounds.Where(x => soundMix.SoundIds.Contains(x.Id)).ToList();

                var hydratedMix = new Sound()
                {
                    Id = soundMix.Id,
                    Name = soundMix.Name,
                    IsMix = true,
                    SoundIds = soundsForThisMix.Select(x => x.Id).ToArray(),
                    ImagePaths = soundsForThisMix.Select(x => x.ImagePath).ToArray()
                };

                await _soundDataProvider.AddLocalSoundAsync(hydratedMix);
            }
        }

        private string RandomName()
        {
            var rand = new Random();
            var result = rand.Next(4);
            return _namePlaceholders[result];
        }
    }
}
