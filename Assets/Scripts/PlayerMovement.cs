using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float maxSpeed;
    public float acceleration;
    public float deceleration;

    public float turnSpeed;

    private Rigidbody2D rb;

    private float horizontalInput;
    private float verticalInput;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate() {
        float hComponent = 0;
        float vComponent = 0;

        float stopThreshold = deceleration / 100f;

        if (horizontalInput != 0) {
            //turn around with different speed
            if (horizontalInput > 0 && rb.velocity.x < 0) {
                hComponent = turnSpeed;
            } else if (horizontalInput < 0 && rb.velocity.x > 0) {
                hComponent = turnSpeed * -1;
            } else {
                hComponent = acceleration * horizontalInput;
            }
        } else if (Mathf.Abs(rb.velocity.x) >= stopThreshold) {
            //only decelerate if above a certain velocity
            if (rb.velocity.x > 0) {
                hComponent = deceleration * -1;
            } else if (rb.velocity.x < 0) {
                hComponent = deceleration;
            }
        } else {
            //set velocity component to 0 if under a small enough speed
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (verticalInput != 0) {
            //turn around with different speed
            if (verticalInput > 0 && rb.velocity.y < 0) {
                vComponent = turnSpeed;
            } else if (verticalInput < 0 && rb.velocity.y > 0) {
                vComponent = turnSpeed * -1;
            } else {
                vComponent = acceleration * verticalInput;
            }
        } else if (Mathf.Abs(rb.velocity.y) >= stopThreshold) {
            //only decelerate if above a certain velocity
            if (rb.velocity.y > 0) {
                vComponent = deceleration * -1;
            } else if (rb.velocity.y < 0) {
                vComponent = deceleration;
            }
        } else {
            //set velocity component to 0 if under a small enough speed
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }

        rb.velocity += Time.fixedDeltaTime * new Vector2(hComponent, vComponent);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
    }
}
