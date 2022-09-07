using System;

namespace SC.App.Services.Bill.Business.Repositories
{
    public class BaseRepository
    {
        protected readonly IServiceProvider ServiceProvider;

        public BaseRepository(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
    }
}