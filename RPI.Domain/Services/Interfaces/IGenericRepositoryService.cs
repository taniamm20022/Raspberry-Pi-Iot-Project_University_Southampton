using CSDI.Data.Repositories.Interfaces;


namespace CSDI.Data.Services.Interfaces
{
    public interface IGenericRepositoryService<TEntity> :
        IGenericService, IGenericRepository<TEntity>
        where TEntity : class
    {
    }
}
