using UnityEngine;
using System.Collections;

// サウンド管理
public class SoundManager : MonoBehaviour {

	public GameObject[] soundEffectsClip;

    // pitch固定で鳴らす
	public void Play(int id) {

		if(id>=0 && id<soundEffectsClip.Length && !soundEffectsClip[id].GetComponent<AudioSource>().isPlaying){
            this.soundEffectsClip[id].GetComponent<AudioSource>().pitch = 1.0f;
            this.soundEffectsClip[id].GetComponent<AudioSource>().Play();
		}
	}

    // pitch指定で鳴らす
    public void Play(int id, float pitch)
    {
        if (id >= 0 && id < soundEffectsClip.Length && !soundEffectsClip[id].GetComponent<AudioSource>().isPlaying)
        {
            this.soundEffectsClip[id].GetComponent<AudioSource>().pitch = pitch;
            this.soundEffectsClip[id].GetComponent<AudioSource>().Play();
        }
    }

    public void Stop(int id) {
		this.soundEffectsClip[id].GetComponent<AudioSource>().Stop();
	}
}
