namespace ArgentRadio.Conditions.Matches
{
    /// <summary>
    /// Interfaz que define un grupo de condiciones
    /// </summary>
    public interface IMatchGroup
    {
        /// <summary>
        /// Evalúa todas las condiciones contenidas en el grupo contra la cadena que se le pase
        /// </summary>
        /// <returns>true si las condiciones se cumplen, false si no</returns>
        bool Evaluate(string text);
    }
}
