using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SwordSwing : MonoBehaviour
{
    public Camera camera;

    private Collider2D collider;

    private Transform swordTransform;

    void Start()
    {
        collider = GetComponent<Collider2D>();
        collider.enabled = false;
        swordTransform = this.gameObject.transform.parent.transform;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Vector2 playerPosition = swordTransform.position;
            Vector2 mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
            float angleFromPlayerToMouse = Vector2.Angle(new Vector2(0,1), mousePosition - playerPosition);
            if (mousePosition.x >= playerPosition.x) {
                angleFromPlayerToMouse *= -1;
            }
            swordTransform.rotation = Quaternion.Euler(0,0,angleFromPlayerToMouse);
        }
    }
}
