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

        private IInstitutionUsersDTORepository _institutionUsersDTORepository;
        public IInstitutionUsersDTORepository InstitutionUsersDTORepository => _institutionUsersDTORepository = _institutionUsersDTORepository ?? new InstitutionUsersDTORepository(_NevladinaOrgContext);

        private IOrganizationInstitutionUsersDTORepository _organizationInstitutionUsersDTORepository;
        public IOrganizationInstitutionUsersDTORepository OrganizationInstitutionUsersDTORepository => _organizationInstitutionUsersDTORepository = _organizationInstitutionUsersDTORepository ?? new OrganizationInstitutionUsersDTORepository(_NevladinaOrgContext);


        private IPersonUsersDTORepository _personUsersDTORepository;
        public IPersonUsersDTORepository PersonUsersDTORepository => _personUsersDTORepository = _personUsersDTORepository ?? new PersonUsersDTORepository(_NevladinaOrgContext);

        private IPersonsDTORepository _personsDTORepository;
        public IPersonsDTORepository PersonsDTORepository => _personsDTORepository = _personsDTORepository ?? new PersonsDTORepository(_NevladinaOrgContext);

        private ICitiesDTORepository _citiesDTORepository;
        public ICitiesDTORepository CitiesDTORepository => _citiesDTORepository = _citiesDTORepository ?? new CitiesDTORepository(_NevladinaOrgContext);

        private IRegionsDTORepository _regionsDTORepository;
        public IRegionsDTORepository RegionsDTORepository => _regionsDTORepository = _regionsDTORepository ?? new RegionsDTORepository(_NevladinaOrgContext);

        private IEventItemsDTORepository _eventItemsDTORepository;
        public IEventItemsDTORepository EventItemsDTORepository => _eventItemsDTORepository = _eventItemsDTORepository ?? new EventItemsDTORepository(_NevladinaOrgContext);

        private IEventsDTORepository _eventsDTORepository;
        public IEventsDTORepository EventsDTORepository => _eventsDTORepository = _eventsDTORepository ?? new EventsDTORepository(_NevladinaOrgContext);

        private ISponsorsDTORepository _sponsorsDTORepository;
        public ISponsorsDTORepository SponsorsDTORepository => _sponsorsDTORepository = _sponsorsDTORepository ?? new SponsorsDTORepository(_NevladinaOrgContext);

        private ILicenseDTORepository _licenseDTORepository;
        public ILicenseDTORepository LicenseDTORepository => _licenseDTORepository = _licenseDTORepository ?? new LicenseDTORepository(_NevladinaOrgContext);

        private IOrganizationsDTORepository _organizationsDTORepository;
        public IOrganizationsDTORepository OrganizationsDTORepository => _organizationsDTORepository = _organizationsDTORepository ?? new OrganizationDTORepository(_NevladinaOrgContext);
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

        private IPersonsRepository _personsRepository;
        public IPersonsRepository PersonsRepository => _personsRepository = _personsRepository ?? new PersonsRepository(_NevladinaOrgContext);

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



        private IEventsRepository _eventsRepository;
        public IEventsRepository EventsRepository => _eventsRepository = _eventsRepository ?? new EventsRepository(_NevladinaOrgContext);

        private IEventDocumentsRepository _eventDocumentsRepository;
        public IEventDocumentsRepository EventDocumentsRepository => _eventDocumentsRepository = _eventDocumentsRepository ?? new EventDocumentsRepository(_NevladinaOrgContext);

        private IEventImagesRepository _eventImagesRepository;
        public IEventImagesRepository EventImagesRepository => _eventImagesRepository = _eventImagesRepository ?? new EventImagesRepository(_NevladinaOrgContext);

        private IEventItemsRepository _eventItemsRepository;
        public IEventItemsRepository EventItemsRepository => _eventItemsRepository = _eventItemsRepository ?? new EventItemsRepository(_NevladinaOrgContext);

        private IEventItemEventTypesRepository _eventItemEventTypesRepository;
        public IEventItemEventTypesRepository EventItemEventTypesRepository => _eventItemEventTypesRepository = _eventItemEventTypesRepository ?? new EventItemEventTypesRepository(_NevladinaOrgContext);

        private IEventItemTypesRepository _eventItemTypesRepository;
        public IEventItemTypesRepository EventItemTypesRepository => _eventItemTypesRepository = _eventItemTypesRepository ?? new EventItemTypesRepository(_NevladinaOrgContext);

        private IEventRegistrationsRepository _eventRegistrationsRepository;
        public IEventRegistrationsRepository EventRegistrationsRepository => _eventRegistrationsRepository = _eventRegistrationsRepository ?? new EventRegistrationsRepository(_NevladinaOrgContext);

        private IEventSponsorsRepository _eventSponsorsRepository;
        public IEventSponsorsRepository EventSponsorsRepository => _eventSponsorsRepository = _eventSponsorsRepository ?? new EventSponsorsRepository(_NevladinaOrgContext);

        private IEventUsersRepository _eventUsersRepository;
        public IEventUsersRepository EventUsersRepository => _eventUsersRepository = _eventUsersRepository ?? new EventUsersRepository(_NevladinaOrgContext);

        private ILecturesRepository _lecturesRepository;
        public ILecturesRepository LecturesRepository => _lecturesRepository = _lecturesRepository ?? new LecturesRepository(_NevladinaOrgContext);

        private ILecturersRepository _lecturersRepository;
        public ILecturersRepository LecturersRepository => _lecturersRepository = _lecturersRepository ?? new LecturersRepository(_NevladinaOrgContext);

        private ILectureLecturersRepository _lectureLecturersRepository;
        public ILectureLecturersRepository LectureLecturersRepository => _lectureLecturersRepository = _lectureLecturersRepository ?? new LectureLecturersRepository(_NevladinaOrgContext);

        private IPaymentsRepository _paymentsRepository;
        public IPaymentsRepository PaymentsRepository => _paymentsRepository = _paymentsRepository ?? new PaymentsRepository(_NevladinaOrgContext);

        private ISponsorsRepository _sponsorsRepository;
        public ISponsorsRepository SponsorsRepository => _sponsorsRepository = _sponsorsRepository ?? new SponsorsRepository(_NevladinaOrgContext);

        private ISponsorTypesRepository _sponsorTypesRepository;
        public ISponsorTypesRepository SponsorTypesRepository => _sponsorTypesRepository = _sponsorTypesRepository ?? new SponsorTypesRepository(_NevladinaOrgContext);

        private ILicensePeriodsRepository _licensePeriodsRepository;
        public ILicensePeriodsRepository LicensePeriodsRepository => _licensePeriodsRepository = _licensePeriodsRepository ?? new LicensePeriodsRepository(_NevladinaOrgContext);

        private ILicenseTypesRepository _licenseTypesRepository;
        public ILicenseTypesRepository LicenseTypesRepository => _licenseTypesRepository = _licenseTypesRepository ?? new LicenseTypesRepository(_NevladinaOrgContext);

        private IMembersRepository _membersRepository;
        public IMembersRepository MembersRepository => _membersRepository = _membersRepository ?? new MembersRepository(_NevladinaOrgContext);

        private IMemberLicensesRepository _memberLicensesRepository;
        public IMemberLicensesRepository MemberLicensesRepository => _memberLicensesRepository = _memberLicensesRepository ?? new MemberLicensesRepository(_NevladinaOrgContext);


        private IPersonContactsRepository _personContactsRepository;
        public IPersonContactsRepository PersonContactsRepository => _personContactsRepository = _personContactsRepository ?? new PersonContactsRepository(_NevladinaOrgContext);

        private IPersonDetailsRepository _personDetailsRepository;
        public IPersonDetailsRepository PersonDetailsRepository => _personDetailsRepository = _personDetailsRepository ?? new PersonDetailsRepository(_NevladinaOrgContext);

        private IPersonPhotosRepository _personPhotosRepository;
        public IPersonPhotosRepository PersonPhotosRepository => _personPhotosRepository = _personPhotosRepository ?? new PersonPhotosRepository(_NevladinaOrgContext);

        private IUserRolesRepository _userRolesRepository;
        public IUserRolesRepository UserRolesRepository => _userRolesRepository = _userRolesRepository ?? new UserRolesRepository(_NevladinaOrgContext);

        private IEmployeeStatusesRepository _employeeStatusesRepository;
        public IEmployeeStatusesRepository EmployeeStatusesRepository => _employeeStatusesRepository = _employeeStatusesRepository ?? new EmployeeStatusesRepository(_NevladinaOrgContext);

        private IAnnouncementsRepository _announcementsRepository;
        public IAnnouncementsRepository AnnouncementsRepository => _announcementsRepository = _announcementsRepository ?? new AnnouncementsRepository(_NevladinaOrgContext);

        private IAnnouncementTypesRepository _announcementTypesRepository;
        public IAnnouncementTypesRepository AnnouncementTypesRepository => _announcementTypesRepository = _announcementTypesRepository ?? new AnnouncementTypesRepository(_NevladinaOrgContext);

        private IAnnouncementOrganizationsRepository _announcementOrganizationsRepository;
        public IAnnouncementOrganizationsRepository AnnouncementOrganizationsRepository => _announcementOrganizationsRepository = _announcementOrganizationsRepository ?? new AnnouncementOrganizationsRepository(_NevladinaOrgContext);

        private IAnnouncementPhotosRepository _announcementPhotosRepository;
        public IAnnouncementPhotosRepository AnnouncementPhotosRepository => _announcementPhotosRepository = _announcementPhotosRepository ?? new AnnouncementPhotosRepository(_NevladinaOrgContext);

        private IAnnouncementDocumentsRepository _announcementDocumentsRepository;
        public IAnnouncementDocumentsRepository AnnouncementDocumentsRepository => _announcementDocumentsRepository = _announcementDocumentsRepository ?? new AnnouncementDocumentsRepository(_NevladinaOrgContext);

        private IAnnouncementUsersRepository _announcementUsersRepository;
        public IAnnouncementUsersRepository AnnouncementUsersRepository => _announcementUsersRepository = _announcementUsersRepository ?? new AnnouncementUsersRepository(_NevladinaOrgContext);

        private IOrganizationRepository _organizationRepository;
        public IOrganizationRepository OrganizationRepository => _organizationRepository = _organizationRepository ?? new OrganizationRepository(_NevladinaOrgContext);

        private IOrganizationTypesRepository _organizationTypesRepository;
        public IOrganizationTypesRepository OrganizationTypesRepository => _organizationTypesRepository = _organizationTypesRepository ?? new OrganizationTypesRepository(_NevladinaOrgContext);

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
