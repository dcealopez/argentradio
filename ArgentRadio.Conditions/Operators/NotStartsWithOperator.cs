namespace ArgentRadio.Conditions.Operators
{
    /// <summary>
    /// Operador que evalúa si una cadena no empieza por otra
    /// </summary>
    [Operator(InternalName = "notstartswith")]
    public class NotStartsWithOperator : StartsWithOperator
    {
        public new static NotStartsWithOperator Instance = new NotStartsWithOperator();

        public new bool Evaluate(string match, string text)
        {
            return !base.Evaluate(match, text);
        }
    }
}
