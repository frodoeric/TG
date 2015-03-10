using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class LevelSound : MonoBehaviour {

	public AudioClip levelSound;

	// Use this for initialization
	void Start () {
		audio.PlayOneShot(levelSound, 1.0F);
		audio.loop = true;
	}
	
	/* Update is called once per frame
	void Update () {
	
	}*/
}
