using Core.Entities.Base.DTO;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository.DTO;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.Repository.DTO
{
    public class EventItemsDTORepository : Repository<EventItemDTO, int>, IEventItemsDTORepository
    {
        public EventItemsDTORepository(NevladinaOrgContext context) : base(context) { }

        public IEnumerable<EventItemDTO> GetByEventId(int eventId)
        {
            return Context.EventItemEventTypes.Where(x => !x.IsDeleted).Include(x => x.EventItem).Include(x => x.EventItemType).Where(x => !x.EventItem.IsDeleted && x.EventItem.EventId == eventId)
                .GroupBy(x => x.EventItem.Id)
                .Select(x => x.Select(e => new EventItemDTO
                {
                    Id = e.EventItem.Id,
                    ConferenceRoom = e.EventItem.ConferenceRoom,
                    StartTime = e.EventItem.StartTime.Value,
                    EndTime = e.EventItem.EndTime.Value,
                    Name = e.EventItem.Name,
                    EventItemTypeName = string.Join(", ", x.Select(t => t.EventItemType.Name).ToList()),
                    AboutLecture = Context.Lectures.Where(l => !l.IsDeleted && l.Id == e.EventItem.Id).FirstOrDefault().AboutLecture ?? null,
                    Lecturers = Context.LectureLecturers.Where(le => !le.IsDeleted && le.LectureId == e.EventItem.Id).Include(i => i.Lecturer).Select(l => l.Lecturer.UserId).FirstOrDefault() == null ? string.Join(",", Context.LectureLecturers.Where(le => !le.IsDeleted && le.LectureId == e.EventItem.Id).Include(i => i.Lecturer).Select(le => le.Lecturer.FirstName + " " + le.Lecturer.LastName).ToList()) : Context.LectureLecturers.Where(le => !le.IsDeleted && le.LectureId == e.EventItem.Id).Include(i => i.Lecturer).Select(l => l.Lecturer.UserId).FirstOrDefault().ToString()

                }).FirstOrDefault()).OrderBy(x=>x.StartTime);
        }
    }
}
