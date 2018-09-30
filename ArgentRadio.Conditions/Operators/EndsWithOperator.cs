using ArgentRadio.Core;

namespace ArgentRadio.Conditions.Operators
{
    /// <summary>
    /// Operador que evalúa si una cadena termina con otra
    /// </summary>
    public class EndsWithOperator : BaseOperator
    {
        public static EndsWithOperator Instance = new EndsWithOperator { InternalName = "endswith" };

        public override bool Evaluate(string match, string text)
        {
            return StringHelper.Normalize(text).EndsWith(StringHelper.Normalize(match));
        }
    }
}
