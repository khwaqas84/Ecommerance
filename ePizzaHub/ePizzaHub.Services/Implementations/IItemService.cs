using ePizzaHub.Core.Entities;
using ePizzaHub.Models;
using ePizzaHub.Repositories.Interfaces;
using ePizzaHub.Services.Interfaces;


namespace ePizzaHub.Services.Implementations
{
    public class ItemService : Service<Item>, IItemService
    {
        IRepository<Item> _repository;
        public ItemService(IRepository<Item> repository) : base(repository)
        {
            _repository= repository;
        }

        public IEnumerable<ItemModel> GetItems()
        {
           var data=_repository.GetAll().Select(i=>new ItemModel 
           { 
               Id = i.Id,   
               Name = i.Name,
               CategoryId = i.CategoryId,
               Description = i.Description,
               ImageUrl = i.ImageUrl,
               ItemTypeId = i.ItemTypeId,
               UnitPrice    = i.UnitPrice                    
           });

            return data;
        }
    }


}
