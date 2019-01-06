using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System.Collections.Generic;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class LectureLecturersRepository:Repository<LectureLecturer,int>,ILectureLecturersRepository
    {
        public LectureLecturersRepository(NevladinaOrgContext context) : base(context) { }
        public List<int> RemoveByLectureId(int lectureId)
        {
            var removedIds = Context.LectureLecturers.Where(x => !x.IsDeleted && x.LectureId == lectureId).Select(x => x.Id).ToList();
            Context.LectureLecturers.RemoveRange(Context.LectureLecturers.Where(x => !x.IsDeleted && x.LectureId == lectureId));
            return removedIds;
        }
        public IEnumerable<LectureLecturer> GetByLectureId(int Id)
        {
            return Context.LectureLecturers.Where(x => !x.IsDeleted && x.LectureId == Id);
        }
        public List<int> Remove(int lectureId)
        {
            var removedIds = Context.LectureLecturers.Where(x => x.LectureId == lectureId).Select(x => x.Id).ToList();
            var lectureLecturers = Context.LectureLecturers.Where(x => x.LectureId == lectureId);
            Context.LectureLecturers.RemoveRange(lectureLecturers);
            return removedIds;
        }
    }
}
