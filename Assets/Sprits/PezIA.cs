using UnityEngine;

public class PezIA : MonoBehaviour
{
    Vector2 limitesPantalla;
    int direccion = 1;
    private float tam;
    [SerializeField] private Transform pezSprite;
    [SerializeField] private float velocidad = 3;

    //Mod
    public static float TamMaximo = 2.5f; // Tamaño máximo ajustable.

    //Mod
    public void SetVelocidad(float nuevaVelocidad)
    {
        velocidad = nuevaVelocidad;
    }


    void Start()
    {
        limitesPantalla = Camera.main.ScreenToWorldPoint(new Vector3(UnityEngine.Screen.width, UnityEngine.Screen.height, 0));

        if (transform.position.x < 0)//cambiado
        {
            direccion = 1;
            pezSprite.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else
        {
            direccion = -1;
            pezSprite.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }

        //Tamaño aleatorio
        float tamAleatorio = Random.Range(0.5f, TamMaximo);
        tam = tamAleatorio;
        transform.localScale = new Vector3(tamAleatorio, tamAleatorio, tamAleatorio);

        Debug.Log($"Pez generado con tamaño:  {tam}");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (Vector3.right * direccion * velocidad * Time.deltaTime);

        //Debug.Log($"Posición del pez: {transform.position}, Dirección: {direccion}");


        if (transform.position.x <= -limitesPantalla.x - 5 || transform.position.x > limitesPantalla.x + 5)//cambiado
        {
            Destroy(gameObject);
        }
    }

    public float GetTam()
    {

        return tam;
    }
}
