namespace ArgentRadio.Conditions.Matches
{
    /// <summary>
    /// Clase base para los grupos de condiciones
    /// </summary>
    public class BaseMatchGroup : IMatchGroup
    {
        /// <summary>
        /// Condiciones que contiene el grupo de condiciones
        /// </summary>
        public Match[] Matches { get; set; }

        /// <summary>
        /// Grupos de condiciones que contiene el grupo
        /// </summary>
        public IMatchGroup[] ChildMatchGroups { get; set; }

        public virtual bool Evaluate(string text)
        {
            throw new System.NotImplementedException();
        }
    }
}
