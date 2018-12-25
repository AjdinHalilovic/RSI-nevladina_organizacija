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
        #endregion

        #region Entities
        IFunctionalitiesRepository FunctionalitiesRepository { get; }
        IUsersRepository UsersRepository{ get; }
        IInstitutionUsersRepository InstitutionUsersRepository{ get; }
        IOrganizationInstitutionUsersRepository OrganizationInstitutionUsersRepository{ get; }

        #endregion
        #endregion
    }
}
