using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

  public static SceneController instance { get; private set; }
  private string[] nonLevelScenes = new string[] { "Core", "AudioCore", "Game", "Game UI" };

  void Awake() {
    if (instance != null && instance != this) {
      Destroy(this);
    } else {
      instance = this;
    }
  }

  Scene[] getSceneList() {
    int countLoaded = SceneManager.sceneCount;
    Scene[] loadedScenes = new Scene[countLoaded];

    for (int i = 0; i < countLoaded; i++) {
      loadedScenes[i] = SceneManager.GetSceneAt(i);
    }
    return loadedScenes;
  }

  Scene getLevelScene() {
    Scene[] sceneList = getSceneList();
    foreach (Scene scene in sceneList) {
      if (!Array.Exists(nonLevelScenes, element => element == scene.name)) {
        return scene;
      }
    }
    // TODO: Return first level scene instead of new scene
    return new Scene();
  }

  public void reloadLevel() {
    Scene scene = getLevelScene();
    SceneManager.UnloadSceneAsync(scene.name);
    SceneManager.LoadScene(scene.name, LoadSceneMode.Additive);
    // SceneManager.LoadScene("Core");
    // SceneManager.LoadScene("AudioCore", LoadSceneMode.Additive);
    // SceneManager.LoadScene("Game", LoadSceneMode.Additive);
    // SceneManager.LoadScene("Game UI", LoadSceneMode.Additive);
    // SceneManager.LoadScene("ChinaShop", LoadSceneMode.Additive);
  }
}
