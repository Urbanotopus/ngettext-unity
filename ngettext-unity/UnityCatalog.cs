using System.Globalization;
using NGettext;

namespace ngettext_unity {
    class UnityCatalog : Catalog {
        #region Constructors
#if !NETSTANDARD1_0
        /// <summary>
        /// Initializes a new instance of the <see cref="Catalog"/> class using the current UI culture info
        /// and loads data for specified domain and locale directory using a new <see cref="UnityMoLoader"/> instance.
        /// </summary>
        /// <param name="domain">Catalog domain name.</param>
        /// <param name="localeDir">Directory that contains gettext localization files.</param>
        public UnityCatalog(string domain, string localeDir)
            : base(new UnityMoLoader(domain, localeDir)) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Catalog"/> class using given culture info
        /// and loads data for specified domain and locale directory using a new <see cref="UnityMoLoader"/> instance.
        /// </summary>
        /// <param name="domain">Catalog domain name.</param>
        /// <param name="localeDir">Directory that contains gettext localization files.</param>
        /// <param name="cultureInfo">Culture info.</param>
        public UnityCatalog(string domain, string localeDir, CultureInfo cultureInfo)
            : base(new UnityMoLoader(domain, localeDir), cultureInfo) {
        }
#endif

        #endregion
    }
}
