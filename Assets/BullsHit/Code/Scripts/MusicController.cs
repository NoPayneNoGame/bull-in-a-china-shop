using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

  private AudioSource music;

  void Awake() {
    music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
  }

  public void updateMusicVolume(float volume) {
    music.volume = volume;
  }
}
