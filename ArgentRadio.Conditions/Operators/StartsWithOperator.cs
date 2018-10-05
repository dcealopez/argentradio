using ArgentRadio.Common;

namespace ArgentRadio.Conditions.Operators
{
    /// <summary>
    /// Operador que evalúa si una cadena empieza por otra
    /// </summary>
    [Operator(InternalName = "startswith")]
    public class StartsWithOperator : IOperator
    {
        public static StartsWithOperator Instance = new StartsWithOperator();

        public bool Evaluate(string match, string text)
        {
            return StringHelper.Normalize(text).StartsWith(StringHelper.Normalize(match));
        }
    }
}
