using UnityEngine;

// TODO: This class :)
public class MenuClickHandler : MonoBehaviour {
  private GameObject options;

  void Awake() {
    GameObject ui = GameObject.FindGameObjectWithTag("UI");
    options = ui.transform.Find("Options").gameObject;
  }

  public void Play() {
    if (menuOpen()) return;
    Debug.Log("Play");
    SceneController.instance.loadLevel(0);
  }

  public void Credits() {
    if (menuOpen()) return;
    Debug.Log("Credits");
  }

  public void Options() {
    if (menuOpen()) return;
    Debug.Log("Options");
    options.SetActive(true);
  }

  public void Exit() {
    if (menuOpen()) return;
    Debug.Log("Exit");
  }

  bool menuOpen() => options.activeSelf;
}
