using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Wave : MonoBehaviour
{
    public float pushForce = 5f;
    public LayerMask affectedLayers;



    void Update()
{
    transform.localScale += Vector3.one * Time.deltaTime;
}

   private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & affectedLayers) != 0)
        {
            Rigidbody2D rb = other.attachedRigidbody;
            if (rb != null)
            {
                // Empuje en dirección desde la ola hacia afuera
                Vector2 pushDir = (other.transform.position - transform.position).normalized;
                rb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);
            }
        }
    }
}