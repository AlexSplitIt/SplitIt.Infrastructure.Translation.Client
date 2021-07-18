using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace SplitIt.Infrastructure.Translation.Client
{
    public interface ITranslationClientFactory
    {
        TClient CreateClient<TClient>(string authToken = null, IDictionary<string, string> requestHeaders = null)
            where TClient : ITranslationClientBase;
    }

    public class TranslationClientFactory : ITranslationClientFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _baseUrl;

        public TranslationClientFactory(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _baseUrl = configuration[TranslationConfigurationKeys.BASE_URL];
        }

        public TClient CreateClient<TClient>(string authToken = null, IDictionary<string, string> requestHeaders = null)
            where TClient : ITranslationClientBase
        {
            var instanceType = typeof(TranslationClientFactory).Assembly
                .GetTypes()
                .FirstOrDefault(p => p.GetInterfaces()
                    .Any(i => i == typeof(TClient)));

            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(_baseUrl);

            if (!string.IsNullOrWhiteSpace(authToken))
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
            }

            if (requestHeaders != null)
            {
                foreach (var entry in requestHeaders)
                {
                    httpClient.DefaultRequestHeaders.Add(entry.Key, entry.Value);
                }
            }

            return (TClient) Activator.CreateInstance(instanceType, httpClient);
        }
    }
}
