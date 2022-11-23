using UnityEngine;

// TODO: This class :)
public class MenuClickHandler : MonoBehaviour {
  public void Play() {
    Debug.Log("Play");
    SceneController.instance.loadLevel(0);
  }

  public void Credits() {
    Debug.Log("Credits");
  }

  public void Options() {
    Debug.Log("Options");
  }

  public void Exit() {
    Debug.Log("Exit");
  }
}
