using UnityEngine;
using Cinemachine;

public class PlayerMovement : MonoBehaviour {

  public Rigidbody rb;
  public Animator animator;

  public float moveSpeed = 20;
  public float drag = 0.98f;
  public float rotationSpeed = 10;
  public float bonkThreshold = 0.2f;

  [Tooltip("The max distance between player and bonkable object where the object will fall.")]
  public float bonkPower = 10; // The max distance (in metres?) between player and bonkable object where the object will fall
  public float jumpHeight = 10;
  public float fallSpeed = 10;

  [Tooltip("Seconds to go from current speed to nearly zero.")]
  public float brakeTime = 0.2f; // seconds
  [Tooltip("Minimum speed to stop the braking animation and return to run/idle even if still holding the brake key.")]
  public float minSpeedForBrakeAnim = 2;
  [Tooltip("Seconds to remain stunned after bonking.")]
  public float stunDuration = 1f; // seconds

  [SerializeField] private GameObject dashLight;
  [SerializeField] private float dashMultiplier = 50f;
  [SerializeField] private float dashCooldown = 1f;
  [SerializeField] private float dashDuration = 0.2f;


  [SerializeField] private Vector3 explosionOffset = Vector3.zero;
  [SerializeField] private float explosionDistance = 5;
  [SerializeField] private float explosionDistanceUp = 5;
  [SerializeField] private float explosionPower = 10;
  [SerializeField] private float explosionRadius = 5;
  [SerializeField] private float upDog = 1;

  private int maxColliders = 2000;
  private Collider[] hitColliders;

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
  private bool canDash = true;
  private bool shouldJump = false;
  private bool shouldDash = false;
  private bool shouldBrake = false;

  private CinemachineTransposer transposer;

  void enableMovement() {
    canMove = true;
    animator.SetBool("WallCollision", false);
    // This is a temp fix for the player getting stuck in a loop after hitting a wall and then not having a vertical input / change in rotation
    //moveForce = new Vector3(0, 0, 0);
  }

  void enableDash() {
    canDash = true;
  }

  void endDash() {
    shouldDash = false;
    dashLight.SetActive(false);
    animator.SetBool("Dashing", false);
  }

  void unfreezeBonkable(GameObject bonkable) {
    Rigidbody bonkRb = bonkable.GetComponent<Rigidbody>();
    bonkRb.constraints = RigidbodyConstraints.None;
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
    animator.SetBool("WallCollision", true);
    Invoke("enableMovement", stunDuration);
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
      animator.SetBool("Jumping", false);
    }
  }

  void Start() {
    hitColliders = new Collider[maxColliders];

    transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
    if (animator == null) {
      animator = GetComponentInChildren<Animator>();
    }
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
        animator.SetBool("Jumping", true);
        rb.AddForce(rb.transform.up * jumpHeight, ForceMode.Impulse);
        canJump = false;
      }

      if (shouldDash && canDash) {
        animator.SetBool("Dashing", true);
        rb.velocity = Vector3.zero;
        float dashDirection = vertical != 0 ? vertical : 1;
        rb.AddForce(moveSpeed * dashMultiplier * dashDirection * rb.transform.forward);
        canDash = false;
        // TODO: Don't love the look of the light. Might want to try make the model tinted instead or use an animation
        dashLight.SetActive(true);
        Invoke("endDash", dashDuration);
        Invoke("enableDash", dashCooldown);
        Explode();
      }

      if (shouldBrake) {
        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.deltaTime * (1 / brakeTime));
        animator.SetBool("Braking", rb.velocity.magnitude >= minSpeedForBrakeAnim); // stop the braking anim once slow enough
      } else {
        animator.SetBool("Braking", false);
      }
    }
  }

  void Respawn() {
      horizontal = 0;
      vertical = 0;
      shouldJump = false;
      shouldBrake = false;
      shouldDash = false;
      canDash = true;
      canJump = true;
      canMove = true;
      animator.SetBool("Running", false);
      animator.SetBool("Braking", false);
      animator.SetBool("Dashing", false);
      animator.SetBool("Jumping", false);
      animator.SetBool("WallCollision", false);

      SceneController.instance.movePlayerToSpawn();
  }

  void Update() {
    bool respawn = Input.GetButton("Respawn");
    if (respawn) {
      Respawn();
      return;
    }

    horizontal = Input.GetAxisRaw("Horizontal");
    vertical = Input.GetAxisRaw("Vertical");
    shouldJump = Input.GetButton("Jump");
    shouldDash = Input.GetButton("Dash");
    shouldBrake = Input.GetButton("Brake");

    animator.SetBool("Running", vertical != 0);

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

  void PreExplodeFracture(int numColliders) {
    for (int i = 0; i < numColliders; i++) {
      Collider hit = hitColliders[i];
      Fracture fracture = hit.GetComponent<Fracture>();
      if (fracture != null) {
        fracture.CauseFracture();
      }
    }
  }

  void Explode() {
    Vector3 expPoint1 = transform.position + explosionOffset;
    Vector3 expPoint2 = expPoint1 + (transform.forward * explosionDistance) + (Vector3.up * explosionDistanceUp);
    float radius = explosionRadius;

    int numColliders = Physics.OverlapCapsuleNonAlloc(expPoint1, expPoint2, radius, hitColliders);
    PreExplodeFracture(numColliders);
    numColliders = Physics.OverlapSphereNonAlloc(expPoint1, radius, hitColliders);
    PreExplodeFracture(numColliders);

    numColliders = Physics.OverlapCapsuleNonAlloc(expPoint1, expPoint2, radius, hitColliders);
    for (int i = 0; i < numColliders; i++)
    {
      Collider hit = hitColliders[i];
      Rigidbody rb = hit.GetComponent<Rigidbody>();
      if (rb != null && hit.tag != "Player") {
        Vector3 explosionPosition = hit.transform.position;
        float distanceFrom = Vector3.Distance(hit.transform.position, transform.position);
        rb.AddExplosionForce(explosionPower * (1/distanceFrom), explosionPosition, explosionRadius, upDog, ForceMode.Impulse);
      }
    }

    numColliders = Physics.OverlapSphereNonAlloc(expPoint1, radius, hitColliders);
    for (int i = 0; i < numColliders; i++)
    {
      Collider hit = hitColliders[i];
      Rigidbody rb = hit.GetComponent<Rigidbody>();
      if (rb != null && hit.tag != "Player") {
        rb.AddExplosionForce(explosionPower, expPoint1, explosionRadius * 2f, upDog, ForceMode.Impulse);
      }
    }
  }

  private void OnDrawGizmos () {
    Gizmos.color = Color.red;
    Vector3 expPoint1 = transform.position + explosionOffset;
    Vector3 expPoint2 = expPoint1 + (transform.forward * explosionDistance) + (Vector3.up * explosionDistanceUp);

    Gizmos.DrawWireSphere(expPoint1, explosionRadius);
    Gizmos.DrawWireSphere(expPoint2, explosionRadius);

    Gizmos.color = Color.blue;
    Gizmos.DrawWireSphere(expPoint1, explosionRadius * 2f);
  }
}
