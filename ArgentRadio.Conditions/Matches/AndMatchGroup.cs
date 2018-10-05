namespace ArgentRadio.Conditions.Matches
{
    /// <summary>
    /// Grupo de condiciones AND
    /// </summary>
    public class AndMatchGroup : MatchGroup
    {
        public override bool Evaluate(string text)
        {
            // Evaluar las condiciones del grupo
            bool matchesResult = true;

            if (Matches?.Count > 0)
            {
                for (int i = 0; i < Matches.Count; i++)
                {
                    matchesResult = Matches[i].Evaluate(text);

                    if (!matchesResult)
                    {
                        break;
                    }
                }
            }

            // Evaluar los grupos de condiciones hijos de este grupo        
            bool childMatchGroupsResults = true;

            if (ChildMatchGroups?.Count > 0)
            {
                for (int i = 0; i < ChildMatchGroups.Count; i++)
                {
                    childMatchGroupsResults = ChildMatchGroups[i].Evaluate(text);

                    if (!childMatchGroupsResults)
                    {
                        break;
                    }
                }
            }

            return matchesResult && childMatchGroupsResults;
        }
    }
}
