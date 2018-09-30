namespace ArgentRadio.Conditions.Matches
{
    /// <summary>
    /// Grupo de condiciones AND
    /// </summary>
    public class AndMatchGroup : BaseMatchGroup
    {
        public override bool Evaluate(string text)
        {
            #region Evaluar las condiciones del grupo
            bool matchesResult = true;

            if (Matches?.Length > 0)
            {
                for (int i = 0; i < Matches.Length; i++)
                {
                    matchesResult = Matches[i].Evaluate(text);

                    if (!matchesResult)
                    {
                        break;
                    }
                }
            }
            #endregion

            #region Evaluar los grupos de condiciones hijos de este grupo        
            bool childMatchGroupsResults = true;

            if (ChildMatchGroups?.Length > 0)
            {
                for (int i = 0; i < ChildMatchGroups.Length; i++)
                {
                    childMatchGroupsResults = ChildMatchGroups[i].Evaluate(text);

                    if (!childMatchGroupsResults)
                    {
                        break;
                    }
                }
            }
            #endregion

            return matchesResult && childMatchGroupsResults;
        }
    }
}
