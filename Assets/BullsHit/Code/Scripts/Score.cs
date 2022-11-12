using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour {
  // I like the idea of centralizing the score text on screen and increasing text size as you do more damage

  public TMP_Text scoreText;
  public RawImage trophyLeft;
  public RawImage trophyRight;
  public Texture[] trophyTextures;

  private int score;
  private int maxScore = 160; // Generate this based on all score value scripts in scene?
  private float green = 0.8705883f;
  private float trophyOffset = 20;
  private float scoreWidth;

  enum Trophy {
    bronze,
    silver,
    gold,
    none
  }

  private Trophy lastTrophy = Trophy.none;

  void Start() {
    updateTrophyImages();
    repositionTrophies();
  }

  private void Start () {
    if (scoreText != null) return;
    Canvas canvas = FindObjectOfType<Canvas>();
    scoreText = canvas.transform.Find("Score").GetComponent<TMP_Text>();
  }

  void OnCollisionEnter(Collision hit) {
    if (hit.gameObject.tag == "Destructible") {
      ScoreValue scoreValue;
      // Since OpenFragments inherit the Destructible tag from their parent we will get an error on collision with them if we try to access the ScoreValue script
      if (scoreValue = hit.gameObject.GetComponent<ScoreValue>()) {
        int objectValue = scoreValue.value;
        scoreValue.setValue(0);
        score += objectValue;
      }
      scoreText.text = "$" + score;
      updateTrophyImages();
    }
  }

  void onTrophyChange() {
    increaseFontIntensity();
    repositionTrophies();
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

  Trophy getTrophy() {
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
