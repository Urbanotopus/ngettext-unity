using System.Globalization;
using NGettext;

namespace ngettext_unity {
    internal class UnityCatalog : Catalog {
        #region Constructors
        
        /// <inheritdoc />
        public UnityCatalog(string domain, string localeDir)
            : base(new UnityMoLoader(domain, localeDir)) {
        }

        /// <inheritdoc />
        public UnityCatalog(string domain, string localeDir, CultureInfo cultureInfo)
            : base(new UnityMoLoader(domain, localeDir), cultureInfo) {
        }
        
        #endregion
    }
}
