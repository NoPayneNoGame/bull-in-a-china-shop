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
  private RectTransform canvasRect;

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
    // Is this problematic? Might break ChinaShop scene if HUD isn't in scene
    canvas = FindObjectOfType<Canvas>();
    canvasRect = canvas.GetComponent<RectTransform>();
    scoreText = canvas.transform.Find("Score Text (TMP)").GetComponent<TMP_Text>();
    //floatingText = canvas.transform.Find("Floating Text (TMP)").GetComponent<TMP_Text>();
    trophyLeft = canvas.transform.Find("TrophyLeft").GetComponent<RawImage>();
    trophyRight = canvas.transform.Find("TrophyRight").GetComponent<RawImage>();
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

  // Convert this to class so I can track position and animate
  void floatingScoreTextCanvas(Vector3 pos, int score) {
    Vector2 viewportPosition = cam.WorldToViewportPoint(pos);
    Vector2 screenPosition = new Vector2(
    ((viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
    ((viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

    GameObject floatingText = Instantiate(floatingTextPrefab, Vector3.zero, Quaternion.identity);
    floatingText.transform.SetParent(canvas.transform);
    RectTransform floatingTextRect = floatingText.GetComponent<RectTransform>();
    floatingTextRect.anchoredPosition = screenPosition;
  }

  public void onFracture(Collider hit, GameObject destructible, Vector3 pos) {
    int scoreIncrease = destructible.GetComponent<ScoreValue>().value;
    score += scoreIncrease;
    scoreText.text = "$" + score;
    updateTrophyImages();
    floatingScoreTextCanvas(destructible.transform.position, scoreIncrease);
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
