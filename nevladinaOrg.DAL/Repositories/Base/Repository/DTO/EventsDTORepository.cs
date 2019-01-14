using Core.Entities.Base.DTO;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository.DTO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.Repository.DTO
{
    public class EventsDTORepository:Repository<EventDTO,int>,IEventsDTORepository
    {
        public EventsDTORepository(NevladinaOrgContext context) : base(context) { }
        public override IEnumerable<EventDTO> GetAll()
        {
            return Context.Events.Where(x => !x.IsDeleted).Include(x => x.City).Include(x => x.Country)
                .Select(x => new EventDTO {
                    Id = x.Id,
                    DateFrom = x.DateFrom.Value.ToShortDateString(),
                    DateTo = x.DateTo.Value.ToShortDateString(),
                    Name = x.Name,
                    City = x.City.Name,
                    Country = x.Country.Name,
                    Place = x.Place,
                    NumberOfPayments = Context.Payments.Include(r => r.EventRegistration).Where(er => er.EventRegistration.EventId == x.Id).Count(),
                    NumberOfRegistrations = Context.EventRegistrations.Where(e => e.EventId == x.Id).Count(),
                    Description =x.Description });
        }
        public  EventDTO GetByEventId(int eventId)
        {
            return Context.Events.Where(x => !x.IsDeleted && x.Id==eventId).Include(x => x.City).Include(x => x.Country)
                .Select(x => new EventDTO {
                    Id = x.Id,
                    DateFrom = x.DateFrom.Value.ToShortDateString(),
                    DateTo = x.DateTo.Value.ToShortDateString(),
                    Name = x.Name,
                    City = x.City.Name,
                    Country = x.Country.Name,
                    Place = x.Place,
                    NumberOfPayments = Context.Payments.Include(r => r.EventRegistration).Where(er => er.EventRegistration.EventId == x.Id).Count(),
                    NumberOfRegistrations = Context.EventRegistrations.Where(e => e.EventId == x.Id).Count(),
                    Description = x.Description })
                .FirstOrDefault();
        }
        public IEnumerable<EventDTO> GetByUserId(int userId)
        {
            return Context.Payments
                .Include(x=>x.EventRegistration)
                .Include(x => x.EventRegistration.Event)
                .Include(x=>x.EventRegistration.Event.City)
                .Include(x=>x.EventRegistration.Event.Country)
                .Where(x =>!x.IsDeleted && !x.EventRegistration.IsDeleted && !x.EventRegistration.Event.IsDeleted && x.EventRegistration.UserId == userId)
                .Select(x => new EventDTO
                {
                    Id = x.EventRegistration.Event.Id,
                    DateFrom = x.EventRegistration.Event.DateFrom.Value.ToShortDateString(),
                    DateTo = x.EventRegistration.Event.DateTo.Value.ToShortDateString(),
                    Name = x.EventRegistration.Event.Name,
                    City = x.EventRegistration.Event.City.Name,
                    Country = x.EventRegistration.Event.Country.Name,
                    Place = x.EventRegistration.Event.Place,
                    NumberOfPayments = Context.Payments.Include(r => r.EventRegistration).Where(er => er.EventRegistration.EventId == x.Id).Count(),
                    NumberOfRegistrations = Context.EventRegistrations.Where(e => e.EventId == x.Id).Count(),
                    Description = x.EventRegistration.Event.Description
                });
        }
    }
}
