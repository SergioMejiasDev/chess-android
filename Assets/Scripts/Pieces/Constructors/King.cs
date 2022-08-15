using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contiene las constantes y métodos necesarios para permitir el movimiento del rey.
/// </summary>
public class King
{
    /// <summary>
    /// La posición de la pieza.
    /// </summary>
    readonly Vector2 position;
    /// <summary>
    /// Indica si la pieza se ha movido por primera vez.
    /// </summary>
    readonly bool firstMove;
    /// <summary>
    /// El color de la pieza.
    /// </summary>
    readonly Pieces.Colour colour;

    public King(Vector2 position, bool firstMove, Pieces.Colour colour)
    {
        this.position = position;
        this.firstMove = firstMove;
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
                List<Vector2> tempList = GetMovePositionsWhite();

                for (int i = 0; i < tempList.Count; i++)
                {
                    if (Chess.VerifyBlackCheckPosition(tempList[i]))
                    {
                        tempList.Remove(tempList[i]);

                        i--;
                    }
                }

                return tempList;
            }

            else
            {
                List<Vector2> tempList = GetMovePositionsBlack();

                for (int i = 0; i < tempList.Count; i++)
                {
                    if (Chess.VerifyWhiteCheckPosition(tempList[i]))
                    {
                        tempList.Remove(tempList[i]);

                        i--;
                    }
                }

                return tempList;
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
    public int PositionValue
    {
        get
        {
            bool whiteQueen = false;
            bool blackQueen = false;
            int whitePieces = 0;
            int blackPieces = 0;

            for (int i = 0; i < Chess.PiecesWhite.Count; i++)
            {
                if (Chess.PiecesWhite[i].activeSelf)
                {
                    whitePieces++;

                    if (Chess.PiecesWhite[i].GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Queen)
                    {
                        whiteQueen = true;
                    }
                }
            }

            for (int i = 0; i < Chess.PiecesBlack.Count; i++)
            {
                if (Chess.PiecesBlack[i].activeSelf)
                {
                    blackPieces++;

                    if (Chess.PiecesBlack[i].GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Queen)
                    {
                        blackQueen = true;
                    }
                }
            }

            if (colour == Pieces.Colour.White)
            {
                if ((!whiteQueen & !blackQueen) || (whiteQueen && whitePieces <= 3))
                {
                    return GetPositionValueWhiteEnd();
                }

                return GetPositionValueWhite();
            }

            else
            {
                if ((!whiteQueen & !blackQueen) || (blackQueen && blackPieces <= 3))
                {
                    return GetPositionValueBlackEnd();
                }

                return GetPositionValueBlack();
            }
        }
    }

    /// <summary>
    /// Lista de movimientos que puede realizar la pieza blanca.
    /// Esta lista de movimientos no ha sido filtrada para eliminar los movimientos ilegales.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetMovePositionsWhite()
    {
        List<Vector2> tempList = new List<Vector2>
        {
            new Vector2(position.x, position.y + 1),
            new Vector2(position.x + 1, position.y + 1),
            new Vector2(position.x + 1, position.y),
            new Vector2(position.x + 1, position.y - 1),
            new Vector2(position.x, position.y - 1),
            new Vector2(position.x - 1, position.y - 1),
            new Vector2(position.x - 1, position.y),
            new Vector2(position.x - 1, position.y + 1)
        };

        for (int i = 0; i < tempList.Count; i++)
        {
            if (tempList[i].x < 1 || tempList[i].x > 8 || tempList[i].y < 1 || tempList[i].y > 8 || Chess.CheckSquareWhite(tempList[i]))
            {
                tempList.Remove(tempList[i]);

                i--;
            }
        }

        if (!firstMove)
        {
            if (Chess.CastlingLeft(colour))
            {
                tempList.Add(new Vector2(3, 1));
            }

            if (Chess.CastlingRight(colour))
            {
                tempList.Add(new Vector2(7, 1));
            }
        }

        for (int i = 0; i < tempList.Count; i++)
        {
            if (Chess.VerifyBlackCheckPosition(tempList[i]))
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
            new Vector2(position.x, position.y + 1),
            new Vector2(position.x + 1, position.y + 1),
            new Vector2(position.x + 1, position.y),
            new Vector2(position.x + 1, position.y - 1),
            new Vector2(position.x, position.y - 1),
            new Vector2(position.x - 1, position.y - 1),
            new Vector2(position.x - 1, position.y),
            new Vector2(position.x - 1, position.y + 1)
        };

        for (int i = 0; i < tempList.Count; i++)
        {
            if (tempList[i].x < 1 || tempList[i].x > 8 || tempList[i].y < 1 || tempList[i].y > 8 || Chess.CheckSquareBlack(tempList[i]))
            {
                tempList.Remove(tempList[i]);

                i--;
            }
        }

        if (!firstMove)
        {
            if (Chess.CastlingLeft(colour))
            {
                tempList.Add(new Vector2(3, 8));
            }

            if (Chess.CastlingRight(colour))
            {
                tempList.Add(new Vector2(7, 8));
            }
        }

        for (int i = 0; i < tempList.Count; i++)
        {
            if (Chess.VerifyWhiteCheckPosition(tempList[i]))
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
        List<Vector2> tempList = new List<Vector2>
        {
            new Vector2(position.x, position.y + 1),
            new Vector2(position.x + 1, position.y + 1),
            new Vector2(position.x + 1, position.y),
            new Vector2(position.x + 1, position.y - 1),
            new Vector2(position.x, position.y - 1),
            new Vector2(position.x - 1, position.y - 1),
            new Vector2(position.x - 1, position.y),
            new Vector2(position.x - 1, position.y + 1)
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
    /// Lista de posiciones donde la pieza negra puede capturar a otra.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetPositionsInCheckBlack()
    {
        List<Vector2> tempList = new List<Vector2>
        {
            new Vector2(position.x, position.y + 1),
            new Vector2(position.x + 1, position.y + 1),
            new Vector2(position.x + 1, position.y),
            new Vector2(position.x + 1, position.y - 1),
            new Vector2(position.x, position.y - 1),
            new Vector2(position.x - 1, position.y - 1),
            new Vector2(position.x - 1, position.y),
            new Vector2(position.x - 1, position.y + 1)
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
    /// Lista de posiciones donde la pieza blanca puede realizar un jaque mate.
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetMenacingPositionsWhite()
    {
        List<Vector2> tempList = new List<Vector2>
        {
            new Vector2(position.x, position.y + 1),
            new Vector2(position.x + 1, position.y + 1),
            new Vector2(position.x + 1, position.y),
            new Vector2(position.x + 1, position.y - 1),
            new Vector2(position.x, position.y - 1),
            new Vector2(position.x - 1, position.y - 1),
            new Vector2(position.x - 1, position.y),
            new Vector2(position.x - 1, position.y + 1)
        };

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
        List<Vector2> tempList = new List<Vector2>
        {
            new Vector2(position.x, position.y + 1),
            new Vector2(position.x + 1, position.y + 1),
            new Vector2(position.x + 1, position.y),
            new Vector2(position.x + 1, position.y - 1),
            new Vector2(position.x, position.y - 1),
            new Vector2(position.x - 1, position.y - 1),
            new Vector2(position.x - 1, position.y),
            new Vector2(position.x - 1, position.y + 1)
        };

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
            { -20, -30, -10, 00, 00, -10, -30, -20 },
            { -20, -20, 00, 00, 00, 00, -20, -20 },
            { 10, 20, 20, 20, 20, 20, 20, 10 },
            { 20, 30, 30, 40, 40, 30, 30, 20 },
            { 30, 40, 40, 50, 50, 40, 40, 30 },
            { 30, 40, 40, 50, 50, 40, 40, 30 },
            { 30, 40, 40, 50, 50, 40, 40, 30 },
            { 30, 40, 40, 50, 50, 40, 40, 30 }
        };

        return -900 + whiteValue[(int)position.y - 1, (int)position.x - 1];
    }

    /// <summary>
    /// El valor de la pieza negra en la posición actual.
    /// </summary>
    /// <returns></returns>
    int GetPositionValueBlack()
    {
        int[,] blackValue =
        {
            { 30, 40, 40, 50, 50, 40, 40, 30 },
            { 30, 40, 40, 50, 50, 40, 40, 30 },
            { 30, 40, 40, 50, 50, 40, 40, 30 },
            { 30, 40, 40, 50, 50, 40, 40, 30 },
            { 20, 30, 30, 40, 40, 30, 30, 20 },
            { 10, 20, 20, 20, 20, 20, 20, 10 },
            { -20, -20, 00, 00, 00, 00, -20, -20 },
            { -20, -30, -10, 00, 00, -10, -30, -20 }
        };

        return 900 + blackValue[(int)position.y - 1, (int)position.x - 1];
    }

    /// <summary>
    /// Valor alternativo para la posición del rey blanco cuando la partida está más avanzada.
    /// </summary>
    /// <returns></returns>
    int GetPositionValueWhiteEnd()
    {
        int[,] whiteValueEnd =
        {
            { -50, -30, -30, -30, -30, -30, -30, -50 },
            { -30, -30, 00, 00, 00, 00, -30, -30 },
            { -30, -10, 20, 30, 30, 20, -10, -30 },
            { -30, -10, 30, 40, 40, 30, -10, -30 },
            { -30, -10, 30, 40, 40, 30, -10, -30 },
            { -30, -10, 20, 30, 30, 20, -10, -30 },
            { -30, -20, -10, 00, 00, -10, -20, -30 },
            { -50, -40, -30, -20, -20, -30, -40, -50 }
        };

        return -900 + whiteValueEnd[(int)position.y - 1, (int)position.x - 1];
    }

    /// <summary>
    /// Valor alternativo para la posición del rey negro cuando la partida está más avanzada.
    /// </summary>
    /// <returns></returns>
    int GetPositionValueBlackEnd()
    {
        int[,] blackValueEnd =
        {
            { 50, 40, 30, 20, 20, 30, 40, 50 },
            { 30, 20, 10, 00, 00, 10, 20, 30 },
            { 30, 10, -20, -30, -30, -20, 10, 30 },
            { 30, 10, -30, -40, -40, -30, 10, 30 },
            { 30, 10, -30, -40, -40, -30, 10, 30 },
            { 30, 10, -20, -30, -30, -20, 10, 30 },
            { 30, 30, 00, 00, 00, 00, 30, 30 },
            { 50, 30, 30, 30, 30, 30, 30, 50 }
        };

        return 900 + blackValueEnd[(int)position.y - 1, (int)position.x - 1];
    }
}