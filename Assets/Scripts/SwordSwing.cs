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
        
    }
}
