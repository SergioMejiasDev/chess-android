using System.Collections;
using UnityEngine;

/// <summary>
/// Activa todos los eventos temporales a través de corrutinas. Clase estática que hereda de MonoBehaviour.
/// </summary>
public class TimeEvents : MonoBehaviour
{
    /// <summary>
    /// Singleton de la clase.
    /// </summary>
    public static TimeEvents timeEvents;

    void Awake()
    {
        timeEvents = this;
    }

    /// <summary>
    /// Introduce una espera de un segundo antes de que la IA mueva sus piezas.
    /// Desde aquí se activa la corrutina desde otras clases que no hereden de MonoBehaviour.
    /// </summary>
    public void StartWaitForAI()
    {
        StartCoroutine(WaitForAI());
    }

    /// <summary>
    /// Corrutina que inicia el movimiento de la IA tras una espera de un segundo.
    /// </summary>
    /// <returns>El método "MovePieceAI" del GameManager se activa tras un segundo.</returns>
    IEnumerator WaitForAI()
    {
        Application.targetFrameRate = 120;

        yield return new WaitForSeconds(1.0f);

        Chess.MoveAIPiece();
        Interface.interfaceClass.EnableButtonPause(true);

        Application.targetFrameRate = 60;
    }
}