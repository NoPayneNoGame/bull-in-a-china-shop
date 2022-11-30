using UnityEditor;
using UnityEditor.SceneManagement;

public class LoadMultiScene : Editor {
  [MenuItem("Tools/Load MainMenu")]
  static void LoadMainMenu() {

    string[] paths = new string[]{
      "Assets/BullsHit/Scenes/Core.unity",
      "Assets/BullsHit/Scenes/AudioCore.unity",
      "Assets/BullsHit/Scenes/Game UI.unity",
      "Assets/BullsHit/Scenes/MainMenu.unity",
    };

    LoadScenes(paths);
  }

  [MenuItem("Tools/Load ChinaShopElise")]
  static void LoadChinaShopScenes() {
    string[] paths = new string[]{
      "Assets/BullsHit/Scenes/Core.unity",
      "Assets/BullsHit/Scenes/AudioCore.unity",
      "Assets/BullsHit/Scenes/Game.unity",
      "Assets/BullsHit/Scenes/Game UI.unity",
      "Assets/BullsHit/Scenes/ChinaShopElise.unity",
    };

    LoadScenes(paths);
  }

  static void LoadScenes(string[] paths) {
    SceneSetup[] scenes = new SceneSetup[paths.Length];
    for (int i = 0; i < paths.Length; i++) {
      scenes[i] = new SceneSetup();
      scenes[i].path = paths[i];
      scenes[i].isActive = false;
      scenes[i].isLoaded = true;
      scenes[i].isSubScene = false;
    }

    scenes[paths.Length - 1].isActive = true;

    EditorSceneManager.RestoreSceneManagerSetup(scenes);
  }
}