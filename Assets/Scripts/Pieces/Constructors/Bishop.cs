using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contiene las constantes y métodos necesarios para permitir el movimiento del alfil.
/// </summary>
public class Bishop
{
    /// <summary>
    /// La posición de la pieza.
    /// </summary>
    readonly Vector2 position;
    /// <summary>
    /// El color de la pieza.
    /// </summary>
    readonly Pieces.Colour colour;

    public Bishop(Vector2 position, Pieces.Colour colour)
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
    /// Bloquea los movimientos de las piezas cercanas que puedan producir que esta pieza haga un jaque mate.
    /// </summary>
    public void ActivateForbiddenPositions()
    {
        if (colour == Pieces.Colour.White)
        {
            SetForbiddenPositionsWhite();
        }

        else
        {
            SetForbiddenPositionsBlack();
        }
    }

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
        List<Vector2> tempList = new List<Vector2>();

        int loops = 0;

        // Diagonal arriba-derecha.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // Si hay una pieza blanca en la posición, o se sale del tablero, no se continua mirando en esta dirección.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y + loops)) || position.y + loops > 8)
            {
                break;
            }

            // Si la casilla está vacía, la añadimos a la lista de movimientos posibles y continuamos mirando en esta dirección.

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));
            }

            // Si hay una pieza negra en la casilla, la añadimos a la lista de movimientos y dejamos de mirar en esta dirección.

            else
            {
                tempList.Add(new Vector2(i, position.y + loops));

                break;
            }
        }

        loops = 0;

        // Diagonal abajo-derecha.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // Si hay una pieza blanca en la posición, o se sale del tablero, no se continua mirando en esta dirección.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y - loops)) || position.y - loops < 1)
            {
                break;
            }

            // Si la casilla está vacía, la añadimos a la lista de movimientos posibles y continuamos mirando en esta dirección.

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));
            }

            // Si hay una pieza negra en la casilla, la añadimos a la lista de movimientos y dejamos de mirar en esta dirección.

            else
            {
                tempList.Add(new Vector2(i, position.y - loops));

                break;
            }
        }

        loops = 0;

        // Diagonal arriba-izquierda.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // Si hay una pieza blanca en la posición, o se sale del tablero, no se continua mirando en esta dirección.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y + loops)) || position.y + loops > 8)
            {
                break;
            }

            // Si la casilla está vacía, la añadimos a la lista de movimientos posibles y continuamos mirando en esta dirección.

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));
            }

            // Si hay una pieza negra en la casilla, la añadimos a la lista de movimientos y dejamos de mirar en esta dirección.

            else
            {
                tempList.Add(new Vector2(i, position.y + loops));

                break;
            }
        }

        loops = 0;

        // Diagonal abajo-izquierda.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // Si hay una pieza blanca en la posición, o se sale del tablero, no se continua mirando en esta dirección.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y - loops)) || position.y - loops < 1)
            {
                break;
            }

            // Si la casilla está vacía, la añadimos a la lista de movimientos posibles y continuamos mirando en esta dirección.

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));
            }

            // Si hay una pieza negra en la casilla, la añadimos a la lista de movimientos y dejamos de mirar en esta dirección.

            else
            {
                tempList.Add(new Vector2(i, position.y - loops));

                break;
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
        List<Vector2> tempList = new List<Vector2>();

        int loops = 0;

        // Diagonal arriba-derecha.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // Si hay una pieza negra en la posición, o se sale del tablero, no se continua mirando en esta dirección.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y + loops)) || position.y + loops > 8)
            {
                break;
            }

            // Si la casilla está vacía, la añadimos a la lista de movimientos posibles y continuamos mirando en esta dirección.

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));
            }

            // Si hay una pieza blanca en la casilla, la añadimos a la lista de movimientos y dejamos de mirar en esta dirección.

            else
            {
                tempList.Add(new Vector2(i, position.y + loops));

                break;
            }
        }

        loops = 0;

        // Diagonal abajo-derecha.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // Si hay una pieza negra en la posición, o se sale del tablero, no se continua mirando en esta dirección.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y - loops)) || position.y - loops < 1)
            {
                break;
            }

            // Si la casilla está vacía, la añadimos a la lista de movimientos posibles y continuamos mirando en esta dirección.

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));
            }

            // Si hay una pieza blanca en la casilla, la añadimos a la lista de movimientos y dejamos de mirar en esta dirección.

            else
            {
                tempList.Add(new Vector2(i, position.y - loops));

                break;
            }
        }

        loops = 0;

        // Diagonal arriba-izquierda.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // Si hay una pieza negra en la posición, o se sale del tablero, no se continua mirando en esta dirección.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y + loops)) || position.y + loops > 8)
            {
                break;
            }

            // Si la casilla está vacía, la añadimos a la lista de movimientos posibles y continuamos mirando en esta dirección.

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));
            }

            // Si hay una pieza blanca en la casilla, la añadimos a la lista de movimientos y dejamos de mirar en esta dirección.

            else
            {
                tempList.Add(new Vector2(i, position.y + loops));

                break;
            }
        }

        loops = 0;

        // Diagonal abajo-izquierda.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // Si hay una pieza negra en la posición, o se sale del tablero, no se continua mirando en esta dirección.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y - loops)) || position.y - loops < 1)
            {
                break;
            }

            // Si la casilla está vacía, la añadimos a la lista de movimientos posibles y continuamos mirando en esta dirección.

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));
            }

            // Si hay una pieza blanca en la casilla, la añadimos a la lista de movimientos y dejamos de mirar en esta dirección.

            else
            {
                tempList.Add(new Vector2(i, position.y - loops));

                break;
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
        List<Vector2> tempList = new List<Vector2>();

        int loops = 0;

        // Diagonal arriba-derecha.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // Si hay una pieza blanca en la casilla, la añadimos a la lista y no continuamos mirando en esta dirección.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));

                break;
            }

            // Si la casilla está vacía, la añadimos a la lista de movimientos posibles y continuamos mirando en esta dirección.

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));
            }

            // Si hay una pieza negra en la casilla, la añadimos a la lista y no continuamos mirando en esta dirección.

            else
            {
                tempList.Add(new Vector2(i, position.y + loops));

                if (Chess.BlackKingPosition != new Vector2(i, position.y + loops))
                {
                    break;
                }
            }
        }

        loops = 0;

        // Diagonal abajo-derecha.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // Si hay una pieza blanca en la casilla, la añadimos a la lista y no continuamos mirando en esta dirección.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));

                break;
            }

            // Si la casilla está vacía, la añadimos a la lista de movimientos posibles y continuamos mirando en esta dirección.

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));
            }

            // Si hay una pieza negra en la casilla, la añadimos a la lista y no continuamos mirando en esta dirección.

            else
            {
                tempList.Add(new Vector2(i, position.y - loops));

                if (Chess.BlackKingPosition != new Vector2(i, position.y - loops))
                {
                    break;
                }
            }
        }

        loops = 0;

        // Diagonal arriba-izquierda.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // Si hay una pieza blanca en la casilla, la añadimos a la lista y no continuamos mirando en esta dirección.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));

                break;
            }

            // Si la casilla está vacía, la añadimos a la lista de movimientos posibles y continuamos mirando en esta dirección.

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));
            }

            // Si hay una pieza negra en la casilla, la añadimos a la lista y no continuamos mirando en esta dirección.

            else
            {
                tempList.Add(new Vector2(i, position.y + loops));

                if (Chess.BlackKingPosition != new Vector2(i, position.y + loops))
                {
                    break;
                }
            }
        }

        loops = 0;

        // Diagonal abajo-izquierda.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // Si hay una pieza blanca en la casilla, la añadimos a la lista y no continuamos mirando en esta dirección.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));

                break;
            }

            // Si la casilla está vacía, la añadimos a la lista de movimientos posibles y continuamos mirando en esta dirección.

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));
            }

            // Si hay una pieza negra en la casilla, la añadimos a la lista y no continuamos mirando en esta dirección.

            else
            {
                tempList.Add(new Vector2(i, position.y - loops));

                if (Chess.BlackKingPosition != new Vector2(i, position.y - loops))
                {
                    break;
                }
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
        List<Vector2> tempList = new List<Vector2>();

        int loops = 0;

        // Diagonal arriba-derecha.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // Si hay una pieza negra en la casilla, la añadimos a la lista y no continuamos mirando en esta dirección.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));

                break;
            }

            // Si la casilla está vacía, la añadimos a la lista de movimientos posibles y continuamos mirando en esta dirección.

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));
            }

            // Si hay una pieza blanca en la casilla, la añadimos a la lista y no continuamos mirando en esta dirección.

            else
            {
                tempList.Add(new Vector2(i, position.y + loops));

                if (Chess.WhiteKingPosition != new Vector2(i, position.y + loops))
                {
                    break;
                }
            }
        }

        loops = 0;

        // Diagonal abajo-derecha.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // Si hay una pieza negra en la casilla, la añadimos a la lista y no continuamos mirando en esta dirección.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));

                break;
            }

            // Si la casilla está vacía, la añadimos a la lista de movimientos posibles y continuamos mirando en esta dirección.

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));
            }

            // Si hay una pieza blanca en la casilla, la añadimos a la lista y no continuamos mirando en esta dirección.

            else
            {
                tempList.Add(new Vector2(i, position.y - loops));

                if (Chess.WhiteKingPosition != new Vector2(i, position.y - loops))
                {
                    break;
                }
            }
        }

        loops = 0;

        // Diagonal arriba-izquierda.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // Si hay una pieza negra en la casilla, la añadimos a la lista y no continuamos mirando en esta dirección.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));

                break;
            }

            // Si la casilla está vacía, la añadimos a la lista de movimientos posibles y continuamos mirando en esta dirección.

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));
            }

            // Si hay una pieza blanca en la casilla, la añadimos a la lista y no continuamos mirando en esta dirección.

            else
            {
                tempList.Add(new Vector2(i, position.y + loops));

                if (Chess.WhiteKingPosition != new Vector2(i, position.y + loops))
                {
                    break;
                }
            }
        }

        loops = 0;

        // Diagonal abajo-izquierda.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // Si hay una pieza negra en la casilla, la añadimos a la lista y no continuamos mirando en esta dirección.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));

                break;
            }

            // Si la casilla está vacía, la añadimos a la lista de movimientos posibles y continuamos mirando en esta dirección.

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));
            }

            // Si hay una pieza blanca en la casilla, la añadimos a la lista y no continuamos mirando en esta dirección.

            else
            {
                tempList.Add(new Vector2(i, position.y - loops));

                if (Chess.WhiteKingPosition != new Vector2(i, position.y - loops))
                {
                    break;
                }
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
        List<Vector2> tempList = new List<Vector2>();

        int loops = 0;

        // Diagonal arriba-derecha.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // Si hay una pieza blanca, no continuamos mirando en esta dirección.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                break;
            }

            // Si la casilla está vacía, la añadimos a la lista y seguimos mirando en esta dirección.

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));
            }

            // Si el rey opuesto está en la casilla, añadimos la posición de nuestra pieza a la lista.

            else
            {
                if (Chess.BlackKingPosition == new Vector2(i, position.y + loops))
                {
                    tempList.Add(position);

                    return tempList;
                }
            }
        }

        loops = 0;
        tempList.Clear();

        // Diagonal abajo-derecha.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // Si hay una pieza blanca, no continuamos mirando en esta dirección.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                break;
            }

            // Si la casilla está vacía, la añadimos a la lista y seguimos mirando en esta dirección.

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));
            }

            // Si el rey opuesto está en la casilla, añadimos la posición de nuestra pieza a la lista.

            else
            {
                if (Chess.BlackKingPosition == new Vector2(i, position.y - loops))
                {
                    tempList.Add(position);

                    return tempList;
                }
            }
        }

        loops = 0;
        tempList.Clear();

        // Diagonal arriba-izquierda.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // Si hay una pieza blanca, no continuamos mirando en esta dirección.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                break;
            }

            // Si la casilla está vacía, la añadimos a la lista y seguimos mirando en esta dirección.

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));
            }

            // Si el rey opuesto está en la casilla, añadimos la posición de nuestra pieza a la lista.

            else
            {
                if (Chess.BlackKingPosition == new Vector2(i, position.y + loops))
                {
                    tempList.Add(position);

                    return tempList;
                }
            }
        }

        loops = 0;
        tempList.Clear();

        // Diagonal abajo-izquierda.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // Si hay una pieza blanca, no continuamos mirando en esta dirección.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                break;
            }

            // Si la casilla está vacía, la añadimos a la lista y seguimos mirando en esta dirección.

            if (!Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));
            }

            // Si el rey opuesto está en la casilla, añadimos la posición de nuestra pieza a la lista.

            else
            {
                if (Chess.BlackKingPosition == new Vector2(i, position.y - loops))
                {
                    tempList.Add(position);

                    return tempList;
                }
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
        List<Vector2> tempList = new List<Vector2>();

        int loops = 0;

        // Diagonal arriba-derecha.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // Si hay una pieza negra, no continuamos mirando en esta dirección.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                break;
            }

            // Si la casilla está vacía, la añadimos a la lista y seguimos mirando en esta dirección.

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));
            }

            // Si el rey opuesto está en la casilla, añadimos la posición de nuestra pieza a la lista.

            else
            {
                if (Chess.WhiteKingPosition == new Vector2(i, position.y + loops))
                {
                    tempList.Add(position);

                    return tempList;
                }
            }
        }

        tempList.Clear();
        loops = 0;

        // Diagonal abajo-derecha.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // Si hay una pieza negra, no continuamos mirando en esta dirección.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                break;
            }

            // Si la casilla está vacía, la añadimos a la lista y seguimos mirando en esta dirección.

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));
            }

            // Si el rey opuesto está en la casilla, añadimos la posición de nuestra pieza a la lista.

            else
            {
                if (Chess.WhiteKingPosition == new Vector2(i, position.y - loops))
                {
                    tempList.Add(position);

                    return tempList;
                }
            }
        }

        tempList.Clear();
        loops = 0;

        // Diagonal arriba-izquierda.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // Si hay una pieza negra, no continuamos mirando en esta dirección.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                break;
            }

            // Si la casilla está vacía, la añadimos a la lista y seguimos mirando en esta dirección.

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                tempList.Add(new Vector2(i, position.y + loops));
            }

            // Si el rey opuesto está en la casilla, añadimos la posición de nuestra pieza a la lista.

            else
            {
                if (Chess.WhiteKingPosition == new Vector2(i, position.y + loops))
                {
                    tempList.Add(position);

                    return tempList;
                }
            }
        }

        tempList.Clear();
        loops = 0;

        // Diagonal abajo-izquierda.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // Si hay una pieza negra, no continuamos mirando en esta dirección.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                break;
            }

            // Si la casilla está vacía, la añadimos a la lista y seguimos mirando en esta dirección.

            if (!Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                tempList.Add(new Vector2(i, position.y - loops));
            }

            // Si el rey opuesto está en la casilla, añadimos la posición de nuestra pieza a la lista.

            else
            {
                if (Chess.WhiteKingPosition == new Vector2(i, position.y - loops))
                {
                    tempList.Add(position);

                    return tempList;
                }
            }
        }

        return new List<Vector2>();
    }

    /// <summary>
    /// Bloquea los movimientos de las piezas negras cercanas que pueden causar que esta pieza haga un jaque mate.
    /// </summary>
    void SetForbiddenPositionsWhite()
    {
        bool firstJump = false;
        GameObject selectedPiece = null;
        int loops = 0;

        // Diagonal arriba-derecha.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // Si hay una pieza blanca en la casilla, no seguimos mirando en esta dirección.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                break;
            }

            if (Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                // Si hay una pieza negra y no es el rey, seguimos mirando en esta dirección. Podemos buscar un máximo de dos piezas.
                // Será necesario bloquear los movimientos del rey o de la otra pieza para evitar que se produzca un jaque.

                if (!firstJump)
                {
                    selectedPiece = Chess.GetPieceBlackInPosition(new Vector2(i, position.y + loops));

                    if ((Vector2)selectedPiece.transform.position == Chess.BlackKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.TopRight);

                        return;
                    }

                    else
                    {
                        firstJump = true;
                    }
                }

                else
                {
                    if (new Vector2(i, position.y + loops) == Chess.BlackKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.TopLeft);
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.BottomRight);
                    }

                    else
                    {
                        break;
                    }
                }
            }
        }

        loops = 0;
        firstJump = false;
        selectedPiece = null;

        // Diagonal abajo-derecha.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // Si hay una pieza blanca en la casilla, no seguimos mirando en esta dirección.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                break;
            }

            // Si hay una pieza negra y no es el rey, seguimos mirando en esta dirección. Podemos buscar un máximo de dos piezas.
            // Será necesario bloquear los movimientos del rey o de la otra pieza para evitar que se produzca un jaque.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                if (!firstJump)
                {
                    selectedPiece = Chess.GetPieceBlackInPosition(new Vector2(i, position.y - loops));

                    if ((Vector2)selectedPiece.transform.position == Chess.BlackKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.BottomRight);

                        return;
                    }

                    else
                    {
                        firstJump = true;
                    }
                }

                else
                {
                    if (new Vector2(i, position.y - loops) == Chess.BlackKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.TopRight);
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.BottomLeft);
                    }

                    else
                    {
                        break;
                    }
                }
            }
        }

        loops = 0;
        firstJump = false;
        selectedPiece = null;

        // Diagonal arriba-izquierda.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // Si hay una pieza blanca en la casilla, no seguimos mirando en esta dirección.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                break;
            }

            // Si hay una pieza negra y no es el rey, seguimos mirando en esta dirección. Podemos buscar un máximo de dos piezas.
            // Será necesario bloquear los movimientos del rey o de la otra pieza para evitar que se produzca un jaque.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                if (!firstJump)
                {
                    selectedPiece = Chess.GetPieceBlackInPosition(new Vector2(i, position.y + loops));

                    if ((Vector2)selectedPiece.transform.position == Chess.BlackKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.TopLeft);

                        return;
                    }

                    else
                    {
                        firstJump = true;
                    }
                }

                else
                {
                    if (new Vector2(i, position.y + loops) == Chess.BlackKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.TopRight);
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.BottomLeft);
                    }

                    else
                    {
                        break;
                    }
                }
            }
        }

        loops = 0;
        firstJump = false;
        selectedPiece = null;

        // Diagonal abajo-izquierda.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // Si hay una pieza blanca en la casilla, no seguimos mirando en esta dirección.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                break;
            }

            // Si hay una pieza negra y no es el rey, seguimos mirando en esta dirección. Podemos buscar un máximo de dos piezas.
            // Será necesario bloquear los movimientos del rey o de la otra pieza para evitar que se produzca un jaque.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                if (!firstJump)
                {
                    selectedPiece = Chess.GetPieceBlackInPosition(new Vector2(i, position.y - loops));

                    if ((Vector2)selectedPiece.transform.position == Chess.BlackKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.BottomLeft);

                        return;
                    }

                    else
                    {
                        firstJump = true;
                    }
                }

                else
                {
                    if (new Vector2(i, position.y - loops) == Chess.BlackKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.TopLeft);
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.BottomRight);
                    }

                    else
                    {
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Bloquea los movimientos de las piezas blancas cercanas que pueden causar que esta pieza haga un jaque mate.
    /// </summary>
    void SetForbiddenPositionsBlack()
    {
        bool firstJump = false;
        GameObject selectedPiece = null;
        int loops = 0;

        // Diagonal arriba-derecha.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // Si hay una pieza negra en la casilla, no seguimos mirando en esta dirección.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                break;
            }

            // Si hay una pieza blanca y no es el rey, seguimos mirando en esta dirección. Podemos buscar un máximo de dos piezas.
            // Será necesario bloquear los movimientos del rey o de la otra pieza para evitar que se produzca un jaque.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                if (!firstJump)
                {
                    selectedPiece = Chess.GetPieceWhiteInPosition(new Vector2(i, position.y + loops));

                    if ((Vector2)selectedPiece.transform.position == Chess.WhiteKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.TopRight);

                        return;
                    }

                    else
                    {
                        firstJump = true;
                    }
                }

                else
                {
                    if (new Vector2(i, position.y + loops) == Chess.WhiteKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.TopLeft);
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.BottomRight);
                    }

                    else
                    {
                        break;
                    }
                }
            }
        }

        loops = 0;
        firstJump = false;
        selectedPiece = null;

        // Diagonal abajo-derecha.

        for (int i = (int)position.x + 1; i <= 8; i++)
        {
            loops++;

            // Si hay una pieza negra en la casilla, no seguimos mirando en esta dirección.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                break;
            }

            // Si hay una pieza blanca y no es el rey, seguimos mirando en esta dirección. Podemos buscar un máximo de dos piezas.
            // Será necesario bloquear los movimientos del rey o de la otra pieza para evitar que se produzca un jaque.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                if (!firstJump)
                {
                    selectedPiece = Chess.GetPieceWhiteInPosition(new Vector2(i, position.y - loops));

                    if ((Vector2)selectedPiece.transform.position == Chess.WhiteKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.BottomRight);

                        return;
                    }

                    else
                    {
                        firstJump = true;
                    }
                }

                else
                {
                    if (new Vector2(i, position.y - loops) == Chess.WhiteKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.TopRight);
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.BottomLeft);
                    }

                    else
                    {
                        break;
                    }
                }
            }
        }

        loops = 0;
        firstJump = false;
        selectedPiece = null;

        // Diagonal arriba-izquierda.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // Si hay una pieza negra en la casilla, no seguimos mirando en esta dirección.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y + loops)))
            {
                break;
            }

            // Si hay una pieza blanca y no es el rey, seguimos mirando en esta dirección. Podemos buscar un máximo de dos piezas.
            // Será necesario bloquear los movimientos del rey o de la otra pieza para evitar que se produzca un jaque.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y + loops)))
            {
                if (!firstJump)
                {
                    selectedPiece = Chess.GetPieceWhiteInPosition(new Vector2(i, position.y + loops));

                    if ((Vector2)selectedPiece.transform.position == Chess.WhiteKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.TopLeft);

                        return;
                    }

                    else
                    {
                        firstJump = true;
                    }
                }

                else
                {
                    if (new Vector2(i, position.y + loops) == Chess.WhiteKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.TopRight);
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.BottomLeft);
                    }

                    else
                    {
                        break;
                    }
                }
            }
        }

        loops = 0;
        firstJump = false;
        selectedPiece = null;

        // Diagonal abajo-izquierda.

        for (int i = (int)position.x - 1; i >= 1; i--)
        {
            loops++;

            // Si hay una pieza negra en la casilla, no seguimos mirando en esta dirección.

            if (Chess.CheckSquareBlack(new Vector2(i, position.y - loops)))
            {
                break;
            }

            // Si hay una pieza blanca y no es el rey, seguimos mirando en esta dirección. Podemos buscar un máximo de dos piezas.
            // Será necesario bloquear los movimientos del rey o de la otra pieza para evitar que se produzca un jaque.

            if (Chess.CheckSquareWhite(new Vector2(i, position.y - loops)))
            {
                if (!firstJump)
                {
                    selectedPiece = Chess.GetPieceWhiteInPosition(new Vector2(i, position.y - loops));

                    if ((Vector2)selectedPiece.transform.position == Chess.WhiteKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.BottomLeft);

                        return;
                    }

                    else
                    {
                        firstJump = true;
                    }
                }

                else
                {
                    if (new Vector2(i, position.y - loops) == Chess.WhiteKingPosition)
                    {
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.TopLeft);
                        selectedPiece.GetComponent<PiecesMovement>().EnableLock(Pieces.Directions.BottomRight);
                    }

                    else
                    {
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// El valor de la pieza blanca en la posición actual.
    /// </summary>
    /// <returns></returns>
    int GetPositionValueWhite()
    {
        int[,] whiteValue =
        {
            { 20, 10, 10, 10, 10, 10, 10, 20 },
            { 10, -05, 00, 00, 00, 00, -05, 10 },
            { 10, -10, -10, -10, -10, -10, -10, 10 },
            { 10, 00, -10, -10, -10, -10, 00, 10 },
            { 10, -05, -05, -10, -10, -05, -05, 10 },
            { 10, 00, -05, -10, -10, -05, 00, 10 },
            { 10, 00, 00, 00, 00, 00, 00, 10 },
            { 20, 10, 10, 10, 10, 10, 10, 20 }
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
            { -20, -10, -10, -10, -10, -10, -10, -20 },
            { -10, 00, 00, 00, 00, 00, 00, -10 },
            { -10, 00, 05, 10, 10, 05, 00, -10 },
            { -10, 05, 05, 10, 10, 05, 05, -10 },
            { -10, 00, 10, 10, 10, 10, 00, -10 },
            { -10, 10, 10, 10, 10, 10, 10, -10 },
            { -10, 05, 00, 00, 00, 00, 05, -10 },
            { -20, -10, -10, -10, -10, -10, -10, -20 }
        };

        return 30 + blackValue[(int)position.y - 1, (int)position.x - 1];
    }
}