using ArgentRadio.Conditions.Matches;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ArgentRadio.Conditions
{
    /// <summary>
    /// Clase para las condiciones
    /// </summary>
    public class Condition
    {
        /// <summary>
        /// Descripción de la condición
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Grupo de condiciones raíz que contiene todas las condiciones
        /// </summary>
        public AndMatchGroup Rules { get; set; }

        /// <summary>
        /// Acciones a ejecutar de la condición
        /// Contiene una lista de tuplas:
        /// El primer valor contiene el nombre interno de la acción
        /// El segundo valor contiene los argumentos a pasar a la acción
        /// </summary>
        public List<Tuple<string, string>> Actions { get; set; }

        /// <summary>
        /// Evalúa la condición contra la cadena que se le pase
        /// </summary>
        /// <param name="text">cadena contra la que evaluar la condición</param>
        /// <returns>true si se cumple la condición, false si no</returns>
        public bool Evaluate(string text)
        {
            return Rules.Evaluate(text);
        }

        /// <summary>
        /// Ejecuta todas las acciones asociadas a esta condición
        /// </summary>
        public void ExecuteActions()
        {
            Actions.Select(action => action).ToList()
                .ForEach(action => ArgentRadio.Actions.Action.Execute(action.Item1, action.Item2));
        }
    }
}