using System;
using System.Collections;
using UnityEngine;
using Crogen.CrogenPooling;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public class SoundPlayer : MonoBehaviour, IPoolingObject
{
	[field:SerializeField] public AudioSource AudioSource { get; private set; }
	[field:SerializeField] public AudioMixer AudioMixer { get; private set; }
	
	public string OriginPoolType { get; set; }
	GameObject IPoolingObject.gameObject { get; set; }

	public void SetAudioResource(AudioClip audioResource, bool loop = false, float pitch = 1f)
	{
		AudioSource.clip = audioResource;
		AudioSource.Play();
		AudioSource.outputAudioMixerGroup = AudioMixer.FindMatchingGroups($"Master/{(loop ? "BGM" : "Effect")}")[0];
		if(loop == false)
			StartCoroutine(CoroutineOnPlay());
		
		AudioSource.pitch = pitch;
	}

	private void Awake()
	{
	}

	public void OnPop()
	{
	}

	public void OnPush()
	{
		StopAllCoroutines();
	}

	private IEnumerator CoroutineOnPlay()
	{
		yield return new WaitWhile(()=>AudioSource.isPlaying);
		this.Push();
	}
}
