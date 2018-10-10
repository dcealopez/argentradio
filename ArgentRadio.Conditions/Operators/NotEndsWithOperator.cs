namespace ArgentRadio.Conditions.Operators
{
    /// <summary>
    /// Operador que evalúa si una cadena no termina por otra
    /// </summary>
    [Operator(InternalName = "notendswith")]
    public class NotEndsWithOperator : EndsWithOperator
    {
        public new static readonly NotEndsWithOperator Instance = new NotEndsWithOperator();

        public override bool Evaluate(string match, string text)
        {
            return !base.Evaluate(match, text);
        }
    }
}
