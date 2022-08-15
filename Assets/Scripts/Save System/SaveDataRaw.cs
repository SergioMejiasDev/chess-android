using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Datos sin procesar que queremos guardar pero no han pasado por la conversión para ser serializados.
/// </summary>
public class SaveDataRaw
{
    /// <summary>
    /// El jugador al que le toca jugar.
    /// </summary>
    public Enums.Colours playerInTurn;

    /// <summary>
    /// La posición de los peones que están en posición de captura al paso.
    /// </summary>
    public Vector2 enPassantDoublePosition;

    /// <summary>
    /// La posición letal de los peones que están en posición de captura al paso.
    /// </summary>
    public Vector2 enPassantPosition;

    /// <summary>
    /// El número de movimientos realizados sin capturar otras piezas o mover peones.
    /// </summary>
    public int movements;

    /// <summary>
    /// Lista de todos los estados guardados.
    /// </summary>
    public List<PositionRecord> savedPositions;

    /// <summary>
    /// Lista de todas las piezas blancas en el tablero.
    /// </summary>
    public List<GameObject> piecesWhite;

    /// <summary>
    /// Lista de todas las piezas negras en el tablero.
    /// </summary>
    public List<GameObject> piecesBlack;
}