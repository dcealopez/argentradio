﻿namespace ArgentRadio.Conditions.Operators
{
    /// <summary>
    /// Operador que evalúa si una cadena no es igual a otra
    /// </summary>
    [Operator(InternalName = "notequals")]
    public class NotEqualsOperator : EqualsOperator
    {
        public new static readonly NotEqualsOperator Instance = new NotEqualsOperator();

        public override bool Evaluate(string match, string text)
        {
            return !base.Evaluate(match, text);
        }
    }
}
