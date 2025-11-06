using UnityEngine;

public class WindSpawner : MonoBehaviour
{
    public GameObject windPrefab;   // Prefab de la ráfaga
    public float windSpeed = 6f;    // Velocidad de la ráfaga
    public float windLifetime = 2f; // Duración antes de destruirse

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Clic derecho
        {
            
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;

            GameObject wind = Instantiate(windPrefab, mousePos, Quaternion.identity);

            // Dirección hacia el centro
            Vector2 dir = (Vector2.zero - (Vector2)mousePos).normalized;

            Rigidbody2D rb = wind.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = dir * windSpeed;

            Destroy(wind, windLifetime);
        }
    }
}
