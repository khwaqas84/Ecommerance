using ePizzaHub.Repositories.Interfaces;
using ePizzaHub.Services.Interfaces;

namespace ePizzaHub.Services.Implementations
{
    public class Service<TEntity> : IService<TEntity> where TEntity : class
    {
        private IRepository<TEntity> _repository;
        public Service(IRepository<TEntity> repository)
        {
            _repository = repository;


        }
        public void Add(TEntity entity)
        {
            _repository.Add(entity);
            _repository.SaveChanges();
        }

        public void Delete(object Id)
        {
            _repository.Delete(Id);
            _repository.SaveChanges();
        }

        public TEntity Find(object Id)
        {
           return _repository.Find(Id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _repository.GetAll();
        }

        public void Update(TEntity entity)
        {
             _repository.Update(entity);
            _repository.SaveChanges();
        }
    }
}
