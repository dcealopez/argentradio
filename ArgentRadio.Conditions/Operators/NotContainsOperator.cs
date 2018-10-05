namespace ArgentRadio.Conditions.Operators
{
    /// <summary>
    /// Operador que evalúa si una cadena no contiene otra
    /// </summary>
    [Operator(InternalName = "notcontains")]
    public class NotContainsOperator : ContainsOperator
    {
        public new static NotContainsOperator Instance = new NotContainsOperator();

        public new bool Evaluate(string match, string text)
        {
            return !base.Evaluate(match, text);
        }
    }
}
