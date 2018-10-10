using ArgentRadio.Common;

namespace ArgentRadio.Conditions.Operators
{
    /// <summary>
    /// Operador que evalúa si una cadena contiene otra
    /// </summary>
    [Operator(InternalName = "contains")]
    public class ContainsOperator : IOperator
    {
        public static readonly ContainsOperator Instance = new ContainsOperator();

        public virtual bool Evaluate(string match, string text)
        {
            return StringHelper.Normalize(text).Contains(StringHelper.Normalize(match));
        }
    }
}
