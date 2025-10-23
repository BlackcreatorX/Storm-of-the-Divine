using UnityEngine;

public class BarcoController : MonoBehaviour
{
    [Header("Configuración del área de detección")]
    [SerializeField] private float radio = 5f; // Radio del área circular
    [SerializeField, Range(4, 360)] private int rayos = 36; // Cantidad de rayos a lanzar
    [SerializeField] private LayerMask capasDetectables; // Capas que se pueden detectar

    [Header("Visualización")]
    [SerializeField] private Color colorRayos = Color.green;
    [SerializeField] private Color colorSinDetección = Color.red;
    [SerializeField] private bool mostrarRayos = true;

    [Header("Resultados")]
    public Transform objetoMasCercano;
    public Vector2 puntoImpacto;

    private void Update()
    {
        DetectarObjetoMasCercano();
    }

    private void DetectarObjetoMasCercano()
    {
        float menorDistancia = Mathf.Infinity;
        RaycastHit2D hitMasCercano = new RaycastHit2D();

        // Lanza varios raycasts en un círculo
        for (int i = 0; i < rayos; i++)
        {
            float angulo = i * (360f / rayos);
            Vector2 direccion = new Vector2(Mathf.Cos(angulo * Mathf.Deg2Rad), Mathf.Sin(angulo * Mathf.Deg2Rad));

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direccion, radio, capasDetectables);

            if (mostrarRayos)
            {
                Color color = hit.collider ? colorRayos : colorSinDetección;
                Vector3 destino = hit.collider ? (Vector3)hit.point : transform.position + (Vector3)direccion * radio;
                Debug.DrawLine(transform.position, destino, color);
            }

            if (hit.collider != null && hit.distance < menorDistancia)
            {
                menorDistancia = hit.distance;
                hitMasCercano = hit;
            }
        }

        if (hitMasCercano.collider != null)
        {
            objetoMasCercano = hitMasCercano.transform;
            puntoImpacto = hitMasCercano.point;
        }
        else
        {
            objetoMasCercano = null;
            puntoImpacto = Vector2.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Dibuja el radio del área de detección
        Gizmos.color = new Color(0, 1, 1, 0.25f);
        Gizmos.DrawWireSphere(transform.position, radio);
    }
}