using System;
using System.Collections.Generic;

namespace FLS.Data.WebApi.Globalization
{
    public class Translation
    {
        public string LanguageKey { get; set; }
        public Dictionary<string, string> Translations { get; set; }
    }
}
