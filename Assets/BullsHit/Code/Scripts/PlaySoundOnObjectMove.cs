using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnObjectMove : MonoBehaviour {
  [SerializeField] private string soundObjectName;
  private Vector3 lastPosition;
  GameObject sound;
  GameObject newSoundObject;
  AudioSource newSound;
  Rigidbody rb;
  bool isGrounded;

  void Start() {
    sound = GameObject.FindGameObjectWithTag("SFX").transform.Find(soundObjectName).gameObject;
    lastPosition = transform.position;
    rb = gameObject.GetComponent<Rigidbody>();
    InvokeRepeating("playSoundIfMoved", 1.0f, 0.5f);
  }

  bool checkIfMoved() {
    //Debug.Log(Vector3.SqrMagnitude(transform.position - lastPosition));
    if (Vector3.SqrMagnitude(transform.position - lastPosition) < 0.2) {
      return false;
    }
    return true;
  }

  void OnCollisionExit(Collision hit) {
    if (hit.gameObject.tag == "Floor") isGrounded = false;
  }

  void OnCollisionEnter(Collision hit) {
    if (hit.gameObject.tag == "Floor") isGrounded = true;
  }

  void playSoundIfMoved() {
    // TODO: Consider raising volume if velocity is higher
    if (checkIfMoved() && isGrounded) { // == and != are weird for vector comparisons
      //Debug.Log("Moving" + transform.position.ToString("f8") + lastPosition.ToString("f8"));
      if (newSoundObject == null) {
        newSoundObject = Instantiate(sound);
        newSound = newSoundObject.GetComponent<AudioSource>();
        newSound.Play();
      }
    } else {
      if (newSoundObject != null) {
        newSound.Stop();
        newSoundObject.SetActive(false);
        Destroy(newSoundObject);
      }
    }
    lastPosition = transform.position;
  }
}
