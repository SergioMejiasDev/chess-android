using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Conjunto de métodos encargados de las funciones de guardado y cargado.
/// </summary>
public static class SaveManager
{
    #region Game Data

    /// <summary>
    /// Guarda los datos del juego en un archivo binario.
    /// </summary>
    /// <param name="saveSlot">La ranura de guardado en la que queremos guardar la partida (1, 2, 3; 0 está reservado para autoguardado).</param>
    /// <param name="dataRaw">Los datos sin procesar de la partida que queremos guardar.</param>
    public static void SaveGame(int saveSlot, SaveDataRaw dataRaw)
    {
        // Creamos un archivo serializable a partir de los datos sin procesar.

        SaveData data = new SaveData(dataRaw);

        // Serializamos los datos y creamos un archivo binario en el directorio indicado en "path".

        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/AutoSave.sav";

        switch (saveSlot)
        {
            case 1:
                path = Application.persistentDataPath + "/Save1.sav";
                break;
            case 2:
                path = Application.persistentDataPath + "/Save2.sav";
                break;
            case 3:
                path = Application.persistentDataPath + "/Save3.sav";
                break;
        }

        FileStream fileStream = new FileStream(path, FileMode.Create);

        formatter.Serialize(fileStream, data);

        fileStream.Close();

        // Actualizamos los menús de cargado-guardado con la fecha y hora de la nueva partida guardada.

        Interface.interfaceClass.UpdateSaveDates();
    }

    /// <summary>
    /// Eliminamos el archivo guardado en la ranura de autoguardado.
    /// </summary>
    public static void DeleteAutoSave()
    {
        string path = Application.persistentDataPath + "/AutoSave.sav";

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    /// <summary>
    /// Cargamos los datos existentes en un archivo binario en el dispositivo.
    /// </summary>
    /// <param name="saveSlot">La ranura de guardado de la que queremos cargar la partida (1, 2, 3; 0 está reservado para autoguardado).</param>
    /// <returns>Un objeto SaveData con los datos guardados de la partida.</returns>
    public static SaveData LoadGame(int saveSlot)
    {
        // De acuerdo con la ranura elegida, guardamos la ubicación del archivo en la variable "path".

        string path = Application.persistentDataPath + "/AutoSave.sav";

        switch (saveSlot)
        {
            case 1:
                path = Application.persistentDataPath + "/Save1.sav";
                break;
            case 2:
                path = Application.persistentDataPath + "/Save2.sav";
                break;
            case 3:
                path = Application.persistentDataPath + "/Save3.sav";
                break;
        }

        // Si existe un archivo en la ubicación, lo convertimos en un archivo legible para extraer los datos.

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }

        return null;
    }

    #endregion

    #region Settings

    /// <summary>
    /// Guarda los ajustes del juego en un archivo binario.
    /// </summary>
    /// <param name="data">Variable con los datos que se van a guardar.</param>
    public static void SaveSettings(SettingsData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/Settings.sav";

        FileStream fileStream = new FileStream(path, FileMode.Create);

        formatter.Serialize(fileStream, data);

        fileStream.Close();
    }

    /// <summary>
    /// Carga unos ajustes del juego guardados previamente.
    /// </summary>
    /// <returns>Variable con los datos de los ajustes.</returns>
    public static SettingsData LoadSettings()
    {
        SettingsData data;

        string path = Application.persistentDataPath + "/Settings.sav";

        // Si hay una partida guardada, la cargamos.

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as SettingsData;
            stream.Close();
        }

        // Si no existe ningún archivo, creamos uno con los valores por defecto.

        else
        {
            Options.DefaultValues();

            return LoadSettings();
        }

        return data;
    }

    /// <summary>
    /// Obtenemos los datos con la información de las fechas y horas guardadas.
    /// </summary>
    /// <returns>Array con los cuatro strings con la información de las fechas y las horas guardadas.</returns>
    public static string[] GetDates()
    {
        string[] dates = new string[4];

        SaveData data;

        string path = Application.persistentDataPath + "/AutoSave.sav";

        // Si existen archivos guardados, guardamos los datos en el array.

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            dates[0] = data.saveDate;
        }

        // Si no existen guardados en una ranura, guardamos el valor "0" como distintivo.

        else
        {
            dates[0] = "0";
        }

        // Lo repetimos con todas las ranuras de guardado.

        path = Application.persistentDataPath + "/Save1.sav";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            dates[1] = data.saveDate;
        }

        else
        {
            dates[1] = "0";
        }

        path = Application.persistentDataPath + "/Save2.sav";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            dates[2] = data.saveDate;
        }

        else
        {
            dates[2] = "0";
        }

        path = Application.persistentDataPath + "/Save3.sav";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            dates[3] = data.saveDate;
        }

        else
        {
            dates[3] = "0";
        }

        return dates;
    }

    #endregion

    #region Photon Serialization

    /// <summary>
    /// Serializa los datos de modo que puedan transferirse a través de los servidores de Photon.
    /// </summary>
    /// <param name="data">Variable con los datos que se van a serializar.</param>
    /// <returns>Los datos en forma de array de bytes.</returns>
    public static byte[] Serialize(object data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream();

        formatter.Serialize(stream, data);

        return stream.ToArray();
    }

    /// <summary>
    /// Deserializa los datos del array de bytes para que puedan ser legibles.
    /// </summary>
    /// <param name="byteData">Los datos en forma de array de bytes.</param>
    /// <returns></returns>
    public static SaveData Deserialize(byte[] byteData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream(byteData);

        return formatter.Deserialize(stream) as SaveData;
    }

    #endregion
}