using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class LoadMultiScene : Editor {
  // [MenuItem("Tools/Save current scenes")]
  static void SaveScenes() {
    SceneSetup[] scenes = EditorSceneManager.GetSceneManagerSetup();

    foreach (SceneSetup scene in scenes) {
      Debug.Log(scene.path);
    }
  }

  [MenuItem("Tools/Load china shop")]
  static void LoadChinaShopScenes() {
    // This is dumb but idk how to save these SceneSetups properly

    string[] paths = new string[]{
      "Assets/BullsHit/Scenes/Core.unity",
      "Assets/BullsHit/Scenes/AudioCore.unity",
      "Assets/BullsHit/Scenes/Game.unity",
      "Assets/BullsHit/Scenes/Game UI.unity",
      "Assets/BullsHit/Scenes/ChinaShopElise.unity",
    };

    SceneSetup[] scenes = new SceneSetup[5];
    for (int i = 0; i < paths.Length; i++) {
      scenes[i] = new SceneSetup();
      scenes[i].path = paths[i];
      scenes[i].isActive = false;
      scenes[i].isLoaded = true;
      scenes[i].isSubScene = false;
    }

    scenes[4].isActive = true;

    EditorSceneManager.RestoreSceneManagerSetup(scenes);
  }
}