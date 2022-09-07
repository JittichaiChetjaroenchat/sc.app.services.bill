namespace SC.App.Services.Bill.Client
{
    public interface IBaseHttpClient
    {
        void SetAuthorization(string authorization);

        void SetAcceptLanguage(string acceptLanguage);
    }
}