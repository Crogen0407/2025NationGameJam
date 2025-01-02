using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

public enum SoundType
{
	None,
	SFX,
	BGM,
}

[Serializable]
public class SoundData
{
	public string name;
	public SoundType type;
	public AudioClip clip;
}

[CreateAssetMenu(fileName = "SoundDataSO", menuName = "SO/SoundDataSO")]
public class SoundDataSO : ScriptableObject
{
	public List<SoundData> soundDataList;
	
	private void OnValidate()
	{
		PairInit();
	}

	public void PairInit()
	{
		if (soundDataList == null) return;
		foreach (var soundData in soundDataList)
		{
			if (soundData.clip == null)
				return;

			if (soundData.name.Equals(string.Empty) && soundData.clip != null)
			{
				soundData.name = soundData.clip.name;
			}
		}
	}
}
