using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

  public static SceneController instance { get; private set; }
  private string[] nonLevelScenes = new string[] { "Core", "AudioCore", "Game", "Game UI" };
  private string[] essentialScenes = new string[] { "Core", "AudioCore" };
  private string[] levelList = new string[] { "ChinaShopElise" };

  // TODO: Active scene is always core for some reason

  void Awake() {
    if (instance != null && instance != this) {
      Destroy(this);
    } else {
      instance = this;
    }
  }

  Scene[] getLoadedSceneList() {
    int countLoaded = SceneManager.sceneCount;
    Scene[] loadedScenes = new Scene[countLoaded];

    for (int i = 0; i < countLoaded; i++) {
      loadedScenes[i] = SceneManager.GetSceneAt(i);
    }
    return loadedScenes;
  }

  string[] getLoadedSceneListNames() {
    int countLoaded = SceneManager.sceneCount;
    string[] loadedScenes = new string[countLoaded];

    for (int i = 0; i < countLoaded; i++) {
      loadedScenes[i] = SceneManager.GetSceneAt(i).name;
    }
    return loadedScenes;
  }

  Scene getLevelScene() {
    Scene[] sceneList = getLoadedSceneList();
    foreach (Scene scene in sceneList) {
      if (!Array.Exists(nonLevelScenes, element => element == scene.name)) {
        return scene;
      }
    }
    // TODO: Return first level scene instead of new scene
    return new Scene();
  }

  void unloadNonEssentialScenes() {
    Scene[] loadedScenes = getLoadedSceneList();
    foreach (Scene scene in loadedScenes) {
      if (!Array.Exists(essentialScenes, element => element == scene.name)) {
        SceneManager.UnloadSceneAsync(scene.name);
      }
    }
  }

  public void reloadLevel() {
    Scene scene = getLevelScene();
    SceneManager.UnloadSceneAsync(scene.name);
    SceneManager.LoadScene(scene.name, LoadSceneMode.Additive);
  }

  public void loadMainMenu() {
    unloadNonEssentialScenes();
    SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
  }

  void loadNonLevelScenes() {
    string[] loadedScenes = getLoadedSceneListNames();
    foreach (string scene in nonLevelScenes) {
      if (!Array.Exists(loadedScenes, element => element == scene)) {
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
      }
    }
  }

  public void loadLevel(int levelIndex) {
    // This is kinda slow. I wonder if we could preload the level scene (which I assume is the slow part but it might not be)
    unloadNonEssentialScenes();
    loadNonLevelScenes();
    SceneManager.LoadScene(levelList[levelIndex], LoadSceneMode.Additive);
  }
}
