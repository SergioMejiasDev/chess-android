using UnityEngine;

/// <summary>
/// Contiene las diferentes traducciones de un texto.
/// Estas serán leídas a través de la clase MultiText para gestionar las traducciones de los textos mostrados en pantalla.
/// </summary>
[CreateAssetMenu]
public class TranslateText : ScriptableObject
{
    [TextArea(5, 10)] [SerializeField] string english = null;
    [TextArea(5, 10)] [SerializeField] string spanish = null;
    [TextArea(5, 10)] [SerializeField] string catalan = null;
    [TextArea(5, 10)] [SerializeField] string italian = null;

    /// <summary>
    /// Elige la versión correcta del texto para el idioma elegido.
    /// </summary>
    /// <returns>La versión del texto en el idioma elegido.</returns>
    public string GetText(Options.Language language)
    {
        switch (language)
        {
            case Options.Language.EN:
                return english;
            case Options.Language.ES:
                return spanish;
            case Options.Language.CA:
                return catalan;
            case Options.Language.IT:
                return italian;
            default:
                return english;
        }
    }
}