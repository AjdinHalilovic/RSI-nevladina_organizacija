
namespace nevladinaOrg.Web.Constants
{
    public static class Enumerations
    {
        public enum DefaultUserRoles
        {
            Administrator = 1
        }

        public enum Functionalities
        {
            Index = 1,
            Add,
            Edit,
            Delete,
            Details,
            ChangeStatus,

            SystemSettings,
            GeneralSettings,
            RegionalSettings
        }

        public enum Steps
        {
            First = 1,
            Second,
            Third,
            Fourth,
            Fifth,
            Sixth,
            Seventh,
            Eighth,
            Ninth,
            Tenth,
            Finish
        }

        public enum Tabs
        {
            First = 1,
            Second,
            Third,
            Fourth,
            Fifth,
            Sixth,
            Seventh,
            Eighth,
            Ninth,
            Tenth,
            Finish
        }

        public enum LogActivity
        {
            LoginSuccessful = 1,
            LoginUnsuccessful = 2,
            Logout = 3,
            SystemError = 4,
            UnauthorizedAccess = 5,
            AnonimousUser = 6,
            Visitor = 7,
            Add = 8,
            Edit = 9,
            Delete = 10,
            EditStatus = 11,
            PasswordChange = 12,
            Undefined = 13
        }

        public enum LogTypes
        {
            Info,
            Warning,
            Error,
            Fatal
        }

        public enum AjaxFlashMessageType
        {
            Success,
            Danger
        }

        public enum FileTypes
        {
            Document,
            Photo,
            Video,
            Archive

        }

        public enum YouTubeCategories
        {
            FilmAndAnimation = 1,
            AutosAndVehicles = 2,
            Music = 10,
            PetsAndAnimals = 15,
            Sports = 17,
            TravelAndEvents = 19,
            Gaming = 20,
            PeopleAndBlogs = 22,
            Comedy = 23,
            Entertainment = 24,
            NewsAndPolitics = 25,
            HowtoAndStyle = 26,
            Education = 27,
            ScienceAndTechnology = 28,
            NonprofitsAndActivism = 29
        }

        public enum YouTubePrivacy
        {
            Private,
            Public,
            Unlisted
        }

        public enum SaveAndOptions
        {
            SaveAndClose,
            SaveAndNew,
            SaveAndDuplicate
        }

        public enum UserStoryStatus
        {
            Pending=1,
            Finished,
            Rejected,
            Implementation
        }
        public enum BugStatus
        {
            Pending=1,
            Urgent,
            Finished
        }
        public enum ContactTypes
        {
            PrimaryEmail = 1,
            SecondaryEmail,
            PhoneNumber,
            MobileNumber,
            Fax
        }
    }
}