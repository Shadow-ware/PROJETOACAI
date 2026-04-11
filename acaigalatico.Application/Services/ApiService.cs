using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using acaigalatico.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace acaigalatico.Application.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiService> _logger;

        public ApiService(HttpClient httpClient, ILogger<ApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<dynamic>> GetPostsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/posts");

                if (response.IsSuccessStatusCode)
                {
                    var posts = await response.Content.ReadFromJsonAsync<IEnumerable<dynamic>>();
                    return posts ?? new List<dynamic>();
                }

                _logger.LogWarning($"Erro ao buscar posts: {response.StatusCode}");
                return new List<dynamic>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro na chamada GET para jsonplaceholder");
                throw;
            }
        }

        public async Task<dynamic> CreatePostAsync(object postData)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://jsonplaceholder.typicode.com/posts", postData);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<dynamic>();
                    return result;
                }

                _logger.LogWarning($"Erro ao criar post: {response.StatusCode}");
                throw new Exception($"Erro na API: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro na chamada POST para jsonplaceholder");
                throw;
            }
        }
    }
}
