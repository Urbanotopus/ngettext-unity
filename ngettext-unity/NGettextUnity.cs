using System.Globalization;
using NGettext;

namespace ngettext_unity {
    public class NGettextUnity {
        private const string LOCALE_PATH = "Locale/";
        private static readonly NGettextUnity _instance = new NGettextUnity();

        private string _localeDomain = "messages_mo";
        private string _currentLocale = null;
        private Catalog _currentCatalogue;

        private NGettextUnity() { }

        public static NGettextUnity Instance {
            get {
                return _instance;
            }
        }

        public static Catalog Catalog {
            get {
                return _instance.GetCatalog();
            }
        }

        public Catalog GetCatalog() {
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
