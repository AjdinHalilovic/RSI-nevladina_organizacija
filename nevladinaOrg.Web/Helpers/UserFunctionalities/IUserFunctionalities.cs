using nevladinaOrg.Web.Constants;

namespace nevladinaOrg.Web.Helpers.UserFunctionalities
{
    public interface IUserFunctionalities
    {
        bool Index { get; set; }
        bool Add { get; set; }
        bool Edit { get; set; }
        bool Delete { get; set; }
        bool Details { get; set; }
        bool ChangeStatus { get; set; }

        bool SystemSettings { get; set; }
        bool GeneralSettings { get; set; }
        bool RegionalSettings { get; set; }

        bool AnyAction { get; }
        bool AnySettings { get; }

        void Prepare(string controllerName);
        bool IsAuthorized(string controllerName, Enumerations.Functionalities functionality);
    }
}