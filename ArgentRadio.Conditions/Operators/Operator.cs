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
        /// <param name="name">nombre interno del operador</param>
        /// <returns>la instancia del operador con el nombre interno pasado como parámetro</returns>
        public static IOperator GetInstanceFromInternalName(string name)
        {
            return (IOperator) (Assembly.GetExecutingAssembly().GetTypes().Where(type =>
                    type.GetCustomAttribute(typeof(Operator)) != null &&
                    ((Operator) type.GetCustomAttribute(typeof(Operator))).InternalName == name &&
                    type.GetInterfaces().Contains(typeof(IOperator)) &&
                    type.GetField("Instance") != null &&
                    type.GetField("Instance").GetValue(null) != null)
                .Select(type => type.GetField("Instance").GetValue(null))
                .FirstOrDefault());
        }

        /// <summary>
        /// Devuelve el nombre interno del operador pasado como parámetro
        /// </summary>
        /// <param name="conditionalOperator">operador condicional</param>
        /// <returns>el nombre interno del operador pasado como parámetro</returns>
        public static string GetOperatorInternalName(IOperator conditionalOperator)
        {
            return (Assembly.GetExecutingAssembly().GetTypes()
                .Where(type =>
                    type == conditionalOperator.GetType() &&
                    type.GetCustomAttribute(typeof(Operator)) != null)
                .Select(type => ((Operator) type.GetCustomAttribute(typeof(Operator))).InternalName)
                .FirstOrDefault());
        }
    }
}