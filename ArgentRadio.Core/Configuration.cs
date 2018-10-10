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
        public static readonly string ConfigurationFile = "config.xml";

        /// <summary>
        /// Parámetros de configuración a nivel de aplicación
        /// </summary>
        public static Settings AppSettings { get; set; } = new Settings();

        /// <summary>
        /// Condiciones para las notificaciones sonoras
        /// </summary>
        public static List<Condition> Conditions { get; set; } = new List<Condition>();

        /// <summary>
        /// Lee y carga la configuración del fichero de configuración
        /// </summary>
        public static void LoadXmlConfigurationFile()
        {
            var xmlConfigDocument = XDocument.Load($"./{ConfigurationFile}");

            if (xmlConfigDocument.Root == null)
            {
                return;
            }

            Conditions = new List<Condition>();

            // Cargar la configuración a nivel de aplicación
            var settingsSection = xmlConfigDocument.Root.Element("Settings");

            AppSettings = new Settings
            {
                UnitDesignation = (string) settingsSection?.Element("UnitDesignation")
            };

            // Cargar las condiciones
            var xmlConditions = xmlConfigDocument.Root.Element("Conditions")?.Elements()
                .Where(e => e.Name == "Condition");

            if (xmlConditions == null)
            {
                return;
            }

            foreach (var xmlCondition in xmlConditions)
            {
                var description = (string) xmlCondition.Attribute("Description");
                var actions = new List<Tuple<string, string>>();

                // Cargar las acciones de la condición
                xmlCondition.Element("Actions")?.Elements("Command").ToList()
                    .ForEach(action =>
                        actions.Add(
                            new Tuple<string, string>(
                                (string) action.Attribute("Name"),
                                (string) action.Attribute("Args"))));

                // Crear la condición y cargar sus reglas
                Conditions.Add(new Condition
                {
                    Description = description,
                    Actions = actions,
                    Rules = GetConditions<AndMatchGroup>(xmlCondition.Element("Rules"))
                });
            }
        }

        /// <summary>
        /// Escribe el fichero de configuración XML con la configuración cargada actualmente
        /// </summary>
        public static void WriteXmlConfiguratonFile()
        {
            if (AppSettings == null)
            {
                AppSettings = new Settings();
            }

            if (Conditions == null)
            {
                Conditions = new List<Condition>();
            }

            var rootElement = new XElement("ArgentRadio");
            rootElement.Add(new XElement("Settings"));
            rootElement.Add(new XElement("Conditions"));

            // Construir la configuración a nivel de aplicación
            rootElement.Element("Settings")
                ?.Add(new XElement("UnitDesignation", AppSettings.UnitDesignation));

            // Construir las condiciones
            foreach (var condition in Conditions)
            {
                var conditionElement = new XElement("Condition");
                conditionElement.Add(new XElement("Actions"));
                conditionElement.SetAttributeValue("Description", condition.Description);

                // Construir las acciones
                if (condition.Actions != null)
                {
                    foreach (var action in condition.Actions)
                    {
                        var commandElement = new XElement("Command");
                        commandElement.SetAttributeValue("Name", action.Item1);
                        commandElement.SetAttributeValue("Args", action.Item2);

                        conditionElement.Element("Actions")?.Add(commandElement);
                    }
                }

                // Construir las reglas de la condición
                var rulesElement = new XElement("Rules");
                rulesElement = CreateXmlConditionRules(condition.Rules, rulesElement);

                conditionElement.Add(rulesElement);

                rootElement.Element("Conditions")?.Add(conditionElement);
            }

            var xmlConfigurationFile = new XDocument(rootElement);
            xmlConfigurationFile.Save($"./{ConfigurationFile}");
        }

        /// <summary>
        /// (Recursivo) Obtiene todas las condiciones contenidas dentro
        /// del elemento XML pasado como argumento
        /// </summary>
        /// <typeparam name="T">tipo de grupo de condiciones a devolver</typeparam>
        /// <param name="rulesElement">elemento XML donde están definidas las condiciones</param>
        /// <returns>un grupo de condiciones raíz con todas las condiciones cargadas</returns>
        private static T GetConditions<T>(XContainer rulesElement) where T : MatchGroup, new()
        {
            var rootGroup = new T
            {
                Matches = rulesElement.Elements("Match")
                    .Where(matchElement => !matchElement.HasElements)
                    .Select(matchElement =>
                        new Match
                        {
                            Text = matchElement.Value,
                            ConditionalOperator =
                                Operator.GetInstanceFromInternalName(
                                    (string) matchElement.Attribute("Operator"))
                        })
                    .ToList(),

                ChildMatchGroups = new List<MatchGroup>()
            };

            var matchGroups = rulesElement.Elements()
                .Where(element =>
                    (element.Name == "OrGroup" || element.Name == "AndGroup") &&
                    element.Parent == rulesElement)
                .ToList();

            foreach (var matchGroup in matchGroups)
            {
                if (matchGroup.Name == "OrGroup")
                {
                    rootGroup.ChildMatchGroups.Add(GetConditions<OrMatchGroup>(matchGroup));
                }
                else if (matchGroup.Name == "AndGroup")
                {
                    rootGroup.ChildMatchGroups.Add(GetConditions<AndMatchGroup>(matchGroup));
                }
            }

            return rootGroup;
        }

        /// <summary>
        /// (Recursivo) Construye y guarda en la instancia XElement pasada como parámetro
        /// el árbol de condiciones completo de una condición
        /// </summary>
        /// <param name="matchGroup">
        /// grupo de condiciones sobre el que empezar a construir el árbol, normalmente es el raíz
        /// </param>
        /// <param name="rulesElement">
        /// XElement donde se almacenará el árbol de condiciones completo de la condición
        /// </param>
        /// <returns>el árbol de condiciones completo de una condición</returns>
        private static XElement CreateXmlConditionRules(MatchGroup matchGroup,
            XElement rulesElement)
        {
            // Escribimos las condiciones del grupo actual
            foreach (var match in matchGroup.Matches)
            {
                var matchElement = new XElement("Match", match.Text);
                matchElement.SetAttributeValue("Operator",
                    Operator.GetOperatorInternalName(match.ConditionalOperator));

                rulesElement.Add(matchElement);
            }

            if (matchGroup.ChildMatchGroups == null)
            {
                return rulesElement;
            }

            // Escribimos los grupos de condiciones hijos del grupo de condiciones actual
            foreach (var childMatchGroup in matchGroup.ChildMatchGroups)
            {
                var matchGroupElementName = childMatchGroup is OrMatchGroup
                    ? "OrGroup"
                    : childMatchGroup is AndMatchGroup ? "AndGroup" : null;

                if (matchGroupElementName == null)
                {
                    continue;
                }

                var matchGroupElement = new XElement(matchGroupElementName);
                matchGroupElement = CreateXmlConditionRules(childMatchGroup, matchGroupElement);

                rulesElement.Add(matchGroupElement);
            }

            return rulesElement;
        }
    }
}