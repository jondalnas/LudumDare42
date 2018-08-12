using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour {
	void Update () {
		if (!GetComponent<AudioSource>().isPlaying) Destroy(gameObject);
	}
}
