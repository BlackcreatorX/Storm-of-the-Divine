using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class WindBlast : MonoBehaviour
{
    [Tooltip("Intensidad del cambio de dirección")]
    public float directionChange = 45f; // en grados

    [Tooltip("Velocidad mínima que deben tener los objetos para ser afectados")]
    public float minSpeed = 0.1f;
    public LayerMask affectedLayers;

    void Update()
{
    transform.Rotate(Vector3.forward * 180 * Time.deltaTime); // Gira
    transform.localScale += Vector3.one * Time.deltaTime * 0.5f; // Crece
}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & affectedLayers) != 0)
        {
        Rigidbody2D rb = other.attachedRigidbody;
            if (rb != null)
            {
                // Calcular nueva dirección rotando su velocidad actual
                Vector2 currentVel = rb.linearVelocity;
                if (currentVel.magnitude > minSpeed)
                {
                    float randomSign = Random.value > 0.5f ? 1f : -1f;
                    float angle = directionChange * randomSign;

                    Vector2 newDir = Quaternion.Euler(0, 0, angle) * currentVel.normalized;
                    rb.linearVelocity = newDir * currentVel.magnitude; // misma velocidad, distinta dirección
                }
            }
        }
    }
}
