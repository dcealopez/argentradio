namespace ArgentRadio.Conditions.Operators
{
    /// <summary>
    /// Operador que evalúa si una cadena no contiene otra
    /// </summary>
    public class NotContainsOperator : ContainsOperator
    {
        public new static NotContainsOperator Instance = new NotContainsOperator { InternalName = "notcontains" };

        public override bool Evaluate(string match, string text)
        {
            return !base.Evaluate(match, text);
        }
    }
}
