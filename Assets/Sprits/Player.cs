using UnityEngine;
using UnityEngine.Device;

public class Player : MonoBehaviour
{
    //Serialized para que las variables privadas se vean en el editor
    [SerializeField] private Transform pezSprite;
    [SerializeField] private PlayerIA playerIA;

    float velocidad = 3;

    private float tam;
    private int pecesComidos = 0;

    //Audio
    [SerializeField] private AudioClip sonidoComerPez;
    private AudioSource audioSource;


    void Start()
    {
        tam = transform.localScale.x;
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("No se encontró un AudioSource en el objeto Jugador.");
        }
    }

    void Update()
    {
        //Movimiento
        float inputVertical = Input.GetAxis("Vertical") * Time.deltaTime * velocidad;
        float inputHorizontal = Input.GetAxis("Horizontal") * Time.deltaTime * velocidad;

        transform.position = transform.position + new Vector3(inputHorizontal, inputVertical, 0);

        //Evitar salir de la pantalla
        Vector2 limitesPantalla = Camera.main.ScreenToWorldPoint(new Vector3(UnityEngine.Screen.width, UnityEngine.Screen.height, 0));

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, limitesPantalla.x * -1, limitesPantalla.x),
            Mathf.Clamp(transform.position.y, limitesPantalla.y * -1, limitesPantalla.y),
            0
        );

        //Rotación
        if (inputHorizontal < 0)
        {
            pezSprite.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else
        {
            pezSprite.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //No se procesan colisiones si el juego está parado
        if (Time.timeScale == 0) return;

        if (collision.gameObject.CompareTag("Pez"))
        {
            PezIA pezIA = collision.gameObject.GetComponent<PezIA>();
            Debug.Log($"Jugador: {tam}, Pez: {pezIA.GetTam()}");

            if (tam >= pezIA.GetTam())
            {
                pecesComidos++;
                playerIA.ActualizarPuntos(pecesComidos);

                transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
                tam = transform.localScale.x;

                Destroy(collision.gameObject);

                //Reproducir sonido
                if(audioSource != null && sonidoComerPez != null)
                {
                    Debug.Log("Se reproduce el sonido");
                    audioSource.PlayOneShot(sonidoComerPez);
                }
                else{
                    Debug.Log("Audio no configurado");
                }
                

                if (pecesComidos >= 15)
                {
                    GameManager.Instancia.ActualizarMaquinaDeEstados(GameManager.MaquinaDeEstados.JuegoGanado);
                    velocidad = 0;
                }

                //Cada 5 peces comidos se incrementa el nivel
                if (pecesComidos % 5 == 0)
                {
                    GameManager.Instancia.IncrementarNivel();
                }

                //if (pecesComidos >= 10)
                //{
                //GameManager.Instancia.ActualizarMaquinaDeEstados(GameManager.MaquinaDeEstados.JuegoGanado);
                //velocidad = 0;
                //}
            }
            else
            {
                GameManager.Instancia.ActualizarMaquinaDeEstados(GameManager.MaquinaDeEstados.JuegoTerminado);
                Destroy(gameObject);
            }

        }

    }
}
