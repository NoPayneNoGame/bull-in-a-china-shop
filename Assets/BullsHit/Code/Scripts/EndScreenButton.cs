using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EndScreenButton : MonoBehaviour {
  private Outline[] outlines;
  private Button button;
  [SerializeField] private bool isDefault = false;

  void Start() {
    outlines = gameObject.GetComponents<Outline>();
    button = gameObject.GetComponent<Button>();
    if (isDefault) {
      button.Select();
    }
  }

  public void onHover() {
    button.Select();
  }

  void Update() {
    // Probably not a good idea to have 3 outlines but I didn't really want to create a shader
    if (EventSystem.current.currentSelectedGameObject == gameObject) {
      foreach (Outline outline in outlines) outline.enabled = true;
    } else {
      foreach (Outline outline in outlines) outline.enabled = false;
    }
  }
}
