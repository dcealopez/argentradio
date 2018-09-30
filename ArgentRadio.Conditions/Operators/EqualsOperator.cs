using ArgentRadio.Core;

namespace ArgentRadio.Conditions.Operators
{
    /// <summary>
    /// Operador que evalúa si una cadena es igual a otra
    /// </summary>
    public class EqualsOperator : BaseOperator
    {
        public static EqualsOperator Instance = new EqualsOperator { InternalName = "equals" };

        public override bool Evaluate(string match, string text)
        {
            return StringHelper.Normalize(text).Equals(StringHelper.Normalize(match));
        }
    }
}
