using System;
using System.Collections.Generic;
using System.Linq;
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
            var dbEntityList = GetLanguages();

            var list = dbEntityList.Select(language => language.ToLanguageListItem()).ToList();

            return list;
        }

        internal List<Language> GetLanguages()
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var list = context.Languages.OrderBy(c => c.LanguageName).ToList();

                return list;
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
            var language = GetLanguage(languageKey);

            if (language == null) throw new Exception(string.Format("Language with key: {0} not found", languageKey));

            var translations = GetTranslations(language.LanguageId);

            var translation = new Translation();
            translation.LanguageKey = languageKey;
            translation.Translations = new Dictionary<string, string>();

            foreach (var languageTranslation in translations)
            {
                translation.Translations.Add(languageTranslation.TranslationKey, languageTranslation.TranslationValue);
            }

            return translation.Translations;
        }

        internal List<LanguageTranslation> GetTranslations(int languageId)
        {
            using (var context = _dataAccessService.CreateDbContext())
            {
                var translations = context.LanguageTranslations.Where(l => l.LanguageId == languageId).OrderBy(l => l.TranslationKey);

                return translations.ToList();
            }
        }
        #endregion Translation

    }
}
