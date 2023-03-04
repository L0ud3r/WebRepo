using Microsoft.EntityFrameworkCore;
using WebRepo.DAL.Default;
using WebRepo.DAL;

namespace WebRepo.Infra
{
    public class Repository<TEntity, TContext>: IRepository<TEntity> where TEntity : DefaultEntity where TContext : DbContext
    {
        private TContext _context { get; set; }

        public Repository(TContext context)
        {
            this._context = context;
        }

        public IQueryable<TEntity> Get()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public TEntity GetByID(int id) 
        {
            return _context.Set<TEntity>().Where(x => x.Id == id).SingleOrDefault();
        }

        public void Insert(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
        
        public void Delete(int id)
        {
            var entity = _context.Set<TEntity>().AsQueryable().Where(x => x.Id == id).SingleOrDefault();

            if (entity != null)
                _context.Remove(entity);
        }

        public bool Exists(int id)
        {
            return _context.Set<TEntity>().Where(x => x.Id == id).Any();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
