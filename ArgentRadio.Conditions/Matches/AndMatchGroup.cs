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
            var matchesResult = true;

            if (Matches != null)
            {
                foreach (var match in Matches)
                {
                    matchesResult = match.Evaluate(text);

                    if (!matchesResult)
                    {
                        break;
                    }
                }
            }

            // Evaluar los grupos de condiciones hijos de este grupo
            var childMatchGroupsResults = true;

            if (ChildMatchGroups != null)
            {
                foreach (var childMatchGroup in ChildMatchGroups)
                {
                    childMatchGroupsResults = childMatchGroup.Evaluate(text);

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
