/// <summary>
/// Contiene los datos de los ajustes que pueden guardarse en un archivo binario.
/// </summary>
[System.Serializable]
public class SettingsData
{
    /// <summary>
    /// La resolución en la que se mostrará la aplicación.
    /// </summary>
    public Options.Resolution resolution;

    /// <summary>
    /// El idioma en el que se mostrará la interfaz del juego.
    /// </summary>
    public Options.Language language;

    /// <summary>
    /// El servidor de Photon al que se conectará el juego para las partidas online.
    /// </summary>
    public Options.Server server;
}