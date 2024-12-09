using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instancia { get; private set; }
    private MaquinaDeEstados maquinaDeEstados;

    [SerializeField] private GameObject perdistePanel;
    [SerializeField] private GameObject ganastePanel;

    //Mod
    [SerializeField] private TMP_Text nivelTexto; // Referencia al texto del nivel.
    private int nivelActual = 1;

    //Transición niveles
    [SerializeField] private GameObject nivelTransicionPanel; // Panel de transición
    [SerializeField] private TMP_Text nivelTransicionTexto;   // Texto
    [SerializeField] private Button continuarBoton;          // Botón

    //Para no incrementar niveles si el juego ha terminado
    private bool juegoTerminado = false;

    //Audio
    [SerializeField] private AudioClip sonidoGanar;
    [SerializeField] private AudioClip sonidoPerder;
    private AudioSource audiosource;

    private void Awake()
    {
        if (Instancia != null)
            Destroy(gameObject);
        else
            Instancia = this;
    }

    //Mod
    void Start()
    {
        nivelTexto.text = "Nivel: " + nivelActual;

        //Audio
        audiosource = GetComponent<AudioSource>();
        if (audiosource == null)
        {
            Debug.Log("No se ha encontrado el AudioSource");
        }
    }

    //Mod
    public void IncrementarNivel()
    {
        //Para que no se incremente niveles si el juego ha terminado: 
        if (juegoTerminado)
        {
            Debug.Log("Juego terminado");
            return;
        }

        nivelActual++;
        MostrarTransicionNivel();
        nivelTexto.text = "Nivel: " + nivelActual;

        // Aumentar dificultad
        PezSpawner.VelocidadPez = 3f + nivelActual * 0.5f;
        PezSpawner.SpawnRate = Mathf.Max(0.5f, 1.5f - nivelActual * 0.1f);
        PezIA.TamMaximo = Mathf.Min(2.5f + nivelActual * 0.2f, 5f);

        Debug.Log($"Nivel {nivelActual}: Velocidad = {PezSpawner.VelocidadPez}, SpawnRate = {PezSpawner.SpawnRate}");
        Debug.Log($"Nuevo nivel: {nivelActual}, TamMaximo: {PezIA.TamMaximo}");
    }

    //Transicion nivel
    public void MostrarTransicionNivel()
    {
        Time.timeScale = 0; //Se pausa el juego

        nivelTransicionTexto.text = $"¡Nivel {nivelActual - 1} completado!";
        nivelTransicionPanel.SetActive(true);
        continuarBoton.onClick.AddListener(IniciarSiguienteNivel);
    }

    private void IniciarSiguienteNivel()
    {
        nivelTransicionPanel.SetActive(false);
        continuarBoton.onClick.RemoveListener(IniciarSiguienteNivel);

        Time.timeScale = 1; //Se reanuda el juego
    }

    public void ActualizarMaquinaDeEstados(MaquinaDeEstados nuevoEstado)
    {
        if (juegoTerminado) return;

        juegoTerminado = true;

        switch (nuevoEstado)
        {
            case MaquinaDeEstados.Jugando:
                break;
            case MaquinaDeEstados.JuegoTerminado:
                perdistePanel.SetActive(true);

                //Audio
                if (audiosource && sonidoPerder)
                {
                    audiosource.PlayOneShot(sonidoPerder);
                }
                break;
            case MaquinaDeEstados.JuegoGanado:
                nivelTransicionPanel.SetActive(false); //Para que no se muestren los dos paneles juntos
                ganastePanel.SetActive(true);

                //Audio
                if (audiosource && sonidoGanar)
                {
                    audiosource.PlayOneShot(sonidoGanar);
                }
                break;
            default:
                break;
        }

        //Para que se detenga el juego al ganar o al perder
        Time.timeScale = 0;
    }

    public enum MaquinaDeEstados
    {
        Jugando,
        JuegoTerminado,
        JuegoGanado
    }
}
