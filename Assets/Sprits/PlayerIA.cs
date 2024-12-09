using UnityEngine;
using TMPro;

public class PlayerIA : MonoBehaviour
{
    private TMP_Text puntosTexto;

    private void Awake()
    {
        puntosTexto = GetComponent<TMP_Text>();
    }

    public void ActualizarPuntos(int puntos)
    {
        puntosTexto.text = "Peces Comidos: " + puntos.ToString();
    }
}
