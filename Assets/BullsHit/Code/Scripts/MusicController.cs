using UnityEngine;

public class MusicController : MonoBehaviour {

  private AudioSource music;

  void Awake() {
    music = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
  }

  public void updateMusicVolume(float volume) {
    music.volume = volume;
  }

  public void toggleMusic(bool shouldMusicPlay) {
    // This can probably bug out and display opposite somehow but who cares
    music.enabled = !music.enabled;
  }
}
