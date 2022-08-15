/// <summary>
/// Colección de enumeraciones públicas que pueden ser usadas en otras clases.
/// </summary>
public static class Enums
{
    /// <summary>
    /// El color del jugador que puede mover las piezas en este dispositivo.
    /// </summary>
    public enum Colours {
        /// <summary>
        /// El jugador juega con las piezas negras.
        /// </summary>
        Black,
        /// <summary>
        /// El jugador juega con las piezas blancas.
        /// </summary>
        White,
        /// <summary>
        /// Todas las piezas juegan en este dispositivo (multijugador local).
        /// </summary>
        All
    };

    /// <summary>
    /// Las posibles piezas a las que puede promocionar un peón cuando alcanza el extremo del tablero.
    /// </summary>
    public enum PromotablePieces {
        /// <summary>
        /// El peón promociona en una torre.
        /// </summary>
        Rook,
        /// <summary>
        /// El peón promociona en un caballo.
        /// </summary>
        Knight,
        /// <summary>
        /// El peón promociona en un alfil.
        /// </summary>
        Bishop,
        /// <summary>
        /// El peón promociona en una reina.
        /// </summary>
        Queen
    }

    /// <summary>
    /// Las diferentes formas por las que una partida puede finalizar en tablas.
    /// </summary>
    public enum DrawModes {
        /// <summary>
        /// La partida termina en tablas por un ahogado.
        /// </summary>
        Stalemate,
        /// <summary>
        /// La partida termina en tablas por ser imposible conseguir un jaque mate con las piezas actuales.
        /// </summary>
        Impossibility,
        /// <summary>
        /// La partida termina en tablas por haberse realizado 75 movimientos sin que se haya capturado ninguna pieza.
        /// </summary>
        Move75,
        /// <summary>
        /// La partida termina en tablas por haberse repetido la misma posición del tablero tres veces.
        /// </summary>
        ThreefoldRepetition
    }
}