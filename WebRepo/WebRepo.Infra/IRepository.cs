namespace WebRepo.Infra
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Get();
        TEntity GetByID(int entityId);
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(int entityID);
        bool Exists(int entityId);
        void Save();
    }
}
