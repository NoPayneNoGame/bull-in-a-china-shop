using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour {

  //private GameObject floatingTextPrefab;
  private Vector3 spawn;
  private Vector2 position;
  private string text;
  private GameObject floatingTextObject;
  private RectTransform floatingTextRect;
  private Camera cam;
  private Canvas canvas;
  private RectTransform canvasRect;

  public void constructor(Canvas canvas, Camera cam, GameObject floatingTextObject, Vector3 spawn, string text) {
    this.text = text;
    this.spawn = spawn;
    this.floatingTextObject = floatingTextObject;
    this.cam = cam;
    this.canvas = canvas;
  }

  Vector2 getCanvasPositionFromWorldPosition(Vector3 pos) {
    Vector2 viewportPosition = cam.WorldToViewportPoint(pos);
    return new Vector2(
    ((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
    ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));
  }

  void instantiateText() {
    Vector2 screenPosition = getCanvasPositionFromWorldPosition(spawn);
    floatingTextObject.transform.SetParent(canvas.transform);
    floatingTextRect = floatingTextObject.GetComponent<RectTransform>();
    floatingTextObject.GetComponent<TMP_Text>().text = text;
    positionText();
  }

  void positionText() {
    floatingTextRect.anchoredPosition = getCanvasPositionFromWorldPosition(spawn);
  }

  void Start() {
    canvasRect = canvas.GetComponent<RectTransform>();
    instantiateText();
  }

  void Update() {
    if (floatingTextObject == null) return;
    positionText();
  }
}
