using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public float bulletSpeed = 5f;      // Velocidad de la bala, ajustable desde el Inspector
    [SerializeField] public float bulletLifeTime = 4f;   // Tiempo en segundos antes de que la bala se destruya, ajustable desde el Inspector
    [SerializeField] public float frequency = 1f;        // Frecuencia de la onda sinusoidal, ajustable desde el Inspector
    [SerializeField] public float amplitude = 1f;        // Amplitud de la onda sinusoidal o cuadrática, ajustable desde el Inspector
    public bool invertCurve = false;    // Controla si la curva está invertida

    private Vector3 moveDirection;      // Dirección en la que la bala se moverá
    private Vector3 startPosition;      // Posición inicial de la bala
    private float timeElapsed = 0f;     // Tiempo transcurrido desde que la bala se disparó
    private string movementType = "sinusoidal";  // Tipo de movimiento por defecto

    private void Start()
    {
        // Destruir la bala después de bulletLifeTime segundos
        Destroy(gameObject, bulletLifeTime);
        startPosition = transform.position;  // Guardar la posición inicial de la bala
    }

    private void Update()
    {
        // Actualizar el tiempo transcurrido
        timeElapsed += Time.deltaTime;

        // Movimiento lineal basado en la dirección
        Vector3 movement = moveDirection * bulletSpeed * timeElapsed;

        // Movimiento adicional basado en el tipo de movimiento
        Vector3 additionalMovement = Vector3.zero;

        if (movementType == "sinusoidal")
        {
            // Movimiento sinusoidal perpendicular a la dirección de la bala
            float sineOffset = Mathf.Sin(timeElapsed * frequency) * amplitude;

            // Crear un vector perpendicular a la dirección de movimiento
            Vector3 perpendicular = new Vector3(-moveDirection.z, 0f, moveDirection.x).normalized;

            // Aplicar el desplazamiento a lo largo de la dirección perpendicular para crear la curva sinusoidal
            additionalMovement = perpendicular * sineOffset;

            // Invertir la curva si es necesario
            if (invertCurve)
            {
                additionalMovement = -additionalMovement;
            }
        }
        else if (movementType == "quadratic")
        {
            // Movimiento cuadrático en función del tiempo
            float curveOffset = Mathf.Pow(timeElapsed * 10f, 2) * amplitude;  // Ajustar el factor según lo necesario

            Vector3 perpendicular = new Vector3(-moveDirection.z, 0f, moveDirection.x).normalized;
            additionalMovement = perpendicular * curveOffset;

            if (invertCurve)
            {
                additionalMovement = -additionalMovement;
            }
        }

        // Aplicar el movimiento final a la bala
        transform.position = startPosition + movement + additionalMovement;
    }
    
    public void SetDirection(Vector3 direction)
    {
        // Establecer la dirección en la que la bala se moverá
        moveDirection = direction;
    }

    public void SetInversion(bool invert)
    {
        // Establecer si la curva será invertida o no
        invertCurve = invert;
    }

    public void SetQuadratic()
    {
        // Configuración específica para trayectoria cuadrática
        amplitude = 1f;
        movementType = "quadratic";
    }

    public void SetSinusoidal()
    {
        // Configuración específica para trayectoria sinusoidal
        frequency = 1f;
        amplitude = 1f;
        movementType = "sinusoidal";
    }
}
