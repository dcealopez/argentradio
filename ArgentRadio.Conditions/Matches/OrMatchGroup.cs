namespace ArgentRadio.Conditions.Matches
{
    /// <summary>
    /// Grupo de condiciones OR
    /// </summary>
    public class OrMatchGroup : BaseMatchGroup
    {
        public override bool Evaluate(string text)
        {
            #region Evaluar las condiciones del grupo
            bool matchesResult = Matches?.Length == 0;

            if (Matches?.Length > 0)
            {
                for (int i = 0; i < Matches.Length; i++)
                {
                    matchesResult = Matches[i].Evaluate(text);

                    if (matchesResult)
                    {
                        break;
                    }
                }
            }
            #endregion

            #region Evaluar los grupos de condiciones hijos de este grupo        
            bool childMatchGroupsResults = ChildMatchGroups?.Length == 0;

            if (ChildMatchGroups?.Length > 0)
            {
                for (int i = 0; i < ChildMatchGroups.Length; i++)
                {
                    childMatchGroupsResults = ChildMatchGroups[i].Evaluate(text);

                    if (childMatchGroupsResults)
                    {
                        break;
                    }
                }
            }
            #endregion

            return matchesResult || childMatchGroupsResults;
        }
    }
}
