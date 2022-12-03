using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomSound : MonoBehaviour {

  // TODO: This probably doesn't need to be attached to every single piece of china

  private GameObject sounds;
  private GameObject newSound;

  void Start() {
    sounds = GameObject.FindGameObjectWithTag("SFX");
  }

  public void play() {
    int randomChildId = UnityEngine.Random.Range(0, sounds.transform.childCount);
    GameObject randomChild = sounds.transform.GetChild(randomChildId).gameObject;
    newSound = Instantiate(randomChild);
    newSound.GetComponent<AudioSource>().Play();
    Invoke("destroyNewSound", 3);
  }

  void destroyNewSound() {
    Destroy(newSound);
  }
}
