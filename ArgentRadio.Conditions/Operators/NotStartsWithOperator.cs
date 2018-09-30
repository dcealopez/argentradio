namespace ArgentRadio.Conditions.Operators
{
    /// <summary>
    /// Operador que evalúa si una cadena no empieza por otra
    /// </summary>
    public class NotStartsWithOperator : StartsWithOperator
    {
        public new static NotStartsWithOperator Instance = new NotStartsWithOperator { InternalName = "notstartswith" };

        public override bool Evaluate(string match, string text)
        {
            return !base.Evaluate(match, text);
        }
    }
}
