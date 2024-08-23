using UnityEngine;
using UnityEngine.UI;

public class BulletCounterUI : MonoBehaviour
{
    public Text bulletCounterText;  // Referencia al componente Text de la UI

    private void Update()
    {
        // Contar el número de objetos Bullet(Clone) en la escena
        int bulletCount = GameObject.FindObjectsOfType<Bullet>().Length;

        // Actualizar el contador en la UI
        bulletCounterText.text = "Bullets: " + bulletCount.ToString();
    }
}
