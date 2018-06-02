using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playAudioTrack : MonoBehaviour {

	public AudioClip[] audioClip;
	AudioSource audio;//


	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource> (); //AudioSource kompanentine erişim sağladık.
	}
	


	public void playAudioTracks(){
		audio.clip = audioClip [0]; //Dizi içerisinden gelen indise göre değişkene aktarılır.
		audio.Play ();//Oynatma işlemi
	}

	public void playAudioTrack2(){
		audio.clip = audioClip [1]; //Dizi içerisinden gelen indise göre değişkene aktarılır.
		audio.Play ();//Oynatma işlemi
	}

}
