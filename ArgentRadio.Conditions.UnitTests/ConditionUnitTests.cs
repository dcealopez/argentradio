using ArgentRadio.Conditions.Matches;
using ArgentRadio.Conditions.Operators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ArgentRadio.Conditions.UnitTests
{
    /// <summary>
    /// Clase para las pruebas unitarias de la librería ArgentRadio.Conditions
    /// </summary>
    [TestClass()]
    public class ConditionUnitTests
    {
        /// <summary>
        /// Prueba unitaria del método Evaluate de la clase Condition
        /// </summary>
        [TestMethod()]
        public void EvaluateUnitTest()
        {
            // Crear una condición de prueba
            var testCondition = new Condition
            {
                Rules = new AndMatchGroup
                {
                    Matches = new List<Match>
                    {
                        new Match
                        {
                            ConditionalOperator = ContainsOperator.Instance,
                            Text = "OutsideContains"
                        },

                        new Match
                        {
                            ConditionalOperator = NotStartsWithOperator.Instance,
                            Text = "OutsideNotStartsWith"
                        },

                        new Match
                        {
                            ConditionalOperator = NotEndsWithOperator.Instance,
                            Text = "OutsideNotEndsWith"
                        },

                        new Match
                        {
                            ConditionalOperator = NotEqualsOperator.Instance,
                            Text = "StartsWithOutsideContains"
                        }
                    },

                    ChildMatchGroups = new List<MatchGroup>
                    {
                        new OrMatchGroup
                        {
                            Matches = new List<Match>
                            {
                                new Match
                                {
                                    ConditionalOperator = StartsWithOperator.Instance,
                                    Text = "StartsWith"
                                },

                                new Match
                                {
                                    ConditionalOperator = EndsWithOperator.Instance,
                                    Text = "EndsWith"
                                },

                                new Match
                                {
                                    ConditionalOperator = EqualsOperator.Instance,
                                    Text = "OutsideContainsEquals"
                                }
                            },

                            ChildMatchGroups = new List<MatchGroup>()
                            {
                                new AndMatchGroup
                                {
                                    Matches = new List<Match>
                                    {
                                        new Match
                                        {
                                            ConditionalOperator = ContainsOperator.Instance,
                                            Text = "DeepContains"
                                        },

                                        new Match
                                        {
                                            ConditionalOperator = NotContainsOperator.Instance,
                                            Text = "DeepNotContains"
                                        }
                                    },

                                    ChildMatchGroups = new List<MatchGroup>
                                    {
                                        new OrMatchGroup
                                        {
                                            Matches = new List<Match>
                                            {
                                                new Match
                                                {
                                                    ConditionalOperator = ContainsOperator.Instance,
                                                    Text = "DeeperContains"
                                                },

                                                new Match
                                                {
                                                    ConditionalOperator =
                                                        NotContainsOperator.Instance,
                                                    Text = "DeeperNotContains"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            // Validar los resultados de la evaluación
            Assert.AreEqual(false, testCondition.Evaluate("sTaRtSWithOutsideContains"));
            Assert.AreEqual(true, testCondition.Evaluate("OutSidEConTainsEndsWith"));
            Assert.AreEqual(true, testCondition.Evaluate("AAAAAOutSidEConTainsEndsWith"));
            Assert.AreEqual(false, testCondition.Evaluate("AAAAAOutSidEConTainsEndsWithEEE"));
            Assert.AreEqual(true, testCondition.Evaluate("OutsideContAiNsEquals"));
            Assert.AreEqual(true, testCondition.Evaluate("OutsideContainsDeepContains"));
            Assert.AreEqual(false,
                testCondition.Evaluate("OutsideContainsDeepContainsDeeperNotContains"));
            Assert.AreEqual(false,
                testCondition.Evaluate("OutsideContainsDeepContainsDeepNotContains"));
            Assert.AreEqual(false, testCondition.Evaluate("OutsideContainsDeepNotContains"));
            Assert.AreEqual(true,
                testCondition.Evaluate("OutsideContainsDeepContainsDeeperContains"));
            Assert.AreEqual(false, testCondition.Evaluate("OutsideContainsDeeperNotContains"));
            Assert.AreEqual(false, testCondition.Evaluate("ThisIsGoingToReturnFalse"));
        }
    }
}