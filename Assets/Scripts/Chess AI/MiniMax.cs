using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contiene las funciones relacionadas con el algoritmo MiniMax usado por la IA.
/// </summary>
public static class MiniMax
{
    /// <summary>
    /// Calcula el movimiento más adecuado para las piezas blancas siguiendo el algoritmo MiniMax.
    /// </summary>
    /// <returns>La pieza y movimientos más optimos para las piezas blancas.</returns>
    public static AIMovePosition BestMovementWhite()
    {
        int value = 0;
        List<AIMovePosition> selectedMove = new List<AIMovePosition>();

        for (int i = 0; i < Chess.PiecesWhite.Count; i++)
        {
            // Para todas las piezas blancas calculamos todos los movimientos legales posibles.

            List<Vector2> greenPositions = Chess.PiecesWhite[i].GetComponent<PiecesMovement>().SearchGreenPositions();

            // Si no hay movimientos posibles para esta pieza, pasamos a la siguiente.

            if (greenPositions.Count == 0)
            {
                continue;
            }

            for (int j = 0; j < greenPositions.Count; j++)
            {
                // Guardamos de forma temporal las variables de la pieza (posición y si puede moverse) para poder recuperarlas después.

                Vector2 startPosition = Chess.PiecesWhite[i].transform.position;
                bool hasMoved = Chess.PiecesWhite[i].GetComponent<PiecesMovement>().FirstMove;

                // Movemos la pieza a una de las posibles posiciones.

                Chess.PiecesWhite[i].transform.position = greenPositions[j];
                Chess.PiecesWhite[i].GetComponent<PiecesMovement>().FirstMove = true;

                int currentValue = BoardValueWhite(greenPositions[j]);

                if (currentValue > value && value != 0)
                {
                    Chess.PiecesWhite[i].transform.position = startPosition;
                    Chess.PiecesWhite[i].GetComponent<PiecesMovement>().FirstMove = hasMoved;

                    continue;
                }

                // Desde esta nueva posición, obtenemos el mejor valor para las piezas negras.

                int valueTemp = BestValueBlack(4, currentValue);

                // Las piezas blancas buscan minimizar el valor, y las negras maximizarlo.
                // Por lo tanto, vamos a elegir el menor valor posible para las negras.

                // Si la lista de posibles movimientos está vacía, añadimos este.
                // Si el valor obtenido es igual al que ya tenemos, lo añadimos también a la lista.

                if (selectedMove.Count == 0 || valueTemp <= value)
                {
                    value = valueTemp;

                    if (valueTemp < value)
                    {
                        selectedMove.Clear();
                    }

                    selectedMove.Add(new AIMovePosition(Chess.PiecesWhite[i], greenPositions[j]));
                }

                // Devolvemos las piezas a sus posiciones iniciales.

                Chess.PiecesWhite[i].transform.position = startPosition;
                Chess.PiecesWhite[i].GetComponent<PiecesMovement>().FirstMove = hasMoved;

                if (value == -100000)
                {
                    return selectedMove[Random.Range(0, selectedMove.Count)];
                }
            }
        }

        // Nos quedamos con un movimiento aleatorio de los que tenemos en la lista.

        return selectedMove[Random.Range(0, selectedMove.Count)];
    }

    /// <summary>
    /// Calcula el movimiento más adecuado para las piezas negras siguiendo el algoritmo MiniMax.
    /// </summary>
    /// <returns>La pieza y movimientos más optimos para las piezas negras.</returns>
    public static AIMovePosition BestMovementBlack()
    {
        int value = 0;
        List<AIMovePosition> selectedMove = new List<AIMovePosition>();

        for (int i = 0; i < Chess.PiecesBlack.Count; i++)
        {
            // Para todas las piezas negras calculamos todos los movimientos legales posibles.

            List<Vector2> greenPositions = Chess.PiecesBlack[i].GetComponent<PiecesMovement>().SearchGreenPositions();

            // Si no hay movimientos posibles para esta pieza, pasamos a la siguiente.

            if (greenPositions.Count == 0)
            {
                continue;
            }

            for (int j = 0; j < greenPositions.Count; j++)
            {
                // Guardamos de forma temporal las variables de la pieza (posición y si puede moverse) para poder recuperarlas después.

                Vector2 startPosition = Chess.PiecesBlack[i].transform.position;
                bool hasMoved = Chess.PiecesBlack[i].GetComponent<PiecesMovement>().FirstMove;

                // Movemos la pieza a una de las posibles posiciones.

                Chess.PiecesBlack[i].transform.position = greenPositions[j];
                Chess.PiecesBlack[i].GetComponent<PiecesMovement>().FirstMove = true;

                int currentValue = BoardValueBlack(greenPositions[j]);

                if (currentValue < value && value != 0)
                {
                    Chess.PiecesBlack[i].transform.position = startPosition;
                    Chess.PiecesBlack[i].GetComponent<PiecesMovement>().FirstMove = hasMoved;

                    continue;
                }

                // Desde esta nueva posición, obtenemos el mejor valor para las piezas blancas.

                int valueTemp = BestValueWhite(4, currentValue);

                // Las piezas blancas buscan minimizar el valor, y las negras maximizarlo.
                // Por lo tanto, vamos a elegir el mayor valor posible para las blancas.

                // Si la lista de posibles movimientos está vacía, añadimos este.
                // Si el valor obtenido es igual al que ya tenemos, lo añadimos también a la lista.

                if (selectedMove.Count == 0 || valueTemp >= value)
                {
                    value = valueTemp;

                    if (valueTemp > value)
                    {
                        selectedMove.Clear();
                    }

                    selectedMove.Add(new AIMovePosition(Chess.PiecesBlack[i], greenPositions[j]));
                }

                // Devolvemos las piezas a sus posiciones iniciales.

                Chess.PiecesBlack[i].transform.position = startPosition;
                Chess.PiecesBlack[i].GetComponent<PiecesMovement>().FirstMove = hasMoved;

                if (value == 100000)
                {
                    return selectedMove[Random.Range(0, selectedMove.Count)];
                }
            }
        }

        // Nos quedamos con un movimiento aleatorio de los que tenemos en la lista.

        return selectedMove[Random.Range(0, selectedMove.Count)];
    }

    /// <summary>
    /// Calcula el mejor valor para las piezas blancas en un momento concreto.
    /// </summary>
    /// <param name="depth">La profundidad a la que queremos explorar los resultados del algoritmo.
    /// Una profundidad mayor implica mejores movimientos, pero también un mayor consumo de recursos del dispositivo.</param>
    /// <param name="previousValue">El mejor valor obtenido de momento en la búsqueda anterior.</param>
    /// <returns>El mejor valor posible con el estado actual del tablero.</returns>
    static int BestValueWhite(int depth, int previousValue)
    {
        // La primera cosa que haremos es comprobar que no haya piezas blancas y negras en la misma casilla.
        // Esto significaría en el movimiento previo esta pieza blanca ha sido capturada por una negra.
        // En ese caso desactivamos la pieza blanca capturada para que no sea detectable por el algoritmo.

        List<GameObject> capturedPieces = new List<GameObject>();

        foreach (GameObject piece in Chess.PiecesWhite)
        {
            if (Chess.CheckSquareBlack(piece.transform.position))
            {
                if (Chess.BlackKingPosition == (Vector2)piece.transform.position)
                {
                    foreach (GameObject capturedPiece in capturedPieces)
                    {
                        capturedPiece.SetActive(true);
                    }

                    return -100000;
                }

                capturedPieces.Add(piece);

                piece.SetActive(false);
            }
        }

        // Empezamos tomando un valor extremo inicial para que cualquier resultado obtenido sea mejor.

        int value = 100000;

        for (int i = 0; i < Chess.PiecesWhite.Count; i++)
        {
            // Para cada pieza actualizamos la lista de posiciones y comprobamos los movimientos legales.

            Chess.CheckVerification();

            List<Vector2> greenPositions = Chess.PiecesWhite[i].GetComponent<PiecesMovement>().SearchGreenPositions();

            // Si la pieza no puede realizar ningún movimiento, la saltamos y probamos con la siguiente.

            if (greenPositions.Count == 0)
            {
                continue;
            }

            for (int j = 0; j < greenPositions.Count; j++)
            {
                // Para cada pieza, guardamos la posición inicial para poder recuperarla después.

                Vector2 startPosition = Chess.PiecesWhite[i].transform.position;
                bool hasMoved = Chess.PiecesWhite[i].GetComponent<PiecesMovement>().FirstMove;

                // Movemos la pieza a la nueva posición y obtenemos el valor del tablero.

                Chess.PiecesWhite[i].transform.position = greenPositions[j];
                Chess.PiecesWhite[i].GetComponent<PiecesMovement>().FirstMove = true;

                int currentValue = BoardValueWhite(greenPositions[j]);

                if (currentValue > previousValue)
                {
                    Chess.PiecesWhite[i].transform.position = startPosition;
                    Chess.PiecesWhite[i].GetComponent<PiecesMovement>().FirstMove = hasMoved;

                    continue;
                }

                int valueTemp;

                // Si estamos en el menor nivel de profundidad (0), este será el valor que devolvamos.

                if (depth == 0)
                {
                    valueTemp = currentValue;
                }

                // Si no estamos aun en el nivel de profundidad 0, ascendemos un nivel y calculamos la misma función para el color opuesto.

                else
                {
                    depth--;

                    valueTemp = BestValueBlack(depth, currentValue);
                }

                // Como queremos el mejor valor para las blancas, buscamos el menor valor de las negras.

                if (valueTemp < value)
                {
                    value = valueTemp;
                }

                // Tras esto, devolvemos las piezas a su posición original.

                Chess.PiecesWhite[i].transform.position = startPosition;
                Chess.PiecesWhite[i].GetComponent<PiecesMovement>().FirstMove = hasMoved;
            }
        }

        // Devolvemos las piezas a su estado inicial antes de realizar nuevas comprobaciones.

        foreach (GameObject piece in capturedPieces)
        {
            piece.SetActive(true);
        }

        return value;
    }

    /// <summary>
    /// Calcula el mejor valor para las piezas negras en un momento concreto.
    /// </summary>
    /// <param name="depth">La profundidad a la que queremos explorar los resultados del algoritmo.
    /// Una profundidad mayor implica mejores movimientos, pero también un mayor consumo de recursos del dispositivo.</param>
    /// <param name="previousValue">El mejor valor obtenido de momento en la búsqueda anterior.</param>
    /// <returns>El mejor valor posible con el estado actual del tablero.</returns>
    static int BestValueBlack(int depth, int previousValue)
    {
        // La primera cosa que haremos es comprobar que no haya piezas blancas y negras en la misma casilla.
        // Esto significaría en el movimiento previo esta pieza negra ha sido capturada por una blanca.
        // En ese caso desactivamos la pieza negra capturada para que no sea detectable por el algoritmo.

        List<GameObject> capturedPieces = new List<GameObject>();

        foreach (GameObject piece in Chess.PiecesBlack)
        {
            if (Chess.CheckSquareWhite(piece.transform.position))
            {
                if (Chess.WhiteKingPosition == (Vector2)piece.transform.position)
                {
                    foreach (GameObject capturedPiece in capturedPieces)
                    {
                        capturedPiece.SetActive(true);
                    }

                    return 100000;
                }

                capturedPieces.Add(piece);

                piece.SetActive(false);
            }
        }

        // Empezamos tomando un valor extremo inicial para que cualquier resultado obtenido sea mejor.

        int value = -100000;

        for (int i = 0; i < Chess.PiecesBlack.Count; i++)
        {
            // Para cada pieza actualizamos la lista de posiciones y comprobamos los movimientos legales.

            Chess.CheckVerification();

            List<Vector2> greenPositions = Chess.PiecesBlack[i].GetComponent<PiecesMovement>().SearchGreenPositions();

            // Si la pieza no puede realizar ningún movimiento, la saltamos y probamos con la siguiente.

            if (greenPositions.Count == 0)
            {
                continue;
            }

            for (int j = 0; j < greenPositions.Count; j++)
            {
                // Para cada pieza, guardamos la posición inicial para poder recuperarla después.

                Vector2 startPosition = Chess.PiecesBlack[i].transform.position;
                bool hasMoved = Chess.PiecesBlack[i].GetComponent<PiecesMovement>().FirstMove;

                // Movemos la pieza a la nueva posición y obtenemos el valor del tablero.

                Chess.PiecesBlack[i].transform.position = greenPositions[j];
                Chess.PiecesBlack[i].GetComponent<PiecesMovement>().FirstMove = true;

                int currentValue = BoardValueBlack(greenPositions[j]);

                if (currentValue < previousValue)
                {
                    Chess.PiecesBlack[i].transform.position = startPosition;
                    Chess.PiecesBlack[i].GetComponent<PiecesMovement>().FirstMove = hasMoved;

                    continue;
                }

                int valueTemp;

                // Si estamos en el menor nivel de profundidad (0), este será el valor que devolvamos.

                if (depth == 0)
                {
                    valueTemp = currentValue;
                }

                // Si no estamos aun en el nivel de profundidad 0, ascendemos un nivel y calculamos la misma función para el color opuesto.

                else
                {
                    depth--;

                    valueTemp = BestValueWhite(depth, currentValue);
                }

                // Como queremos el mejor valor para las negras, buscamos el mayor valor de las blancas.

                if (valueTemp > value)
                {
                    value = valueTemp;
                }

                // Tras esto, devolvemos las piezas a su posición original.

                Chess.PiecesBlack[i].transform.position = startPosition;
                Chess.PiecesBlack[i].GetComponent<PiecesMovement>().FirstMove = hasMoved;
            }
        }

        // Devolvemos las piezas a su estado inicial antes de realizar nuevas comprobaciones.

        foreach (GameObject piece in Chess.PiecesBlack)
        {
            piece.SetActive(true);
        }

        return value;
    }

    /// <summary>
    /// Busca el valor actual del tablero para las piezas blancas.
    /// </summary>
    /// <param name="position">La posición del último movimiento.</param>
    /// <returns>El valor entero del tablero.</returns>
    static int BoardValueWhite(Vector2 position)
    {
        int value = 0;

        // Lo primero que hacemos es eliminar la pieza del color opuesto que pudiera existir en nuestra casilla.
        // Esto significa que la habremos capturado en el movimiento previo.

        GameObject pieceInPosition = Chess.GetPieceBlackInPosition(position);

        if (pieceInPosition != null)
        {
            pieceInPosition.SetActive(false);
        }

        // El siguiente paso es añadir el valor de cada pieza del tablero de ambos colores.

        for (int i = 0; i < Chess.PiecesWhite.Count; i++)
        {
            value += Chess.PiecesWhite[i].GetComponent<PiecesMovement>().Value;
        }

        for (int i = 0; i < Chess.PiecesBlack.Count; i++)
        {
            value += Chess.PiecesBlack[i].GetComponent<PiecesMovement>().Value;
        }

        // Finalmente, si en el primer paso habíamos eliminado a alguna pieza, la restauramos.

        if (pieceInPosition != null)
        {
            pieceInPosition.SetActive(true);
        }

        return value;
    }

    /// <summary>
    /// Busca el valor actual del tablero para las piezas negras.
    /// </summary>
    /// <param name="position">La posición del último movimiento.</param>
    /// <returns>El valor entero del tablero.</returns>
    static int BoardValueBlack(Vector2 position)
    {
        int value = 0;

        // Lo primero que hacemos es eliminar la pieza del color opuesto que pudiera existir en nuestra casilla.
        // Esto significa que la habremos capturado en el movimiento previo.

        GameObject pieceInPosition = Chess.GetPieceWhiteInPosition(position);

        if (pieceInPosition != null)
        {
            pieceInPosition.SetActive(false);
        }

        // El siguiente paso es añadir el valor de cada pieza del tablero de ambos colores.

        for (int i = 0; i < Chess.PiecesWhite.Count; i++)
        {
            value += Chess.PiecesWhite[i].GetComponent<PiecesMovement>().Value;
        }

        for (int i = 0; i < Chess.PiecesBlack.Count; i++)
        {
            value += Chess.PiecesBlack[i].GetComponent<PiecesMovement>().Value;
        }

        // Finalmente, si en el primer paso habíamos eliminado a alguna pieza, la restauramos.

        if (pieceInPosition != null)
        {
            pieceInPosition.SetActive(true);
        }

        return value;
    }
}