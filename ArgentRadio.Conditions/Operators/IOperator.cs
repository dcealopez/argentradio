namespace ArgentRadio.Conditions.Operators
{
    /// <summary>
    /// Interfaz para los operadores condicionales
    /// </summary>
    public interface IOperator
    {        
        /// <summary>
        /// Evalúa una cadena con otra con el operador especificado
        /// </summary>
        /// <param name="match">cadena a evaluar</param>
        /// <param name="text">cadena original</param>
        /// <returns>true si se cumple la condición, false si no</returns>
        bool Evaluate(string match, string text);
    }
}
