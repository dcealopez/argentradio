namespace ArgentRadio.Actions
{
    /// <summary>
    /// Interfaz para las acciones
    /// </summary>
    internal interface IAction
    {
        /// <summary>
        /// Ejecuta la acción
        /// </summary>
        void Execute(params object[] args);
    }
}
