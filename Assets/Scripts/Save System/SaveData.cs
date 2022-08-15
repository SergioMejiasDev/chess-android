using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Los diferentes datos de la partida que pueden ser guardados en un archivo serializado.
/// </summary>
[Serializable]
public class SaveData
{
    /// <summary>
    /// String donde se guardan la fecha y hora de guardado.
    /// </summary>
    public readonly string saveDate;


    /// <summary>
    /// El jugador al que le corresponde jugar.
    /// </summary>
    public readonly Enums.Colours playerInTurn;

    /// <summary>
    /// La posición en el eje X del peón que está en posición de captura al paso.
    /// </summary>
    public readonly int enPassantDoublePositionX;

    /// <summary>
    /// La posición en el eje Y del peón que está en posición de captura al paso.
    /// </summary>
    public readonly int enPassantDoublePositionY;

    /// <summary>
    /// La posición letal en el eje X del peón que está en posición de captura al paso.
    /// </summary>
    public readonly int enPassantPositionX;

    /// <summary>
    /// La posición letal en el eje Y del peón que está en posición de captura al paso.
    /// </summary>
    public readonly int enPassantPositionY;

    /// <summary>
    /// El número de movimientos realizados sin capturar piezas o mover peones.
    /// </summary>
    public readonly int movements;


    /// <summary>
    /// Número de estados anteriores que van a ser guardados.
    /// </summary>
    public readonly int positionsSaved;

    /// <summary>
    /// El número de piezas de los estados guardados.
    /// </summary>
    public int numberOfPieces;

    /// <summary>
    /// Las posiciones en el eje X de todos los estados guardados.
    /// </summary>
    public int[] savedPositionsX;

    /// <summary>
    /// Las posiciones en el eje Y de todos los estados guardados.
    /// </summary>
    public int[] savedPositionsY;

    /// <summary>
    /// Los tipos de todas las piezas guardadas.
    /// </summary>
    public Pieces.Piece[] savedPieces;

    /// <summary>
    /// Los colores de todas las piezas guardadas.
    /// </summary>
    public Pieces.Colour[] savedColours;


    /// <summary>
    /// Todas las posiciones en el eje X de las piezas blancas en el tablero.
    /// </summary>
    public int[] whitePositionsX;

    /// <summary>
    /// Todas las posiciones en el eje Y de las piezas blancas en el tablero.
    /// </summary>
    public int[] whitePositionsY;

    /// <summary>
    /// Todas las posiciones en el eje X de las piezas negras en el tablero.
    /// </summary>
    public int[] blackPositionsX;

    /// <summary>
    /// Todas las posiciones en el eje Y de las piezas negras en el tablero.
    /// </summary>
    public int[] blackPositionsY;


    /// <summary>
    /// Los tipos de las piezas blancas en el tablero.
    /// </summary>
    public Pieces.Piece[] whitePieces;

    /// <summary>
    /// Los tipos de las piezas negras en el tablero.
    /// </summary>
    public Pieces.Piece[] blackPieces;

    /// <summary>
    /// Lista de booleanos que indica si las piezas blancas en el tablero se han movido por primera vez.
    /// </summary>
    public bool[] whiteFirstMove;

    /// <summary>
    /// Lista de booleanos que indica si las piezas negras en el tablero se han movido por primera vez.
    /// </summary>
    public bool[] blackFirstMove;


    /// <summary>
    /// Constructor de la clase para que los datos sin procesar puedan ser serializados.
    /// </summary>
    /// <param name="data">Datos sin procesar que no pueden ser serializados.</param>
    public SaveData(SaveDataRaw data)
    {
        // Obtenemos los datos de fecha y hora.

        saveDate = SetDate();

        // Guardamos varios datos de la partida, como los movimientos realizados o las posibles posiciones de captura al paso.

        playerInTurn = data.playerInTurn;
        enPassantDoublePositionX = (int)data.enPassantDoublePosition.x;
        enPassantDoublePositionY = (int)data.enPassantDoublePosition.y;
        enPassantPositionX = (int)data.enPassantPosition.x;
        enPassantPositionY = (int)data.enPassantPosition.y;
        movements = data.movements;

        // Guardamos los datos de las últimas posiciones.

        positionsSaved = data.savedPositions.Count;
        SetPositionRecord(data.savedPositions);

        // Finalmente, guardamos el estado actual del tablero.

        SetPositions(data.piecesWhite, data.piecesBlack);
        SetPieces(data.piecesWhite, data.piecesBlack);
        SetFirstMove(data.piecesWhite, data.piecesBlack);
    }

    /// <summary>
    /// Calcula la fecha y hora de la partida guardada.
    /// </summary>
    /// <returns>String con el formato "DD-MM-AAAA  HH:MM:SS".</returns>
    string SetDate()
    {
        DateTime time = DateTime.Now;

        return time.ToString("dd-MM-yyyy  HH:mm:ss");
    }

    /// <summary>
    /// Convierte los datos sin procesar de las últimas posiciones en datos que pueden ser serializados.
    /// </summary>
    /// <param name="savedPositions">Lista con las posiciones guardadas.</param>
    void SetPositionRecord(List<PositionRecord> savedPositions)
    {
        // Primero de todo guardamos el número de estados guardados para cuando llegue el momento de cargar la partida.

        numberOfPieces = savedPositions[0].positions.Count;

        // Creamos diferentes listas para las posiciones guardadas (una para X y otra para Y), los tipos y los colores de las piezas.

        List<int> tempListPositionsX = new List<int>();
        List<int> tempListPositionsY = new List<int>();
        List<Pieces.Piece> tempListPieces = new List<Pieces.Piece>();
        List<Pieces.Colour> tempListColours = new List<Pieces.Colour>();

        for (int i = 0; i < savedPositions.Count; i++)
        {
            tempListPositionsX = tempListPositionsX.Concat(savedPositions[i].GetPositionsX()).ToList();
            tempListPositionsY = tempListPositionsY.Concat(savedPositions[i].GetPositionsY()).ToList();
            tempListPieces = tempListPieces.Concat(savedPositions[i].pieces).ToList();
            tempListColours = tempListColours.Concat(savedPositions[i].colours).ToList();
        }

        // Convertimos todas las listas en arrays para que puedan ser serializados.

        savedPositionsX = tempListPositionsX.ToArray();
        savedPositionsY = tempListPositionsY.ToArray();
        savedPieces = tempListPieces.ToArray();
        savedColours = tempListColours.ToArray();
    }

    /// <summary>
    /// Convertimos las posiciones sin procesar de las piezas en el tablero en datos serializables.
    /// </summary>
    /// <param name="piecesWhite">Lista de todas las piezas blancas en el tablero.</param>
    /// <param name="piecesBlack">Lista de todas las piezas negras en el tablero.</param>
    void SetPositions(List<GameObject> piecesWhite, List<GameObject> piecesBlack)
    {
        // Creamos dos listas para las posiciones guardadas en X e Y, finalmente las guardamos como arrays.

        List<int> tempListX = new List<int>();
        List<int> tempListY = new List<int>();

        for (int i = 0; i < piecesWhite.Count; i++)
        {
            tempListX.Add((int)piecesWhite[i].transform.position.x);
            tempListY.Add((int)piecesWhite[i].transform.position.y);
        }

        whitePositionsX = tempListX.ToArray();
        whitePositionsY = tempListY.ToArray();

        // Reutilizamos la variable para hacer lo mismo con las piezas negras.

        tempListX.Clear();
        tempListY.Clear();

        for (int i = 0; i < piecesBlack.Count; i++)
        {
            tempListX.Add((int)piecesBlack[i].transform.position.x);
            tempListY.Add((int)piecesBlack[i].transform.position.y);
        }

        blackPositionsX = tempListX.ToArray();
        blackPositionsY = tempListY.ToArray();
    }

    /// <summary>
    /// Convierte los tipos de las piezas sin procesar en datos serializables.
    /// </summary>
    /// <param name="piecesWhite">Lista de todas las piezas blancas en el tablero.</param>
    /// <param name="piecesBlack">Lista de todas las piezas negras en el tablero.</param>
    void SetPieces(List<GameObject> piecesWhite, List<GameObject> piecesBlack)
    {
        // Creamos una lista temporal para guardar los tipos de las piezas blancas en el tablero.

        List<Pieces.Piece> tempList = new List<Pieces.Piece>();

        for (int i = 0; i < piecesWhite.Count; i++)
        {
            tempList.Add(piecesWhite[i].GetComponent<PiecesMovement>().PieceType);
        }

        whitePieces = tempList.ToArray();

        // Reutilizamos la variable para hacer lo mismo con las piezas negras.

        tempList.Clear();

        for (int i = 0; i < piecesBlack.Count; i++)
        {
            tempList.Add(piecesBlack[i].GetComponent<PiecesMovement>().PieceType);

            blackPieces = tempList.ToArray();
        }
    }

    /// <summary>
    /// Convierte los datos sin procesar sobre el primer movimiento de las piezas en datos serializables.
    /// </summary>
    /// <param name="piecesWhite">Lista de todas las piezas blancas en el tablero.</param>
    /// <param name="piecesBlack">Lista de todas las piezas negras en el tablero.</param>
    void SetFirstMove(List<GameObject> piecesWhite, List<GameObject> piecesBlack)
    {
        // Creamos una lista temporal para guardar los datos sobre el primer movimiento de las piezas blancas en el tablero.

        List<bool> tempList = new List<bool>();

        for (int i = 0; i < piecesWhite.Count; i++)
        {
            tempList.Add(piecesWhite[i].GetComponent<PiecesMovement>().FirstMove);
        }

        whiteFirstMove = tempList.ToArray();

        // Reutilizamos la variable para hacer lo mismo con las piezas negras.

        tempList.Clear();

        for (int i = 0; i < piecesBlack.Count; i++)
        {
            tempList.Add(piecesBlack[i].GetComponent<PiecesMovement>().FirstMove);

            blackFirstMove = tempList.ToArray();
        }
    }
}