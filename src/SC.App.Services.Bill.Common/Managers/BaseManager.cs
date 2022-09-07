using SC.App.Services.Bill.Common.Repositories;

namespace SC.App.Services.Bill.Common.Managers
{
    public class BaseManager<T> where T : IRepository
    {
        protected T Repository { get; private set; }

        public BaseManager(T repository)
        {
            Repository = repository;
        }
    }
}