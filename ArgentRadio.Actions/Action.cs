using System;
using System.Linq;
using System.Reflection;

namespace ArgentRadio.Actions
{
    /// <summary>
    /// Atributo para marcar clases como acciones
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class Action : Attribute
    {
        /// <summary>
        /// Nombre interno de la acción
        /// </summary>
        public string InternalName { get; set; }

        /// <summary>
        /// Ejecuta la acción con el nombre interno especificado y
        /// los argumentos pasados como argumentos
        /// </summary>
        /// <param name="name">nombre interno de la acción</param>
        /// <param name="args">argumentos a pasarle a la acción</param>
        public static void Execute(string name, params object[] args)
        {
            var action = (IAction) Assembly.GetExecutingAssembly().GetTypes()
                .Where(type =>
                    type.GetCustomAttribute(typeof(Action)) != null &&
                    ((Action) type.GetCustomAttribute(typeof(Action))).InternalName == name &&
                    type.GetInterfaces().Contains(typeof(IAction)) &&
                    type.GetField("Instance") != null &&
                    type.GetField("Instance").GetValue(null) != null)
                .Select(type => type.GetField("Instance").GetValue(null))
                .FirstOrDefault();

            action?.Execute(args);
        }
    }
}