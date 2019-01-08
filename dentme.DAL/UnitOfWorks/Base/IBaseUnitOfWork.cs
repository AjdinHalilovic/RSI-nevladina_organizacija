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

        IEventItemsDTORepository EventItemsDTORepository{ get; }
        IEventsDTORepository EventsDTORepository{ get; }
        ISponsorsDTORepository SponsorsDTORepository{ get; }
        IPersonsDTORepository PersonsDTORepository{ get; }


        #endregion

        #region Entities
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
        IAcademicDegreesRepository AcademicDegreesRepository { get; }
        IAcademicTitlesRepository AcademicTitlesRepository { get; }
        IMartialStatusesRepository MartialStatusesRepository { get; }
        IContactTypesRepository ContactTypesRepository { get; }
        IRolesRepository RolesRepository { get; }

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


        #endregion
        #endregion
    }
}
