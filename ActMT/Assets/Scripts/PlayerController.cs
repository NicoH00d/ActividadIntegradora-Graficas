using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")] //Titulo para la visualizacion en el inspector
    public float moveSpeed = 5f;  // Velocidad de movimiento del personaje
    private float originalMoveSpeed;  // Almacena la velocidad original del personaje

    [Header("Movement Boundaries")]
    public float minX = -4f;  // Límite mínimo en X
    public float maxX = 14f;  // Límite máximo en X
    public float minZ = -20f;   //Límite minimo en z
    public float maxZ = 15f;  //límite maximo en Z

    [Header("Shooting Settings")]
    public GameObject Lazer;  // Prefab de la bala (Lazer)
    public float fireRate = 0.5f;  // Tiempo entre disparos en segundos
    private float fireTimer = 0f;  // Temporizador para controlar la cadencia de disparo

    void Start()
    {
        originalMoveSpeed = moveSpeed;
    }

    void Update()
    {
        HandleMovement();
        HandleShooting();
    }

    private void HandleMovement()
    {
        // Reducir la velocidad al presionar F
        if (Input.GetKeyDown(KeyCode.F))
        {
            moveSpeed = originalMoveSpeed / 2f; //0.5 veces la velocidad normal
        }

        // Restaurar la velocidad al soltar F
        if (Input.GetKeyUp(KeyCode.F))
        {
            moveSpeed = originalMoveSpeed;
        }

        // Obtener las entradas del teclado
        float moveX = -Input.GetAxis("Vertical");   // Movimiento en el eje X (adelante/atrás), invertido por la orientacion
        float moveZ = Input.GetAxis("Horizontal");  // Movimiento en el eje Z (izquierda/derecha)

        // Crear un vector de movimiento
        Vector3 movement = new Vector3(moveX, 0f, moveZ) * moveSpeed * Time.deltaTime;

        // Aplicar el movimiento al personaje
        transform.Translate(movement, Space.World);

        // Limitar la posición del personaje dentro de los rangos especificados
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedZ = Mathf.Clamp(transform.position.z, minZ, maxZ);
        transform.position = new Vector3(clampedX, transform.position.y, clampedZ);
    }

    private void HandleShooting()
    {
        fireTimer += Time.deltaTime;

        // Disparar mientras se mantiene presionada la barra espaciadora y respetando el fireRate
        if (Input.GetKey(KeyCode.Space) && fireTimer >= fireRate)
        {
            Shoot();
            fireTimer = 0f;  // Reiniciar el temporizador después de disparar
        }
    }

    private void Shoot()
    {
        // Instanciar la bala en la posición y rotación actuales del jugador
        Instantiate(Lazer, transform.position, transform.rotation);
    }
}
