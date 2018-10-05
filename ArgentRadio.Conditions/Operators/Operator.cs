using System;
using System.Linq;
using System.Reflection;

namespace ArgentRadio.Conditions.Operators
{
    /// <summary>
    /// Atributo para marcar clases como operadores lógicos
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class Operator : Attribute
    {
        /// <summary>
        /// Nombre interno del operador
        /// </summary>
        public string InternalName { get; set; }

        /// <summary>
        /// Devuelve la instancia del operador con el nombre interno pasado como parámetro
        /// </summary>
        /// <param name="internalName">nombre interno del operador</param>
        /// <returns>la instancia del operador con el nombre interno pasado como parámetro</returns>
        public static IOperator GetInstanceFromInternalName(string internalName)
        {
            return (IOperator)(Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetCustomAttribute(typeof(Operator)) != null && ((Operator)t.GetCustomAttribute(typeof(Operator))).InternalName == internalName && t.GetInterfaces().Contains(typeof(IOperator)) && t.GetField("Instance") != null && t.GetField("Instance").GetValue(null) != null).Select(i => i.GetField("Instance").GetValue(null)).FirstOrDefault());
        }

        /// <summary>
        /// Devuelve el nombre interno del operador pasado como parámetro
        /// </summary>
        /// <param name="conditionalOperator">operador condicional</param>
        /// <returns>el nombre interno del operador pasado como parámetro</returns>
        public static string GetOperatorInternalName(IOperator conditionalOperator)
        {
            return (string)(Assembly.GetExecutingAssembly().GetTypes().Where(t => t == conditionalOperator.GetType() && t.GetCustomAttribute(typeof(Operator)) != null).Select(n => ((Operator)n.GetCustomAttribute(typeof(Operator))).InternalName).FirstOrDefault());
        }
    }
}
