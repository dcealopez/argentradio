using System.Collections.Generic;

namespace ArgentRadio.Conditions.Matches
{
    /// <summary>
    /// Clase base para los grupos de condiciones
    /// </summary>
    public abstract class MatchGroup : IMatchGroup
    {
        /// <summary>
        /// Condiciones que contiene el grupo de condiciones
        /// </summary>
        public List<Match> Matches { get; set; }

        /// <summary>
        /// Grupos de condiciones que contiene el grupo
        /// </summary>
        public List<IMatchGroup> ChildMatchGroups { get; set; }

        public abstract bool Evaluate(string text);        
    }
}
