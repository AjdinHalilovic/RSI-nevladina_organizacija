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
        private readonly NevladinaOrgContext _NevladinaOrgContext;
        public IDbContextTransaction Transaction() => _NevladinaOrgContext.Database.CurrentTransaction ?? _NevladinaOrgContext.Database.BeginTransaction();
        public BaseUnitOfWork(NevladinaOrgContext NevladinaOrgContext)
        {
            _NevladinaOrgContext = NevladinaOrgContext;
        }



        #region Repositories
        #region Dto's
        private IUsersDTORepository _usersDTORepository;
        public IUsersDTORepository UsersDTORepository => _usersDTORepository = _usersDTORepository ?? new UsersDTORepository(_NevladinaOrgContext);
        private IPersonUsersDTORepository _personUsersDTORepository;
        public IPersonUsersDTORepository PersonUsersDTORepository => _personUsersDTORepository = _personUsersDTORepository ?? new PersonUsersDTORepository(_NevladinaOrgContext);

        private ICitiesDTORepository _citiesDTORepository;
        public ICitiesDTORepository CitiesDTORepository => _citiesDTORepository = _citiesDTORepository ?? new CitiesDTORepository(_NevladinaOrgContext);

        private IRegionsDTORepository _regionsDTORepository;
        public IRegionsDTORepository RegionsDTORepository => _regionsDTORepository = _regionsDTORepository ?? new RegionsDTORepository(_NevladinaOrgContext);
        #endregion

        #region Entities
        private IFunctionalitiesRepository _functionalitiesRepository;
        public IFunctionalitiesRepository FunctionalitiesRepository => _functionalitiesRepository = _functionalitiesRepository ?? new FunctionalitiesRepository(_NevladinaOrgContext);

        private IInstitutionUsersRepository _institutionUsersRepository;
        public IInstitutionUsersRepository InstitutionUsersRepository => _institutionUsersRepository = _institutionUsersRepository ?? new InstitutionUsersRepository(_NevladinaOrgContext);

        private IOrganizationInstitutionUsersRepository _organizationInstitutionUsersRepository;
        public IOrganizationInstitutionUsersRepository OrganizationInstitutionUsersRepository => _organizationInstitutionUsersRepository = _organizationInstitutionUsersRepository ?? new OrganizationInstitutionUsersRepository(_NevladinaOrgContext);

        private IUsersRepository _usersRepository;
        public IUsersRepository UsersRepository => _usersRepository = _usersRepository ?? new UsersRepository(_NevladinaOrgContext);

        private ICitiesRepository _citiesRepository;
        public ICitiesRepository CitiesRepository => _citiesRepository = _citiesRepository ?? new CitiesRepository(_NevladinaOrgContext);

        private ICountriesRepository _countriesRepository;
        public ICountriesRepository CountriesRepository => _countriesRepository = _countriesRepository ?? new CountriesRepository(_NevladinaOrgContext);

        private IRegionsRepository _regionsRepository;
        public IRegionsRepository RegionsRepository => _regionsRepository = _regionsRepository ?? new RegionsRepository(_NevladinaOrgContext);

        private ICitizenshipsRepository _citizenshipsRepository;
        public ICitizenshipsRepository CitizenshipsRepository => _citizenshipsRepository = _citizenshipsRepository ?? new CitizenshipsRepository(_NevladinaOrgContext);

        private INationalitiesRepository _nationalitiesRepository;
        public INationalitiesRepository NationalitiesRepository => _nationalitiesRepository = _nationalitiesRepository ?? new NationalitiesRepository(_NevladinaOrgContext);

        private IAcademicDegreesRepository _academicDegreesRepository;
        public IAcademicDegreesRepository AcademicDegreesRepository => _academicDegreesRepository = _academicDegreesRepository ?? new AcademicDegreesRepository(_NevladinaOrgContext);

        private IAcademicTitlesRepository _academicTitlesRepository;
        public IAcademicTitlesRepository AcademicTitlesRepository => _academicTitlesRepository = _academicTitlesRepository ?? new AcademicTitlesRepository(_NevladinaOrgContext);

        private IMartialStatusesRepository _martialStatusesRepository;
        public IMartialStatusesRepository MartialStatusesRepository => _martialStatusesRepository = _martialStatusesRepository ?? new MartialStatusesRepository(_NevladinaOrgContext);

        private IContactTypesRepository _contactTypesRepository;
        public IContactTypesRepository ContactTypesRepository => _contactTypesRepository = _contactTypesRepository ?? new ContactTypesRepository(_NevladinaOrgContext);

        private IRolesRepository _rolesRepository;
        public IRolesRepository RolesRepository => _rolesRepository = _rolesRepository ?? new RolesRepository(_NevladinaOrgContext);
        #endregion
        #endregion



        #region Transaction
        public IDbContextTransaction BeginTransaction() => _NevladinaOrgContext.Database.BeginTransaction();

        public async Task<IDbContextTransaction> BeginTransactionAsync() => await _NevladinaOrgContext.Database.BeginTransactionAsync();

        #endregion

        #region SaveChanges
        public int SaveChanges() => _NevladinaOrgContext.SaveChanges();

        public async Task<int> SaveChangesAsync() => await _NevladinaOrgContext.SaveChangesAsync();
        #endregion

        #region DiscardChanges
        public void DiscardChanges()
        {
            foreach (var entry in _NevladinaOrgContext.ChangeTracker.Entries()
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
        public void Dispose() => _NevladinaOrgContext.Dispose();
        #endregion
    }
}
