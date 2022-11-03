using System.Collections;
using System.Collections.Generic;
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
            StartCoroutine(Swing(90, 1));
        }
    }

    private IEnumerator Swing(float angle, float time)
    {
        Vector3 startAngles = (swordTransform.eulerAngles.z - angle/2) * Vector3.forward;
        Vector3 endAngles = (swordTransform.eulerAngles.z + angle/2) * Vector3.forward;

        swordTransform.rotation = Quaternion.Euler(startAngles);
        collider.enabled = true;

        float elapsedTime = 0;
        while (elapsedTime < time) {
            swordTransform.rotation = Quaternion.Euler(Vector3.Lerp(startAngles, endAngles, elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        collider.enabled = false;
    }
}
