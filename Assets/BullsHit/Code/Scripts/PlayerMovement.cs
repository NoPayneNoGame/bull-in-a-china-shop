using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour {

  public Rigidbody rb;
  public float moveSpeed = 20;
  public float maxSpeed = 30;
  public float drag = 0.98f;
  public float rotationSpeed = 10;
  public float bonkThreshold = 500;
  public float bonkPower = 10; // The max distance (in metres?) between player and bonkable object where the object will fall
  public float jumpHeight = 10;

  [Header("Camera Things")]
  public CinemachineVirtualCamera vcam;
  public float minCameraDistance = 18;
  public float maxCameraDistance = 40;
  public float zoomOutSpeed = .5f;
  public float zoomInSpeed = 3;

  private float horizontal;
  private Vector3 moveForce;
  private bool canMove = true;
  private bool canJump = true;
  private bool isJumping = false;

  private CinemachineTransposer transposer;

  void enableMovement() {
    canMove = true;
    // This is a temp fix for the player getting stuck in a loop after hitting a wall and then not having a vertical input / change in rotation
    moveForce = new Vector3(0, 0, 0);
  }

  void unfreezeBonkable(GameObject bonkable) {
    Rigidbody bonkrb = bonkable.GetComponent<Rigidbody>();
    bonkrb.constraints = RigidbodyConstraints.None;
  }

  void checkBonkables(Vector3 position) {
    GameObject[] bonkables = GameObject.FindGameObjectsWithTag("Bonkable");
    foreach (GameObject bonkable in bonkables) {
      if (Vector3.Distance(bonkable.transform.position, position) < bonkPower) {
        unfreezeBonkable(bonkable);
      }
    }
  }

  void bonk(Collision hit) {
    canMove = false;
    Invoke("enableMovement", 1.0f);
    moveForce = Vector3.Reflect(moveForce.normalized, hit.contacts[0].normal) * moveForce.magnitude;
    checkBonkables(rb.position);
  }

  void jump() {

  }

  void OnCollisionEnter(Collision hit) {
    if (hit.gameObject.tag == "Enemy" || (hit.gameObject.tag == "Wall" && moveForce.sqrMagnitude > bonkThreshold)) {
      bonk(hit);
    }
  }

  void Start() {
    transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
  }

  void FixedUpdate() {
    if (horizontal != 0) {
      // Remove moveForce.magnitude if we want rotation while not moving
      rb.transform.Rotate(Vector3.up * horizontal * rotationSpeed * moveForce.magnitude * Time.deltaTime);
    }
  }

  void Update() {
    if (canMove) {
      horizontal = Input.GetAxisRaw("Horizontal");
      float vertical = Input.GetAxisRaw("Vertical");

      if (Input.GetButtonDown("Jump")) { // Jump
        jump();
      }

      // if (Input.GetKeyDown("Shift")) { // Dash

      // }

      moveForce += rb.transform.forward * moveSpeed * Time.deltaTime * vertical * drag;
      if (isJumping) moveForce += rb.transform.up * jumpHeight * Time.deltaTime;

      rb.velocity = Vector3.ClampMagnitude(moveForce, maxSpeed);
    } else {
      rb.velocity = moveForce / 2;
    }

    if (moveForce.magnitude > ((maxSpeed - moveSpeed) / 2)) {
      transposer.m_FollowOffset.y = Mathf.Lerp(transposer.m_FollowOffset.y, maxCameraDistance, zoomOutSpeed * Time.deltaTime);
    } else {
      transposer.m_FollowOffset.y = Mathf.Lerp(transposer.m_FollowOffset.y, minCameraDistance, zoomInSpeed * Time.deltaTime);
    }
  }
}
