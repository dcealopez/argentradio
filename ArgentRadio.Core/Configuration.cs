using ArgentRadio.Conditions;
using ArgentRadio.Conditions.Matches;
using ArgentRadio.Conditions.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ArgentRadio.Core
{
    /// <summary>
    /// Clase estática de configuración
    /// Contiene los parámetros de configuración y métodos para cargarla y guardarla
    /// </summary>
    public static class Configuration
    {
        /// <summary>
        /// Nombre del fichero de configuración XML
        /// </summary>
        public const string ConfigurationFile = "config.xml";

        /// <summary>
        /// Parámetros de configuración a nivel de aplicación
        /// </summary>
        public static Settings AppSettings { get; set; }

        /// <summary>
        /// Condiciones para las notificaciones sonoras
        /// </summary>
        public static List<Condition> Conditions { get; set; }

        /// <summary>
        /// Lee y carga la configuración del fichero de configuración
        /// </summary>
        public static void LoadXmlConfigurationFile()
        {
            XDocument xmlConfigDocument = XDocument.Load($"./{ConfigurationFile}");
            Conditions = new List<Condition>();
            
            // Cargar la configuración a nivel de aplicación
            var settingsSection = xmlConfigDocument.Root.Element("Settings");

            AppSettings = new Settings
            {
                UnitDesignation = (string)settingsSection.Element("UnitDesignation")
            };

            // Cargar las condiciones
            IEnumerable<XElement> xmlConditions = xmlConfigDocument.Root.Element("Conditions").Elements().Where(e => e.Name == "Condition");

            foreach(XElement xmlCondition in xmlConditions)
            {
                string description = (string)xmlCondition.Attribute("Description");
                List<Tuple<string, string>> actions = new List<Tuple<string, string>>();

                // Cargar las acciones de la condición
                xmlCondition.Element("Actions").Elements("Command").ToList().ForEach(action => actions.Add(new Tuple<string, string>((string)action.Attribute("Name"), (string)action.Attribute("Args"))));

                // Cargar las reglas de la condición
                AndMatchGroup rootMatchGroup = new AndMatchGroup
                {
                    Matches = xmlCondition.Element("Rules").Elements("Match").Where(e => !e.HasElements).Select(m => new Match { Text = m.Value, ConditionalOperator = Operator.GetInstanceFromInternalName((string)m.Attribute("Operator")) }).ToList(),

                    ChildMatchGroups = GetConditionMatchGroupsFromXmlMatches(xmlCondition.Element("Rules"))
                };

                Conditions.Add(new Condition
                {
                    Description = description,
                    Actions = actions,
                    Rules = rootMatchGroup
                }); 
            }
        }

        /// <summary>
        /// Escribe el fichero de configuración XML con la configuración cargada actualmente
        /// </summary>
        public static void WriteXmlConfiguratonFile()
        {
            XElement rootElement = new XElement("ArgentRadio");
            rootElement.Add(new XElement("Settings"));
            rootElement.Add(new XElement("Conditions"));

            // Construir la configuración a nivel de aplicación
            rootElement.Element("Settings").Add(new XElement("UnitDesignation", AppSettings.UnitDesignation));

            // Construir las condiciones
            foreach(Condition condition in Conditions)
            {
                XElement conditionElement = new XElement("Condition");
                conditionElement.Add(new XElement("Actions"));
                conditionElement.SetAttributeValue("Description", condition.Description);

                // Construir las acciones
                condition.Actions.ForEach(action =>
                {
                    XElement commandElement = new XElement("Command");
                    commandElement.SetAttributeValue("Name", action.Item1);
                    commandElement.SetAttributeValue("Args", action.Item2);

                    conditionElement.Element("Actions").Add(commandElement);
                });

                // Construir las reglas de la condición
                XElement rulesElement = new XElement("Rules");
                rulesElement = CreateXmlConditionRules(condition.Rules, rulesElement);

                conditionElement.Add(rulesElement);

                rootElement.Element("Conditions").Add(conditionElement);
            }

            XDocument xmlConfigurationFile = new XDocument(rootElement);
            xmlConfigurationFile.Save($"./{ConfigurationFile}");
        }

        /// <summary>
        /// (Recursivo) Devuelve instancias de todos los grupos
        /// de condiciones almacenados en una condición XML
        /// </summary>
        /// <param name="xmlCondition">elemento XML raíz donde están almacendas los grupos de condiciones</param>
        /// <returns>los grupos de condiciones almacenados en una condición XML</returns>
        private static List<IMatchGroup> GetConditionMatchGroupsFromXmlMatches(XElement xmlCondition)
        {
            List<IMatchGroup> matchGroups = new List<IMatchGroup>();
            IEnumerable<XElement> xmlMatchGroups = xmlCondition.Elements().Where(e => (e.Name == "OrGroup" || e.Name == "AndGroup") && e.Parent == xmlCondition);   
            
            // Cargamos las condiciones de los grupos de condiciones encontrados
            foreach(XElement xmlMatchGroup in xmlMatchGroups)
            {
                List<Match> matchGroupConditions = xmlMatchGroups.Elements("Match").Where(e => !e.HasElements).Select(m => new Match { Text = m.Value, ConditionalOperator = Operator.GetInstanceFromInternalName((string)m.Attribute("Operator")) }).ToList();
                IMatchGroup matchGroup = null;

                if(xmlMatchGroup.Name == "OrGroup")
                {
                    matchGroup = new OrMatchGroup
                    {
                        Matches = matchGroupConditions,
                        ChildMatchGroups = GetConditionMatchGroupsFromXmlMatches(xmlMatchGroup)
                    };
                }
                else if(xmlMatchGroup.Name == "AndGroup")
                {
                    matchGroup = new AndMatchGroup
                    {
                        Matches = matchGroupConditions,
                        ChildMatchGroups = GetConditionMatchGroupsFromXmlMatches(xmlMatchGroup)
                    };
                }

                if(matchGroup != null)
                {
                    matchGroups.Add(matchGroup);
                }
            }

            return matchGroups;
        }

        /// <summary>
        /// (Recursivo) Construye y guarda en la instancia XElement pasada como parámetro 
        /// el árbol de condiciones completo de una condición
        /// </summary>
        /// <param name="matchGroup">grupo de condiciones sobre elque empezar a construir el árbol, normalmente es el raíz</param>
        /// <param name="rulesElement">XElement donde se almacenará el árbol de condiciones completo de la condición</param>
        /// <returns>el árbol de condiciones completo de una condición</returns>
        private static XElement CreateXmlConditionRules(MatchGroup matchGroup, XElement rulesElement)
        {
            // Escribimos las condiciones del grupo actual
            foreach(Match match in matchGroup.Matches)
            {
                XElement matchElement = new XElement("Match", match.Text);
                matchElement.SetAttributeValue("Operator", Operator.GetOperatorInternalName(match.ConditionalOperator));
                rulesElement.Add(matchElement);
            }

            // Escribimos los grupos de condiciones hijos del grupo de condiciones actual
            foreach(MatchGroup childMatchGroup in matchGroup.ChildMatchGroups)
            {
                XElement matchGroupElement = new XElement(childMatchGroup.GetType() == typeof(OrMatchGroup) ? "OrGroup" : (childMatchGroup.GetType() == typeof(AndMatchGroup) ? "AndGroup" : null));

                matchGroupElement = CreateXmlConditionRules(childMatchGroup, matchGroupElement);
                rulesElement.Add(matchGroupElement);
            }            

            return rulesElement;
        }
    }
}
