using UnityEngine;

public class Lazer : MonoBehaviour
{
    public float speed = 10f;  // Velocidad del láser
    public float lifeTime = 2f;  // Tiempo de vida del láser antes de destruirse

    void Start()
    {
        // Destruir el láser después de un tiempo determinado
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Mover el láser hacia adelante (Adelante es left por la orientación del escenario)
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}
