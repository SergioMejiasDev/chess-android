using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// El estado del tablero en un momento específico de la partida. Se guarda para verificar la regla de la triple repetición.
/// </summary>
public class PositionRecord
{
    /// <summary>
    /// La posición de todas las piezas en el tablero.
    /// </summary>
    public readonly List<Vector2> positions;

    /// <summary>
    /// La figura de todas las piezas en el tablero.
    /// </summary>
    public readonly List<Pieces.Piece> pieces;

    /// <summary>
    /// El color de todas las piezas en el tablero.
    /// </summary>
    public readonly List<Pieces.Colour> colours;

    /// <summary>
    /// Constructor que se crea cuando un estado del tablero se guarda.
    /// </summary>
    /// <param name="whitePieces">Las piezas blancas en el tablero.</param>
    /// <param name="blackPieces">Las piezas negras en el tablero.</param>
    public PositionRecord(List<GameObject> whitePieces, List<GameObject> blackPieces)
    {
        // Inicializamos la tres listas temporales para guardar los datos de las piezas (posición, figura y color).

        List<Vector2> tempPositions = new List<Vector2>();
        List<Pieces.Piece> tempPieces = new List<Pieces.Piece>();
        List<Pieces.Colour> tempColours = new List<Pieces.Colour>();

        // Guardamos los valores en las listas temporales de forma consecutiva para los dos colores.

        for (int i = 0; i < whitePieces.Count; i++)
        {
            tempPositions.Add(whitePieces[i].transform.position);
            tempPieces.Add(whitePieces[i].GetComponent<PiecesMovement>().PieceType);
            tempColours.Add(Pieces.Colour.White);
        }

        for (int i = 0; i < blackPieces.Count; i++)
        {
            tempPositions.Add(blackPieces[i].transform.position);
            tempPieces.Add(blackPieces[i].GetComponent<PiecesMovement>().PieceType);
            tempColours.Add(Pieces.Colour.Black);
        }

        // Guardamos los valores temporales en las variables globales.

        positions = tempPositions;
        pieces = tempPieces;
        colours = tempColours;
    }

    /// <summary>
    /// Constructor que se crea cuando queremos rescatar las posiciones de un archivo guardado.
    /// </summary>
    /// <param name="savedPositions">Las posiciones de todas las piezas en el tablero.</param>
    /// <param name="savedPieces">Las figuras de todas las piezas en el tablero.</param>
    /// <param name="savedColours">Los colores de todas las piezas en el tablero.</param>
    public PositionRecord(Vector2[] savedPositions, Pieces.Piece[] savedPieces, Pieces.Colour[] savedColours)
    {
        // Guardamos los valores (arrays) en las variables globales (listas).

        positions = savedPositions.ToList();
        pieces = savedPieces.ToList();
        colours = savedColours.ToList();
    }

    /// <summary>
    /// Método para obtener la lista de todas las posiciones de las piezas en el eje X.
    /// </summary>
    /// <returns>Una lista de los enteros de las posiciones en el eje X (no podemos serializar los vectores en el archivo de guardado).</returns>
    public List<int> GetPositionsX()
    {
        List<int> tempList = new List<int>();

        for (int i = 0; i < positions.Count; i++)
        {
            tempList.Add((int)positions[i].x);
        }

        return tempList;
    }

    /// <summary>
    /// Método para obtener la lista de todas las posiciones de las piezas en el eje Y.
    /// </summary>
    /// <returns>Una lista de los enteros de las posiciones en el eje Y (no podemos serializar los vectores en el archivo de guardado).</returns>
    public List<int> GetPositionsY()
    {
        List<int> tempList = new List<int>();

        for (int i = 0; i < positions.Count; i++)
        {
            tempList.Add((int)positions[i].y);
        }

        return tempList;
    }

    /// <summary>
    /// Método para comprobar si dos estados guardados son iguales.
    /// </summary>
    /// <param name="other">El estado que queremos comparar con los estados guardados.</param>
    /// <returns>Verdadero si dos posiciones son iguales. Falso si esto no ocurre.</returns>
    public bool Equals(PositionRecord other)
    {
        // Si no hay la misma cantidad de piezas, los estados nunca serán iguales.

        if (positions.Count != other.positions.Count)
        {
            return false;
        }

        // Comprobamos si todas las variables son iguales.
        // Debido a la forma en que se guarda el estado, si las piezas son iguales, se guardan en el mismo orden.

        for (int i = 0; i < positions.Count; i++)
        {
            if (positions[i] != other.positions[i] || pieces[i] != other.pieces[i] || colours[i] != other.colours[i])
            {
                // Tan pronto como haya una mínima diferencia, los estados no son iguales.

                return false;
            }
        }

        return true;
    }
}