using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour {
  // I like the idea of centralizing the score text on screen and increasing text size as you do more damage

  private TMP_Text scoreText;
  private RawImage trophyLeft;
  private RawImage trophyRight;
  public Texture[] trophyTextures;
  [SerializeField] private GameObject floatingTextPrefab;
  private Camera cam;
  private Canvas canvas;
  private EndScreen endScreenScript;
  private RectTransform canvasRect;

  private int score;
  private int maxScore = 160;
  private float green = 0.8705883f;
  private float trophyOffset = 20;
  private float scoreWidth;

  public enum Trophy {
    bronze,
    silver,
    gold,
    none
  }

  private Trophy lastTrophy = Trophy.none;

  void Start() {
    // Is this problematic? Might break ChinaShop scene if HUD isn't in scene
    canvas = FindObjectOfType<Canvas>();
    scoreText = canvas.transform.Find("HUD").Find("Score Text (TMP)").GetComponent<TMP_Text>();
    //floatingText = canvas.transform.Find("Floating Text (TMP)").GetComponent<TMP_Text>();
    trophyLeft = canvas.transform.Find("HUD").Find("TrophyLeft").GetComponent<RawImage>();
    trophyRight = canvas.transform.Find("HUD").Find("TrophyRight").GetComponent<RawImage>();
    endScreenScript = canvas.transform.Find("EndGame").GetComponent<EndScreen>();
    cam = Camera.main;
    maxScore = getMaxScore();
    updateTrophyImages();
    repositionTrophies();
  }

  int getMaxScore() {
    int total = 0;
    GameObject[] destructibles = GameObject.FindGameObjectsWithTag("Destructible");
    foreach (GameObject destructible in destructibles) {
      // Probably want error checking here
      total += destructible.GetComponent<ScoreValue>().value;
    }
    return total;
  }

  // Code for if Score.cs is attached to Player object
  // void OnCollisionEnter(Collision hit) {
  //   if (hit.gameObject.tag == "Destructible") {
  //     ScoreValue scoreValue;
  //     // Since OpenFragments inherit the Destructible tag from their parent we will get an error on collision with them if we try to access the ScoreValue script
  //     if (scoreValue = hit.gameObject.GetComponent<ScoreValue>()) {
  //       int objectValue = scoreValue.value;
  //       scoreValue.setValue(0);
  //       score += objectValue;
  //     }
  //     scoreText.text = "$" + score;
  //     updateTrophyImages();
  //   }
  // }

  void createFloatingText(Vector3 pos, string text) {
    GameObject floatingTextObject = Instantiate(floatingTextPrefab, Vector3.zero, Quaternion.identity);
    FloatingText floatingTextScript = floatingTextObject.GetComponent<FloatingText>();
    floatingTextScript.constructor(canvas, cam, floatingTextObject, pos, text);
  }

  public int getScore() {
    return score;
  }

  public void onFracture(Collider hit, GameObject destructible, Vector3 pos) {
    ScoreValue scoreValueScript = destructible.GetComponent<ScoreValue>();
    int scoreIncrease = scoreValueScript.value;
    scoreValueScript.setValue(0); // Set value to 0 after grabbing the value to prevent duplicate scores
    score += scoreIncrease;
    scoreText.text = "$" + score;
    updateTrophyImages();
    createFloatingText(destructible.transform.position, "$" + scoreIncrease);
  }

  void onTrophyChange() {
    increaseFontIntensity();
    repositionTrophies();
    if (getTrophy() == Trophy.gold) {
      // End game
      endScreenScript.levelOver();
    }
  }

  void repositionTrophies() {
    scoreWidth = scoreText.GetComponent<RectTransform>().sizeDelta.x;
    //trophyOffset += offset;
    Vector3 leftPos = trophyLeft.GetComponent<RectTransform>().localPosition;
    Vector3 rightPos = trophyRight.GetComponent<RectTransform>().localPosition;
    trophyLeft.GetComponent<RectTransform>().localPosition = new Vector3((trophyOffset + scoreWidth / 2) * -1, leftPos.y, leftPos.z);
    trophyRight.GetComponent<RectTransform>().localPosition = new Vector3((trophyOffset + scoreWidth / 2), rightPos.y, rightPos.z);
  }

  void increaseFontIntensity() {
    // TODO: Move to its own script, this is also used in Timer.cs
    green -= 0.1f;
    scoreText.color = new Color(1f, green, 0f);
    scoreText.fontSize += 6;
  }

  public Trophy getTrophy() {
    // Not sure how we decide this. Just 1/3 of max, 2/3 max, 100%? (95%? so you can miss a couple and not be punished)
    float[] trophyScores = new float[3] { maxScore / 3, maxScore / 3 * 2, maxScore };
    if (score < trophyScores[0]) {
      return Trophy.none;
    }
    if (score < trophyScores[1]) {
      return Trophy.bronze;
    }
    if (score < trophyScores[2]) {
      return Trophy.silver;
    }
    return Trophy.gold;
  }

  public Texture getTrophyTexture() {
    return trophyTextures[(int)getTrophy()];
  }

  void updateTrophyImages() {

    Trophy trophyIndex = getTrophy();
    if (lastTrophy != trophyIndex) {
      lastTrophy = trophyIndex;
      onTrophyChange();
    }
    if (trophyIndex == Trophy.none) {
      trophyLeft.enabled = false;
      trophyRight.enabled = false;
    } else {
      trophyLeft.enabled = true;
      trophyRight.enabled = true;
      trophyLeft.texture = trophyTextures[(int)trophyIndex];
      trophyRight.texture = trophyTextures[(int)trophyIndex];
    }
  }
}
