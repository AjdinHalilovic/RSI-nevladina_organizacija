using DAL.Repositories.Base.IRepository;
using DAL.Repositories.Base.IRepository.DTO;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace DAL
{
    public interface IBaseUnitOfWork : IUnitOfWork
    {
        IDbContextTransaction Transaction();

        #region Repositories
        IUsersDTORepository UsersDTORepository { get; }
        IPersonUsersDTORepository PersonUsersDTORepository { get; }

        #region Dto's
        ICitiesDTORepository CitiesDTORepository { get; }
        IRegionsDTORepository RegionsDTORepository { get; }
        #endregion

        #region Entities
        IFunctionalitiesRepository FunctionalitiesRepository { get; }
        IUsersRepository UsersRepository{ get; }
        IInstitutionUsersRepository InstitutionUsersRepository{ get; }
        IOrganizationInstitutionUsersRepository OrganizationInstitutionUsersRepository{ get; }
        ICitiesRepository CitiesRepository{ get; }
        ICountriesRepository CountriesRepository { get; }
        IRegionsRepository RegionsRepository { get; }
        ICitizenshipsRepository CitizenshipsRepository { get; }
        INationalitiesRepository NationalitiesRepository { get; }
        IAcademicDegreesRepository AcademicDegreesRepository { get; }
        IAcademicTitlesRepository AcademicTitlesRepository { get; }
        IMartialStatusesRepository MartialStatusesRepository { get; }
        IContactTypesRepository ContactTypesRepository { get; }
        IRolesRepository RolesRepository { get; }

        #endregion
        #endregion
    }
}
