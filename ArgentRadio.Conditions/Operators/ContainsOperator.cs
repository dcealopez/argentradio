using ArgentRadio.Conditions.Helpers;

namespace ArgentRadio.Conditions.Operators
{
    /// <summary>
    /// Operador que evalúa si una cadena contiene otra
    /// </summary>
    public class ContainsOperator : BaseOperator
    {
        public static ContainsOperator Instance = new ContainsOperator { InternalName = "contains" };

        public override bool Evaluate(string match, string text)
        {
            return StringHelper.Normalize(text).Contains(StringHelper.Normalize(match));
        }
    }
}
