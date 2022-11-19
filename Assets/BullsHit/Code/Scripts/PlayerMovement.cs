using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour {

  public Rigidbody rb;
  public float moveSpeed = 20;
  public float drag = 0.98f;
  public float rotationSpeed = 10;
  public float bonkThreshold = 500;
  public float bonkPower = 10; // The max distance (in metres?) between player and bonkable object where the object will fall
  public float jumpHeight = 10;
  public float fallSpeed = 10;
  [SerializeField] private GameObject dashLight;
  [SerializeField] private float dashMultiplier = 50f;
  [SerializeField] private float dashCooldown = 1f;
  [SerializeField] private float dashDuration = 0.2f;

  [Header("Camera Things")]
  public CinemachineVirtualCamera vcam;
  public float minCameraDistance = 18;
  public float maxCameraDistance = 40;
  public float zoomOutSpeed = .5f;
  public float zoomInSpeed = 3;

  private float horizontal;
  private float vertical;
  private Vector3 moveForce;
  private Vector3 prevPosition;
  private float currentSpeed;
  private bool canMove = true;
  private bool canJump = true;
  private bool shouldJump = false;
  private bool shouldDash = false;
  private bool canDash = true;
  private bool shouldBrake = false;

  private CinemachineTransposer transposer;

  void enableMovement() {
    canMove = true;
    // This is a temp fix for the player getting stuck in a loop after hitting a wall and then not having a vertical input / change in rotation
    //moveForce = new Vector3(0, 0, 0);
  }

  void enableDash() {
    canDash = true;
  }

  void endDash() {
    shouldDash = false;
    dashLight.SetActive(false);
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
    Vector3 reflected = Vector3.Reflect(moveForce, hit.contacts[0].normal) / 2;
    rb.velocity = Vector3.zero;
    rb.AddForce(reflected, ForceMode.Impulse);
    checkBonkables(rb.position);
  }

  void OnCollisionEnter(Collision hit) {
    if ((hit.gameObject.tag == "Enemy" || (hit.gameObject.tag == "Wall")) && currentSpeed > bonkThreshold) {
      bonk(hit);
    }
  }

  void OnCollisionStay(Collision hit) {
    if (hit.gameObject.tag == "Floor") {
      canJump = true;
    }
  }

  void Start() {
    transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
  }

  void FixedUpdate() {
    if (canMove) {
      if (horizontal != 0 && !shouldDash) {
        // Remove rb.velocity.magnitude if we want rotation while not moving
        rb.transform.Rotate(Vector3.up * horizontal * rotationSpeed * rb.velocity.magnitude * Time.deltaTime);
      }
      if (vertical != 0 && !shouldDash) {
        moveForce = rb.transform.forward * vertical * moveSpeed;
        rb.AddForce(moveForce);
      }
      if (shouldJump && canJump) {
        rb.AddForce(rb.transform.up * jumpHeight, ForceMode.Impulse);
        canJump = false;
      }
      if (shouldDash && canDash) {
        rb.velocity = Vector3.zero;
        float dashDirection = vertical != 0 ? vertical : 1;
        rb.AddForce(moveSpeed * dashMultiplier * dashDirection * rb.transform.forward);
        canDash = false;
        // TODO: Don't love the look of the light. Might want to try make the model tinted instead or use an animation
        dashLight.SetActive(true);
        Invoke("endDash", dashDuration);
        Invoke("enableDash", dashCooldown);
      }
      if (shouldBrake) {
        rb.velocity = rb.velocity * 0.9f;
      }
    }
  }

  void Update() {

    horizontal = Input.GetAxisRaw("Horizontal");
    vertical = Input.GetAxisRaw("Vertical");
    shouldJump = Input.GetButton("Jump");
    shouldDash = Input.GetButton("Dash");
    shouldBrake = Input.GetButton("Brake");

    // For monitoring speed
    if (prevPosition != rb.transform.position) {
      currentSpeed = Vector3.Distance(prevPosition, rb.transform.position); // This is sometimes 0 even while moving
      prevPosition = rb.transform.position;
    }

    if (currentSpeed > moveSpeed / 1000) {
      transposer.m_FollowOffset.y = Mathf.Lerp(transposer.m_FollowOffset.y, maxCameraDistance, zoomOutSpeed * Time.deltaTime);
    } else {
      transposer.m_FollowOffset.y = Mathf.Lerp(transposer.m_FollowOffset.y, minCameraDistance, zoomInSpeed * Time.deltaTime);
    }
  }
}
