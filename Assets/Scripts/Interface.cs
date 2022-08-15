using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Contiene los métodos necesarios para gestionar la interfaz del juego.
/// </summary>
public class Interface : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// Singleton de la clase.
    /// </summary>
    public static Interface interfaceClass;

    /// <summary>
    /// Panel que contiene todos los subpaneles del menú principal.
    /// </summary>
    [Header("Main Menu")]
    [SerializeField] GameObject mainMenu = null;
    /// <summary>
    /// Todos los subpaneles del menú principal.
    /// </summary>
    [SerializeField] GameObject[] panelsMenu = null;
    /// <summary>
    /// Texto donde se mostrarán los mensajes de error relacionados con la conexión.
    /// </summary>
    [SerializeField] Text textErrorConnection = null;
    /// <summary>
    /// Texto donde se indica el servidor al que estamos conectados.
    /// </summary>
    [SerializeField] Text serverText = null;

    /// <summary>
    /// Texto con la fecha de guardado de la ranura 0.
    /// </summary>
    [Header("Panel Load")]
    [SerializeField] Text textLoad0 = null;
    /// <summary>
    /// Texto con la fecha de guardado de la ranura 1.
    /// </summary>
    [SerializeField] Text textLoad1 = null;
    /// <summary>
    /// Texto con la fecha de guardado de la ranura 2.
    /// </summary>
    [SerializeField] Text textLoad2 = null;
    /// <summary>
    /// Texto con la fecha de guardado de la ranura 3.
    /// </summary>
    [SerializeField] Text textLoad3 = null;
    /// <summary>
    /// Botón para cargar la partida de la ranura 0.
    /// </summary>
    [SerializeField] Button buttonLoad0 = null;
    /// <summary>
    /// Botón para cargar la partida de la ranura 1.
    /// </summary>
    [SerializeField] Button buttonLoad1 = null;
    /// <summary>
    /// Botón para cargar la partida de la ranura 2.
    /// </summary>
    [SerializeField] Button buttonLoad2 = null;
    /// <summary>
    /// Botón para cargar la partida de la ranura 3.
    /// </summary>
    [SerializeField] Button buttonLoad3 = null;

    /// <summary>
    /// Botón de pausa en la esquina de la pantalla con forma de engranaje.
    /// </summary>
    [Header("Panel Pause")]
    [SerializeField] GameObject buttonPauseObject = null;
    /// <summary>
    /// Panel que contiene todos los subpaneles del menú de pausa.
    /// </summary>
    [SerializeField] GameObject panelPause = null;
    /// <summary>
    /// Todos los subpaneles del menú de pausa.
    /// </summary>
    [SerializeField] GameObject[] panelsPause = null;
    /// <summary>
    /// Texto con la fecha de guardado de la ranura 1.
    /// </summary>
    [SerializeField] Text textSave1 = null;
    /// <summary>
    /// Texto con la fecha de guardado de la ranura 2.
    /// </summary>
    [SerializeField] Text textSave2 = null;
    /// <summary>
    /// Texto con la fecha de guardado de la ranura 3.
    /// </summary>
    [SerializeField] Text textSave3 = null;
    /// <summary>
    /// Botón para guardar la partida en la ranura 1.
    /// </summary>
    [SerializeField] Button buttonSave1 = null;
    /// <summary>
    /// Botón para guardar la partida en la ranura 2.
    /// </summary>
    [SerializeField] Button buttonSave2 = null;
    /// <summary>
    /// Botón para guardar la partida en la ranura 3.
    /// </summary>
    [SerializeField] Button buttonSave3 = null;
    /// <summary>
    /// Botón para confirmar que se va a guardar la partida en caso de sobreescribir.
    /// </summary>
    [SerializeField] Button buttonConfirmSave = null;
    /// <summary>
    /// Botón de guardado en el menú de pausa.
    /// </summary>
    [SerializeField] GameObject buttonSave = null;
    /// <summary>
    /// Botón de reinicio en el menú de pausa.
    /// </summary>
    [SerializeField] GameObject buttonRestart = null;
    /// <summary>
    /// Botón para confirmar que se va a reiniciar la partida.
    /// </summary>
    [SerializeField] Button buttonConfirmRestart = null;

    /// <summary>
    /// Panel marrón mostrado a la derecha de la pantalla durante la partida.
    /// </summary>
    [Header("Panel Game")]
    [SerializeField] GameObject panelGame = null;
    /// <summary>
    /// Todos los subpaneles del panel marrón activo durante la partida.
    /// </summary>
    [SerializeField] GameObject[] panelsGame = null;
    /// <summary>
    /// El botón para volver a jugar tras terminar la partida.
    /// </summary>
    [SerializeField] Button buttonRestartEndGame = null;
    /// <summary>
    /// Botón para empezar a jugar contra la IA usando las piezas blancas.
    /// </summary>
    [SerializeField] Button buttonColourWhite = null;
    /// <summary>
    /// Botón para empezar a jugar contra la IA usando las piezas negras.
    /// </summary>
    [SerializeField] Button buttonColourBlack = null;
    /// <summary>
    /// Sonido que se reproduce cuando una pieza se mueve.
    /// </summary>
    [SerializeField] AudioSource moveSound = null;

    /// <summary>
    /// Panel donde aparecen las notificaciones de la partida.
    /// </summary>
    [Header("Panel Notifications")]
    [SerializeField] GameObject panelNotifications = null;
    /// <summary>
    /// Texto del panel de notificaciones.
    /// </summary>
    [SerializeField] Text notificationsText = null;
    /// <summary>
    /// Círculo de colores dentro del panel de notificaciones.
    /// </summary>
    [SerializeField] Image notificationsImage = null;

    /// <summary>
    /// Panel donde se indica que un jugador está en jaque.
    /// </summary>
    [Header("Panel Check")]
    [SerializeField] GameObject panelCheck = null;
    /// <summary>
    /// Texto que indica que un jugador está en jaque.
    /// </summary>
    [SerializeField] Text checkText = null;

    /// <summary>
    /// Texto con el nombre de la sala que indica que estamos esperando a que se una otro jugador a la partida.
    /// </summary>
    [Header("Panel Waiting Player 2")]
    [SerializeField] Text textWaitingRoomName = null;

    /// <summary>
    /// Texto donde escribimos el nombre de la sala a la que nos queremos unir.
    /// </summary>
    [Header("Panel Keyboard")]
    [SerializeField] Text textRoomName = null;

    /// <summary>
    /// Texto que indica que un peón ha sido promocionado.
    /// </summary>
    [Header("Panel Promotion")]
    [SerializeField] Text promotionText = null;
    /// <summary>
    /// Panel para elegir la pieza en la que promociona un peón blanco.
    /// </summary>
    [SerializeField] GameObject panelPiecesWhite = null;
    /// <summary>
    /// Panel para elegir la pieza en la que promociona un peón negro.
    /// </summary>
    [SerializeField] GameObject panelPiecesBlack = null;

    #endregion

    public void Awake()
    {
        interfaceClass = this;

        Options.LoadOptions();

        LetterBoxer.AddLetterBoxing();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        QualitySettings.vSyncCount = 0;

        Application.targetFrameRate = 60;

        UpdateSaveDates();
    }

    #region Main Menu

    /// <summary>
    /// Abre el menú principal con el subpanel indicado.
    /// </summary>
    /// <param name="panel">El subpanel que se va a abrir.</param>
    public void OpenPanelMenu(GameObject panel)
    {
        for (int i = 0; i < panelsMenu.Length; i++)
        {
            panelsMenu[i].SetActive(false);
        }

        panel.SetActive(true);
        mainMenu.SetActive(true);
        panelPause.SetActive(false);
        panelGame.SetActive(false);
    }

    /// <summary>
    /// Abre el menú principal con el subpanel indicado.
    /// </summary>
    /// <param name="panel">El índice del subpanel que se va a abrir.</param>
    public void OpenPanelMenu(int panel)
    {
        for (int i = 0; i < panelsMenu.Length; i++)
        {
            panelsMenu[i].SetActive(false);
        }

        panelsMenu[panel].SetActive(true);
        mainMenu.SetActive(true);
        panelPause.SetActive(false);
        panelGame.SetActive(false);
    }

    /// <summary>
    /// Abre el panel que muestra la notificación de error de conexión.
    /// </summary>
    /// <param name="cause">La causa del error de conexión.</param>
    public void OpenErrorPanel(Photon.Realtime.DisconnectCause cause)
    {
        switch (cause)
        {
            case Photon.Realtime.DisconnectCause.MaxCcuReached:
                textErrorConnection.text = ServerFull();
                break;
            case Photon.Realtime.DisconnectCause.DnsExceptionOnConnect:
                textErrorConnection.text = NoInternetConnection();
                break;
            case Photon.Realtime.DisconnectCause.InvalidRegion:
                textErrorConnection.text = InvalidRegion();
                break;
            default:
                textErrorConnection.text = GenericServerError();
                break;
        }

        Chess.CleanScene();

        OpenPanelMenu(3);
    }

    /// <summary>
    /// Comienza una nueva partida con las piezas blancas contra la IA.
    /// </summary>
    public void NewGameWhite()
    {
        buttonPauseObject.SetActive(true);
        panelCheck.SetActive(false);
        OpenPanelGame(0);

        buttonRestartEndGame.onClick.AddListener(delegate { OpenPanelColours(); });
        buttonConfirmRestart.onClick.AddListener(delegate { OpenPanelColours(); });

        Chess.SelectColor(Enums.Colours.White, null);
    }

    /// <summary>
    /// Comienza una nueva partida con las piezas negras contra la IA.
    /// </summary>
    public void NewGameBlack()
    {
        buttonPauseObject.SetActive(true);
        panelCheck.SetActive(false);
        OpenPanelGame(0);

        buttonRestartEndGame.onClick.AddListener(delegate { OpenPanelColours(); });
        buttonConfirmRestart.onClick.AddListener(delegate { OpenPanelColours(); });

        Chess.SelectColor(Enums.Colours.Black, null);
    }

    /// <summary>
    /// Comienza una partida cargada con las piezas blancas contra la IA.
    /// </summary>
    /// <param name="saveSlot">La ranura que se va a cargar.</param>
    public void LoadGameWhite(int saveSlot)
    {
        buttonPauseObject.SetActive(true);
        panelCheck.SetActive(false);
        OpenPanelGame(0);

        buttonRestartEndGame.onClick.AddListener(delegate { OpenPanelColours(); });
        buttonConfirmRestart.onClick.AddListener(delegate { OpenPanelColours(); });

        Chess.SelectColor(Enums.Colours.White, SaveManager.LoadGame(saveSlot));
    }

    /// <summary>
    /// Comienza una partida cargada con las piezas negras contra la IA.
    /// </summary>
    /// <param name="saveSlot">La ranura que se va a cargar.</param>
    public void LoadGameBlack(int saveSlot)
    {
        buttonPauseObject.SetActive(true);
        panelCheck.SetActive(false);
        OpenPanelGame(0);

        buttonRestartEndGame.onClick.AddListener(delegate { OpenPanelColours(); });
        buttonConfirmRestart.onClick.AddListener(delegate { OpenPanelColours(); });

        Chess.SelectColor(Enums.Colours.Black, SaveManager.LoadGame(saveSlot));
    }

    /// <summary>
    /// Comienza una nueva partida multijugador local.
    /// </summary>
    public void NewGame()
    {
        panelCheck.SetActive(false);
        OpenPanelGame(0);

        buttonRestartEndGame.onClick.AddListener(delegate { NewGame(); });
        buttonConfirmRestart.onClick.AddListener(delegate { NewGame(); });

        buttonPauseObject.SetActive(true);

        Chess.CleanScene();
        Chess.StartNewGame();
    }

    /// <summary>
    /// Comienza una partida cargada multijugador local.
    /// </summary>
    /// <param name="saveSlot">La ranura que se va a cargar.</param>
    public void LoadGame(int saveSlot)
    {
        panelCheck.SetActive(false);
        OpenPanelGame(0);

        buttonRestartEndGame.onClick.AddListener(delegate { NewGame(); });
        buttonConfirmRestart.onClick.AddListener(delegate { NewGame(); });

        buttonPauseObject.SetActive(true);

        SaveData data = SaveManager.LoadGame(saveSlot);

        Chess.CleanScene();
        Chess.StartLoadedGame(data);
    }

    /// <summary>
    /// Actualiza el texto donde se muestra el servidor al que estamos conectados.
    /// </summary>
    public void UpdateServerName()
    {
        string server = "";

        switch (Options.ActiveServer)
        {
            case Options.Server.Asia:
                server = "Asia";
                break;
            case Options.Server.Australia:
                server = "Australia";
                break;
            case Options.Server.CanadaEast:
                server = "Canada East";
                break;
            case Options.Server.Europe:
                server = "Europe";
                break;
            case Options.Server.India:
                server = "India";
                break;
            case Options.Server.Japan:
                server = "Japan";
                break;
            case Options.Server.RussiaEast:
                server = "Russia East";
                break;
            case Options.Server.RussiaWest:
                server = "Russia West";
                break;
            case Options.Server.SouthAfrica:
                server = "South Africa";
                break;
            case Options.Server.SouthAmerica:
                server = "South America";
                break;
            case Options.Server.SouthKorea:
                server = "South Korea";
                break;
            case Options.Server.Turkey:
                server = "Turkey";
                break;
            case Options.Server.USAEast:
                server = "USA East";
                break;
            case Options.Server.USAWest:
                server = "USA West";
                break;
        }

        serverText.text = "Server " + server;
    }

    /// <summary>
    /// Activa el botón de guardado durante la partida online.
    /// Al principio de la partida, el botón estará desactivado para impedir que se guarde la partida antes de que comience.
    /// </summary>
    public void EnableOnlineSave()
    {
        buttonSave.SetActive(true);
    }

    /// <summary>
    /// Guarda la partida en la ranura de guardado seleccionada.
    /// </summary>
    /// <param name="saveSlot">La ranura en la que se va a guardar la partida.</param>
    public void SaveGame(int saveSlot)
    {
        Chess.SaveGame(saveSlot);

        UpdateSaveDates();
        OpenPanelPause(1);
    }

    /// <summary>
    /// Abre un enlace externo.
    /// </summary>
    /// <param name="link">El enlace que se va a abrir de entre los que aparecen en el método.</param>
    public void OpenLink(int link)
    {
        if (link == 0)
        {
            Application.OpenURL("https://play.google.com/store/apps/developer?id=Sergio+Mejias");
        }

        else
        {
            Application.OpenURL("https://gitlab.com/SergioMejiasDev/chess-android");
        }
    }

    /// <summary>
    /// Cambia la resolución de la pantalla por la seleccionada.
    /// </summary>
    /// <param name="resolution">El índice de la resolución que queremos elegir.</param>
    public void ChangeResolution(int resolution)
    {
        switch (resolution)
        {
            case 0:
                Options.ActiveResolution = Options.Resolution.Fullscreen;
                Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.ExclusiveFullScreen);
                break;

            case 1:
                Options.ActiveResolution = Options.Resolution.Windowed720;
                Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
                break;

            case 2:
                Options.ActiveResolution = Options.Resolution.Windowed480;
                Screen.SetResolution(854, 480, FullScreenMode.Windowed);
                break;
        }

        Options.SaveOptions();
    }

    /// <summary>
    /// Cambia el idioma de la aplicación por el seleccionado.
    /// </summary>
    /// <param name="language">El índice del idioma elegido.</param>
    public void ChangeLanguage(int language)
    {
        switch (language)
        {
            case 0:
                Options.ActiveLanguage = Options.Language.EN;
                break;

            case 1:
                Options.ActiveLanguage = Options.Language.ES;
                break;

            case 2:
                Options.ActiveLanguage = Options.Language.CA;
                break;

            case 3:
                Options.ActiveLanguage = Options.Language.IT;
                break;
        }

        Options.SaveOptions();
        UpdateSaveDates();

        OpenPanelMenu(10);
    }

    /// <summary>
    /// Cambia el servidor de Photon por el seleccionado.
    /// </summary>
    /// <param name="server">El índice del servidor de Photon que queremos activar.</param>
    public void ChangeServer(int server)
    {
        Options.ActiveServer = (Options.Server)server;

        Options.SaveOptions();

        OpenPanelMenu(10);
    }

    /// <summary>
    /// Cierra la aplicación completamente.
    /// </summary>
    public void CloseGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    #endregion

    #region Pause Panel

    /// <summary>
    /// Abre el menú de pausa, haciendo las piezas invisibles.
    /// </summary>
    public void OpenPause()
    {
        if (!panelPause.activeSelf)
        {
            OpenPanelPause(0);
            Chess.PauseGame(true);
        }

        else
        {
            panelPause.SetActive(false);
            panelGame.SetActive(true);
            Chess.PauseGame(false);
        }
    }

    /// <summary>
    /// Abre el menú de pausa junto con el subpanel elegido.
    /// </summary>
    /// <param name="panel">El subpanel que se va a abrir.</param>
    public void OpenPanelPause(GameObject panel)
    {
        for (int i = 0; i < panelsPause.Length; i++)
        {
            panelsPause[i].SetActive(false);
        }

        panel.SetActive(true);
        panelGame.SetActive(false);
        panelPause.SetActive(true);
    }

    /// <summary>
    /// Abre el menú de pausa junto con el subpanel elegido.
    /// </summary>
    /// <param name="panel">El índice del subpanel que se va a abrir.</param>
    public void OpenPanelPause(int panel)
    {
        for (int i = 0; i < panelsPause.Length; i++)
        {
            panelsPause[i].SetActive(false);
        }

        panelsPause[panel].SetActive(true);
        panelGame.SetActive(false);
        panelPause.SetActive(true);
    }

    /// <summary>
    /// Actualiza los textos de los botones de cargado y guardado.
    /// Si una ranura está vacía, desactiva los botones de cargado.
    /// </summary>
    public void UpdateSaveDates()
    {
        string[] dates = SaveManager.GetDates();

        textLoad0.text = dates[0] != "0" ? ("AutoSave:  " + dates[0]) : ("AutoSave:  " + EmptyText());
        textLoad1.text = dates[1] != "0" ? ("Save 01:  " + dates[1]) : ("Save 01:  " + EmptyText());
        textLoad2.text = dates[2] != "0" ? ("Save 02:  " + dates[2]) : ("Save 02:  " + EmptyText());
        textLoad3.text = dates[3] != "0" ? ("Save 03:  " + dates[3]) : ("Save 03:  " + EmptyText());

        buttonLoad0.enabled = dates[0] != "0";
        buttonLoad1.enabled = dates[1] != "0";
        buttonLoad2.enabled = dates[2] != "0";
        buttonLoad3.enabled = dates[3] != "0";

        textSave1.text = dates[1] != "0" ? ("Save 01:  " + dates[1]) : ("Save 01:  " + EmptyText());
        textSave2.text = dates[2] != "0" ? ("Save 02:  " + dates[2]) : ("Save 02:  " + EmptyText());
        textSave3.text = dates[3] != "0" ? ("Save 03:  " + dates[3]) : ("Save 03:  " + EmptyText());

        buttonSave1.onClick.AddListener(delegate { ButtonSave(1, dates[1] == "0"); });
        buttonSave2.onClick.AddListener(delegate { ButtonSave(2, dates[2] == "0"); });
        buttonSave3.onClick.AddListener(delegate { ButtonSave(3, dates[3] == "0"); });
    }

    /// <summary>
    /// Actualiza los listeners de los botones de cargado y guardado de acuerdo con el modo de juego activo.
    /// </summary>
    /// <param name="gameMode">1 Un jugador, 2 Multijugador local, 3 Multijugador Online.</param>
    public void UpdateLoadButton(int gameMode)
    {
        switch (gameMode)
        {
            case 1:
                buttonLoad0.onClick.AddListener(delegate { OpenPanelColoursLoad(0); });
                buttonLoad1.onClick.AddListener(delegate { OpenPanelColoursLoad(1); });
                buttonLoad2.onClick.AddListener(delegate { OpenPanelColoursLoad(2); });
                buttonLoad3.onClick.AddListener(delegate { OpenPanelColoursLoad(3); });
                break;

            case 2:
                buttonLoad0.onClick.AddListener(delegate { LoadGame(0); });
                buttonLoad1.onClick.AddListener(delegate { LoadGame(1); });
                buttonLoad2.onClick.AddListener(delegate { LoadGame(2); });
                buttonLoad3.onClick.AddListener(delegate { LoadGame(3); });
                break;

            case 3:
                buttonLoad0.onClick.AddListener(delegate { NetworkManager.manager.CreateLoadedRoom(0); });
                buttonLoad1.onClick.AddListener(delegate { NetworkManager.manager.CreateLoadedRoom(1); });
                buttonLoad2.onClick.AddListener(delegate { NetworkManager.manager.CreateLoadedRoom(2); });
                buttonLoad3.onClick.AddListener(delegate { NetworkManager.manager.CreateLoadedRoom(3); });
                break;
        }
    }

    /// <summary>
    /// Cierra la partida actual y vuelve al menú.
    /// </summary>
    public void BackToMenuEndGame()
    {
        Chess.CleanScene();
        buttonRestart.SetActive(true);

        buttonPauseObject.SetActive(true);
        OpenPanelMenu(0);
        NetworkManager.manager.DisconnectFromServer();
    }

    /// <summary>
    /// Guarda la partida actual.
    /// </summary>
    /// <param name="saveSlot">La ranura en la que se va a guardar la partida.</param>
    /// <param name="emptySlot">Indica si la ranura en la que se va a guardar está vacía.</param>
    public void ButtonSave(int saveSlot, bool emptySlot)
    {
        // Si la ranura está vacía, guardamos la partida directamente.

        if (emptySlot)
        {
            SaveGame(saveSlot);
        }

        // Si hay una partida guardada, aparece un mensaje para confirmar que queremos sobreescribir esta partida.

        else
        {
            buttonConfirmSave.onClick.AddListener(delegate { SaveGame(saveSlot); });
            OpenPanelPause(4);
        }
    }

    /// <summary>
    /// Activa o desactiva el botón de pausa.
    /// </summary>
    /// <param name="enable">Activar o desactivar.</param>
    public void EnableButtonPause(bool enable)
    {
        if (enable)
        {
            buttonPauseObject.SetActive(true);
        }

        else
        {
            buttonPauseObject.SetActive(false);
        }
    }

    #endregion

    #region Game Panel

    /// <summary>
    /// Abre el panel marrón a la derecha de la pantalla junto con el subpanel elegido.
    /// </summary>
    /// <param name="panel">El subpanel que se va a abrir.</param>
    public void OpenPanelGame(GameObject panel)
    {
        for (int i = 0; i < panelsGame.Length; i++)
        {
            panelsGame[i].SetActive(false);
        }

        panel.SetActive(true);
        mainMenu.SetActive(false);
        panelPause.SetActive(false);
        panelGame.SetActive(true);
    }

    /// <summary>
    /// Abre el panel marrón a la derecha de la pantalla junto con el subpanel elegido.
    /// </summary>
    /// <param name="panel">El índice del subpanel que se va a abrir.</param>
    public void OpenPanelGame(int panel)
    {
        for (int i = 0; i < panelsGame.Length; i++)
        {
            panelsGame[i].SetActive(false);
        }

        panelsGame[panel].SetActive(true);
        mainMenu.SetActive(false);
        panelPause.SetActive(false);
        panelGame.SetActive(true);
    }

    /// <summary>
    /// Abre un panel de error para indicar que la conexión con el otro jugador se ha perdido, cerrando la partida.
    /// </summary>
    public void ErrorPlayerLeftRoom()
    {
        Chess.CleanScene();

        buttonPauseObject.SetActive(true);
        OpenPanelMenu(8);
    }

    /// <summary>
    /// Abre el panel de selección de color en una nueva partida contra la IA.
    /// </summary>
    public void OpenPanelColours()
    {
        Chess.CleanScene();
        buttonPauseObject.SetActive(false);

        buttonColourWhite.onClick.AddListener(delegate { NewGameWhite(); });
        buttonColourBlack.onClick.AddListener(delegate { NewGameBlack(); });
        
        OpenPanelGame(7);
    }

    /// <summary>
    /// Abre el panel de selección de color en una partida cargada contra la IA.
    /// </summary>
    /// <param name="saveSlot">La ranura que se va a cargar.</param>
    public void OpenPanelColoursLoad(int saveSlot)
    {
        Chess.CleanScene();
        buttonPauseObject.SetActive(false);

        buttonColourWhite.onClick.AddListener(delegate { LoadGameWhite(saveSlot); });
        buttonColourBlack.onClick.AddListener(delegate { LoadGameBlack(saveSlot); });
        
        OpenPanelGame(7);
    }

    /// <summary>
    /// Reproduce un sonido al moverse las piezas.
    /// </summary>
    public void PlayMoveSound()
    {
        moveSound.Play();
    }

    #endregion

    #region Waiting Panel

    /// <summary>
    /// Activa el mensaje que indica que es el turno de un jugador y que le toca jugar.
    /// </summary>
    /// <param name="colour">El color del jugador en turno.</param>
    public void SetWaitingMessage(Enums.Colours colour)
    {
        OpenPanelGame(0);

        notificationsText.text = (colour == Enums.Colours.White) ? WaitingMessageWhite() : WaitingMessageBlack();
        notificationsImage.color = (colour == Enums.Colours.White) ? Color.white : Color.black;
    }

    #endregion

    #region Waiting Player 2 Panel

    /// <summary>
    /// Abre el panel que indica que estamos esperando a un segundo jugador para empezar la partida.
    /// </summary>
    public void OpenPanelWaitingPlayer()
    {
        OpenPanelGame(2);
        textWaitingRoomName.text = RoomName();
        buttonSave.SetActive(false);
        buttonRestart.SetActive(false);
    }

    #endregion

    #region Keyboard Panel

    /// <summary>
    /// El nombre de la sala.
    /// </summary>
    string roomName;

    /// <summary>
    /// Activa el panel con el teclado para introducir el nombre de la sala a la que unirnos.
    /// </summary>
    public void OpenPanelKeyboard()
    {
        OpenPanelGame(3);
        roomName = "";
        textRoomName.text = roomName;
        buttonSave.SetActive(false);
        buttonRestart.SetActive(false);
    }

    /// <summary>
    /// Añade el carácter introducido al nombre de la sala.
    /// </summary>
    /// <param name="character">El carácter introducido.</param>
    public void EnterRoomLetter(string character)
    {
        if (roomName.Length < 3)
        {
            roomName += character;

            textRoomName.text = roomName;
        }
    }

    /// <summary>
    /// Elimina el último carácter del nombre de la sala.
    /// </summary>
    public void DeleteRoomLetter()
    {
        if (roomName.Length > 0)
        {
            string newRoomName = "";

            for (int i = 0; i < roomName.Length - 1; i++)
            {
                newRoomName += roomName[i];
            }

            roomName = newRoomName;
            textRoomName.text = roomName;
        }
    }

    /// <summary>
    /// Entra en la sala con el nombre intoducido.
    /// </summary>
    public void JoinRoom()
    {
        if (roomName.Length == 3)
        {
            NetworkManager.manager.JoinRoom(roomName);
        }
    }

    #endregion

    #region Check Message

    /// <summary>
    /// Activa un panel que indica que un jugador está en jaque.
    /// </summary>
    /// <param name="colour">El color del jugador en jaque.</param>
    public void ActivatePanelCheck(Enums.Colours colour)
    {
        checkText.text = (colour == Enums.Colours.White) ? CheckMessageWhite() : CheckMessageBlack();

        panelCheck.SetActive(true);
    }

    /// <summary>
    /// Desactiva el panel que notifica los jaques.
    /// </summary>
    public void DeactivatePanelCheck()
    {
        panelCheck.SetActive(false);
    }

    #endregion

    #region Promotion Message

    /// <summary>
    /// Activa el panel que indica que un peón blanco ha promocionado.
    /// </summary>
    /// <param name="inTurn">Verdadero si el peón que promociona está controlado por el jugador en este dispositivo.
    /// Falso si está controlado por la IA o por otro jugador (multijugador online).</param>
    public void ActivatePromotionWhite(bool inTurn)
    {
        OpenPanelGame(1);

        // Si el el turno del jugador, un panel se abre para que eliga una de las cuatro posibles piezas.

        if (inTurn)
        {
            panelPiecesWhite.SetActive(true);
            promotionText.text = PromotionMessageWhite();
        }

        // Si no es su turno, se abre una notificación que indica que estamos esperando a que el otro jugador elija.

        else
        {
            promotionText.text = WaitPromotionMessageWhite();
        }
    }

    /// <summary>
    /// Activa el panel que indica que un peón negro ha promocionado.
    /// </summary>
    /// <param name="inTurn">Verdadero si el peón que promociona está controlado por el jugador en este dispositivo.
    /// Falso si está controlado por la IA o por otro jugador (multijugador online).</param>
    public void ActivatePromotionBlack(bool inTurn)
    {
        OpenPanelGame(1);

        // Si el el turno del jugador, un panel se abre para que eliga una de las cuatro posibles piezas.

        if (inTurn)
        {
            panelPiecesBlack.SetActive(true);
            promotionText.text = PromotionMessageBlack();
        }

        // Si no es su turno, se abre una notificación que indica que estamos esperando a que el otro jugador elija.

        else
        {
            promotionText.text = WaitPromotionMessageBlack();
        }
    }

    /// <summary>
    /// Promociona a un peón blanco en la pieza elegida.
    /// </summary>
    /// <param name="piece">La pieza elegida.</param>
    public void PromotePieceWhite(string piece)
    {
        if (!NetworkManager.manager.IsConnected)
        {
            switch (piece)
            {
                case "Rook":
                    Chess.PieceSelectedToPromotion(Enums.PromotablePieces.Rook, Pieces.Colour.White);
                    break;
                case "Knight":
                    Chess.PieceSelectedToPromotion(Enums.PromotablePieces.Knight, Pieces.Colour.White);
                    break;
                case "Bishop":
                    Chess.PieceSelectedToPromotion(Enums.PromotablePieces.Bishop, Pieces.Colour.White);
                    break;
                case "Queen":
                    Chess.PieceSelectedToPromotion(Enums.PromotablePieces.Queen, Pieces.Colour.White);
                    break;
            }
        }

        else
        {
            switch (piece)
            {
                case "Rook":
                    NetworkManager.manager.PromotePiece(Enums.PromotablePieces.Rook, Pieces.Colour.White);
                    break;
                case "Knight":
                    NetworkManager.manager.PromotePiece(Enums.PromotablePieces.Knight, Pieces.Colour.White);
                    break;
                case "Bishop":
                    NetworkManager.manager.PromotePiece(Enums.PromotablePieces.Bishop, Pieces.Colour.White);
                    break;
                case "Queen":
                    NetworkManager.manager.PromotePiece(Enums.PromotablePieces.Queen, Pieces.Colour.White);
                    break;
            }
        }
    }

    /// <summary>
    /// Promociona a un peón negro en la pieza elegida.
    /// </summary>
    /// <param name="piece">La pieza elegida.</param>
    public void PromotePieceBlack(string piece)
    {
        if (!NetworkManager.manager.IsConnected)
        {
            switch (piece)
            {
                case "Rook":
                    Chess.PieceSelectedToPromotion(Enums.PromotablePieces.Rook, Pieces.Colour.Black);
                    break;
                case "Knight":
                    Chess.PieceSelectedToPromotion(Enums.PromotablePieces.Knight, Pieces.Colour.Black);
                    break;
                case "Bishop":
                    Chess.PieceSelectedToPromotion(Enums.PromotablePieces.Bishop, Pieces.Colour.Black);
                    break;
                case "Queen":
                    Chess.PieceSelectedToPromotion(Enums.PromotablePieces.Queen, Pieces.Colour.Black);
                    break;
            }
        }

        else
        {
            switch (piece)
            {
                case "Rook":
                    NetworkManager.manager.PromotePiece(Enums.PromotablePieces.Rook, Pieces.Colour.Black);
                    break;
                case "Knight":
                    NetworkManager.manager.PromotePiece(Enums.PromotablePieces.Knight, Pieces.Colour.Black);
                    break;
                case "Bishop":
                    NetworkManager.manager.PromotePiece(Enums.PromotablePieces.Bishop, Pieces.Colour.Black);
                    break;
                case "Queen":
                    NetworkManager.manager.PromotePiece(Enums.PromotablePieces.Queen, Pieces.Colour.Black);
                    break;
            }
        }
    }

    /// <summary>
    /// Desactiva el panel de promoción.
    /// </summary>
    public void DisablePromotions()
    {
        panelsGame[1].SetActive(false);
        panelPiecesBlack.SetActive(false);
        panelPiecesWhite.SetActive(false);
    }

    #endregion

    #region Checkmate Message

    /// <summary>
    /// Activa el panel de jaque mate.
    /// </summary>
    /// <param name="colour">El color del jugador que ha ganado la partida.</param>
    public void ActivateCheckmateMessage(Enums.Colours colour)
    {
        if (!NetworkManager.manager.IsConnected)
        {
            OpenPanelGame(4);
        }

        // El panel que se abre será diferente en función de si estamos jugando online o no.
        // Además, si la partida es online, nos desconectamos del servidor.

        else
        {
            OpenPanelGame(6);
            NetworkManager.manager.DisconnectAll();
        }

        panelNotifications.SetActive(true);
        buttonPauseObject.SetActive(false);
        panelCheck.SetActive(false);

        notificationsText.text = (colour == Enums.Colours.White) ? CheckmateMessageWhite() : CheckmateMessageBlack();
        notificationsImage.color = (colour == Enums.Colours.White) ? Color.white : Color.black;
    }

    #endregion

    #region Draw Message

    /// <summary>
    /// Activa el panel que indica que la partida ha terminado en tablas.
    /// </summary>
    /// <param name="drawType">El motivo por el que la partida ha terminado en tablas.</param>
    public void ActivateDrawMessage(Enums.DrawModes drawType)
    {
        if (!NetworkManager.manager.IsConnected)
        {
            OpenPanelGame(4);
        }

        else
        {
            OpenPanelGame(6);
            NetworkManager.manager.DisconnectAll();
        }

        panelNotifications.SetActive(true);
        notificationsImage.color = Color.blue;

        switch (drawType)
        {
            case Enums.DrawModes.Stalemate:
                notificationsText.text = DrawStalemateMessage();
                break;
            case Enums.DrawModes.Impossibility:
                notificationsText.text = DrawImpossibilityMessage();
                break;
            case Enums.DrawModes.Move75:
                notificationsText.text = Draw75MovesMessage();
                break;
            case Enums.DrawModes.ThreefoldRepetition:
                notificationsText.text = DrawRepetitionMessage();
                break;
        }
    }

    #endregion

    #region Texts

    /// <summary>
    /// Indica que el servidor está lleno en este momento (se ha alcanzado el límite de 20 personas).
    /// </summary>
    /// <returns></returns>
    string ServerFull()
    {
        return Resources.Load<TranslateText>("Texts/ServerFull").GetText(Options.ActiveLanguage);
    }

    /// <summary>
    /// Indica que el dispositivo no está conectado a internet.
    /// </summary>
    /// <returns></returns>
    string NoInternetConnection()
    {
        return Resources.Load<TranslateText>("Texts/NoInternetConnection").GetText(Options.ActiveLanguage);
    }

    /// <summary>
    /// Indica que la región seleccionada no está disponible en este momento.
    /// No es un problema con los servidores de Photon en general, sino solo con el de la región seleccionada.
    /// </summary>
    /// <returns></returns>
    string InvalidRegion()
    {
        return Resources.Load<TranslateText>("Texts/InvalidRegion").GetText(Options.ActiveLanguage);
    }

    /// <summary>
    /// Indica que ha ocurrido un error genérico en el servidor.
    /// </summary>
    /// <returns></returns>
    string GenericServerError()
    {
        return Resources.Load<TranslateText>("Texts/GenericServerError").GetText(Options.ActiveLanguage);
    }

    /// <summary>
    /// Texto que indica que una ranura de guardado está vacía.
    /// </summary>
    /// <returns></returns>
    string EmptyText()
    {
        return Resources.Load<TranslateText>("Texts/EmptyText").GetText(Options.ActiveLanguage);
    }

    /// <summary>
    /// Texto que indica que estamos esperando a que jueguen las blancas.
    /// </summary>
    /// <returns></returns>
    string WaitingMessageWhite()
    {
        return Resources.Load<TranslateText>("Texts/WaitingMessageWhite").GetText(Options.ActiveLanguage);
    }

    /// <summary>
    /// Texto que indica que estamos esperando a que jueguen las negras.
    /// </summary>
    /// <returns></returns>
    string WaitingMessageBlack()
    {
        return Resources.Load<TranslateText>("Texts/WaitingMessageBlack").GetText(Options.ActiveLanguage);
    }

    /// <summary>
    /// Texto que indica que el rey blanco está en jaque.
    /// </summary>
    /// <returns></returns>
    string CheckMessageWhite()
    {
        return Resources.Load<TranslateText>("Texts/CheckMessageWhite").GetText(Options.ActiveLanguage);
    }

    /// <summary>
    /// Texto que indica que el rey negro está en jaque.
    /// </summary>
    /// <returns></returns>
    string CheckMessageBlack()
    {
        return Resources.Load<TranslateText>("Texts/CheckMessageBlack").GetText(Options.ActiveLanguage);
    }

    /// <summary>
    /// Texto que indica que un peón blanco va a promocionar.
    /// </summary>
    /// <returns></returns>
    string PromotionMessageWhite()
    {
        return Resources.Load<TranslateText>("Texts/PromotionMessageWhite").GetText(Options.ActiveLanguage);
    }

    /// <summary>
    /// Texto que indica que un peón negro va a promocionar.
    /// </summary>
    /// <returns></returns>
    string PromotionMessageBlack()
    {
        return Resources.Load<TranslateText>("Texts/PromotionMessageBlack").GetText(Options.ActiveLanguage);
    }

    /// <summary>
    /// Texto que indica que estamos esperando a que el otro jugador elija la pieza a la que promociona su peón blanco.
    /// </summary>
    /// <returns></returns>
    string WaitPromotionMessageWhite()
    {
        return Resources.Load<TranslateText>("Texts/WaitPromotionMessageWhite").GetText(Options.ActiveLanguage);
    }

    /// <summary>
    /// Texto que indica que estamos esperando a que el otro jugador elija la pieza a la que promociona su peón negro.
    /// </summary>
    /// <returns></returns>
    string WaitPromotionMessageBlack()
    {
        return Resources.Load<TranslateText>("Texts/WaitPromotionMessageBlack").GetText(Options.ActiveLanguage);
    }

    /// <summary>
    /// Texto que indica que las blancas han ganado.
    /// </summary>
    /// <returns></returns>
    string CheckmateMessageWhite()
    {
        return Resources.Load<TranslateText>("Texts/CheckmateMessageWhite").GetText(Options.ActiveLanguage);
    }

    /// <summary>
    /// Texto que indica que las negras han ganado.
    /// </summary>
    /// <returns></returns>
    string CheckmateMessageBlack()
    {
        return Resources.Load<TranslateText>("Texts/CheckmateMessageBlack").GetText(Options.ActiveLanguage);
    }

    /// <summary>
    /// Texto que indica que la partida ha terminado en tablas por ahogado.
    /// </summary>
    /// <returns></returns>
    string DrawStalemateMessage()
    {
        return Resources.Load<TranslateText>("Texts/DrawStalemateMessage").GetText(Options.ActiveLanguage);
    }

    /// <summary>
    /// Texto que indica que la partida ha terminado en tablas por imposibilidad de jaque mate con las piezas actuales.
    /// </summary>
    /// <returns></returns>
    string DrawImpossibilityMessage()
    {
        return Resources.Load<TranslateText>("Texts/DrawImpossibilityMessage").GetText(Options.ActiveLanguage);
    }

    /// <summary>
    /// Texto que indica que la partida ha terminado en tablas por realizarse 75 movimientos sin captura de piezas o movimientos de peones.
    /// </summary>
    /// <returns></returns>
    string Draw75MovesMessage()
    {
        return Resources.Load<TranslateText>("Texts/Draw75MovesMessage").GetText(Options.ActiveLanguage);
    }

    /// <summary>
    /// Texto que indica que la partida ha terminado en tablas por repetirse las mismas posiciones tres veces.
    /// </summary>
    /// <returns></returns>
    string DrawRepetitionMessage()
    {
        return Resources.Load<TranslateText>("Texts/DrawRepetitionMessage").GetText(Options.ActiveLanguage);
    }

    /// <summary>
    /// El nombre de la sala.
    /// </summary>
    /// <returns></returns>
    string RoomName()
    {
        return Resources.Load<TranslateText>("Texts/RoomName").GetText(Options.ActiveLanguage) + "" + NetworkManager.manager.ActiveRoom;
    }

    #endregion
}