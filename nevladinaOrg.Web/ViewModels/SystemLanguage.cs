using System.Globalization;

namespace nevladinaOrg.Web.ViewModels
{
    public class SystemLanguage
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string DateFormat { get; set; }
        public string DateTimeFormat { get; set; }
        public int ClockTimeType { get; set; }
        public bool Default { get; set; }
        public CultureInfo CultureInfo { get; set; }
    }
}