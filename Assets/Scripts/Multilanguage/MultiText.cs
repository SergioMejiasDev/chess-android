using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Alterna el idioma de los textos de acuerdo con el idioma seleccionado en los ajustes.
/// </summary>
[RequireComponent(typeof(Text))]
public class MultiText : MonoBehaviour
{
    /// <summary>
    /// El texto que va a ser traducido.
    /// </summary>
    Text text = null;

    /// <summary>
    /// El archivo que contiene las diferentes traducciones del texto.
    /// </summary>
    [SerializeField] TranslateText textAsset = null;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void OnEnable()
    {
        // Cuando el texto se active, se mostrará en el idioma seleccionado.

        UpdateText(Options.ActiveLanguage);
    }

    /// <summary>
    /// Actualiza el idioma del texto por el seleccionado.
    /// </summary>
    /// <param name="language">El idioma al que se va a traducir el texto.</param>
    void UpdateText(Options.Language language)
    {
        text.text = textAsset.GetText(language);
    }
}