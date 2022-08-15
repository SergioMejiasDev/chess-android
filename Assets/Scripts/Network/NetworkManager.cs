using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

/// <summary>
/// Contiene los métodos necesarios para las funciones online del juego.
/// Hereda de MonoBehaviourPunCallbacks, por lo que se asignará a un objeto de la escena.
/// </summary>
[RequireComponent(typeof(PhotonView))]
public class NetworkManager : MonoBehaviourPunCallbacks
{
    /// <summary>
    /// El singleton de la clase.
    /// </summary>
    public static NetworkManager manager;

    /// <summary>
    /// La partida cargada, si existe.
    /// </summary>
    SaveData loadData = null;

    #region Properties

    /// <summary>
    /// Indica si estamos conectados a los servidores de Photon.
    /// </summary>
    public bool IsConnected { get; private set; }

    /// <summary>
    /// El nombre (tres carácteres) de la sala en la que estamos jugando.
    /// </summary>
    public string ActiveRoom { get; private set; }

    /// <summary>
    /// El identificador del servidor al que nos conectaremos. Viene dado por la documentación de Photon PUN.
    /// El valor usado será el que esté guardado en la opciones del juego, y podrá cambiarse desde allí.
    /// </summary>
    string Token
    {
        get
        {
            switch (Options.ActiveServer)
            {
                case Options.Server.Asia:
                    return "asia";
                case Options.Server.Australia:
                    return "au";
                case Options.Server.CanadaEast:
                    return "cae";
                case Options.Server.Europe:
                    return "eu";
                case Options.Server.India:
                    return "in";
                case Options.Server.Japan:
                    return "jp";
                case Options.Server.RussiaEast:
                    return "rue";
                case Options.Server.RussiaWest:
                    return "ru";
                case Options.Server.SouthAfrica:
                    return "za";
                case Options.Server.SouthAmerica:
                    return "sa";
                case Options.Server.SouthKorea:
                    return "kr";
                case Options.Server.Turkey:
                    return "tr";
                case Options.Server.USAEast:
                    return "us";
                case Options.Server.USAWest:
                    return "usw";
                default:
                    return "eu";
            }
        }
    }

    /// <summary>
    /// String compuesto por tres carácteres aleatorios usado para crear el nombre de la sala.
    /// </summary>
    string RandomRoom
    {
        get
        {
            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            char char1 = characters[Random.Range(0, characters.Length)];
            char char2 = characters[Random.Range(0, characters.Length)];
            char char3 = characters[Random.Range(0, characters.Length)];

            return char1.ToString() + char2.ToString() + char3.ToString();
        }
    }

    #endregion

    private void Awake()
    {
        manager = this;
    }

    #region Conection

    /// <summary>
    /// Conecta el juego a los servidores de Photon.
    /// </summary>
    public void ConnectToServer()
    {
        // Un mensaje aparece en la pantalla indicando que nos estamos conectando.

        Interface.interfaceClass.OpenPanelMenu(2);

        // Nos conectamos al servidor elegido.

        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = Token;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        // Indicamos que estamos conectados a Photon.

        IsConnected = true;

        // Indicamos a Photon como serializar y deserializar la clase SaveData para poder transmitirla a través del servidor.
        // Esto será necesario para cuando carguemos una partida online.

        PhotonPeer.RegisterType(typeof(SaveData), (byte)'S', SaveManager.Serialize, SaveManager.Deserialize);

        // El mensaje de "Conectando" desaparece y se abre el menú de selección de partida.

        Interface.interfaceClass.UpdateServerName();
        Interface.interfaceClass.OpenPanelMenu(6);
    }

    /// <summary>
    /// Termina la conexión del juego con los servidores de Photon.
    /// </summary>
    public void DisconnectFromServer()
    {
        // Indicamos que estamos desconectados y llama al método necesario para la desconexión.

        IsConnected = false;
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // Hay muchas posibles razones para que se produzca la desconexión del servidor.
        // Desde aquí se lanza el mensaje que explica lo sucedido al usuario.

        switch (cause)
        {
            // El servidor está completo (se ha alcanzado el límite de 20 jugadores simultaneos).
            case DisconnectCause.MaxCcuReached:
                Interface.interfaceClass.OpenErrorPanel(cause);
                break;

            // El dispositivo no está conectado a Internet en este momento.
            case DisconnectCause.DnsExceptionOnConnect:
                Interface.interfaceClass.OpenErrorPanel(cause);
                break;

            // El servidor (la región seleccionada) no está operativa en este momento.
            case DisconnectCause.InvalidRegion:
                Interface.interfaceClass.OpenErrorPanel(cause);
                break;

            // Este caso sucede cuando el jugador 1 se desconecta manualmente, o cuando se pierde la conexión con él.
            // Para el primer caso no será necesario, pero para el segundo, lanzamos un mensaje de error.
            case DisconnectCause.DisconnectByClientLogic:
                if (Chess.IsPlaying)
                    Interface.interfaceClass.ErrorPlayerLeftRoom();
                break;

            // El resto de los posibles errores están incluidos aquí, lanzando un mensaje de error genérico.
            default:
                Interface.interfaceClass.OpenErrorPanel(cause);
                break;
        }

        Debug.Log(cause);
    }

    /// <summary>
    /// Crea una sala con un nombre aleatorio y comienza la espera del segundo jugador para empezar una partida desde el principio.
    /// </summary>
    public void CreateRoom()
    {
        // Indicamos que no hay una partida cargada, por lo que iniciamos la partida desde el principio.

        loadData = null;

        // Obtenemos un nombre aleatorio para la sala.

        ActiveRoom = RandomRoom;

        // Creamos una sala con el nombre obtenido previamente y establecemos un máximo de dos jugadores.

        PhotonNetwork.CreateRoom(ActiveRoom, new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
    }

    /// <summary>
    /// Crea una sala con un nombre aleatorio y comienza la espera del segundo jugador para empezar una partida cargada.
    /// </summary>
    /// <param name="saveSlot">La ranura de guardado que se va a cargar.</param>
    public void CreateLoadedRoom(int saveSlot)
    {
        // Cargamos la partida de la ranura elegida.

        loadData = SaveManager.LoadGame(saveSlot);

        // Obtenemos un nombre aleatorio para la sala.

        ActiveRoom = RandomRoom;

        // Creamos una sala con el nombre obtenido previamente y establecemos un máximo de dos jugadores.

        PhotonNetwork.CreateRoom(ActiveRoom, new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        // Si la sala se ha creado sin problemas, se abre el menú que indica que estamos al segundo jugador.
        // Se mostrará el nombre de la sala para que este pueda unirse.

        Interface.interfaceClass.OpenPanelWaitingPlayer();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        // Si la creación de la sala ha fallado (existe una sala con el mismo nombre), repetimos la función con un nombre diferente.

        ActiveRoom = "";

        CreateRoom();
    }

    /// <summary>
    /// Iniciamos la conexión con la sala creada previamente por otro jugador.
    /// </summary>
    /// <param name="roomName">Nombre de la sala a la que nos conectaremos.</param>
    public void JoinRoom(string roomName)
    {
        ActiveRoom = roomName;
        PhotonNetwork.JoinRoom(ActiveRoom);
    }

    public override void OnJoinedRoom()
    {
        // Si un jugador se une a la sala y el número de jugadores es de uno (has creado la sala), se te asigna el color blanco.

        if (PhotonNetwork.PlayerList.Length == 1)
        {
            Chess.PlayerColour = Enums.Colours.White;
        }

        // Si un jugador se una a la sala y el número de jugadores es de dos (la sala la creó otro jugador), se te asigna el color negro.

        else if (PhotonNetwork.PlayerList.Length == 2)
        {
            Chess.PlayerColour = Enums.Colours.Black;
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        // Si la sala a la que nos vamos a unir no existe o contiene ya dos jugadores, enviamos un mensaje de error al usuario.

        Interface.interfaceClass.OpenPanelGame(5);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // Si el segundo jugador entra en la sala, se inicia la partida.
        // Esta función solo se activa en el dispositivo que creó la partida.

        if (PhotonNetwork.PlayerList.Length == 2)
        {
            // Si no hay ninguna partida cargada, empezamos desde el principio.

            if (loadData == null)
            {
                StartGame();
            }

            // Si hay una partida cargada, se continúa esta partida.

            else
            {
                StartLoadedGame();
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // Si uno de los dos jugadores deja la sala, el otro jugador se desconecta, mostrando un mensaje de error en la pantalla.

        DisconnectFromServer();
    }

    #endregion

    #region Game RPCs

    /// <summary>
    /// Comienza una nueva partida en los dos dispositivos conectados a la sala.
    /// </summary>
    public void StartGame()
    {
        photonView.RPC("StartGameRPC", RpcTarget.All);
    }

    /// <summary>
    /// Una nueva partida comienza en este dispositivo y remotamente en el dispositivo del jugador dos.
    /// </summary>
    [PunRPC]
    void StartGameRPC()
    {
        Chess.StartNewGame();
    }

    /// <summary>
    /// Comienza una partida cargada en los dos dispositivos conectados a la sala.
    /// </summary>
    public void StartLoadedGame()
    {
        photonView.RPC("StartLoadedGameRPC", RpcTarget.All, loadData);
    }

    /// <summary>
    /// Una partida cargada comienza en este dispositivo y remotamente en el dispositivo del jugador dos.
    /// El jugador uno le envía al jugador dos la partida guardada y él la carga también.
    /// </summary>
    [PunRPC]
    void StartLoadedGameRPC(SaveData data)
    {
        Chess.StartLoadedGame(data);
    }

    /// <summary>
    /// Mueve una pieza en todos los dispositivos conectados a la sala.
    /// </summary>
    /// <param name="piecePosition">La posición de la pieza que se ha movido.</param>
    /// <param name="movePosition">La posición a la que se va a mover la pieza..</param>
    public void MovePiece(Vector2 piecePosition, Vector2 movePosition)
    {
        photonView.RPC("MovePieceRPC", RpcTarget.All, piecePosition, movePosition);
    }

    /// <summary>
    /// La pieza indicada se mueve en este dispositivo y un RPC se lanza para que ocurra lo mismo en el otro dispositivo.
    /// </summary>
    /// <param name="piecePosition">La posición de la pieza que se ha movido.</param>
    /// <param name="movePosition">La posición a la que se va a mover la pieza..</param>
    [PunRPC]
    void MovePieceRPC(Vector2 piecePosition, Vector2 movePosition)
    {
        // Localizamos la pieza en la posición indicada.

        Chess.SelectPiece(piecePosition);

        // La movemos a la posición que le hemos señalado.

        Chess.MovePiece(movePosition);
    }

    /// <summary>
    /// Promocionamos a un peón (lo cambiamos por otra pieza) en todos los dispositivos de la sala.
    /// </summary>
    /// <param name="piece">El tipo de pieza a la que vamos a promocionar.</param>
    /// <param name="colour">El color de la pieza que va a promocionar.</param>
    public void PromotePiece(Enums.PromotablePieces piece, Pieces.Colour colour)
    {
        photonView.RPC("PromotePieceRPC", RpcTarget.All, piece, colour);
    }

    /// <summary>
    /// Cambiamos el peón por la pieza elegida en este dispositivo y a través de un RPC hacemos lo mismo en el otro dispositivo.
    /// </summary>
    /// <param name="piece">El tipo de pieza a la que vamos a promocionar.</param>
    /// <param name="colour">El color de la pieza que va a promocionar.</param>
    [PunRPC]
    void PromotePieceRPC(Enums.PromotablePieces piece, Pieces.Colour colour)
    {
        Chess.PieceSelectedToPromotion(piece, colour);
    }

    /// <summary>
    /// Desconectamos a todos los jugadores tras terminar la partida.
    /// </summary>
    public void DisconnectAll()
    {
        photonView.RPC("DisconnectAllRPC", RpcTarget.AllViaServer);
    }

    /// <summary>
    /// Lanzamos un RPC al servidor para que desconecte a todos los jugadores al mismo tiempo.
    /// </summary>
    [PunRPC]
    void DisconnectAllRPC()
    {
        // Antes de desconectarnos del servidor, indicamos que no estamos jugando para que no salte el respectivo mensaje de error.

        Chess.IsPlaying = false;

        DisconnectFromServer();
    }

    #endregion
}