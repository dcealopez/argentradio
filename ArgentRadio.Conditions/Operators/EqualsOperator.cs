using ArgentRadio.Common;

namespace ArgentRadio.Conditions.Operators
{
    /// <summary>
    /// Operador que evalúa si una cadena es igual a otra
    /// </summary>
    [Operator(InternalName = "equals")]
    public class EqualsOperator : IOperator
    {
        public static readonly EqualsOperator Instance = new EqualsOperator();

        public virtual bool Evaluate(string match, string text)
        {
            return StringHelper.Normalize(text).Equals(StringHelper.Normalize(match));
        }
    }
}
