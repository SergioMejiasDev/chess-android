using UnityEngine;

/// <summary>
/// Contiene los datos de la pieza que se va a mover y la posición a la que se va a mover. Es usado por la IA del juego para elegir un movimiento.
/// </summary>
public class AIMovePosition
{
    /// <summary>
    /// La pieza que se va a mover.
    /// </summary>
    public readonly GameObject piece;

    /// <summary>
    /// La posición a la que la pieza se va a mover.
    /// </summary>
    public readonly Vector2 position;

    public AIMovePosition(GameObject piece, Vector2 position)
    {
        this.piece = piece;
        this.position = position;
    }
}