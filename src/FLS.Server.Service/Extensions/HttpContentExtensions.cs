using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FLS.Server.Service.Extensions
{
    /// <summary>
    /// <seealso cref="https://github.com/bolorundurowb/aspnetcore-httpclientextensions/blob/master/AspNetCore.Http.Extensions/HttpClientExtensions.cs"/>
    /// </summary>
    public static class HttpContentExtensions
    {
        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            var dataAsString = await content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(dataAsString);
        }
    }
}
