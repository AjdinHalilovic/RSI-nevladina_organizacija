using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class LecturesRepository:Repository<Lecture,int>,ILecturesRepository
    {
        public LecturesRepository(NevladinaOrgContext context) : base(context) { }
        public int? Remove(int eventItemId)
        {
            var lecture = Context.Lectures.FirstOrDefault(x => x.Id == eventItemId);
            if (lecture != null)
            {
                Context.Lectures.Remove(lecture);
                return lecture.Id;
            }
            return null;
        }
    }
}
