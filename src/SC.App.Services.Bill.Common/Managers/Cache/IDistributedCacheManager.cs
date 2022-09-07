namespace SC.App.Services.Bill.Common.Managers.Cache
{
    public interface IDistributedCacheManager
    {
        string Get(string key);

        T Get<T>(string key) where T : new();

        void Set(string key, string value);

        void Set(string key, string value, int cacheTimeInSeconds);

        void Set<T>(string key, T value) where T : new();

        void Set<T>(string key, T value, int cacheTimeInSeconds) where T : new();

        void Remove(string key);
    }
}