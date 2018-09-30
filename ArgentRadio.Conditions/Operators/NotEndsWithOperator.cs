namespace ArgentRadio.Conditions.Operators
{
    /// <summary>
    /// Operador que evalúa si una cadena no termina por otra
    /// </summary>
    public class NotEndsWithOperator : EndsWithOperator
    {
        public new static NotEndsWithOperator Instance = new NotEndsWithOperator { InternalName = "notendswith" };

        public override bool Evaluate(string match, string text)
        {
            return !base.Evaluate(match, text);
        }
    }
}
