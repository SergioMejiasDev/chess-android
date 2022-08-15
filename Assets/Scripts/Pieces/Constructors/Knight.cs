using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contiene las constantes y métodos necesarios para permitir el movimiento del caballo.
/// </summary>
public class Knight
{
    /// <summary>
    /// La posición de la pieza.
    /// </summary>
    readonly Vector2 position;
    /// <summary>
    /// El color de la pieza.
    /// </summary>
    readonly Pieces.Colour colour;

    public Knight(Vector2 position, Pieces.Colour colour)
    {
        this.position = position;
        this.colour = colour;
    }

    /// <summary>
    /// Lista de movimientos que puede realizar la pieza antes de filtrar los movimientos bloqueados.
    /// </summary>
    public List<Vector2> MovePositions
    {
        get
        {
            if (colour == Pieces.Colour.White)
            {
                // Si juegan las blancas y el rey blanco está en jaque, eliminamos todas las posiciones que no eviten el jaque mate.

                if (Chess.WhiteKingInCheck)
                {
                    List<Vector2> tempList = GetMovePositionsWhite();

                    for (int i = 0; i < tempList.Count; i++)
                    {
                        if (!Chess.VerifyBlackMenacedPosition(tempList[i]))
                        {
                            tempList.Remove(tempList[i]);

                            i--;
                        }
                    }

                    return tempList;
                }

                // Si no es el caso, devolvemos la lista original de movimientos.

                return GetMovePositionsWhite();
            }

            else
            {
                // Si juegan las negras y el rey negro está en jaque, eliminamos todas las posiciones que no eviten el jaque mate.

                if (Chess.BlackKingInCheck)
                {
                    List<Vector2> tempList = GetMovePositionsBlack();

                    for (int i = 0; i < tempList.Count; i++)
                    {
                        if (!Chess.VerifyWhiteMenacedPosition(tempList[i]))
                        {
                            tempList.Remove(tempList[i]);

                            i--;
                        }
                    }

                    return tempList;
                }

                // Si no es el caso, devolvemos la lista original de movimientos.

                return GetMovePositionsBlack();
            }
        }
    }

    /// <summary>
    /// Lista de posiciones donde la pieza puede capturar a otra.
    /// </summary>
    public List<Vector2> PositionsInCheck => (colour == Pieces.Colour.White) ? GetPositionsInCheckWhite() : GetPositionsInCheckBlack();

    /// <summary>
    /// Lista de posiciones donde la pieza es una amenaza para el rey opuesto.
    /// </summary>
    public List<Vector2> MenacingPositions => (colour == Pieces.Colour.White) ? GetMenacingPositionsWhite() : GetMenacingPositionsBlack();

    /// <summary>
    /// El valor de la pieza en la posición actual.
    /// </summary>
    public int PositionValue => (colour == Pieces.Colour.White) ? GetPositionValueWhite() : GetPositionValueBlack();

    /// <summary>
    /// Lista de movimientos que puede realizar la pieza blanca.
    /// Esta lista de movimientos no ha sido filtrada para eliminar los movimientos ilegales.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetMovePositionsWhite()
    {
        List<Vector2> tempList = new List<Vector2>
        {
            new Vector2(position.x + 1, position.y + 2),
            new Vector2(position.x + 2, position.y + 1),
            new Vector2(position.x + 2, position.y - 1),
            new Vector2(position.x + 1, position.y - 2),
            new Vector2(position.x - 1, position.y - 2),
            new Vector2(position.x - 2, position.y - 1),
            new Vector2(position.x - 2, position.y + 1),
            new Vector2(position.x - 1, position.y + 2)
        };

        for (int i = 0; i < tempList.Count; i++)
        {
            if (tempList[i].x < 1 || tempList[i].x > 8 || tempList[i].y < 1 || tempList[i].y > 8 || Chess.CheckSquareWhite(tempList[i]))
            {
                tempList.Remove(tempList[i]);

                i--;
            }
        }

        return tempList;
    }

    /// <summary>
    /// Lista de movimientos que puede realizar la pieza negra.
    /// Esta lista de movimientos no ha sido filtrada para eliminar los movimientos ilegales.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetMovePositionsBlack()
    {
        List<Vector2> tempList = new List<Vector2>
        {
            new Vector2(position.x + 1, position.y + 2),
            new Vector2(position.x + 2, position.y + 1),
            new Vector2(position.x + 2, position.y - 1),
            new Vector2(position.x + 1, position.y - 2),
            new Vector2(position.x - 1, position.y - 2),
            new Vector2(position.x - 2, position.y - 1),
            new Vector2(position.x - 2, position.y + 1),
            new Vector2(position.x - 1, position.y + 2)
        };

        for (int i = 0; i < tempList.Count; i++)
        {
            if (tempList[i].x < 1 || tempList[i].x > 8 || tempList[i].y < 1 || tempList[i].y > 8 || Chess.CheckSquareBlack(tempList[i]))
            {
                tempList.Remove(tempList[i]);

                i--;
            }
        }

        return tempList;
    }

    /// <summary>
    /// Lista de posiciones donde la pieza blanca puede capturar a otra.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetPositionsInCheckWhite()
    {
        List<Vector2> tempList = GetMovePositionsWhite();

        return tempList;
    }

    /// <summary>
    /// Lista de posiciones donde la pieza negra puede capturar a otra.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetPositionsInCheckBlack()
    {
        List<Vector2> tempList = GetMovePositionsBlack();

        return tempList;
    }

    /// <summary>
    /// Lista de posiciones donde la pieza blanca puede realizar un jaque mate.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetMenacingPositionsWhite()
    {
        List<Vector2> tempList = GetMovePositionsWhite();

        for (int i = 0; i < tempList.Count; i++)
        {
            if (Chess.BlackKingPosition == tempList[i])
            {
                return new List<Vector2> { position };
            }
        }

        return new List<Vector2>();
    }

    /// <summary>
    /// Lista de posiciones donde la pieza negra puede realizar un jaque mate.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetMenacingPositionsBlack()
    {
        List<Vector2> tempList = GetMovePositionsBlack();

        for (int i = 0; i < tempList.Count; i++)
        {
            if (Chess.WhiteKingPosition == tempList[i])
            {
                return new List<Vector2> { position };
            }
        }

        return new List<Vector2>();
    }

    /// <summary>
    /// El valor de la pieza blanca en la posición actual.
    /// </summary>
    /// <returns></returns>
    int GetPositionValueWhite()
    {
        int[,] whiteValue =
        {
            { 50, 40, 30, 30, 30, 30, 40, 50 },
            { 40, 20, 00, -05, -05, 00, 20, 40 },
            { 30, -05, -10, -15, -15, -10, -05, 30 },
            { 30, 00, -15, -20, -20, -15, 00, 30 },
            { 30, -05, -15, -20, -20, -15, -05, 30 },
            { 30, 00, -10, -15, -15, -10, 00, 30 },
            { 40, 20, 00, 00, 00, 00, 20, 40 },
            { 50, 40, 30, 30, -30, 30, 40, 50 }
        };

        return -30 + whiteValue[(int)position.y - 1, (int)position.x - 1];
    }

    /// <summary>
    /// El valor de la pieza negra en la posición actual.
    /// </summary>
    /// <returns></returns>
    int GetPositionValueBlack()
    {
        int[,] blackValue =
        {
            { -50, -40, -30, -30, -30, -30, -40, -50 },
            { -40, -20, 00, 00, 00, 00, -20, -40 },
            { -30, 00, 10, 15, 15, 10, 00, -30 },
            { -30, 05, 15, 20, 20, 15, 05, -30 },
            { -30, 00, 15, 20, 20, 15, 00, -30 },
            { -30, 05, 10, 15, 15, 10, 05, -30 },
            { -40, -20, 00, 05, 05, 00, -20, -40 },
            { -50, -40, -30, -30, -30, -30, -40, -50 }
        };

        return 30 + blackValue[(int)position.y - 1, (int)position.x - 1];
    }
}