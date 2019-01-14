namespace nevladinaOrg.Web
{
    public static class MagicStrings
    {
        public static class ViewNames
        {
            //Views
            public const string Registrations = nameof(Registrations);
            public const string Sponsors = nameof(Sponsors);
            public const string EventItems = nameof(EventItems);
            public const string Preview = nameof(Preview);
            public const string Add = nameof(Add);

            //PartielViews
            public const string _Index = nameof(_Index);
            public const string _Add = nameof(_Add);
            public const string _Edit = nameof(_Edit);
            public const string _Delete = nameof(_Delete);
            public const string _Details = nameof(_Details);
            public const string _Preview = nameof(_Preview);
            public const string _Confirm = nameof(_Confirm);
            public const string _AddSponsor = nameof(_AddSponsor);
            public const string _EditSponsor = nameof(_EditSponsor);
            public const string _AddEventItem = nameof(_AddEventItem);
            public const string _EditEventItem = nameof(_EditEventItem);

            public const string _AddSystem = nameof(_AddSystem);
            public const string _AddInstitution = nameof(_AddInstitution);
            public const string _AddOrganization = nameof(_AddOrganization);

            public const string _EditSystem = nameof(_EditSystem);
            public const string _EditInstitution = nameof(_EditInstitution);
            public const string _EditOrganization = nameof(_EditOrganization);
        }

        public static class TempDataNames
        {
            public const string jsNotifications = "jsNotifications";
        }

        public static class AreaNames
        {
            public const string Administration = "Administration";
            public const string Organizations = "Organizations";
            public const string Institutions = "Institutions";
        }
    }
}
