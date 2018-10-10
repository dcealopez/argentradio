using System.Text;

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

        /// <summary>
        /// Elimina los códigos de colores con el formato '{XXXXXX}'
        /// de la cadena que se pase como argumento
        /// </summary>
        /// <param name="text">cadena de la que eliminar los códigos de colores</param>
        /// <returns>la cadena con los códigos de colores eliminados</returns>
        public static string StripColorCodes(string text)
        {
            var result = new byte[text.Length];

            for (int i = 0, j = 0; i < text.Length; i++)
            {
                if (text[i] == '{' && i + 7 < text.Length && text[i + 7] == '}')
                {
                    i += 8;
                }

                result[j++] = (byte) text[i];
            }

            return Encoding.UTF8.GetString(result, 0, result.Length);
        }
    }
}
