using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour {

  public void levelOver() {
    gameObject.SetActive(true);
  }

  public void levelInProgress() {
    gameObject.SetActive(false);
  }

  void Start() {

  }

  void Update() {

  }
}
