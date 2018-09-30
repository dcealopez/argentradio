using ArgentRadio.Conditions.Operators;

namespace ArgentRadio.Conditions.Matches
{
    /// <summary>
    /// Clase base para una condición
    /// </summary>
    public class Match
    {
        /// <summary>
        /// Operador condicional
        /// </summary>
        public IOperator ConditionalOperator { get; set; }

        /// <summary>
        /// Texto que se evaluará
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Evalúa la condición
        /// </summary>
        /// <param name="text">cadena contra la que evaluar la condición</param>
        /// <returns>true si se cumple la condición, false si no</returns>
        public bool Evaluate(string text)
        {
            return ConditionalOperator.Evaluate(Text, text);
        }
    }
}
