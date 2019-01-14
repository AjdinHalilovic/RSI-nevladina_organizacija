using Core.Entities.Base;
using DAL.Contexts;
using DAL.Repositories.Base.IRepository;
using System.Linq;

namespace DAL.Repositories.Base.Repository
{
    public class PersonPhotosRepository:Repository<PersonPhoto,int>,IPersonPhotosRepository
    {
        public PersonPhotosRepository(NevladinaOrgContext context) : base(context) { }

        public PersonPhoto GetByPersonId(int Id)
        {
            return Context.PersonPhotos.FirstOrDefault(x => x.PersonId == Id);
        }
        public byte[] GetByPersonId(int pPersonId, bool pIsDeleted = false)
        {
            var sql = "SELECT \"PP\".\"Photo\"\r\nFROM \"PersonPhotos\" AS \"PP\"\r\nWHERE \"PP\".\"PersonId\" = @pPersonId AND \"PP\".\"IsDeleted\" = @pIsDeleted\nLIMIT 1;";

            return DbConnection.Query<byte[]>(sql, new { pPersonId, pIsDeleted }).SingleOrDefault();
        }
    }
}
