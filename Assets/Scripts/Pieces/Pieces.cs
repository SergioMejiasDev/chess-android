using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contiene las diferentes variables que son comunes en todos los tipos de pieza.
/// </summary>
[CreateAssetMenu]
public class Pieces : ScriptableObject
{
    /// <summary>
    /// Los posibles colores que puede tener una pieza.
    /// </summary>
    public enum Colour {
        /// <summary>
        /// Pieza negra.
        /// </summary>
        Black,
        /// <summary>
        /// Pieza blanca.
        /// </summary>
        White
    };

    /// <summary>
    /// Los diferentes tipos de piezas en el tablero.
    /// </summary>
    public enum Piece {
        /// <summary>
        /// La pieza es un alfil.
        /// </summary>
        Bishop,
        /// <summary>
        /// La pieza es un rey.
        /// </summary>
        King,
        /// <summary>
        /// La pieza es un caballo.
        /// </summary>
        Knight,
        /// <summary>
        /// La pieza es un peón.
        /// </summary>
        Pawn,
        /// <summary>
        /// La pieza es una reina.
        /// </summary>
        Queen,
        /// <summary>
        /// La pieza es una torre.
        /// </summary>
        Rook
    };

    /// <summary>
    /// Los diferentes ejes en los que se puede mover una pieza.
    /// </summary>
    public enum Directions {
        /// <summary>
        /// Movimiento hacia arriba.
        /// </summary>
        Top,
        /// <summary>
        /// Movimiento hacia la derecha.
        /// </summary>
        Right,
        /// <summary>
        /// Movimiento hacia abajo.
        /// </summary>
        Bottom,
        /// <summary>
        /// Movimiento hacia la izquierda.
        /// </summary>
        Left,
        /// <summary>
        /// Movimiento diagonal arriba-derecha.
        /// </summary>
        TopRight,
        /// <summary>
        /// Movimiento diagonal arriba-izquieda.
        /// </summary>
        TopLeft,
        /// <summary>
        /// Movimiento diagonal abajo-derecha.
        /// </summary>
        BottomRight,
        /// <summary>
        /// Movimiento diagonal abajo-izquierda.
        /// </summary>
        BottomLeft
    };

    /// <summary>
    /// El color de la pieza.
    /// </summary>
    [SerializeField] Colour colour;

    /// <summary>
    /// El tipo de pieza.
    /// </summary>
    [SerializeField] Piece piece;

    /// <summary>
    /// Obtiene el color de la pieza.
    /// </summary>
    /// <returns>El color de la pieza.</returns>
    public Colour GetColour()
    {
        return colour;
    }

    /// <summary>
    /// Obtiene el tipo de pieza.
    /// </summary>
    /// <returns>El tipo de la pieza.</returns>
    public Piece GetPiece()
    {
        return piece;
    }

    /// <summary>
    /// Obtiene la lista de movimientos legales que la pieza puede realizar desde su posición actual.
    /// </summary>
    /// <param name="position">La posición de la pieza.</param>
    /// <param name="firstMove">Indica si la pieza ha realizado ya su primer movimiento.</param>
    /// <returns></returns>
    public List<Vector2> GetMovePositions(Vector2 position, bool firstMove)
    {
        if (piece == Piece.Pawn)
        {
            Pawn constructor = new Pawn(position, firstMove, colour);

            return constructor.MovePositions;
        }

        else if (piece == Piece.Rook)
        {
            Rook constructor = new Rook(position, colour);

            return constructor.MovePositions;
        }

        else if (piece == Piece.Bishop)
        {
            Bishop constructor = new Bishop(position, colour);

            return constructor.MovePositions;
        }

        else if (piece == Piece.Queen)
        {
            Queen constructor = new Queen(position, colour);

            return constructor.MovePositions;
        }

        else if (piece == Piece.Knight)
        {
            Knight constructor = new Knight(position, colour);

            return constructor.MovePositions;
        }

        else
        {
            King constructor = new King(position, firstMove, colour);

            return constructor.MovePositions;
        }
    }

    /// <summary>
    /// Calcula la lista de posiciones donde la pieza puede capturar desde su posición actual.
    /// </summary>
    /// <param name="position">La posición de la pieza.</param>
    /// <param name="firstMove">Indica si la pieza ha realizado ya su primer movimiento.</param>
    /// <returns></returns>
    public List<Vector2> GetPositionsInCheck(Vector2 position, bool firstMove)
    {
        if (piece == Piece.Pawn)
        {
            Pawn constructor = new Pawn(position, firstMove, colour);

            return constructor.PositionsInCheck;
        }

        else if (piece == Piece.Rook)
        {
            Rook constructor = new Rook(position, colour);

            return constructor.PositionsInCheck;
        }

        else if (piece == Piece.Bishop)
        {
            Bishop constructor = new Bishop(position, colour);

            return constructor.PositionsInCheck;
        }

        else if (piece == Piece.Queen)
        {
            Queen constructor = new Queen(position, colour);

            return constructor.PositionsInCheck;
        }

        else if (piece == Piece.Knight)
        {
            Knight constructor = new Knight(position, colour);

            return constructor.PositionsInCheck;
        }

        else
        {
            King constructor = new King(position, firstMove, colour);

            return constructor.PositionsInCheck;
        }
    }

    /// <summary>
    /// Calcula la lista de posiciones donde la pieza es una amenaza en caso de jaque.
    /// Se usa principalmente que el rey opuesto no pueda moverse a las posiciones amenazadas.
    /// </summary>
    /// <param name="position">La posición de la pieza.</param>
    /// <param name="firstMove">Indica si la pieza ha realizado ya su primer movimiento.</param>
    /// <returns></returns>
    public List<Vector2> GetMenacingPositions(Vector2 position, bool firstMove)
    {
        if (piece == Piece.Pawn)
        {
            Pawn constructor = new Pawn(position, firstMove, colour);

            return constructor.MenacingPositions;
        }

        else if (piece == Piece.Rook)
        {
            Rook constructor = new Rook(position, colour);

            return constructor.MenacingPositions;
        }

        else if (piece == Piece.Bishop)
        {
            Bishop constructor = new Bishop(position, colour);

            return constructor.MenacingPositions;
        }

        else if (piece == Piece.Queen)
        {
            Queen constructor = new Queen(position, colour);

            return constructor.MenacingPositions;
        }

        else if (piece == Piece.Knight)
        {
            Knight constructor = new Knight(position, colour);

            return constructor.MenacingPositions;
        }

        else
        {
            King constructor = new King(position, firstMove, colour);

            return constructor.MenacingPositions;
        }
    }

    /// <summary>
    /// Bloquea los movimientos que puedan causar una situación de jaque al jugador.
    /// </summary>
    /// <param name="position">La posición de la pieza.</param>
    public void ActivateForbiddenPosition(Vector2 position)
    {
        if (piece == Piece.Pawn)
        {
            return;
        }

        else if (piece == Piece.Rook)
        {
            Rook constructor = new Rook(position, colour);

            constructor.ActivateForbiddenPositions();
        }

        else if (piece == Piece.Bishop)
        {
            Bishop constructor = new Bishop(position, colour);

            constructor.ActivateForbiddenPositions();
        }

        else if (piece == Piece.Queen)
        {
            Queen constructor = new Queen(position, colour);

            constructor.ActivateForbiddenPositions();
        }

        else if (piece == Piece.Knight)
        {
            return;
        }

        else
        {
            return;
        }
    }

    /// <summary>
    /// Busca el valor relativo de la pieza en la posición actual.
    /// Este valor es usado por la IA para elegir su próximo movimiento.
    /// </summary>
    /// <param name="position">La posición de la pieza.</param>
    /// <param name="firstMove">Indica si la pieza ha realizado ya su primer movimiento.</param>
    /// <returns></returns>
    public int GetValue(Vector2 position, bool firstMove)
    {
        if (piece == Piece.Pawn)
        {
            Pawn constructor = new Pawn(position, firstMove, colour);

            return constructor.PositionValue;
        }

        else if (piece == Piece.Rook)
        {
            Rook constructor = new Rook(position, colour);

            return constructor.PositionValue;
        }

        else if (piece == Piece.Bishop)
        {
            Bishop constructor = new Bishop(position, colour);

            return constructor.PositionValue;
        }

        else if (piece == Piece.Queen)
        {
            Queen constructor = new Queen(position, colour);

            return constructor.PositionValue;
        }

        else if (piece == Piece.Knight)
        {
            Knight constructor = new Knight(position, colour);

            return constructor.PositionValue;
        }

        else
        {
            King constructor = new King(position, firstMove, colour);

            return constructor.PositionValue;
        }
    }
}