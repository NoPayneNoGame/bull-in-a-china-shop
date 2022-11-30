using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

  public static SceneController instance { get; private set; }
  private string[] nonLevelScenes = new string[] { "Core", "AudioCore", "Game" };
  private string[] essentialScenes = new string[] { "Core", "AudioCore", "Game UI" };
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
      Scene scene = SceneManager.GetSceneAt(i);
      // Need to check if scene is loaded because of async unloading
      if (scene.isLoaded) {
        loadedScenes[i] = SceneManager.GetSceneAt(i);
      }
    }
    return loadedScenes;
  }

  string[] getLoadedSceneListNames() {
    int countLoaded = SceneManager.sceneCount;
    string[] loadedScenes = new string[countLoaded];

    for (int i = 0; i < countLoaded; i++) {
      Scene scene = SceneManager.GetSceneAt(i);
      if (scene.isLoaded) {
        loadedScenes[i] = scene.name;
      }
    }
    return loadedScenes;
  }

  Scene getLevelScene() {
    Scene[] sceneList = getLoadedSceneList();
    foreach (Scene scene in sceneList) {
      if (Array.Exists(levelList, element => element == scene.name)) {
        return scene;
      }
    }
    // TODO: Return first level scene instead of new scene
    return new Scene();
  }

  int getLevelSceneIndex(Scene scene) {
    return Array.IndexOf(levelList, scene.name);
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
    int sceneIndex = getLevelSceneIndex(scene);
    loadLevel(sceneIndex);
    // SceneManager.UnloadSceneAsync("Game");
    // SceneManager.UnloadSceneAsync(scene.name);
    // SceneManager.LoadScene("Game", LoadSceneMode.Additive);
    // SceneManager.LoadScene(scene.name, LoadSceneMode.Additive);
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

  void movePlayerToSpawn() {
    GameObject respawnPoint = GameObject.FindGameObjectWithTag("Respawn");
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    player.transform.position = respawnPoint.transform.position;
  }

  public void loadLevel(int levelIndex) {
    SceneManager.sceneLoaded += setupScoring;

    // This is kinda slow. I wonder if we could preload the level scene (which I assume is the slow part but it might not be)
    unloadNonEssentialScenes();
    loadNonLevelScenes();
    SceneManager.LoadScene(levelList[levelIndex], LoadSceneMode.Additive);
    enableHud();
    // TODO: Load scoring system if it isn't working (it probably won't be)
  }

  void setupScoring(Scene scene, LoadSceneMode lsm) {
    if (!levelList.Contains(scene.name)) return;

    Debug.Log(scene.name + " has loaded.");

    GameObject scoreGo = GameObject.Find("ScoreHandler");
    if (scoreGo == null) {
      Debug.LogError("Cannot find ScoreHandler");
      return;
    }

    Score score = scoreGo.GetComponent<Score>();
    score.cam = Camera.main;

    GameObject[] gos = GameObject.FindGameObjectsWithTag("Destructible");
    Debug.Log("Found " + gos.Length + " destructibles.");

    foreach (GameObject obj in gos) {
      Fracture frac = obj.GetComponent<Fracture>();
      ScoreValue sv = obj.GetComponent<ScoreValue>();
      if (frac == null || sv == null) continue;
      frac.callbackOptions.onFracture.AddListener(score.onFracture);
    }

    score.loadScoring();
  }

  void enableHud() {
    GameObject ui = GameObject.FindGameObjectWithTag("UI");
    GameObject HUD = ui.transform.Find("HUD").gameObject;
    HUD.SetActive(true);
  }
}
