using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public GameObject wavePrefab; // Prefab de la ola
    public float waveSpeed = 5f;  // Velocidad de la ola
    public float waveLifeTime = 3f; // Cuánto dura antes de destruirse

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f; 

            // Crear ola
            GameObject wave = Instantiate(wavePrefab, mousePos, Quaternion.identity);

            // Dirección hacia el centro
            Vector2 direction = (Vector2.zero - (Vector2)mousePos).normalized;

            // Asignar dirección y velocidad
            Rigidbody2D rb = wave.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = direction * waveSpeed;

            Destroy(wave, waveLifeTime);
        }
    }
}
