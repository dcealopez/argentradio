namespace ArgentRadio.Common
{
    /// <summary>
    /// Clase helper para la manipulación de cadenas
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Elimina los espacios al principio y al final de una cadena y
        /// la convierte en minúsculas
        /// </summary>
        /// <param name="text">cadena a normalizar</param>
        /// <returns>la cadena normalizada</returns>
        public static string Normalize(string text)
        {
            return text.Trim().ToLower();
        }
    }
}
