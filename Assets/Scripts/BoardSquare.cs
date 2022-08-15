using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Nos permite elegir las diferentes casillas del tablero.
/// </summary>
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class BoardSquare : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// Indica si la casilla está seleccionada en este momento.
    /// </summary>
    bool isSelected = false;

    /// <summary>
    /// Indica si la casilla puede ser seleccionada en este momento.
    /// </summary>
    bool selectable = true;

    /// <summary>
    /// Bloquea la casilla para que no pueda ser seleccionada (fin de partida o menú principal).
    /// </summary>
    bool locked = true;

    /// <summary>
    /// El color inicial de la casilla (blanco o negro).
    /// </summary>
    Color initialColour;

    /// <summary>
    /// El componente SpriteRenderer de la casilla.
    /// </summary>
    SpriteRenderer sr = null;

    private void Awake()
    {
        // Inicializamos el Sprite Renderer y guardamos el color inicial de la casilla para que no se pierda cuando realicemos cambios.

        sr = GetComponent<SpriteRenderer>();
        initialColour = sr.color;
    }

    private void OnEnable()
    {
        // Nos suscribimos a los diferentes delegados que nos permitirán hacer cambios en la casilla a través del Game Manager.

        Chess.UpdateColour += UpdateColour;
        Chess.RedSquare += ActivateRedColour;
        Chess.OriginalColour += ResetColour;
        Chess.EnableSelection += UnlockSquare;
        Chess.DisableSelection += LockSquare;
    }

    /// <summary>
    /// Se activa cuando pulsamos sobre una casilla.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        // Si la casilla está bloqueada o no es el turno del jugador, evitamos que sea seleccionada.

        if (locked || !Chess.CheckTurn())
        {
            return;
        }

        // Si puede ser seleccionada y hay una pieza sobre ella, la seleccionamos.

        if (selectable && Chess.SelectPiece(transform.position))
        {
            selectable = false;
            isSelected = true;
        }

        // Si la casilla estaba seleccionada cuando hemos pulsado en ella, la deseleccionamos.

        else if (isSelected)
        {
            Chess.DeselectPosition();
        }

        // Si la casilla es verde (la pieza seleccionada puede moverse a esta posición), realizamos el movimiento.

        else if (sr.color == Color.green)
        {
            // Si no estamos jugando en una partida online, movemos la pieza a la casilla seleccionada.

            if (!NetworkManager.manager.IsConnected)
            {
                Chess.MovePiece(transform.position);
            }

            // Si la partida es online, hacemos lo mismo pero enviando los datos al servidor.

            else
            {
                NetworkManager.manager.MovePiece(Chess.ActivePiecePosition, transform.position);
            }
        }
    }

    /// <summary>
    /// Bloquea la casilla para que no pueda ser seleccionada.
    /// </summary>
    void LockSquare()
    {
        locked = true;
    }

    /// <summary>
    /// Desbloquea la casilla para que pueda ser seleccionada.
    /// </summary>
    void UnlockSquare()
    {
        locked = false;
    }

    /// <summary>
    /// La casilla vuelve a su color original, permitiendo su selección si se cumplen las condiciones.
    /// </summary>
    void ResetColour()
    {
        sr.color = initialColour;
        isSelected = false;
        selectable = true;
    }

    /// <summary>
    /// Actualiza el color de la casilla dependiendo de los parámetros recibidos.
    /// </summary>
    /// <param name="piecePosition">Posición que ha sido seleccionada (hay una pieza sobre ella).</param>
    /// <param name="greenPositions">Posiciones a las que puede moverse la pieza seleccionada.</param>
    void UpdateColour(Vector2 piecePosition, List<Vector2> greenPositions)
    {
        // Si la casilla está seleccionada (hay una pieza sobre ella), se vuelve amarilla.

        if (piecePosition == (Vector2)transform.position)
        {
            sr.color = Color.yellow;

            return;
        }

        // Si se puede mover la pieza seleccionada a esta casilla, se vuelve verde.

        for (int i = 0; i < greenPositions.Count; i++)
        {
            if (greenPositions[i] == (Vector2)transform.position)
            {
                sr.color = Color.green;

                return;
            }
        }
    }

    /// <summary>
    /// Se activa cuando una pieza se mueve a otra casilla.
    /// Si se acaba de mover una pieza a esta casilla, activa la corrutina que la vuelve roja.
    /// </summary>
    /// <param name="position">La posición a la que se acaba de mover la pieza.</param>
    public void ActivateRedColour(Vector2 position, List<Vector2> list)
    {
        if (transform.position.Equals(position))
        {
            StartCoroutine(RedColour());
        }
    }

    /// <summary>
    /// Vuelve la casilla roja durante un momento justo después de moverse una pieza sobre ella.
    /// </summary>
    /// <returns></returns>
    IEnumerator RedColour()
    {
        sr.color = Color.red;

        yield return new WaitForSeconds(0.75f);

        sr.color = initialColour;
    }
}