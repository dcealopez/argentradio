namespace ArgentRadio.Conditions.Matches
{
    /// <summary>
    /// Grupo de condiciones OR
    /// </summary>
    public class OrMatchGroup : MatchGroup
    {
        public override bool Evaluate(string text)
        {
            // Evaluar las condiciones del grupo
            var matchesResult = false;

            if (Matches != null)
            {
                foreach (var match in Matches)
                {
                    matchesResult = match.Evaluate(text);

                    if (matchesResult)
                    {
                        break;
                    }
                }
            }

            // Evaluar los grupos de condiciones hijos de este grupo
            if (ChildMatchGroups == null)
            {
                return matchesResult;
            }

            var childMatchGroupsResults = false;

            foreach (var childMatchGroup in ChildMatchGroups)
            {
                childMatchGroupsResults = childMatchGroup.Evaluate(text);

                if (childMatchGroupsResults)
                {
                    break;
                }
            }

            return matchesResult || childMatchGroupsResults;
        }
    }
}
