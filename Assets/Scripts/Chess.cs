using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Contiene las principales variables y métodos que permiten el correcto funcionamiento de la partida de ajedrez.
/// </summary>
public static class Chess
{
    #region Delegates

    /// <summary>
    /// Delegado que se encarga de controlar los colores de las casillas.
    /// </summary>
    /// <param name="piecePosition">La posición de la pieza elegida.</param>
    /// <param name="greenPositions">Las posiciones donde la pieza elegia puede moverse.</param>
    public delegate void NewColourDelegate(Vector2 piecePosition, List<Vector2> greenPositions);

    /// <summary>
    /// Actualiza el color de las casilla del tablero.
    /// </summary>
    public static event NewColourDelegate UpdateColour;

    /// <summary>
    /// Hace que la casilla se vuelva roja durante un segundo justo después de un movimiento.
    /// </summary>
    public static event NewColourDelegate RedSquare;

    /// <summary>
    /// Delegado cuya función es controlar el uso de las casillas del tablero.
    /// </summary>
    public delegate void BoardsDelegate();

    /// <summary>
    /// Devuelve las casillas a su color inicial.
    /// </summary>
    public static event BoardsDelegate OriginalColour;

    /// <summary>
    /// Permite la selección de las casillas (desactiva el bloqueo).
    /// </summary>
    public static event BoardsDelegate EnableSelection;

    /// <summary>
    /// Prohibe la selección de las casillas (activa el bloqueo).
    /// </summary>
    public static event BoardsDelegate DisableSelection;

    #endregion

    #region Players and game mode

    /// <summary>
    /// El color del jugador que está jugando en este dispositivo.
    /// Permite que un jugador no pueda seleccionar las piezas si no es su turno.
    /// "All" se utilizará para las partidas de multijugador local.
    /// </summary>
    public static Enums.Colours PlayerColour { get; set; } = Enums.Colours.All;

    /// <summary>
    /// Indica si una partida está siendo jugada contra la IA.
    /// </summary>
    static bool singlePlay;

    /// <summary>
    /// El color del jugador al que le toca jugar.
    /// </summary>
    static Enums.Colours playerInTurn;

    /// <summary>
    /// Lista con todas las piezas blancas en el tablero.
    /// </summary>
    public static List<GameObject> PiecesWhite { get; set; } = new List<GameObject>();

    /// <summary>
    /// Lista con todas las piezas negras del tablero.
    /// </summary>
    public static List<GameObject> PiecesBlack { get; set; } = new List<GameObject>();

    /// <summary>
    /// El rey del jugador blanco.
    /// </summary>
    static GameObject whiteKing = null;

    /// <summary>
    /// El rey del jugador negro.
    /// </summary>
    static GameObject blackKing = null;

    /// <summary>
    /// Activa el color elegido e inicia una partida contra la IA.
    /// </summary>
    /// <param name="data">La partida cargada, si existe. Nulo para empezar la partida desde el principio.</param>
    public static void SelectColor(Enums.Colours colour, SaveData data)
    {
        // Limpiamos la escena por seguridad.

        CleanScene();

        // Indicamos el color elegido.

        PlayerColour = colour;

        // Indicamos que la partida será contra la IA.

        singlePlay = true;

        // Llamamos al método correcto para iniciar la partida de acuerdo a si hay o no una partida cargada.

        if (data == null)
        {
            StartNewGame();
        }

        else
        {
            singlePlay = true;

            StartLoadedGame(data);
        }
    }

    /// <summary>
    /// Comienza una partida desde el principio.
    /// </summary>
    public static void StartNewGame()
    {
        // Colocamos todas las piezas en el tablero en su posición inicial.

        InitialSpawn();

        // Eliminamos el historial de posiciones guardadas si existe.

        savedPositions.Clear();
        savedPositions.Add(new PositionRecord(PiecesWhite, PiecesBlack));

        // Definimos las variables principales del juego con su valor por defecto.

        stalemate = false;
        movements = 0;
        playerInTurn = Enums.Colours.White;
        WhiteKingInCheck = false;
        BlackKingInCheck = false;
        IsPlaying = true;

        // Activamos el mensaje en la interfaz para indicar que es el turno de las blancas (juegan primero).
        // Además, activamos el botón de guardado en el menú de pausa.

        Interface.interfaceClass.SetWaitingMessage(Enums.Colours.White);
        Interface.interfaceClass.EnableOnlineSave();

        // Si la IA está activa y juega con las blancas, empieza moviendo.

        if (singlePlay && PlayerColour == Enums.Colours.Black)
        {
            Interface.interfaceClass.EnableButtonPause(false);
            TimeEvents.timeEvents.StartWaitForAI();
        }

        // Desactivamos el bloqueo de las casillas para poder mover las piezas.

        EnableSelection();
    }

    /// <summary>
    /// Empieza una partida a partir de un archivo cargado.
    /// </summary>
    /// <param name="data">El archivo cargado.</param>
    public static void StartLoadedGame(SaveData data)
    {
        // Empezamos convirtiendo los datos guardados para que las variables puedan ser inicializadas.

        playerInTurn = data.playerInTurn;
        enPassantPawnPosition = new Vector2(data.enPassantDoublePositionX, data.enPassantDoublePositionY);
        EnPassantPosition = new Vector2(data.enPassantPositionX, data.enPassantPositionY);
        EnPassantActive = enPassantPawnPosition != Vector2.zero;
        movements = data.movements;

        savedPositions.Clear();

        // Para interpretar las variables de las posiciones guardadas, tenemos en cuenta el número de estados guardados.
        // Como todos los estados tienen el mismo número de posiciones, podemos recupar fácilmente la lista de estados guardados.
        // Dividimos el total de posiciones por el número de estados guardados, obteniendo la lista de estados completos.
        // Como los colores y los tipos de pieza están guardados en el mismo orden, asignamos los valores.

        int numberOfPieces = data.numberOfPieces;
        List<Vector2> tempPositions = new List<Vector2>();
        List<Pieces.Piece> tempPieces = new List<Pieces.Piece>();
        List<Pieces.Colour> tempColours = new List<Pieces.Colour>();

        for (int i = 0; i < data.savedPositionsX.Length; i++)
        {
            tempPositions.Add(new Vector2(data.savedPositionsX[i], data.savedPositionsY[i]));
            tempPieces.Add(data.savedPieces[i]);
            tempColours.Add(data.savedColours[i]);

            numberOfPieces--;

            if (numberOfPieces == 0)
            {
                savedPositions.Add(new PositionRecord(tempPositions.ToArray(), tempPieces.ToArray(), tempColours.ToArray()));

                tempPositions.Clear();
                tempPieces.Clear();
                tempColours.Clear();

                numberOfPieces = data.numberOfPieces;
            }
        }

        // A partir de los valores de las piezas en el tablero, instanciamos las piezas a partir de los prefabs en la carpeta "Resources".

        for (int i = 0; i < data.whitePositionsX.Length; i++)
        {
            string path = "";

            switch (data.whitePieces[i])
            {
                case Pieces.Piece.Bishop:
                    path = "Pieces/bishopW";
                    break;
                case Pieces.Piece.King:
                    path = "Pieces/kingW";
                    break;
                case Pieces.Piece.Knight:
                    path = "Pieces/knightW";
                    break;
                case Pieces.Piece.Pawn:
                    path = "Pieces/pawnW";
                    break;
                case Pieces.Piece.Queen:
                    path = "Pieces/queenW";
                    break;
                case Pieces.Piece.Rook:
                    path = "Pieces/rookW";
                    break;
            }

            GameObject piece = Object.Instantiate(Resources.Load<GameObject>(path), new Vector2(data.whitePositionsX[i], data.whitePositionsY[i]), Quaternion.identity);

            // Debemos tener en cuenta si las piezas se han movido previamente.

            piece.GetComponent<PiecesMovement>().FirstMove = data.whiteFirstMove[i];

            // Añadimos las piezas a la lista de piezas.

            PiecesWhite.Add(piece);

            if (data.whitePieces[i] == Pieces.Piece.King)
            {
                whiteKing = PiecesWhite[i];
            }
        }

        // Repetimos el mismo proceso con las piezas negras.

        for (int i = 0; i < data.blackPositionsX.Length; i++)
        {
            string path = "";

            switch (data.blackPieces[i])
            {
                case Pieces.Piece.Bishop:
                    path = "Pieces/bishopB";
                    break;
                case Pieces.Piece.King:
                    path = "Pieces/kingB";
                    break;
                case Pieces.Piece.Knight:
                    path = "Pieces/knightB";
                    break;
                case Pieces.Piece.Pawn:
                    path = "Pieces/pawnB";
                    break;
                case Pieces.Piece.Queen:
                    path = "Pieces/queenB";
                    break;
                case Pieces.Piece.Rook:
                    path = "Pieces/rookB";
                    break;
            }

            GameObject piece = Object.Instantiate(Resources.Load<GameObject>(path), new Vector2(data.blackPositionsX[i], data.blackPositionsY[i]), Quaternion.identity);

            piece.GetComponent<PiecesMovement>().FirstMove = data.blackFirstMove[i];

            PiecesBlack.Add(piece);

            if (data.blackPieces[i] == Pieces.Piece.King)
            {
                blackKing = PiecesBlack[i];
            }
        }

        // Inicializamos las variables del juego y comprobamos si hay algún rey en jaque en la posición inicial.

        stalemate = false;
        IsPlaying = true;

        CheckVerification();

        // Activamos el mensaje que indica a que jugador le toca jugar.

        Interface.interfaceClass.SetWaitingMessage(playerInTurn);

        // Si no se da ninguna de las opciones anteriores, la partida continua con normalidad.

        ResetValues();

        // Activamos la IA si es su turno.

        if (singlePlay)
        {
            Interface.interfaceClass.EnableButtonPause(false);
            TimeEvents.timeEvents.StartWaitForAI();
        }

        EnableSelection();
    }

    /// <summary>
    /// Elimina todas las piezas en la pantalla, además de desactivar las variables que indican que estamos jugando o la IA si está activa.
    /// Este método es útil para limpiar el tablero antes de empezar una partida.
    /// </summary>
    public static void CleanScene()
    {
        foreach (GameObject piece in PiecesWhite)
        {
            Object.Destroy(piece);
        }

        foreach (GameObject piece in PiecesBlack)
        {
            Object.Destroy(piece);
        }

        DeselectPosition();

        PiecesWhite.Clear();
        PiecesBlack.Clear();

        checkPositionsWhite.Clear();
        checkPositionsBlack.Clear();

        PlayerColour = Enums.Colours.All;

        IsPlaying = false;
        singlePlay = false;
    }

    /// <summary>
    /// Coloca todas las piezas en el tablero de manera ordenada, siguiendo las reglas del ajedrez.
    /// </summary>
    static void InitialSpawn()
    {
        // Instanciamos todas las piezas blancas a partir de los prefabs de la carpeta "Resources". Además, indicamos que pieza es el rey.

        whiteKing = Object.Instantiate(Resources.Load<GameObject>("Pieces/kingW"), new Vector2(5, 1), Quaternion.identity);
        PiecesWhite.Add(whiteKing);

        PiecesWhite.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/rookW"), new Vector2(1, 1), Quaternion.identity));
        PiecesWhite.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/rookW"), new Vector2(8, 1), Quaternion.identity));

        PiecesWhite.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/knightW"), new Vector2(2, 1), Quaternion.identity));
        PiecesWhite.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/knightW"), new Vector2(7, 1), Quaternion.identity));

        PiecesWhite.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/bishopW"), new Vector2(3, 1), Quaternion.identity));
        PiecesWhite.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/bishopW"), new Vector2(6, 1), Quaternion.identity));

        PiecesWhite.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/queenW"), new Vector2(4, 1), Quaternion.identity));

        for (int i = 1; i <= 8; i++)
        {
            PiecesWhite.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/pawnW"), new Vector2(i, 2), Quaternion.identity));
        }

        // Hacemos lo mismo con las piezas negras.

        blackKing = Object.Instantiate(Resources.Load<GameObject>("Pieces/kingB"), new Vector2(5, 8), Quaternion.identity);
        PiecesBlack.Add(blackKing);

        PiecesBlack.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/rookB"), new Vector2(1, 8), Quaternion.identity));
        PiecesBlack.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/rookB"), new Vector2(8, 8), Quaternion.identity));

        PiecesBlack.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/knightB"), new Vector2(2, 8), Quaternion.identity));
        PiecesBlack.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/knightB"), new Vector2(7, 8), Quaternion.identity));

        PiecesBlack.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/bishopB"), new Vector2(3, 8), Quaternion.identity));
        PiecesBlack.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/bishopB"), new Vector2(6, 8), Quaternion.identity));

        PiecesBlack.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/queenB"), new Vector2(4, 8), Quaternion.identity));

        for (int i = 1; i <= 8; i++)
        {
            PiecesBlack.Add(Object.Instantiate(Resources.Load<GameObject>("Pieces/pawnB"), new Vector2(i, 7), Quaternion.identity));
        }
    }

    #endregion

    #region Movement

    /// <summary>
    /// Indica si se está jugando alguna partida actualmente.
    /// </summary>
    public static bool IsPlaying { get; set; }

    /// <summary>
    /// La pieza seleccionada para moverla.
    /// </summary>
    public static GameObject ActivePiece { get; set; } = null;

    /// <summary>
    /// La posición de la pieza seleccionada.
    /// La posición 
    /// </summary>
    public static Vector2 ActivePiecePosition => ActivePiece.transform.position;

    /// <summary>
    /// Mueve la pieza seleccionada a la posición indicada.
    /// </summary>
    /// <param name="position">La posición a la que se va a mover la pieza.</param>
    public static void MovePiece(Vector2 position)
    {
        // Devolvemos todas las casillas a su color original.

        ResetColour();

        // Aumentamos el número de movimientos realizados para controlar si hemos sobrepasado el límite.

        movements++;

        // Dependiendo del tipo de pieza que se ha movido, se pueden dar varias peculiaridades.

        MovementPeculiarities(position);

        // Movemos la pieza a su nueva posición.

        ActivePiece.transform.position = position;

        // Reproducimos un sonido para indicar el movimiento de la pieza.

        Interface.interfaceClass.PlayMoveSound();

        // Hacemos que la casilla a la que se ha movido la pieza se vuelva roja durante un segundo.

        ActivateRed(position);

        // Comprobamos si hay una pieza del color contrario en la casilla. Si se da este casom la habremos capturado.

        Pieces.Colour colour = ActivePiece.GetComponent<PiecesMovement>().PieceColour;

        if (colour == Pieces.Colour.White)
        {
            if (CheckSquareBlack(position))
            {
                // El procedimiento que seguiremos será eliminar a la pieza capturada de la lista correspondiente y destruirla posteriormente.

                GameObject capturedPiece = GetPieceBlackInPosition(position);

                PiecesBlack.Remove(capturedPiece);

                Object.Destroy(capturedPiece);

                // Cuando se ha capturado a una pieza, se reinicia el contador de movimientos.

                movements = 0;

                // Como hay una pieza menos, el número de posiciones no coincidirá con la de los estados guardados, por lo que los eliminamos.

                savedPositions.Clear();
            }
        }

        else
        {
            if (CheckSquareWhite(position))
            {
                GameObject capturedPiece = GetPieceWhiteInPosition(position);

                PiecesWhite.Remove(capturedPiece);

                Object.Destroy(capturedPiece);

                movements = 0;

                savedPositions.Clear();
            }
        }

        // En el método de "MovementPeculiarities" se activará la promoción de los peones si alcanzan el extemo del tablero.

        if (activePromotion)
        {
            // Activamos el mensaje en la pantalla para indicar el color del peón que va a promocionar.

            if (ActivePiece.GetComponent<PiecesMovement>().PieceColour == Pieces.Colour.White)
            {
                if (singlePlay && PlayerColour == Enums.Colours.Black)
                {
                    PieceSelectedToPromotion(Enums.PromotablePieces.Queen, Pieces.Colour.White);

                    return;
                }

                else
                {
                    DisableSelection();

                    Interface.interfaceClass.ActivatePromotionWhite(CheckTurn());
                }
            }

            else
            {
                if (singlePlay && PlayerColour == Enums.Colours.White)
                {
                    PieceSelectedToPromotion(Enums.PromotablePieces.Queen, Pieces.Colour.Black);

                    return;
                }

                else
                {
                    DisableSelection();

                    Interface.interfaceClass.ActivatePromotionBlack(CheckTurn());
                }
            }

            // Después de activar el mensaje en la pantalla, interrumpimos la ejecución del método para que el jugador elija la pieza.

            return;
        }

        // Guardamos un estado con las nuevas posiciones para comprobar si se repite en un futuro.

        savedPositions.Add(new PositionRecord(PiecesWhite, PiecesBlack));

        // Comienza el turno del siguiente jugador.

        NextTurn();
    }

    /// <summary>
    /// Comprueba si la pieza que se va a mover tiene alguna peculiaridad.
    /// </summary>
    /// <param name="position">La posición de la pieza que se va a mover.</param>
    static void MovementPeculiarities(Vector2 position)
    {
        // Identificamos el tipo de pieza.

        Pieces.Piece kindOfPiece = ActivePiece.GetComponent<PiecesMovement>().PieceType;

        switch (kindOfPiece)
        {
            // En el caso de los peones, se comprueba si están en posición de captura al paso o están en posición para promocionar.
            // Además, es necesario indicar que ya se han movido y reiniciar los movimientos.

            case Pieces.Piece.Pawn:
                CheckEnPassant(position);
                ActivateEnPassant(position);
                VerifyPromotion(position);
                ActivePiece.GetComponent<PiecesMovement>().FirstMove = true;
                movements = 0;
                break;

            // En el caso de las torres, indicamos que ya se han movido para que no puedan hacer el movimiento de enroque.

            case Pieces.Piece.Rook:
                ActivePiece.GetComponent<PiecesMovement>().FirstMove = true;
                break;

            // Si el que se mueve es el rey, comprobamos si está en posición de enroque.
            // Además, indicamos que ya se ha movido para que no pueda realizar el enroque con posterioridad.

            case Pieces.Piece.King:
                CheckCastling(position);
                ActivePiece.GetComponent<PiecesMovement>().FirstMove = true;
                break;
        }
    }

    /// <summary>
    /// Bloquea ciertos movimientos de las piezas blancas para que no puedan provocarse un jaque mate.
    /// </summary>
    /// <param name="unblock">Verdadero para desactivar el bloqueo, falso para activarlo.</param>
    static void BlockMovementsWhite(bool unblock)
    {
        // Al inicio del turno de las blancas, bloquemos estos movimientos en las blancas y los desbloqueamos en las negras.

        if (unblock)
        {
            foreach (GameObject piece in PiecesWhite)
            {
                piece.GetComponent<PiecesMovement>().DisableLock();
            }
        }

        else
        {
            foreach (GameObject piece in PiecesBlack)
            {
                piece.GetComponent<PiecesMovement>().ActivateForbiddenPositions();
            }
        }
    }

    /// <summary>
    /// Bloquea ciertos movimientos de las piezas negras para que no puedan provocarse un jaque mate.
    /// </summary>
    /// <param name="unblock">True if we disable the lock, false if we enable it.</param>
    static void BlockMovementsBlack(bool unblock)
    {
        // Al inicio del turno de las negras, bloquemos estos movimientos en las negras y los desbloqueamos en las blancas.

        if (unblock)
        {
            foreach (GameObject piece in PiecesBlack)
            {
                piece.GetComponent<PiecesMovement>().DisableLock();
            }
        }

        else
        {
            foreach (GameObject piece in PiecesWhite)
            {
                piece.GetComponent<PiecesMovement>().ActivateForbiddenPositions();
            }
        }
    }

    #endregion

    #region Castling

    /// <summary>
    /// La torre que está realizando un enroque largo.
    /// </summary>
    static GameObject castlingLeftRook = null;

    /// <summary>
    /// La torre que está realizando un enroque corto.
    /// </summary>
    static GameObject castlingRightRook = null;

    /// <summary>
    /// La positición a la que se debe mover el rey que hace un enroque largo.
    /// </summary>
    static Vector2 castlingLeftDestination;

    /// <summary>
    /// La positición a la que se debe mover el rey que hace un enroque corto.
    /// </summary>
    static Vector2 castlingRightDestination;

    /// <summary>
    /// La positición a la que se debe mover la torre que hace un enroque largo.
    /// </summary>
    static Vector2 castlingLeftPosition;

    /// <summary>
    /// La positición a la que se debe mover la torre que hace un enroque corto.
    /// </summary>
    static Vector2 castlingRightPosition;

    /// <summary>
    /// Comprueba si el movimiento hecho por el rey permite hacer un enroque.
    /// </summary>
    /// <param name="position">Posición a la que se va a mover el rey.</param>
    static void CheckCastling(Vector2 position)
    {
        // Si un enroque largo está en progreso, movemos la torre a la posición correcta.

        if (castlingLeftDestination == position)
        {
            castlingLeftRook.transform.position = castlingLeftPosition;
        }

        // Hacemos lo mismo si hay un enroque corto.

        else if (castlingRightDestination == position)
        {
            castlingRightRook.transform.position = castlingRightPosition;
        }
    }

    /// <summary>
    /// Permite saber si se está realizando un enroque largo.
    /// </summary>
    /// <param name="colour">El color de la pieza que se está moviendo.</param>
    /// <returns>Si el movimiento realizado es un enroque largo.</returns>
    public static bool CastlingLeft(Pieces.Colour colour)
    {
        if (colour == Pieces.Colour.White && CheckSquareWhite(new Vector2(1, 1)))
        {
            // Si el rey blanco se está moviendo, comprobamos si la pieza en A1 es la torre y si nunca se ha movido.

            GameObject capturedPiece = GetPieceWhiteInPosition(new Vector2(1, 1));

            if (capturedPiece.GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Rook &&
                !capturedPiece.GetComponent<PiecesMovement>().FirstMove)
            {
                // Si la posición sobre la que va a saltar el rey no está vacía, no hay enroque.

                if (!CheckSquareEmpty(new Vector2(2, 1)))
                {
                    return false;
                }

                if (VerifyBlackCheckPosition(new Vector2(3, 1)) || !CheckSquareEmpty(new Vector2(3, 1)))
                {
                    return false;
                }

                if (VerifyBlackCheckPosition(new Vector2(4, 1)) || !CheckSquareEmpty(new Vector2(4, 1)))
                {
                    return false;
                }

                if (VerifyBlackCheckPosition(new Vector2(5, 1)))
                {
                    return false;
                }

                // Si no se cumplen las condiciones anteriores, hay enroque. Asignamos las correspondientes variables.

                else
                {
                    castlingLeftRook = capturedPiece;
                    castlingLeftPosition = new Vector2(4, 1);
                    castlingLeftDestination = new Vector2(3, 1);

                    return true;
                }
            }

            else
            {
                return false;
            }
        }

        if (colour == Pieces.Colour.Black && CheckSquareBlack(new Vector2(1, 8)))
        {
            // Si el rey negro se está moviendo, comprobamos si la pieza en A8 es la torre y si nunca se ha movido.

            GameObject capturedPiece = GetPieceBlackInPosition(new Vector2(1, 8));

            if (capturedPiece.GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Rook &&
                !capturedPiece.GetComponent<PiecesMovement>().FirstMove)
            {
                // Si la posición sobre la que va a saltar el rey no está vacía, no hay enroque.

                if (CheckSquareBlack(new Vector2(2, 8)) || CheckSquareWhite(new Vector2(2, 8)))
                {
                    return false;
                }

                if (VerifyWhiteCheckPosition(new Vector2(3, 8)) || CheckSquareBlack(new Vector2(3, 8)) || CheckSquareWhite(new Vector2(3, 8)))
                {
                    return false;
                }

                if (VerifyWhiteCheckPosition(new Vector2(4, 8)) || CheckSquareBlack(new Vector2(4, 8)) || CheckSquareWhite(new Vector2(4, 8)))
                {
                    return false;
                }

                if (VerifyWhiteCheckPosition(new Vector2(5, 8)))
                {
                    return false;
                }

                // Si no se cumplen las condiciones anteriores, hay enroque. Asignamos las correspondientes variables.

                else
                {
                    castlingLeftRook = capturedPiece;
                    castlingLeftPosition = new Vector2(4, 8);
                    castlingLeftDestination = new Vector2(3, 8);

                    return true;
                }
            }

            else
            {
                return false;
            }
        }

        else
        {
            return false;
        }
    }

    /// <summary>
    /// Permite saber si se está realizando un enroque corto.
    /// </summary>
    /// <param name="colour">El color de la pieza que se está moviendo.</param>
    /// <returns>Si el movimiento realizado es un enroque corto.</returns>
    public static bool CastlingRight(Pieces.Colour colour)
    {
        if (colour == Pieces.Colour.White && CheckSquareWhite(new Vector2(8, 1)))
        {
            // Si el rey blanco se está moviendo, comprobamos si la pieza en H1 es la torre y si nunca se ha movido.

            GameObject capturedPiece = GetPieceWhiteInPosition(new Vector2(8, 1));

            if (capturedPiece.GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Rook &&
                !capturedPiece.GetComponent<PiecesMovement>().FirstMove)
            {
                // Si la posición sobre la que va a saltar el rey no está vacía, no hay enroque.

                if (VerifyBlackCheckPosition(new Vector2(7, 1)) || CheckSquareWhite(new Vector2(7, 1)) || CheckSquareBlack(new Vector2(7, 1)))
                {
                    return false;
                }

                if (VerifyBlackCheckPosition(new Vector2(6, 1)) || CheckSquareWhite(new Vector2(6, 1)) || CheckSquareBlack(new Vector2(6, 1)))
                {
                    return false;
                }

                if (VerifyBlackCheckPosition(new Vector2(5, 1)))
                {
                    return false;
                }

                // Si no se cumplen las condiciones anteriores, hay enroque. Asignamos las correspondientes variables.

                else
                {
                    castlingRightRook = capturedPiece;
                    castlingRightPosition = new Vector2(6, 1);
                    castlingRightDestination = new Vector2(7, 1);

                    return true;
                }
            }

            else
            {
                return false;
            }
        }

        if (colour == Pieces.Colour.Black && CheckSquareBlack(new Vector2(8, 8)))
        {
            // Si el rey negro se está moviendo, comprobamos si la pieza en H8 es la torre y si nunca se ha movido.

            GameObject capturedPiece = GetPieceBlackInPosition(new Vector2(8, 8));

            if (capturedPiece.GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Rook &&
                !capturedPiece.GetComponent<PiecesMovement>().FirstMove)
            {
                // Si la posición sobre la que va a saltar el rey no está vacía, no hay enroque.

                if (VerifyWhiteCheckPosition(new Vector2(7, 8)) || CheckSquareBlack(new Vector2(7, 8)) || CheckSquareWhite(new Vector2(7, 8)))
                {
                    return false;
                }

                if (VerifyWhiteCheckPosition(new Vector2(6, 8)) || CheckSquareBlack(new Vector2(6, 8)) || CheckSquareWhite(new Vector2(6, 8)))
                {
                    return false;
                }

                if (VerifyWhiteCheckPosition(new Vector2(5, 8)))
                {
                    return false;
                }

                // Si no se cumplen las condiciones anteriores, hay enroque. Asignamos las correspondientes variables.

                else
                {
                    castlingRightRook = capturedPiece;
                    castlingRightPosition = new Vector2(6, 8);
                    castlingRightDestination = new Vector2(7, 8);

                    return true;
                }
            }

            else
            {
                return false;
            }
        }

        else
        {
            return false;
        }
    }

    #endregion

    #region En Passant

    /// <summary>
    /// La posición del peón que puede ser capturado al paso.
    /// </summary>
    static Vector2 enPassantPawnPosition;

    /// <summary>
    /// La posición a la que se debe mover un peón para realizar una captura al paso al peón del jugador contrario.
    /// </summary>
    public static Vector2 EnPassantPosition { get; private set; }

    /// <summary>
    /// El peón que ha avanzado dos casillas (ha activado la captura al paso).
    /// </summary>
    static GameObject enPassantPiece;

    /// <summary>
    /// Indica si la captura al paso está activa (un peón ha avanzado dos casillas).
    /// </summary>
    public static bool EnPassantActive { get; private set; }

    /// <summary>
    /// Activa la captura al paso (si es el caso) tras el movimiento de un peón.
    /// </summary>
    /// <param name="position">La posición final del peón tras moverse.</param>
    public static void ActivateEnPassant(Vector2 position)
    {
        // Asignamos todas las variables de acuerdo con la posición del peón.

        if (ActivePiece.GetComponent<PiecesMovement>().FirstMove)
        {
            return;
        }

        if (position.y == 4)
        {
            EnPassantActive = true;
            enPassantPawnPosition = position;
            EnPassantPosition = new Vector2(enPassantPawnPosition.x, enPassantPawnPosition.y - 1);
            enPassantPiece = ActivePiece;
        }

        else if (position.y == 5)
        {
            EnPassantActive = true;
            enPassantPawnPosition = position;
            EnPassantPosition = new Vector2(enPassantPawnPosition.x, enPassantPawnPosition.y + 1);
            enPassantPiece = ActivePiece;
        }

        else
        {
            EnPassantActive = false;
            enPassantPawnPosition = Vector2.zero;
            EnPassantPosition = Vector2.zero;
            enPassantPiece = null;
        }
    }

    /// <summary>
    /// Resetea todas las variables relacionadas con la captura al paso.
    /// </summary>
    static void DeactivateEnPassant()
    {
        EnPassantActive = false;

        enPassantPawnPosition = Vector2.zero;
        EnPassantPosition = Vector2.zero;
        enPassantPiece = null;
    }

    /// <summary>
    /// Comprueba si tras el movimiento de un peón se ha capturado al paso a otro peón.
    /// </summary>
    /// <param name="position">La posición a la que se ha movido el peón.</param>
    static void CheckEnPassant(Vector2 position)
    {
        // Si se ha realizado un movimiento previo con el peón, no se activará la captura al paso.

        if (!EnPassantActive)
        {
            return;
        }

        // Si la captura al paso estaba activada y se ha usado, se captura a la pieza correspondiente.

        if (position == EnPassantPosition)
        {
            if (enPassantPiece.GetComponent<PiecesMovement>().PieceColour == Pieces.Colour.White)
            {
                PiecesWhite.Remove(enPassantPiece);
            }

            else if (enPassantPiece.GetComponent<PiecesMovement>().PieceColour == Pieces.Colour.Black)
            {
                PiecesBlack.Remove(enPassantPiece);
            }

            Object.Destroy(enPassantPiece);
            enPassantPiece = null;

            DeactivateEnPassant();
        }
    }

    #endregion

    #region Pawn Promotion

    /// <summary>
    /// Indica si un peón ha promocionado en este turno.
    /// </summary>
    static bool activePromotion = false;

    /// <summary>
    /// Comprueba si un peón ha promocionado tras moverse y activa las variables necesarias.
    /// </summary>
    /// <param name="position">La posición a la que se ha movido el peón.</param>
    static void VerifyPromotion(Vector2 position)
    {
        if (ActivePiece.GetComponent<PiecesMovement>().PieceColour == Pieces.Colour.White && position.y == 8)
        {
            activePromotion = true;
            savedPositions.Clear();
        }

        else if (ActivePiece.GetComponent<PiecesMovement>().PieceColour == Pieces.Colour.Black && position.y == 1)
        {
            activePromotion = true;
            savedPositions.Clear();
        }
    }

    /// <summary>
    /// Sustituye al peón que ha promocionado por la pieza elegida.
    /// </summary>
    /// <param name="piece">La nueva pieza elegida.</param>
    /// <param name="colour">El color de la pieza elegida.</param>
    public static void PieceSelectedToPromotion(Enums.PromotablePieces piece, Pieces.Colour colour)
    {
        Vector2 position = ActivePiece.transform.position;
        GameObject newPiece = null;

        if (colour == Pieces.Colour.White)
        {
            PiecesWhite.Remove(ActivePiece);
            Object.Destroy(ActivePiece);

            switch (piece)
            {
                case Enums.PromotablePieces.Rook:
                    newPiece = Object.Instantiate(Resources.Load<GameObject>("Pieces/rookW"), position, Quaternion.identity);
                    break;
                case Enums.PromotablePieces.Knight:
                    newPiece = Object.Instantiate(Resources.Load<GameObject>("Pieces/knightW"), position, Quaternion.identity);
                    break;
                case Enums.PromotablePieces.Bishop:
                    newPiece = Object.Instantiate(Resources.Load<GameObject>("Pieces/bishopW"), position, Quaternion.identity);
                    break;
                case Enums.PromotablePieces.Queen:
                    newPiece = Object.Instantiate(Resources.Load<GameObject>("Pieces/queenW"), position, Quaternion.identity);
                    break;
            }

            PiecesWhite.Add(newPiece);
            ActivePiece = newPiece;
        }

        else
        {
            PiecesBlack.Remove(ActivePiece);
            Object.Destroy(ActivePiece);

            switch (piece)
            {
                case Enums.PromotablePieces.Rook:
                    newPiece = Object.Instantiate(Resources.Load<GameObject>("Pieces/rookB"), position, Quaternion.identity);
                    break;
                case Enums.PromotablePieces.Knight:
                    newPiece = Object.Instantiate(Resources.Load<GameObject>("Pieces/knightB"), position, Quaternion.identity);
                    break;
                case Enums.PromotablePieces.Bishop:
                    newPiece = Object.Instantiate(Resources.Load<GameObject>("Pieces/bishopB"), position, Quaternion.identity);
                    break;
                case Enums.PromotablePieces.Queen:
                    newPiece = Object.Instantiate(Resources.Load<GameObject>("Pieces/queenB"), position, Quaternion.identity);
                    break;
            }

            PiecesBlack.Add(newPiece);
            ActivePiece = newPiece;
        }

        Interface.interfaceClass.DisablePromotions();

        activePromotion = false;

        savedPositions.Add(new PositionRecord(PiecesWhite, PiecesBlack));

        EnableSelection();

        NextTurn();
    }

    #endregion

    #region Check

    /// <summary>
    /// Lista de posiciones donde una pieza blanca puede hacer jaque.
    /// </summary>
    static List<Vector2> checkPositionsWhite = new List<Vector2>();

    /// <summary>
    /// Lista de posiciones donde una pieza negra puede hacer jaque.
    /// </summary>
    static List<Vector2> checkPositionsBlack = new List<Vector2>();

    /// <summary>
    /// Indica si el rey blanco está en jaque.
    /// </summary>
    public static bool WhiteKingInCheck { get; private set; }

    /// <summary>
    /// Indica si el rey negro está en jaque.
    /// </summary>
    public static bool BlackKingInCheck { get; private set; }

    /// <summary>
    /// La posición del rey blanco en el tablero.
    /// </summary>
    public static Vector2 WhiteKingPosition => whiteKing.transform.position;

    /// <summary>
    /// La posición del rey negro en el tablero.
    /// </summary>
    public static Vector2 BlackKingPosition => blackKing.transform.position;

    /// <summary>
    /// Posiciones que pueden impedir el jaque de las piezas blancas.
    /// </summary>
    static List<Vector2> menacingWhitePositions = new List<Vector2>();

    /// <summary>
    /// Posiciones que pueden impedir el jaque de las piezas negras.
    /// </summary>
    static List<Vector2> menacingBlackPositions = new List<Vector2>();

    /// <summary>
    /// Actualiza la lista de posiciones donde las piezas blancas pueden capturar.
    /// </summary>
    static void SetCheckPositionsWhite()
    {
        checkPositionsWhite.Clear();

        List<Vector2> tempList = new List<Vector2>();

        foreach (GameObject piece in PiecesWhite)
        {
            tempList = tempList.Concat(piece.GetComponent<PiecesMovement>().GetPositionsInCheck()).ToList();
        }

        checkPositionsWhite = tempList.Distinct().ToList();
    }

    /// <summary>
    /// Actualiza la lista de posiciones donde las piezas negras pueden capturar.
    /// </summary>
    static void SetCheckPositionsBlack()
    {
        checkPositionsBlack.Clear();

        List<Vector2> tempList = new List<Vector2>();

        foreach (GameObject piece in PiecesBlack)
        {
            tempList = tempList.Concat(piece.GetComponent<PiecesMovement>().GetPositionsInCheck()).ToList();
        }

        checkPositionsBlack = tempList.Distinct().ToList();
    }

    /// <summary>
    /// Actualiza la lista de posiciones que sean una amenaza para el jugador negro.
    /// </summary>
    static void SetMenacingPositionsWhite()
    {
        menacingWhitePositions.Clear();

        List<Vector2> tempList = new List<Vector2>();

        foreach (GameObject piece in PiecesWhite)
        {
            tempList = tempList.Concat(piece.GetComponent<PiecesMovement>().GetMenacingPositions()).ToList();
        }

        menacingWhitePositions = tempList.Distinct().ToList();
    }

    /// <summary>
    /// Actualiza la lista de posiciones que sean una amenaza para el jugador blanco.
    /// </summary>
    static void SetMenacingPositionsBlack()
    {
        menacingBlackPositions.Clear();

        List<Vector2> tempList = new List<Vector2>();

        foreach (GameObject piece in PiecesBlack)
        {
            tempList = tempList.Concat(piece.GetComponent<PiecesMovement>().GetMenacingPositions()).ToList();
        }

        menacingBlackPositions = tempList.Distinct().ToList();
    }

    /// <summary>
    /// Comprobamos si el turno empieza con el rey en jaque.
    /// </summary>
    public static void CheckVerification()
    {
        // Actualizamos ambas listas y bloqueamos los movimientos ilegales.

        SetCheckPositionsWhite();
        SetMenacingPositionsWhite();
        BlockMovementsBlack(false);
        BlockMovementsWhite(true);

        SetCheckPositionsBlack();
        SetMenacingPositionsBlack();
        BlockMovementsWhite(false);
        BlockMovementsBlack(true);


        // Nos aseguramos de que el jaque sea al jugador al que le toca mover.

        if (playerInTurn == Enums.Colours.Black)
        {            
            for (int i = 0; i < checkPositionsWhite.Count; i++)
            {
                // Si el rey blanco está en jaque, lanzamos una advertencia en la interfaz.

                if ((Vector2)blackKing.transform.position == checkPositionsWhite[i])
                {
                    BlackKingInCheck = true;
                    WhiteKingInCheck = false;

                    Interface.interfaceClass.ActivatePanelCheck(Enums.Colours.Black);

                    return;
                }

                // Si no está en jaque, desactivamos este panel (si se activó en el turno previo).

                else
                {
                    BlackKingInCheck = false;
                    WhiteKingInCheck = false;

                    Interface.interfaceClass.DeactivatePanelCheck();
                }
            }
        }

        else
        {
            for (int i = 0; i < checkPositionsBlack.Count; i++)
            {
                // Si el rey negro está en jaque, lanzamos una advertencia en la interfaz.

                if ((Vector2)whiteKing.transform.position == checkPositionsBlack[i])
                {
                    WhiteKingInCheck = true;
                    BlackKingInCheck = false;

                    Interface.interfaceClass.ActivatePanelCheck(Enums.Colours.White);

                    return;
                }

                // Si no está en jaque, desactivamos este panel (si se activó en el turno previo).

                else
                {
                    WhiteKingInCheck = false;
                    BlackKingInCheck = false;

                    Interface.interfaceClass.DeactivatePanelCheck();
                }
            }
        }
    }

    /// <summary>
    /// Comprueba si una posición específica está en jaque para la piezas blancas.
    /// </summary>
    /// <param name="position">La posición que se va a comprobar.</param>
    /// <returns></returns>
    public static bool VerifyWhiteCheckPosition(Vector2 position)
    {
        for (int i = 0; i < checkPositionsWhite.Count; i++)
        {
            if (position == checkPositionsWhite[i])
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Comprueba si una posición específica está en jaque para la piezas negras.
    /// </summary>
    /// <param name="position">La posición que se va a comprobar.</param>
    /// <returns></returns>
    public static bool VerifyBlackCheckPosition(Vector2 position)
    {
        for (int i = 0; i < checkPositionsBlack.Count; i++)
        {
            if (position == checkPositionsBlack[i])
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Comprueba si una posición es una amenaza para las piezas blancas.
    /// </summary>
    /// <param name="position">La posición que se va a comprobar.</param>
    /// <returns></returns>
    public static bool VerifyWhiteMenacedPosition(Vector2 position)
    {
        for (int i = 0; i < menacingWhitePositions.Count; i++)
        {
            if (position == menacingWhitePositions[i])
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Comprueba si una posición es una amenaza para las piezas negras.
    /// </summary>
    /// <param name="position">La posición que se va a comprobar.</param>
    /// <returns></returns>
    public static bool VerifyBlackMenacedPosition(Vector2 position)
    {
        for (int i = 0; i < menacingBlackPositions.Count; i++)
        {
            if (position == menacingBlackPositions[i])
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Localiza una pieza blanca en una posición específica.
    /// </summary>
    /// <param name="position">La posición que se va a comprobar.</param>
    /// <returns></returns>
    public static GameObject GetPieceWhiteInPosition(Vector2 position)
    {
        for (int i = 0; i < PiecesWhite.Count; i++)
        {
            if ((Vector2)PiecesWhite[i].transform.position == position)
            {
                return PiecesWhite[i];
            }
        }

        return null;
    }

    /// <summary>
    /// Localiza una pieza negra en una posición específica.
    /// </summary>
    /// <param name="position">La posición que se va a comprobar.</param>
    /// <returns></returns>
    public static GameObject GetPieceBlackInPosition(Vector2 position)
    {
        for (int i = 0; i < PiecesBlack.Count; i++)
        {
            if ((Vector2)PiecesBlack[i].transform.position == position)
            {
                return PiecesBlack[i];
            }
        }

        return null;
    }

    #endregion

    #region Checkmate

    /// <summary>
    /// Comprueba si las blancas han hecho jaque mate.
    /// </summary>
    /// <returns>Si las piezas blancas han ganado.</returns>
    static bool CheckmateWhiteVerification()
    {
        // Calculamos todos los movimientos posibles para las negras.

        List<Vector2> tempList = new List<Vector2>();

        foreach (GameObject piece in PiecesBlack)
        {
            tempList = tempList.Concat(piece.GetComponent<PiecesMovement>().SearchGreenPositions()).ToList();
        }

        tempList.Distinct().ToList();

        // Limpiamos la lista y eliminamos las casillas ocupadas por piezas negras.

        for (int i = 0; i < tempList.Count; i++)
        {
            if (CheckSquareBlack(tempList[i]))
            {
                tempList.Remove(tempList[i]);

                i--;
            }
        }

        // Si no hay movimientos posibles, se pueden dar dos posibilidades.

        if (tempList.Count == 0)
        {
            // Si el rey negro está en jaque, ganan las blancas.

            if (BlackKingInCheck)
            {
                return true;
            }

            // Si no lo está, la partida termina en tablas. Activamos las variables necesarias.

            else
            {
                stalemate = true;

                return false;
            }
        }

        return false;
    }

    /// <summary>
    /// Comprueba si las negras han hecho jaque mate.
    /// </summary>
    /// <returns>Si las piezas negras han ganado.</returns>
    static bool CheckmateBlackVerification()
    {
        // Calculamos todos los movimientos posibles para las blancas.

        List<Vector2> tempList = new List<Vector2>();

        foreach (GameObject piece in PiecesWhite)
        {
            tempList = tempList.Concat(piece.GetComponent<PiecesMovement>().SearchGreenPositions()).ToList();
        }

        tempList.Distinct().ToList();

        // Limpiamos la lista y eliminamos las casillas ocupadas por piezas blancas.

        for (int i = 0; i < tempList.Count; i++)
        {
            if (CheckSquareWhite(tempList[i]))
            {
                tempList.Remove(tempList[i]);

                i--;
            }
        }

        // Si no hay movimientos posibles, se pueden dar dos posibilidades.

        if (tempList.Count == 0)
        {
            // Si el rey blanco está en jaque, ganan las negras.

            if (WhiteKingInCheck)
            {
                return true;
            }

            // Si no lo está, la partida termina en tablas. Activamos las variables necesarias.

            else
            {
                stalemate = true;

                return false;
            }
        }

        return false;
    }

    #endregion

    #region Draw

    /// <summary>
    /// Indica, tras ciertas comprobaciones, si la partida va a terminar en tablas por ahogado.
    /// </summary>
    static bool stalemate = false;

    /// <summary>
    /// Número de movimientos hechos sin capturar piezas o mover peones.
    /// </summary>
    static int movements = 0;

    /// <summary>
    /// Historial de estados salvados para saber si se han repetido las mismas posiciones.
    /// </summary>
    static readonly List<PositionRecord> savedPositions = new List<PositionRecord>();

    /// <summary>
    /// Lanza un mensaje en la pantalla que indica que la partida ha terminado en tablas.
    /// </summary>
    /// <param name="drawType">El motivo por el que la partida termina en tablas.</param>
    static void FinishWithDraw(Enums.DrawModes drawType)
    {
        Interface.interfaceClass.ActivateDrawMessage(drawType);

        // Desactivamos la posibilidad de elegir las casillas del tablero.

        DisableSelection();

        // Eliminamos la partida de la ranura de autoguardado.

        DeleteAutoSave();
    }

    /// <summary>
    /// Indica si la partida ha terminado en tablas por ser imposible conseguir un jaque mate con las piezas actuales.
    /// </summary>
    /// <returns>Verdadero si no hay suficientes piezas para terminar la partida.</returns>
    static bool DrawByImpossibility()
    {
        // Hacemos diferentes recuentos para ver si es imposible ganar.

        if (PiecesWhite.Count == 1 && PiecesWhite.Count == 1)
        {
            return true;
        }

        if (PiecesWhite.Count == 2 && PiecesBlack.Count == 1 && PiecesWhite[1].GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Knight)
        {
            return true;
        }

        if (PiecesBlack.Count == 2 && PiecesWhite.Count == 1 && PiecesBlack[1].GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Knight)
        {
            return true;
        }

        if (PiecesWhite.Count == 2 && PiecesBlack.Count == 1 && PiecesWhite[1].GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Bishop)
        {
            return true;
        }

        if (PiecesBlack.Count == 2 && PiecesWhite.Count == 1 && PiecesBlack[1].GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Bishop)
        {
            return true;
        }

        if (PiecesWhite.Count == 3 && PiecesBlack.Count == 1 && PiecesWhite[1].GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Knight
            && PiecesWhite[2].GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Knight)
        {
            return true;
        }

        if (PiecesBlack.Count == 3 && PiecesWhite.Count == 1 && PiecesBlack[1].GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Knight
            && PiecesBlack[2].GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Knight)
        {
            return true;
        }

        if (PiecesWhite.Count == 2 && PiecesBlack.Count == 2 && PiecesWhite[1].GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Bishop
            && PiecesBlack[1].GetComponent<PiecesMovement>().PieceType == Pieces.Piece.Bishop
            && (PiecesWhite[1].transform.position.x + PiecesWhite[1].transform.position.y) % 2 == (PiecesBlack[1].transform.position.x + PiecesBlack[1].transform.position.y) % 2)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Indica si las posiciones de las piezas en el tablero (estado) se han repetido tres veces.
    /// </summary>
    /// <returns></returns>
    static bool VerifyRepeatedPositions()
    {
        for (int i = 0; i < savedPositions.Count - 1; i++)
        {
            int repetitions = 0;

            for (int j = i + 1; j < savedPositions.Count; j++)
            {
                if (savedPositions[i].Equals(savedPositions[j]))
                {
                    repetitions++;

                    if (repetitions == 2)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    #endregion

    #region Turns

    /// <summary>
    /// Comienza el turno del siguiente jugador, comprobando previamente si hay un jaque, jaque mate o tablas.
    /// </summary>
    static void NextTurn()
    {
        // En primer lugar indicamos que es el turno del siguiente jugador y mostramos un mensaje en la pantalla para indicarlo.

        if (playerInTurn == Enums.Colours.White)
        {
            playerInTurn = Enums.Colours.Black;

            Interface.interfaceClass.SetWaitingMessage(Enums.Colours.Black);
        }

        else if (playerInTurn == Enums.Colours.Black)
        {
            playerInTurn = Enums.Colours.White;

            Interface.interfaceClass.SetWaitingMessage(Enums.Colours.White);
        }

        // Después comprobamos si la partida ha terminado por jaque mate.

        CheckVerification();

        if (playerInTurn == Enums.Colours.White)
        {
            if (CheckmateWhiteVerification())
            {
                Interface.interfaceClass.ActivateCheckmateMessage(Enums.Colours.White);
                ResetColour();
                DisableSelection();
                DeleteAutoSave();

                return;
            }

            if (CheckmateBlackVerification())
            {
                Interface.interfaceClass.ActivateCheckmateMessage(Enums.Colours.Black);
                ResetColour();
                DisableSelection();
                DeleteAutoSave();

                return;
            }
        }

        else if (playerInTurn == Enums.Colours.Black)
        {
            if (CheckmateBlackVerification())
            {
                Interface.interfaceClass.ActivateCheckmateMessage(Enums.Colours.Black);
                ResetColour();
                DisableSelection();
                DeleteAutoSave();

                return;
            }

            if (CheckmateWhiteVerification())
            {
                Interface.interfaceClass.ActivateCheckmateMessage(Enums.Colours.White);
                ResetColour();
                DisableSelection();
                DeleteAutoSave();

                return;
            }
        }

        // Si no hay jaque mate, comprobamos que no se hayan realizado 75 movimientos (150 turnos) sin capturar piezas o mover peones.

        if (movements == 150)
        {
            FinishWithDraw(Enums.DrawModes.Move75);

            return;
        }

        // Si el mismo estado del tablero se ha repetido tres veces, la partida termina.

        if (savedPositions.Count > 5 && VerifyRepeatedPositions())
        {
            FinishWithDraw(Enums.DrawModes.ThreefoldRepetition);

            return;
        }

        // Comprobamos también si la variable "stalemate" está activa. En este caso, la partida termina.

        if (stalemate)
        {
            FinishWithDraw(Enums.DrawModes.Stalemate);

            return;
        }

        // Comprobamos también si podemos terminar la partida con las piezas en el tablero.

        if (PiecesWhite.Count <= 3 && PiecesBlack.Count <= 3 && DrawByImpossibility())
        {
            FinishWithDraw(Enums.DrawModes.Impossibility);

            return;
        }

        // Si no se ha dado ninguna de las situaciones anteriores, la partida continúa.

        ResetValues();

        // Activamos la IA si es su turno.

        if (singlePlay)
        {
            Interface.interfaceClass.EnableButtonPause(false);
            TimeEvents.timeEvents.StartWaitForAI();
        }
    }

    /// <summary>
    /// Indica si es el turno del jugador.
    /// </summary>
    /// <returns></returns>
    public static bool CheckTurn()
    {
        if (PlayerColour == Enums.Colours.All || PlayerColour == playerInTurn)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    /// <summary>
    /// Elige la pieza situada en una posición concreta para el siguiente movimiento.
    /// </summary>
    /// <param name="position">La posición en la que está la pieza.</param>
    /// <returns>Verdadero si hay una pieza en esta posición y pertenece al jugador.</returns>
    public static bool SelectPiece(Vector2 position)
    {
        if (playerInTurn == Enums.Colours.White)
        {
            GameObject piece = GetPieceWhiteInPosition(position);

            if (piece != null)
            {
                ActivePiece = piece;

                // Todas las casillas que correspondan a movimientos legales de la pieza se vuelven verdes.

                ChangeColour(ActivePiece.transform.position, ActivePiece.GetComponent<PiecesMovement>().SearchGreenPositions());

                return true;
            }
        }

        else if (playerInTurn == Enums.Colours.Black)
        {
            GameObject piece = GetPieceBlackInPosition(position);

            if (piece != null)
            {
                ActivePiece = piece;

                // Todas las casillas que correspondan a movimientos legales de la pieza se vuelven verdes.

                ChangeColour(ActivePiece.transform.position, ActivePiece.GetComponent<PiecesMovement>().SearchGreenPositions());

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Desmarca la pieza seleccionada y vuelve las casillas a su color original.
    /// </summary>
    public static void DeselectPosition()
    {
        ActivePiece = null;

        ResetColour();
    }

    /// <summary>
    /// Resetea las variables necesarias entre turno y turno.
    /// </summary>
    public static void ResetValues()
    {
        if (enPassantPawnPosition != Vector2.zero && ActivePiece != null)
        {
            if (enPassantPawnPosition != (Vector2)ActivePiece.transform.position)
            {
                DeactivateEnPassant();
            }
        }

        ActivePiece = null;

        castlingLeftRook = null;
        castlingRightRook = null;
        castlingLeftDestination = Vector2.zero;
        castlingRightDestination = Vector2.zero;
        castlingLeftPosition = Vector2.zero;
        castlingRightPosition = Vector2.zero;

        AutoSave();
    }

    #endregion

    #region Square Colours

    /// <summary>
    /// Indica a todas las casillas que cambien de color a través del delegado (si está activo).
    /// </summary>
    /// <param name="piecePosition">La posición de la pieza seleccionada (la casilla se vuelve amarilla).</param>
    /// <param name="greenPositions">Las posiciones a las que se puede mover legalmente la pieza (las casillas se vuelven verdes).</param>
    public static void ChangeColour(Vector2 piecePosition, List<Vector2> greenPositions)
    {
        ResetColour();

        if (CheckTurn())
        {
            UpdateColour(piecePosition, greenPositions);
        }
    }

    /// <summary>
    /// Devuelve a todas las piezas a su color original a través del delegado.
    /// </summary>
    public static void ResetColour()
    {
        OriginalColour();
    }

    /// <summary>
    /// Hace que la casilla a la que se acaba de mover una pieza se vuelva roja durante un momento.
    /// </summary>
    /// <param name="position">La posición de la casilla.</param>
    static void ActivateRed(Vector2 position)
    {
        RedSquare(position, null);
    }

    #endregion

    #region Examine Squares

    /// <summary>
    /// Comprueba si la posición indicada es una casilla vacía.
    /// </summary>
    /// <param name="position">La posición que se va a comprobar.</param>
    /// <returns>Verdadero si la posición está vacía.</returns>
    public static bool CheckSquareEmpty(Vector2 position)
    {
        for (int i = 0; i < PiecesWhite.Count; i++)
        {
            if ((Vector2)PiecesWhite[i].transform.position == position && PiecesWhite[i].activeSelf)
            {
                return false;
            }
        }

        for (int i = 0; i < PiecesBlack.Count; i++)
        {
            if ((Vector2)PiecesBlack[i].transform.position == position && PiecesBlack[i].activeSelf)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Comprueba si en la posición indicada hay una pieza blanca.
    /// </summary>
    /// <param name="position">La posición que se va a comprobar.</param>
    /// <returns>Verdadero si hay una pieza blanca en la casilla.</returns>
    public static bool CheckSquareWhite(Vector2 position)
    {
        for (int i = 0; i < PiecesWhite.Count; i++)
        {
            if ((Vector2)PiecesWhite[i].transform.position == position && PiecesWhite[i].activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Comprueba si en la posición indicada hay una pieza negra.
    /// </summary>
    /// <param name="position">La posición que se va a comprobar.</param>
    /// <returns>Verdadero si hay una pieza negra en la casilla.</returns>
    public static bool CheckSquareBlack(Vector2 position)
    {
        for (int i = 0; i < PiecesBlack.Count; i++)
        {
            if ((Vector2)PiecesBlack[i].transform.position == position && PiecesBlack[i].activeSelf)
            {
                return true;
            }
        }

        return false;
    }

    #endregion

    #region Save

    /// <summary>
    /// Guarda la partida en la ranura de autoguardado.
    /// </summary>
    static void AutoSave()
    {
        SaveDataRaw data = new SaveDataRaw
        {
            playerInTurn = playerInTurn,
            enPassantDoublePosition = enPassantPawnPosition,
            enPassantPosition = EnPassantPosition,
            movements = movements,
            savedPositions = savedPositions,
            piecesWhite = PiecesWhite,
            piecesBlack = PiecesBlack
        };

        SaveManager.SaveGame(0, data);
    }

    /// <summary>
    /// Borra la partida en la ranura de autoguardado.
    /// </summary>
    static void DeleteAutoSave()
    {
        SaveManager.DeleteAutoSave();

        Interface.interfaceClass.UpdateSaveDates();
    }

    /// <summary>
    /// Guarda la partida en la ranura indicada.
    /// </summary>
    /// <param name="saveSlot">La ranura de guardado en la que se va a guardar la partida.</param>
    public static void SaveGame(int saveSlot)
    {
        SaveDataRaw data = new SaveDataRaw
        {
            playerInTurn = playerInTurn,
            enPassantDoublePosition = enPassantPawnPosition,
            enPassantPosition = EnPassantPosition,
            movements = movements,
            savedPositions = savedPositions,
            piecesWhite = PiecesWhite,
            piecesBlack = PiecesBlack
        };

        SaveManager.SaveGame(saveSlot, data);
    }

    #endregion

    #region Pause

    /// <summary>
    /// Hace las piezas invisibles y bloquea la selección durante la pausa.
    /// </summary>
    /// <param name="activePause">Verdadero si se activa la pausa, falso si se desactiva.</param>
    public static void PauseGame(bool activePause)
    {
        if (activePause)
        {
            foreach (GameObject piece in PiecesWhite)
            {
                piece.GetComponent<SpriteRenderer>().enabled = false;
            }

            foreach (GameObject piece in PiecesBlack)
            {
                piece.GetComponent<SpriteRenderer>().enabled = false;
            }

            DeselectPosition();
            DisableSelection();
        }

        else
        {
            foreach (GameObject piece in PiecesWhite)
            {
                piece.GetComponent<SpriteRenderer>().enabled = true;
            }

            foreach (GameObject piece in PiecesBlack)
            {
                piece.GetComponent<SpriteRenderer>().enabled = true;
            }

            EnableSelection();
        }
    }

    #endregion

    #region Artificial Intelligence

    /// <summary>
    /// Inicia el movimiento de la pieza en el turno de la IA.
    /// </summary>
    public static void MoveAIPiece()
    {
        if (PlayerColour == Enums.Colours.White && playerInTurn == Enums.Colours.Black)
        {
            AIMovePosition bestMove = MiniMax.BestMovementBlack();
            ActivePiece = bestMove.piece;

            MovePiece(bestMove.position);
        }

        else if (PlayerColour == Enums.Colours.Black && playerInTurn == Enums.Colours.White)
        {
            AIMovePosition bestMove = MiniMax.BestMovementWhite();
            ActivePiece = bestMove.piece;

            MovePiece(bestMove.position);
        }
    }

    #endregion
}