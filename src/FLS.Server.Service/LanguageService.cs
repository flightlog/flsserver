using System;
using System.Collections.Generic;
using System.Linq;
using FLS.Common.Exceptions;
using FLS.Data.WebApi.Globalization;
using FLS.Server.Data;
using FLS.Server.Data.DbEntities;
using FLS.Server.Data.Mapping;
using NLog;

namespace FLS.Server.Service
{
    public class LanguageService : BaseService
    {
        private readonly DataAccessService _dataAccessService;

        public LanguageService(DataAccessService dataAccessService, IdentityService identityService)
            : base(dataAccessService, identityService)
        {
            _dataAccessService = dataAccessService;
            Logger = LogManager.GetCurrentClassLogger();
        }

        #region Language
        public List<LanguageListItem> GetLanguageListItems()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var languages = context.Languages
                            .OrderBy(c => c.LanguageName)
                            .Select(x => new LanguageListItem()
                    {
                        LanguageId = x.LanguageId,
                        LanguageKey = x.LanguageKey,
                        LanguageName = x.LanguageName
                    })
                    .ToList();

                return languages;
            }
        }
        
        internal Language GetLanguage(string languageKey)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var language = context.Languages.FirstOrDefault(l => l.LanguageKey.ToLower() == languageKey.ToLower());

                return language;
            }
        }
        #endregion Language

        #region Translation
        public Dictionary<string, string> GetTranslation(string languageKey)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var language = context.Languages.FirstOrDefault(l => l.LanguageKey.ToLower() == languageKey.ToLower());

                if (language == null)
                    throw new Exception(string.Format("Language with key: {0} not found", languageKey));

                var translations = context.LanguageTranslations
                                    .Where(l => l.LanguageId == language.LanguageId)
                                    .OrderBy(l => l.TranslationKey);

                var translation = new Translation();
                translation.LanguageKey = languageKey;
                translation.Translations = new Dictionary<string, string>();

                foreach (var languageTranslation in translations)
                {
                    translation.Translations.Add(languageTranslation.TranslationKey,
                        languageTranslation.TranslationValue);
                }

                return translation.Translations;
            }
        }

        public string GetTranslation(string languageKey, string translationKey)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var translation = context.LanguageTranslations
                    .FirstOrDefault(l => l.Language.LanguageKey.ToLower() == languageKey.ToLower()
                                         && l.TranslationKey.ToLower() == translationKey.ToLower());

                if (translation == null)
                {
                    Logger.Warn($"Language translation with key: {translationKey} for language: {languageKey} not found!");
                    return translationKey.ToUpper();
                }

                return translation.TranslationValue;
            }
        }
        #endregion Translation

    }
}
