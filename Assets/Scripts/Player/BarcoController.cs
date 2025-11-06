using System;
using UnityEditor;
using UnityEngine;

public class BarcoController : MonoBehaviour
{
    [Header("Configuración del área de detección")]
    [SerializeField] private float radio = 5f;
    [SerializeField, Range(4, 360)] private int rayos = 36;
    [SerializeField] private LayerMask capasDetectables;

    [Header("Visualización")]
    [SerializeField] private Color colorRayos = Color.green;
    [SerializeField] private Color colorSinDetección = Color.red;
    [SerializeField] private bool mostrarRayos = true;
    public int Vida = 100;
    public int Dano = 10;
    public float CadenciaDisparo = 1f;

    [Header("Resultados")]
    public Transform objetoMasCercano;
    public Vector2 puntoImpacto;

    private float tiempoUltimoDisparo = 0f;
    private bool puedeDisparar = true;
  [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    [Header("Movimiento inicial")]
    [SerializeField] private float fuerzaInicial = 3f; // Fuerza del empujón hacia el centro

    [SerializeField] private GameObject[] drops;

    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
        if (animator == null)
            animator = GetComponent<Animator>();

        //  Calcular dirección hacia el centro (0,0)
        Vector2 direccionCentro = ((Vector2)Vector2.zero - rb.position).normalized;

        // Aplicar empujón inicial
        rb.AddForce(direccionCentro * fuerzaInicial, ForceMode2D.Impulse);
    }



    private void Update()
    {
        DetectarObjetoMasCercano();
        ActualizarAnimacionDireccion();

        if (!puedeDisparar)
        {
            if (Time.time - tiempoUltimoDisparo >= CadenciaDisparo)
                puedeDisparar = true;
        }
    }

    private void DetectarObjetoMasCercano()
    {
        float menorDistancia = Mathf.Infinity;
        RaycastHit2D hitMasCercano = new RaycastHit2D();

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

            if (puedeDisparar)
                Disparar();
        }
        else
        {
            objetoMasCercano = null;
            puntoImpacto = Vector2.zero;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1, 0.25f);
        Gizmos.DrawWireSphere(transform.position, radio);
    }

    private void Disparar()
    {
        var targetController = objetoMasCercano.GetComponent<BarcoController>();
        if (targetController != null)
        {
            targetController.RecibirDano(Dano);
            puedeDisparar = false;
            tiempoUltimoDisparo = Time.time;
            Debug.Log($"{gameObject.name} disparó a {objetoMasCercano.name} - Vida restante: {targetController.Vida}");
        }
    }

    public void RecibirDano(int dano)
    {
        Vida -= dano;
        if (Vida <= 0)
            Morir();
    }


    private void DropPowerUp()
{
    // Verificar si hay drops disponibles
    if (drops != null && drops.Length > 0)
    {
        // Escoger un prefab aleatoriamente de la lista de drops
        GameObject drop = drops[UnityEngine.Random.Range(0, drops.Length)];

        // Instanciar el prefab en la posición del barco
        Instantiate(drop, transform.position, Quaternion.identity);
        
        Debug.Log($"{gameObject.name} soltó un power-up: {drop.name}");
    }
    else
    {
        Debug.LogWarning("No hay drops disponibles o la lista está vacía");
    }
}

    private void Morir()
    {
         DropPowerUp();

        Debug.Log($"{gameObject.name} ha sido destruido!");
        Destroy(gameObject);
        
    }

    private void ActualizarAnimacionDireccion()
    {
        if (rb == null || animator == null)
            return;

        Vector2 vel = rb.linearVelocity;

        if (vel.sqrMagnitude > 0.001f)
        {
            Vector2 dir = vel.normalized;
            animator.SetFloat("DirX", dir.x);
            animator.SetFloat("DirY", dir.y);
        }
    }

   void OnTriggerEnter2D(Collider2D collider)
   {
       switch (collider.gameObject.tag)
       {
           case "Obstacle":
               Morir();
               break;
        case "DanoZone":
            Vida -= 10;         
            break;
            
        case "HPickup":
            Vida += 20;
            Destroy(collider.gameObject);
            break;
            
        case "DanoPickup":
            Dano += 5;
            Destroy(collider.gameObject);
            break;
            
        case "FireRatePickup":
            CadenciaDisparo = Mathf.Max(0.1f, CadenciaDisparo - 0.2f);
            Destroy(collider.gameObject);
            break;
    }
}
}
