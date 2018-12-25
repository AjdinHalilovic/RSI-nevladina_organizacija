using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace DAL
{
    public class DataUnitOfWork : IDataUnitOfWork
    {
        #region Contexts
        private readonly NevladinaOrgContext _nevladinaOrgContext;
        #endregion

        public DataUnitOfWork(NevladinaOrgContext nevladinaOrgContext)
        {
            _nevladinaOrgContext = nevladinaOrgContext;
        }
        
        #region Uow's
        private IBaseUnitOfWork _baseUnitOfWork;
        public IBaseUnitOfWork BaseUow => _baseUnitOfWork = _baseUnitOfWork ?? new BaseUnitOfWork(_nevladinaOrgContext);
        #endregion

 
    }
}
