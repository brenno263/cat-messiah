using System;
using UnityEngine;

namespace _General._Scripts.Sound
{
	[Serializable]
	public class Sound
	{
		public AudioClip clip;

		public SoundFx type;

		[Range(0f,1f)]
		public float volume;
		[Range(0.1f, 3f)]
		public float pitch;

		[HideInInspector]
		public AudioSource source;
	}
}
