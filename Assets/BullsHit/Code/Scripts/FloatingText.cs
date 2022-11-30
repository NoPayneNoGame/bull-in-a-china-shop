using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour {

  private Vector3 spawn;
  private Vector2 position;
  private string text;
  private GameObject floatingTextObject;
  private RectTransform floatingTextRect;
  private TMP_Text floatingTextTMP;
  private Camera cam;
  private Canvas canvas;
  private RectTransform canvasRect;
  private Color floatingTextColor;
  private float reductionInAlphaPerSecond = 0.5f;
  private float textOffset = 0;
  private float textOffsetInterval = 20f;

  public void constructor(Canvas canvas, Camera cam, GameObject floatingTextObject, Vector3 spawn, string text) {
    this.text = text;
    this.spawn = spawn;
    this.floatingTextObject = floatingTextObject;
    this.cam = cam;
    this.canvas = canvas;
  }

  Vector2 applyTextOffset(Vector2 pos) {
    textOffset -= textOffsetInterval * Time.deltaTime;
    return new Vector2(pos.x, pos.y - textOffset);
  }

  void fadeText() {
    floatingTextColor = new Color(floatingTextColor.r, floatingTextColor.g, floatingTextColor.b, floatingTextColor.a - reductionInAlphaPerSecond * Time.deltaTime);
    floatingTextTMP.color = floatingTextColor;
  }

  void animateText() {
    positionText();
    fadeText();
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
    floatingTextTMP = floatingTextObject.GetComponent<TMP_Text>();
    floatingTextTMP.text = text;
    floatingTextColor = floatingTextTMP.color;
    positionText();
  }

  void positionText() {
    floatingTextRect.anchoredPosition = applyTextOffset(getCanvasPositionFromWorldPosition(spawn));
  }

  void Start() {
    canvasRect = canvas.GetComponent<RectTransform>();
    instantiateText();
  }

  void Update() {
    if (floatingTextObject == null) return;
    if (floatingTextColor.a <= 0) Destroy(floatingTextObject);
    if (this.cam != null) animateText();
  }
}
