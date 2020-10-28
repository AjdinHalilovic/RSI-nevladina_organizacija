using Core.Entities.Base;
using System.Collections.Generic;

namespace DAL.Repositories.Base.IRepository
{
    public interface ILectureLecturersRepository:IRepository<LectureLecturer,int>
    {
        List<int> RemoveByLectureId(int lectureId);
        IEnumerable<LectureLecturer> GetByLectureId(int Id);
        List<int> Remove(int lectureId);
    }
}
