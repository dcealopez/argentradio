using ArgentRadio.Conditions.Helpers;

namespace ArgentRadio.Conditions.Operators
{
    /// <summary>
    /// Operador que evalúa si una cadena empieza por otra
    /// </summary>
    public class StartsWithOperator : BaseOperator
    {
        public static StartsWithOperator Instance = new StartsWithOperator { InternalName = "startswith" };

        public override bool Evaluate(string match, string text)
        {
            return StringHelper.Normalize(text).StartsWith(StringHelper.Normalize(match));
        }
    }
}
