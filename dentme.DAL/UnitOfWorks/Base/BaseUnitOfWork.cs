using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using DAL.Repositories.Base.IRepository.DTO;
using DAL.Repositories.Base.Repository;
using DAL.Repositories.Base.Repository.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;
using System.Threading.Tasks;

namespace DAL
{
    public class BaseUnitOfWork : IBaseUnitOfWork
    {
        private readonly NevladinaOrgContext _nevladinaOrgContext;
        public IDbContextTransaction Transaction() => _nevladinaOrgContext.Database.CurrentTransaction ?? _nevladinaOrgContext.Database.BeginTransaction();
        public BaseUnitOfWork(NevladinaOrgContext nevladinaOrgContext)
        {
            _nevladinaOrgContext = nevladinaOrgContext;
        }



        #region Repositories
        #region Dto's
        private IUsersDTORepository _usersDTORepository;
        public IUsersDTORepository UsersDTORepository => _usersDTORepository = _usersDTORepository ?? new UsersDTORepository(_nevladinaOrgContext);
        private IPersonUsersDTORepository _personUsersDTORepository;
        public IPersonUsersDTORepository PersonUsersDTORepository => _personUsersDTORepository = _personUsersDTORepository ?? new PersonUsersDTORepository(_nevladinaOrgContext);

        private ICitiesDTORepository _citiesDTORepository;
        public ICitiesDTORepository CitiesDTORepository => _citiesDTORepository = _citiesDTORepository ?? new CitiesDTORepository(_nevladinaOrgContext);

        private IRegionsDTORepository _regionsDTORepository;
        public IRegionsDTORepository RegionsDTORepository => _regionsDTORepository = _regionsDTORepository ?? new RegionsDTORepository(_nevladinaOrgContext);
        #endregion

        #region Entities
        private IFunctionalitiesRepository _functionalitiesRepository;
        public IFunctionalitiesRepository FunctionalitiesRepository => _functionalitiesRepository = _functionalitiesRepository ?? new FunctionalitiesRepository(_nevladinaOrgContext);

        private IInstitutionUsersRepository _institutionUsersRepository;
        public IInstitutionUsersRepository InstitutionUsersRepository => _institutionUsersRepository = _institutionUsersRepository ?? new InstitutionUsersRepository(_nevladinaOrgContext);

        private IOrganizationInstitutionUsersRepository _organizationInstitutionUsersRepository;
        public IOrganizationInstitutionUsersRepository OrganizationInstitutionUsersRepository => _organizationInstitutionUsersRepository = _organizationInstitutionUsersRepository ?? new OrganizationInstitutionUsersRepository(_nevladinaOrgContext);

        private IUsersRepository _usersRepository;
        public IUsersRepository UsersRepository => _usersRepository = _usersRepository ?? new UsersRepository(_nevladinaOrgContext);

        private ICitiesRepository _citiesRepository;
        public ICitiesRepository CitiesRepository => _citiesRepository = _citiesRepository ?? new CitiesRepository(_nevladinaOrgContext);

        private ICountriesRepository _countriesRepository;
        public ICountriesRepository CountriesRepository => _countriesRepository = _countriesRepository ?? new CountriesRepository(_nevladinaOrgContext);

        private IRegionsRepository _regionsRepository;
        public IRegionsRepository RegionsRepository => _regionsRepository = _regionsRepository ?? new RegionsRepository(_nevladinaOrgContext);

        private ICitizenshipsRepository _citizenshipsRepository;
        public ICitizenshipsRepository CitizenshipsRepository => _citizenshipsRepository = _citizenshipsRepository ?? new CitizenshipsRepository(_nevladinaOrgContext);

        private INationalitiesRepository _nationalitiesRepository;
        public INationalitiesRepository NationalitiesRepository => _nationalitiesRepository = _nationalitiesRepository ?? new NationalitiesRepository(_nevladinaOrgContext);
        #endregion
        #endregion



        #region Transaction
        public IDbContextTransaction BeginTransaction() => _nevladinaOrgContext.Database.BeginTransaction();

        public async Task<IDbContextTransaction> BeginTransactionAsync() => await _nevladinaOrgContext.Database.BeginTransactionAsync();

        #endregion

        #region SaveChanges
        public int SaveChanges() => _nevladinaOrgContext.SaveChanges();

        public async Task<int> SaveChangesAsync() => await _nevladinaOrgContext.SaveChangesAsync();
        #endregion

        #region DiscardChanges
        public void DiscardChanges()
        {
            foreach (var entry in _nevladinaOrgContext.ChangeTracker.Entries()
                                  .Where(e => e.State != EntityState.Unchanged))
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }
        #endregion

        #region Dispose
        public void Dispose() => _nevladinaOrgContext.Dispose();
        #endregion
    }
}
