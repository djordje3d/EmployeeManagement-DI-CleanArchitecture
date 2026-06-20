namespace Zaposleni_Clean_MVC_API.Data
{
    public interface IWebApiExecuter 
    {
        Task InvokeDelete(string relativeUrl);
        Task<TResponse?> InvokeDelete<TResponse>(string relativeUrl);
        Task<T> InvokeGet<T>(string relativeUrl);
        Task<T?> InvokePost<T>(string relativeUrl, T obj);
        Task<TResponse?> InvokePost<TRequest, TResponse>(string relativeUrl, TRequest obj);
        Task InvokePut<T>(string relativeUrl, T obj);
        Task<TResponse?> InvokePut<TRequest, TResponse>(string relativeUrl, TRequest obj);

        // Dodajte podršku za header-e
        void SetAuthorizationHeader(HttpClient httpClient);
    }
}
