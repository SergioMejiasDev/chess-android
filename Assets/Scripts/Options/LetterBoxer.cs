using UnityEngine;

/// <summary>
/// Añade dos letterboxes (o pillarboxes) al juego para corregir la resolución de la pantalla.
/// </summary>
public static class LetterBoxer
{
    /// <summary>
    /// Añade dos letterboxes o pillarboxes a la escena dependiendo de la resolución de la pantalla en la que se está jugando.
    /// </summary>
    public static void AddLetterBoxing()
    {
        // Se crea una cámara alternativa que tendrá el tamaño de la pantalla actual y el fondo negro.
        // Esta cámara se situará detrás de la cámara principal para que no se superponga a esta.

        Camera letterBoxerCamera = new GameObject().AddComponent<Camera>();
        letterBoxerCamera.backgroundColor = Color.black;
        letterBoxerCamera.cullingMask = 0;
        letterBoxerCamera.depth = -100;
        letterBoxerCamera.farClipPlane = 1;
        letterBoxerCamera.useOcclusionCulling = false;
        letterBoxerCamera.allowHDR = false;
        letterBoxerCamera.allowMSAA = false;
        letterBoxerCamera.clearFlags = CameraClearFlags.Color;
        letterBoxerCamera.name = "Letter Boxer Camera";

        // Adaptamos la cámara principal para que tenga una resolución de 16:9.

        PerformSizing();
    }

    /// <summary>
    /// Adapta el tamaño de la cámara principal a la resolución indicada.
    /// </summary>
    static void PerformSizing()
    {
        Camera mainCamera = Camera.main;

        float targetRatio = 16.0f / 9.0f;

        float windowaspect = (float)Screen.width / (float)Screen.height;

        float scaleheight = windowaspect / targetRatio;

        // Si la resolución de la cámara es menor a 16:9, se adaptará el tamaño de la cámara principal para añadir los letterboxes.

        if (scaleheight < 1.0f)
        {
            Rect rect = mainCamera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            mainCamera.rect = rect;
        }

        // Si la resolución de la cámara es superior a 16:9, se adaptará el tamaño de la cámara principal para añadir los pillarboxes.

        else
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = mainCamera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            mainCamera.rect = rect;
        }
    }
}