using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que hereda de MonoBehaviour y que se añadirá a cada pieza. Contiene los métodos necesarios para realizar sus movimientos.
/// </summary>
public class PiecesMovement : MonoBehaviour
{
    /// <summary>
    /// El color de la pieza.
    /// </summary>
    public Pieces.Colour PieceColour { get; private set; }

    /// <summary>
    /// El tipo de la pieza.
    /// </summary>
    public Pieces.Piece PieceType { get; private set; }

    /// <summary>
    /// Indica si la pieza ya se ha movido por primera vez.
    /// </summary>
    public bool FirstMove { get; set; } = false;

    /// <summary>
    /// Asset con las diferentes variables y movimientos de la pieza.
    /// </summary>
    [SerializeField] Pieces variables;

    /// <summary>
    /// Los movimientos hacia arriba están bloqueados.
    /// </summary>
    bool lockTop = false;

    /// <summary>
    /// Los movimientos hacia la derecha están bloqueados.
    /// </summary>
    bool lockRight = false;

    /// <summary>
    /// Los movimientos hacia abajo están bloqueados.
    /// </summary>
    bool lockBottom = false;

    /// <summary>
    /// Los movimientos hacia la izquierda están bloqueados.
    /// </summary>
    bool lockLeft = false;

    /// <summary>
    /// Los movimientos diagonales arriba-derecha están bloqueados.
    /// </summary>
    bool lockTopRight = false;

    /// <summary>
    /// Los movimientos diagonales arriba-izquierda están bloqueados.
    /// </summary>
    bool lockTopLeft = false;

    /// <summary>
    /// Los movimientos diagonales abajo-derecha están bloqueados.
    /// </summary>
    bool lockBottomRight = false;

    /// <summary>
    /// Los movimientos diagonales abajo-izquierda están bloqueados.
    /// </summary>
    bool lockBottomLeft = false;

    void OnEnable()
    {
        PieceColour = variables.GetColour();
        PieceType = variables.GetPiece();
    }

    /// <summary>
    /// El valor de la pieza en su posición actual.
    /// </summary>
    public int Value
    {
        get
        {
            if (!gameObject.activeSelf)
            {
                return -variables.GetValue(transform.position, FirstMove);
            }

            return variables.GetValue(transform.position, FirstMove);
        }
    }

    /// <summary>
    /// Bloquea los movimientos de la pieza en la dirección indicada.
    /// </summary>
    /// <param name="direction">La dirección que se va a bloquear.</param>
    public void EnableLock(Pieces.Directions direction)
    {
        switch (direction)
        {
            case Pieces.Directions.Top:
                lockTop = true;
                break;
            case Pieces.Directions.Right:
                lockRight = true;
                break;
            case Pieces.Directions.Bottom:
                lockBottom = true;
                break;
            case Pieces.Directions.Left:
                lockLeft = true;
                break;
            case Pieces.Directions.TopRight:
                lockTopRight = true;
                break;
            case Pieces.Directions.TopLeft:
                lockTopLeft = true;
                break;
            case Pieces.Directions.BottomRight:
                lockBottomRight = true;
                break;
            case Pieces.Directions.BottomLeft:
                lockBottomLeft = true;
                break;
        }
    }

    /// <summary>
    /// Desbloquea todos los movimientos bloqueados.
    /// </summary>
    public void DisableLock()
    {
        lockTop = false;
        lockRight = false;
        lockBottom = false;
        lockLeft = false;
        lockTopLeft = false;
        lockTopRight = false;
        lockBottomLeft = false;
        lockBottomRight = false;
    }

    /// <summary>
    /// Calcula todos los movimientos legales que puede realizar la pieza.
    /// </summary>
    /// <returns>La lista de movimientos posibles.</returns>
    public List<Vector2> SearchGreenPositions()
    {
        // Si la pieza está desactivada (ocurre en las partidas contra la IA), no hay movimientos posibles.
        // Devolvemos una lista vacía.

        if (!gameObject.activeSelf)
        {
            return new List<Vector2>();
        }

        // Calculamos una lista con los posibles movimientos para el tipo específico de pieza.

        List<Vector2> tempList = variables.GetMovePositions(transform.position, FirstMove);

        // Para la lista obtenida, eliminamos las posiciones bloqueadas.

        for (int i = 0; i < tempList.Count; i++)
        {
            if (lockTop && tempList[i].y > transform.position.y && tempList[i].x == transform.position.x)
            {
                tempList.Remove(tempList[i]);
                i--;

                continue;
            }

            if (lockRight && tempList[i].x > transform.position.x && tempList[i].y == transform.position.y)
            {
                tempList.Remove(tempList[i]);
                i--;

                continue;
            }

            if (lockBottom && tempList[i].y < transform.position.y && tempList[i].x == transform.position.x)
            {
                tempList.Remove(tempList[i]);
                i--;

                continue;
            }

            if (lockLeft && tempList[i].x < transform.position.x && tempList[i].y == transform.position.y)
            {
                tempList.Remove(tempList[i]);
                i--;

                continue;
            }

            if (lockTopRight && tempList[i].x > transform.position.x && tempList[i].y > transform.position.y)
            {
                tempList.Remove(tempList[i]);
                i--;

                continue;
            }

            if (lockTopLeft && tempList[i].x < transform.position.x && tempList[i].y > transform.position.y)
            {
                tempList.Remove(tempList[i]);
                i--;

                continue;
            }

            if (lockBottomRight && tempList[i].x > transform.position.x && tempList[i].y < transform.position.y)
            {
                tempList.Remove(tempList[i]);
                i--;

                continue;
            }

            if (lockBottomLeft && tempList[i].x < transform.position.x && tempList[i].y < transform.position.y)
            {
                tempList.Remove(tempList[i]);
                i--;

                continue;
            }
        }

        return tempList;
    }

    /// <summary>
    /// Calcula todas las posiciones donde la pieza puede capturar.
    /// </summary>
    /// <returns>La lista de posiciones donde la pieza puede capturar a otra pieza.</returns>
    public List<Vector2> GetPositionsInCheck()
    {
        // Si la pieza está desactivada (ocurre en las partidas contra la IA), no hay movimientos posibles.
        // Devolvemos una lista vacía.

        if (!gameObject.activeSelf)
        {
            return new List<Vector2>();
        }

        return variables.GetPositionsInCheck(transform.position, FirstMove);
    }

    /// <summary>
    /// Calcula las posiciones donde la pieza puede producir un jaque al rey opuesto.
    /// </summary>
    /// <returns>La lista de las posiciones donde la pieza puede realizar un jaque.</returns>
    public List<Vector2> GetMenacingPositions()
    {
        // Si la pieza está desactivada (ocurre en las partidas contra la IA), no hay movimientos posibles.
        // Devolvemos una lista vacía.

        if (!gameObject.activeSelf)
        {
            return new List<Vector2>();
        }

        return variables.GetMenacingPositions(transform.position, FirstMove);
    }

    /// <summary>
    /// Bloquea todos los movimientos de la pieza que pueden producir un jaque al rey de su color.
    /// </summary>
    public void ActivateForbiddenPositions()
    {
        // Si la pieza está desactivada (ocurre en las partidas contra la IA), no hay movimientos posibles.

        if (!gameObject.activeSelf)
        {
            return;
        }

        variables.ActivateForbiddenPosition(transform.position);
    }
}