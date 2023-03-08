using UnityEngine;
using System.Collections.Generic;

public class Snd : MonoBehaviour {

    public Dictionary<string, AudioClip> audioClips;

    Main main;
    GameObject snd;
    AudioSource audioSource;


    public void Init(Main inMain) {

        main        = inMain;
        snd         = new GameObject("Sound");
        audioSource = snd.AddComponent<AudioSource>();

		audioClips = new Dictionary<string, AudioClip>();
		
		AddAudioClip("Gun", "Audio/Gun");
		AddAudioClip("Music", "Audio/Music");
		AddAudioClip("Jump", "Audio/Jump");
		AddAudioClip("Land", "Audio/Land");

	}


    public void PlayAudioClip(string inVer, bool _loop) {

        audioSource.PlayOneShot(audioClips[inVer]);
        audioSource.loop = _loop;

    }


    public void AddAudioClip(string inId, string inAddress) {

        AudioClip tClip = Resources.Load<AudioClip>(inAddress);
        audioClips.Add(inId, tClip);
        tClip.LoadAudioData();

    }


}
