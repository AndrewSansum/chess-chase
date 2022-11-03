using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SwordSwing : MonoBehaviour
{
    public Camera camera;

    private Collider2D collider;

    void Start()
    {
        collider = GetComponent<Collider2D>();
        collider.enabled = false;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Vector2 playerPosition = this.gameObject.transform.parent.transform.position;
            Vector2 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            float angleFromPlayerToMouse = Vector2.Angle(new Vector2(0,1), mousePosition - playerPosition);
            if (mousePosition.x >= playerPosition.x) {
                angleFromPlayerToMouse *= -1;
            }
            this.gameObject.transform.parent.transform.rotation = Quaternion.Euler(0,0,angleFromPlayerToMouse);
        }
    }
}
