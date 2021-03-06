﻿using DAL.Repositories.Base.IRepository;
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
        IInstitutionUsersDTORepository InstitutionUsersDTORepository { get; }

        #region Dto's
        ICitiesDTORepository CitiesDTORepository { get; }
        IRegionsDTORepository RegionsDTORepository { get; }
        ILicenseDTORepository LicenseDTORepository{ get; }

        IEventItemsDTORepository EventItemsDTORepository{ get; }
        IEventsDTORepository EventsDTORepository{ get; }
        ISponsorsDTORepository SponsorsDTORepository{ get; }
        IPersonsDTORepository PersonsDTORepository{ get; }
        IOrganizationInstitutionUsersDTORepository OrganizationInstitutionUsersDTORepository{ get; }
        IOrganizationsDTORepository OrganizationsDTORepository { get; }
        IInstitutionsDTORepository InstitutionsDTORepository { get; }

        #region Entities
        IInstitutionsRepository InstitutionsRepository { get; }
        IInstitutionTypesRepository InstitutionTypesRepository { get; }
        IFunctionalitiesRepository FunctionalitiesRepository { get; }
        IUsersRepository UsersRepository{ get; }
        IPersonsRepository PersonsRepository { get; }
        IInstitutionUsersRepository InstitutionUsersRepository{ get; }
        IOrganizationInstitutionUsersRepository OrganizationInstitutionUsersRepository{ get; }
        ICitiesRepository CitiesRepository{ get; }
        ICountriesRepository CountriesRepository { get; }
        IRegionsRepository RegionsRepository { get; }
        ICitizenshipsRepository CitizenshipsRepository { get; }
        INationalitiesRepository NationalitiesRepository { get; }
        IEmployeeStatusesRepository EmployeeStatusesRepository{ get; }
        IAcademicDegreesRepository AcademicDegreesRepository { get; }
        IAcademicTitlesRepository AcademicTitlesRepository { get; }
        IMartialStatusesRepository MartialStatusesRepository { get; }
        IContactTypesRepository ContactTypesRepository { get; }
        IRolesRepository RolesRepository { get; }
        IRolesFunctionalitiesRepository RolesFunctionalitiesRepository { get; }

        IEventsRepository EventsRepository { get; }
        IEventDocumentsRepository EventDocumentsRepository{ get; }
        IEventImagesRepository EventImagesRepository{ get; }
        IEventItemEventTypesRepository EventItemEventTypesRepository{ get; }
        IEventItemsRepository EventItemsRepository{ get; }
        IEventItemTypesRepository EventItemTypesRepository{ get; }
        IEventRegistrationsRepository EventRegistrationsRepository{ get; }
        IEventSponsorsRepository EventSponsorsRepository{ get; }
        IEventUsersRepository EventUsersRepository{ get; }
        ILecturesRepository LecturesRepository{ get; }
        ILecturersRepository LecturersRepository{ get; }
        ILectureLecturersRepository LectureLecturersRepository{ get; }
        IPaymentsRepository PaymentsRepository{ get; }
        ISponsorsRepository SponsorsRepository{ get; }
        ISponsorTypesRepository SponsorTypesRepository{ get; }
        ILicensePeriodsRepository LicensePeriodsRepository { get; }
        ILicenseTypesRepository LicenseTypesRepository { get; }
        IMemberLicensesRepository MemberLicensesRepository{ get; }
        IMembersRepository MembersRepository{ get; }
        IPersonContactsRepository PersonContactsRepository{ get; }
        IPersonDetailsRepository PersonDetailsRepository{ get; }
        IPersonPhotosRepository PersonPhotosRepository{ get; }
        IUserRolesRepository UserRolesRepository{ get; }
        IAnnouncementsRepository AnnouncementsRepository { get; }
        IAnnouncementTypesRepository AnnouncementTypesRepository { get; }
        IAnnouncementOrganizationsRepository AnnouncementOrganizationsRepository { get; }
        IAnnouncementPhotosRepository AnnouncementPhotosRepository { get; }
        IAnnouncementDocumentsRepository AnnouncementDocumentsRepository { get; }
        IAnnouncementUsersRepository AnnouncementUsersRepository { get; }
        IOrganizationRepository OrganizationRepository { get; }
        IOrganizationTypesRepository OrganizationTypesRepository { get; }

        #endregion
        #endregion
        #endregion
    }
}