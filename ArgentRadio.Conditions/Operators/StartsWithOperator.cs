using ArgentRadio.Common;

namespace ArgentRadio.Conditions.Operators
{
    /// <summary>
    /// Operador que evalúa si una cadena empieza por otra
    /// </summary>
    [Operator(InternalName = "startswith")]
    public class StartsWithOperator : IOperator
    {
        public static readonly StartsWithOperator Instance = new StartsWithOperator();

        public virtual bool Evaluate(string match, string text)
        {
            return StringHelper.Normalize(text).StartsWith(StringHelper.Normalize(match));
        }
    }
}
