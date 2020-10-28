using System.Threading.Tasks;

namespace DAL
{
    public interface IDataUnitOfWork
    {
        IBaseUnitOfWork BaseUow { get; }
    }
}
