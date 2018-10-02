using System.Globalization;
using NGettext;

namespace ngettext_unity {
    public class NGettextUnity {
        private const string LOCALE_PATH = "Locale/";

        private string _localeDomain = "messages_mo";
        private string _currentLocale = null;
        private Catalog _currentCatalogue;

        private NGettextUnity() { }

        public static NGettextUnity Instance { get; } = new NGettextUnity();
        public static Catalog Catalog => Instance.GetCatalog();

        private Catalog GetCatalog() {
            return this._currentCatalogue;
        }

        public void SetLocaleDomain(string localeDomain) {
            this._localeDomain = localeDomain;
        }

        public void LoadLocale(string localeName) {
            this._currentCatalogue = new UnityCatalog(
                this._localeDomain, LOCALE_PATH, new CultureInfo(localeName));
            this._currentLocale = localeName;
        }
    }
}
