using UnityEngine;

public class MenuClickHandler : MonoBehaviour {
  private GameObject options;

  void Start() {
    GameObject ui = GameObject.FindGameObjectWithTag("UI");
    options = ui.transform.Find("Options").gameObject;
  }

  void startGame() {
    SceneController.instance.loadLevel(0);
  }

  void exitGame() {
    #if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
    #endif

    Application.Quit();
  }

  public void Play() {
    if (menuOpen()) return;
    Debug.Log("Play");
    startGame();
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
    exitGame();
  }

  bool menuOpen() => options.activeSelf;
}
