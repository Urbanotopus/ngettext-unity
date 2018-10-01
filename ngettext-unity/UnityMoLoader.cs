using NGettext;
using NGettext.Loaders;
using System;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace ngettext_unity {
    class UnityMoLoader : MoLoader {
        private const string LC_MESSAGES = "LC_MESSAGES";
        private TextAsset foundAndLoadedAsset;

        public UnityMoLoader(string domain, string localeDir) : base(domain, localeDir) {
        }

        override protected string FindTranslationFile(CultureInfo cultureInfo, string domain, string localeDir) {
            var possibleFiles = new[] {
                this.GetFileName(localeDir, domain, cultureInfo.Name),
                this.GetFileName(localeDir, domain, cultureInfo.TwoLetterISOLanguageName)
            };

            foreach (string possibleFilePath in possibleFiles) {
                TextAsset configText = Resources.Load(possibleFilePath) as TextAsset;
                if (configText) {
                    this.foundAndLoadedAsset = configText;
                    return possibleFilePath;
                }
            }

            return null;
        }


        /// <summary>
        /// Loads translations to the specified catalog from specified MO file path.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="catalog"></param>
        override protected void Load(string filePath, Catalog catalog) {
            Debug.Log(String.Format(
                "Getting translations from asset \"{0}\"...", filePath));
            using (var stream = new MemoryStream(foundAndLoadedAsset.bytes)) {
                this.Load(stream, catalog);
            }
        }

        /// <summary>
		/// Constructs a standard path to the MO translation file using specified path to the locale directory, 
		/// domain and locale's TwoLetterISOLanguageName string.
		/// </summary>
		/// <param name="localeDir"></param>
		/// <param name="domain"></param>
		/// <param name="locale"></param>
		/// <returns></returns>
		override protected string GetFileName(string localeDir, string domain, string locale)
        {
            string filePath = Path.Combine(
                localeDir, Path.Combine(locale, Path.Combine(LC_MESSAGES, domain)));
            filePath = filePath.Replace('\\', '/');
            return filePath;
        }
    }
}
