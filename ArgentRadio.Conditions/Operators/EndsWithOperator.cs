﻿using ArgentRadio.Common;

namespace ArgentRadio.Conditions.Operators
{
    /// <summary>
    /// Operador que evalúa si una cadena termina con otra
    /// </summary>
    [Operator(InternalName = "endswith")]
    public class EndsWithOperator : IOperator
    {
        public static readonly EndsWithOperator Instance = new EndsWithOperator();

        public virtual bool Evaluate(string match, string text)
        {
            return StringHelper.Normalize(text).EndsWith(StringHelper.Normalize(match));
        }
    }
}
