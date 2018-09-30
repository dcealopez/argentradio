namespace ArgentRadio.Conditions.Operators
{
    /// <summary>
    /// Operador que evalúa si una cadena no es igual a otra
    /// </summary>
    public class NotEqualsOperator : EqualsOperator
    {
        public new static NotEqualsOperator Instance = new NotEqualsOperator { InternalName = "notequals" };

        public override bool Evaluate(string match, string text)
        {
            return !base.Evaluate(match, text);
        }
    }
}
