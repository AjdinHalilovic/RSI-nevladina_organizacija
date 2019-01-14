using Core.Entities.Base;
namespace DAL.Repositories.Base.IRepository
{
    public interface IPersonPhotosRepository:IRepository<PersonPhoto,int>
    {
        PersonPhoto GetByPersonId(int Id);
        byte[] GetByPersonId(int pPersonId, bool pIsDeleted = false);
    }
}
