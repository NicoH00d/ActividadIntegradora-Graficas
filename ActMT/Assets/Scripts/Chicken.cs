using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : MonoBehaviour
{
    public GameObject bulletPrefab;  // Referencia al prefab de la bala
    private float fireRate = 1f;     // Tiempo entre disparos, se ajustará dinámicamente
    private int numberOfBullets = 12;  // Número de direcciones a disparar, se ajustará dinámicamente
    private float bulletSpeed = 10f;  // Velocidad de las balas, se ajustará dinámicamente
    private int styleIndex = 0;       // Índice para cambiar estilos de disparo

    public float moveSpeed = 3f;     // Velocidad de movimiento del Chicken
    public Vector3 xRange = new Vector3(-5f, 5f);  // Rango de movimiento en el eje X
    public Vector3 zRange = new Vector3(-5f, 5f);  // Rango de movimiento en el eje Z
    public float moveChangeInterval = 3f; // Tiempo entre cambios de dirección

    private Vector3 targetPosition;
    private bool isMoving = true;    // Controla si Chicken se mueve o no

    private void Start()
    {
        // Comienza a disparar repetidamente
        InvokeRepeating("FireBullets", 0f, fireRate);

        // Cambia el estilo de disparo después de un tiempo aleatorio inicial
        ScheduleNextStyleChange();

        // Inicializar el primer movimiento
        SetNewTargetPosition();
        InvokeRepeating("SetNewTargetPosition", moveChangeInterval, moveChangeInterval);
    }

    private void Update()
    {
        if (isMoving)
        {
            // Mover el Chicken hacia la posición objetivo
            MoveToTarget();
        }
    }
    private void FireBullets()
    {
        Vector3 fixedPosition = transform.position;
        float angleStep = 360f / numberOfBullets;
        float angle = 0f;
    
        for (int i = 0; i < numberOfBullets; i++)
        {
            // Calcular la dirección de la bala usando ángulos para disparar en todas las direcciones alrededor del Chicken
            float bulletDirX = Mathf.Cos((angle * Mathf.PI) / 180f);
            float bulletDirZ = Mathf.Sin((angle * Mathf.PI) / 180f);
    
            Vector3 bulletDir = new Vector3(bulletDirX, 0f, bulletDirZ).normalized;
    
            // Disparo de balas
            GameObject bullet = Instantiate(bulletPrefab, fixedPosition, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(bulletDir);
                bulletScript.bulletSpeed = bulletSpeed;
                
                if (styleIndex == 1)
                {
                    bulletScript.SetQuadratic();  // Aplicar la función cuadrática
                    bulletScript.amplitude = 0.05f;  // Ajusta la amplitud para una mejor visualización
                }
                else
                {
                    bulletScript.frequency = 0f;  // Sin curva
                    bulletScript.amplitude = 0f;  // Sin curva
                }
            }
    
            angle += angleStep;
        }
    }

    private void FireSinBullets()
    {
        Vector3 fixedPosition = transform.position;
        float angleStep = 360f / numberOfBullets;
        float angle = 0f;

        for (int i = 0; i < numberOfBullets; i++)
        {
            float bulletDirX = Mathf.Cos((angle * Mathf.PI) / 180f);
            float bulletDirZ = Mathf.Sin((angle * Mathf.PI) / 180f);

            Vector3 bulletDir = new Vector3(bulletDirX, 0f, bulletDirZ).normalized;

            // Disparar la bala con curva sin(z)
            GameObject bullet = Instantiate(bulletPrefab, fixedPosition, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(bulletDir);
                bulletScript.bulletSpeed = bulletSpeed;
                bulletScript.frequency = 1f;
                bulletScript.amplitude = 2f;
                bulletScript.SetInversion(false);  // No invertida
            }

            // Disparar la bala con curva -sin(z)
            GameObject invBullet = Instantiate(bulletPrefab, fixedPosition, Quaternion.identity);
            Bullet invBulletScript = invBullet.GetComponent<Bullet>();
            if (invBulletScript != null)
            {
                invBulletScript.SetDirection(bulletDir);
                invBulletScript.bulletSpeed = bulletSpeed;
                invBulletScript.frequency = 1f;
                invBulletScript.amplitude = 2f;
                invBulletScript.SetInversion(true);  // Invertida
            }

            angle += angleStep;
        }
    }

    private void ChangeStyle()
    {
        styleIndex = (styleIndex + 1) % 3;

        switch (styleIndex)
        {
            case 0:
                // Estilo 1: Disparo General
                fireRate = 0.6f;
                numberOfBullets = 30; //numero de direcciones
                bulletSpeed = 10f;
                isMoving = true;  // Chicken se mueve
                moveSpeed = 5f;
                CancelInvoke("FireSinBullets");
                InvokeRepeating("FireBullets", 0f, fireRate);
                break;

            case 1:
                // Estilo 2: Disparo con función cuadrática
                fireRate = 0.2f;
                numberOfBullets = 12; //numero de direcciones
                bulletSpeed = 13f;
                isMoving = true; // Chicken se mueve
                CancelInvoke("FireSinBullets");
                InvokeRepeating("FireBullets", 0f, fireRate);
                break;

            case 2:
                // Estilo 3: Curva Sinusoidal en Flor
                fireRate = 0.2f;
                numberOfBullets = 9; // Número de puntas de la flor
                bulletSpeed = 5f;
                isMoving = false;  // Chicken se detiene
                CancelInvoke("FireBullets");
                InvokeRepeating("FireSinBullets", 0f, fireRate);
                break;
        }

        // Programa el siguiente cambio de estilo en un intervalo aleatorio
        ScheduleNextStyleChange();
    }

    private void ScheduleNextStyleChange()
    {
        // Programa el próximo cambio de estilo en un intervalo aleatorio entre 4 y 10 segundos
        float randomTime = Random.Range(4f, 10f);
        Invoke("ChangeStyle", randomTime);
    }
    //Funciones de movimiento
    private void SetNewTargetPosition()
    {
        float newX = Random.Range(xRange.x, xRange.y);
        float newZ = Random.Range(zRange.x, zRange.z);
        targetPosition = new Vector3(newX, transform.position.y, newZ);
    }

    private void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        // Limitar la posición en X y Z dentro de los rangos especificados
        float clampedX = Mathf.Clamp(transform.position.x, xRange.x, xRange.y);
        float clampedZ = Mathf.Clamp(transform.position.z, zRange.x, zRange.y);
        transform.position = new Vector3(clampedX, transform.position.y, clampedZ);   
    }
}
