namespace ArgentRadio.Conditions.Operators
{
    /// <summary>
    /// Operador que evalúa si una cadena no contiene otra
    /// </summary>
    [Operator(InternalName = "notcontains")]
    public class NotContainsOperator : ContainsOperator
    {
        public new static readonly NotContainsOperator Instance = new NotContainsOperator();

        public override bool Evaluate(string match, string text)
        {
            return !base.Evaluate(match, text);
        }
    }
}
