using System;
using System.Reflection;
using System.Linq;

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
        /// <param name="internalName">nombre interno de la acción</param>
        /// <param name="args">argumentos a pasarle a la acción</param>
        public static void Execute(string internalName, params object[] args)
        {
            IAction action = (IAction)Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetCustomAttribute(typeof(Action)) != null && ((Action)t.GetCustomAttribute(typeof(Action))).InternalName == internalName && t.GetInterfaces().Contains(typeof(IAction)) && t.GetField("Instance") != null && t.GetField("Instance").GetValue(null) != null).Select(i => i.GetField("Instance").GetValue(null)).FirstOrDefault();

            if(action == null)
            {
                return;
            }

            action.Execute(args);
        }
    }
}
