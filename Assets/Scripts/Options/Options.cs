using UnityEngine;

/// <summary>
/// Contiene los diferentes ajustes que pueden ser modificados en la aplicación.
/// </summary>
public static class Options
{
    /// <summary>
    /// Lista de las diferentes resoluciones en las que se puede usar la aplicación.
    /// </summary>
    public enum Resolution {
        /// <summary>
        /// Pantalla completa, con una resolución corregida de 16:9.
        /// </summary>
        Fullscreen,
        /// <summary>
        /// Modo ventana, con una resolución de 1280x720.
        /// </summary>
        Windowed720,
        /// <summary>
        /// Modo ventana, con una resolución de 854x480.
        /// </summary>
        Windowed480
    }

    /// <summary>
    /// Lista de los diferentes idiomas disponibles en la aplicación.
    /// </summary>
    public enum Language { 
        /// <summary>
        /// Inglés.
        /// </summary>
        EN,
        /// <summary>
        /// Español.
        /// </summary>
        ES,
        /// <summary>
        /// Catalán.
        /// </summary>
        CA,
        /// <summary>
        /// Italiano.
        /// </summary>
        IT };

    /// <summary>
    /// Lista de los servidores ofrecidos por Photon para las partidas online.
    /// </summary>
    public enum Server { 
        /// <summary>
        /// Asia (Singapore).
        /// </summary>
        Asia,
        /// <summary>
        /// Australia (Melbourne).
        /// </summary>
        Australia,
        /// <summary>
        /// Canada, East (Montreal).
        /// </summary>
        CanadaEast,
        /// <summary>
        /// Europe (Amsterdam).
        /// </summary>
        Europe,
        /// <summary>
        /// India (Chennai).
        /// </summary>
        India,
        /// <summary>
        /// Japan (Tokyo).
        /// </summary>
        Japan,
        /// <summary>
        /// Russia, East (Khabarovsk).
        /// </summary>
        RussiaEast,
        /// <summary>
        /// Russia, West (Moscow).
        /// </summary>
        RussiaWest,
        /// <summary>
        /// South Africa (Johannesburg).
        /// </summary>
        SouthAfrica,
        /// <summary>
        /// South America (Sao Paulo).
        /// </summary>
        SouthAmerica,
        /// <summary>
        /// South Korea (Seoul).
        /// </summary>
        SouthKorea,
        /// <summary>
        /// USA, East (Washington D.C.).
        /// </summary>
        Turkey,
        /// <summary>
        /// Turkey (Istanbul).
        /// </summary>
        USAEast,
        /// <summary>
        /// Usa, West (San José).
        /// </summary>
        USAWest }

    #region Properties

    /// <summary>
    /// La resolución activa.
    /// </summary>
    public static Resolution ActiveResolution { get; set; }

    /// <summary>
    /// El idioma activo.
    /// </summary>
    public static Language ActiveLanguage { get; set; }

    /// <summary>
    /// El servidor de Photon activo.
    /// </summary>
    public static Server ActiveServer { get; set; }

    #endregion

    /// <summary>
    /// Guarda los datos de los ajustes en un archivo binario.
    /// </summary>
    public static void SaveOptions()
    {
        SettingsData data = new SettingsData
        {
            resolution = ActiveResolution,
            language = ActiveLanguage,
            server = ActiveServer
        };

        SaveManager.SaveSettings(data);
    }

    /// <summary>
    /// Carga los datos de los ajustes a partir de un archivo binario creado previamente.
    /// </summary>
    public static void LoadOptions()
    {
        SettingsData data = SaveManager.LoadSettings();

        ActiveResolution = data.resolution;
        ActiveLanguage = data.language;
        ActiveServer = data.server;
    }

    /// <summary>
    /// Crea un archivo de opciones con los datos por defecto.
    /// </summary>
    public static void DefaultValues()
    {
        // La resolución por defecto es la ventana de 854x480.

        ActiveResolution = Resolution.Windowed480;

        // Dependiendo del idioma del dispositivo, se activará el mismo idioma en la aplicación si está disponible,

        if (Application.systemLanguage == SystemLanguage.Spanish ||
            Application.systemLanguage == SystemLanguage.Basque)
        {
            ActiveLanguage = Language.ES;
        }

        else if (Application.systemLanguage == SystemLanguage.Catalan)
        {
            ActiveLanguage = Language.CA;
        }

        else if (Application.systemLanguage == SystemLanguage.Italian)
        {
            ActiveLanguage = Language.IT;
        }

        // Si el idioma del dispositivo no está entre los incluidos en la aplicación, se usará el inglés por defecto.

        else
        {
            ActiveLanguage = Language.EN;
        }

        // Por defecto, se elegirá el servidor de Europe.

        ActiveServer = Server.Europe;

        // Guardamos los datos.

        SaveOptions();
    }
}