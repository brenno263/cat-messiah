using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _General._Scripts.Sound
{
	public enum SoundFx
	{
		SmashDoor,
		BuildDoor,
		Extinguish,
		Cat,
		Walk
	}

	public class PlayerAudioManager : MonoBehaviour
	{
		#region variables

		[Header("Set in Inspector")]
		public List<Sound> sounds;

		public List<Song> songs;

		public Sound walkSound;

		public int currentSongNdx;

		public float lookaheadTime;

		[Range(0f, 1f)]
		public float musicVolume;

		private AudioSource songSource1;
		private AudioSource songSource2;
		private bool playingSource1;

		public bool WalkLoopPlaying
		{
			get => _walkLoopPlaying;
			set
			{
				if (_walkLoopPlaying != value)
				{
					if (value) StartWalkLoop();
					else StopWalkLoop();
				}

				_walkLoopPlaying = value;
			}
		}

		private bool _walkLoopPlaying;

		#endregion

		#region monobehavior methods

		void Awake()
		{
			foreach (Sound sound in sounds) { InitSound(sound); }

			InitSound(walkSound);
			walkSound.source.loop = true;

			songSource1 = gameObject.AddComponent<AudioSource>();
			songSource2 = gameObject.AddComponent<AudioSource>();
			songSource1.volume = musicVolume;
			songSource2.volume = musicVolume;
			playingSource1 = false;
		}

		private void Start()
		{
			StartCoroutine(StartMusic());
		}

		#endregion

		public void Play(SoundFx fx)
		{
			Sound[] effects = sounds.Where(s => s.type == fx).ToArray();
			effects[Random.Range(0, effects.Length)].source.Play();
		}

		private void StartWalkLoop()
		{
			walkSound.source.Play();
		}

		private void StopWalkLoop()
		{
			walkSound.source.Pause();
		}

		#region private methods

		private void InitSound(Sound sound)
		{
			sound.source = gameObject.AddComponent<AudioSource>();
			sound.source.clip = sound.clip;
			sound.source.volume = sound.volume;
			sound.source.pitch = sound.pitch;
		}

		private IEnumerator StartMusic()
		{
			int oldSongNdx = currentSongNdx;
			Song current = songs[oldSongNdx];
			while (true)
			{
				PlaySong(current.song);
				yield return new WaitForSeconds(current.songLength - lookaheadTime);
				UpdateCurrentSong();
				if (currentSongNdx != oldSongNdx)
				{
					oldSongNdx = currentSongNdx;
					current = songs[oldSongNdx];
				}
			}
		}

		private void PlaySong(AudioClip clip)
		{
			if (playingSource1)
			{
				songSource2.clip = clip;
				songSource2.PlayDelayed(lookaheadTime);
				playingSource1 = false;
			}
			else
			{
				songSource1.clip = clip;
				songSource1.PlayDelayed(lookaheadTime);
				playingSource1 = true;
			}
		}

		private void UpdateCurrentSong()
		{
			Building.Building building = Building.Building.Singleton;
			float firePercent = 1.0f * building.netFireLevel / building.numRooms;

			if (firePercent > 0.0f) { currentSongNdx = 2; }
			else if (firePercent > 0.35f) { currentSongNdx = 1;}
			else currentSongNdx = 0;
		}

		#endregion
	}
}