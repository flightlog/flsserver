using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NVelocity;
using NVelocity.App;
using System.Text;
using System.IO;

namespace Alpinely.TownCrier
{
    /// <summary>
    /// Template parser which uses JQuery Templating style tokens such as {%=FirstName%}
    /// Tokens are case-insensitive
    /// </summary>
    public class VelocityTemplateParser : ITemplateParser
    {
        private string TokenKeyModelName { get; set; }

        public VelocityTemplateParser(string tokenKeyModelName)
        {
            TokenKeyModelName = tokenKeyModelName;
        }

        /// <summary>
        /// Replaces tokens in the template text with the values from the supplied dictionary
        /// </summary>
        /// <param name="templateText">The template text</param>
        /// <param name="tokenValues">Dictionary mapping token names to values</param>
        /// <returns>Text with tokens replaced with their corresponding values from the dictionary</returns>
        public string ReplaceTokens(string templateText, IDictionary<string, object> tokenValues)
        {
            Velocity.Init();
            var velocityContext = new VelocityContext();
            velocityContext.Put(TokenKeyModelName, tokenValues[TokenKeyModelName]);
            var sb = new StringBuilder();
            Velocity.Evaluate(
                velocityContext,
                new StringWriter(sb),
                "test template",
                new StringReader(templateText));
            return sb.ToString();
        }
    }
}