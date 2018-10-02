using NGettext;
using NGettext.Loaders;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace ngettext_unity {
    public delegate object ResourcesLoader(string path);

    public class UnityMoLoader : MoLoader {
        private static ResourcesLoader _loader = Resources.Load;
        private const string _LC_MESSAGES = "LC_MESSAGES";
        private TextAsset _foundAndLoadedAsset;

        public static void SetLoader(ResourcesLoader newLoader) {
            _loader = newLoader;
        }

        public UnityMoLoader(string domain, string localeDir) : base(domain, localeDir) {
        }

        private IEnumerable<string> GetPossibleFilePath(
                CultureInfo cultureInfo, string domain, string localeDir) {
            var possibleFiles = new[] {
                this.GetFileName(localeDir, domain, cultureInfo.Name),
                this.GetFileName(localeDir, domain, cultureInfo.TwoLetterISOLanguageName)
            };
            return possibleFiles;
        }

        protected override string FindTranslationFile(
                CultureInfo cultureInfo, string domain, string localeDir) {

            var possibleFiles = this.GetPossibleFilePath(
                cultureInfo, domain, localeDir);

            foreach (var possibleFilePath in possibleFiles) {
                var configText = _loader(possibleFilePath) as TextAsset;
                
                if (!configText) continue;
            
                this._foundAndLoadedAsset = configText;
                return possibleFilePath;
            }

            return null;
        }


        /// <inheritdoc />
        protected override void Load(string filePath, Catalog catalog) {
            Debug.Log(string.Format(
                "Getting translations from asset \"{0}\"...", filePath));
            using (var stream = new MemoryStream(this._foundAndLoadedAsset.bytes)) {
                this.Load(stream, catalog);
            }
        }

        /// <inheritdoc />
        protected override string GetFileName(
                string localeDir, string domain, string locale) {
            
            var filePath = Path.Combine(
                localeDir, Path.Combine(locale, Path.Combine(_LC_MESSAGES, domain)));
            filePath = filePath.Replace('\\', '/');
            return filePath;
        }
    }
}
