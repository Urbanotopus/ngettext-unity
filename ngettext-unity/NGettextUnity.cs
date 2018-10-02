using System.Globalization;
using System.Runtime.CompilerServices;
using NGettext;

namespace ngettext_unity {
    public class NGettextUnity {
        private const string LOCALE_PATH = "Locale/";

        private string _localeDomain = "messages_mo";
        private Catalog _currentCatalogue;

        public static readonly NGettextUnity Instance = new NGettextUnity();

        private NGettextUnity() { }

        public static Catalog Catalog {
            get {
                return Instance.GetCatalog(); 
            }
        }

        private Catalog GetCatalog() {
            return this._currentCatalogue;
        }

        public void SetLocaleDomain(string localeDomain) {
            this._localeDomain = localeDomain;
        }

        public void LoadLocale(string localeName) {
            this._currentCatalogue = new UnityCatalog(
                this._localeDomain, LOCALE_PATH, new CultureInfo(localeName));
        }
    }
}
