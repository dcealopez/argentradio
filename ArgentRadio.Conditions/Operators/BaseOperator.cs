namespace ArgentRadio.Conditions.Operators
{
    /// <summary>
    /// Operador base para el resto de operadores
    /// </summary>
    public class BaseOperator : IOperator
    {
        /// <summary>
        /// Nombre interno del operador
        /// </summary>
        public string InternalName { get; set; } = null;
        
        public virtual bool Evaluate(string match, string text)
        {
            throw new System.NotImplementedException();
        }
    }
}
