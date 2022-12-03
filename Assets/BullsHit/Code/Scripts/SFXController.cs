using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : MonoBehaviour {

  private GameObject sounds;
  private AudioSource[] audioSources;
  private float[] initialVolumes;

  void Start() {
    sounds = GameObject.FindGameObjectWithTag("SFX");
    audioSources = sounds.GetComponentsInChildren<AudioSource>();
    initialVolumes = getInitialVolumes();
  }

  private float[] getInitialVolumes() {
    float[] volumes = new float[audioSources.Length];
    for (int i = 0; i < audioSources.Length; i++) {
      volumes[i] = audioSources[i].volume;
    }
    return volumes;
  }

  public void adjustVolumeOfAllSounds(float volumePercentage) {
    for (int i = 0; i < audioSources.Length; i++) {
      audioSources[i].volume = volumePercentage * initialVolumes[i];
    }
  }

  public void toggleSounds(bool shouldSoundsPlay) {
    for (int i = 0; i < audioSources.Length; i++) {
      audioSources[i].mute = !audioSources[i].mute;
    }
  }
}
