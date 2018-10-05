namespace ArgentRadio.Conditions.Operators
{
    /// <summary>
    /// Operador que evalúa si una cadena no termina por otra
    /// </summary>
    [Operator(InternalName = "notendswith")]
    public class NotEndsWithOperator : EndsWithOperator
    {
        public new static NotEndsWithOperator Instance = new NotEndsWithOperator();

        public new bool Evaluate(string match, string text)
        {
            return !base.Evaluate(match, text);
        }
    }
}
