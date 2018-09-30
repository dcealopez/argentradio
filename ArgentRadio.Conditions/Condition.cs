using ArgentRadio.Conditions.Matches;

namespace ArgentRadio.Conditions
{
    /// <summary>
    /// Clase para las condiciones
    /// </summary>
    public class Condition
    {
        /// <summary>
        /// Descripción de la condición
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Ruta al archivo de sonido a reproducir como notificación
        /// </summary>
        public string SoundFilePath { get; set; }

        /// <summary>
        /// Grupo de condiciones raíz que contiene todas las condiciones
        /// </summary>
        public AndMatchGroup Matches { get; set; }

        /// <summary>
        /// Evalúa la condición contra la cadena que se le pase
        /// </summary>
        /// <param name="text">cadena contra la que evaluar la condición</param>
        /// <returns>true si se cumple la condición, false si no</returns>
        public bool Evaluate(string text)
        {
            return Matches.Evaluate(text);
        }
    }
}
