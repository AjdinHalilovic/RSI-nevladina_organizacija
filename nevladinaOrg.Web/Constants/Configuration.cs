using System.Linq;
using System.Globalization;
using System.Collections.Generic;

using nevladinaOrg.Web.ViewModels;

namespace nevladinaOrg.Web.Constants
{
    public static class Configuration
    {
        public static readonly List<SystemLanguage> SupportedSystemLanguages = new List<SystemLanguage>
        {
            new SystemLanguage
            {
                Title = "English",
                Name = "en-us",
                Icon = "/app/media/img/languages/us.svg",
                DateFormat = "MM/dd/yyyy",
                DateTimeFormat="MM/dd/yyyy HH:mm",
                ClockTimeType = 12,
                CultureInfo = new CultureInfo("en-US"),
                Default = false
            },
            new SystemLanguage
            {
                Title = "Bosanski",
                Name = "bs-latn-ba",
                Icon = "/app/media/img/languages/bs.svg",
                DateFormat = "dd.MM.yyyy",
                DateTimeFormat="dd.MM.yyyy HH:mm",
                ClockTimeType = 24,
                CultureInfo = new CultureInfo("bs-Latn-BA"),
                Default = true,
            }
        };

        public static string DefaultCulture => SupportedSystemLanguages.Where(sl => sl.Default).Select(sl => sl.Name).Single();
        public static int CurrentClockTimeType => SupportedSystemLanguages.Single(sl => sl.Name == CultureInfo.CurrentCulture.Name).ClockTimeType;

        public const string RequestVerificationToken = "__RequestVerificationToken";
        public const string RequestVerificationTokenHeader = "X-XSRF-TOKEN";
        public const string SharedResourceFileName = "SharedResource";
        public const string MainDatabaseConnectionString = "nevladinaOrgDatabase";
        public const string ResourcesPath = "Resources";
        public const string ErrorsPath = "/error/{0}";

        public const int EmailContactTypeId = 1;
        public const int PhoneContactTypeId = 2;

	    public const int ThumbnailWidth = 640;
	    public const int ThumbnailHeight = 360;

		public const int DefaultInstitutionId = 1;
        public const int DefaultPageSize = 10;

        public const int DefaultMinuteStep = 15;

    }
}