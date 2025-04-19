using FoodOrder.Pedidos.Application.DTOs.Produto;
using FoodOrder.Pedidos.Application.Interfaces;
using Polly;
using Polly.Retry;
using System.Net.Http.Json;

namespace FoodOrder.Pedidos.Application.Service
{
    public class ProdutoHttpService : IProdutoHttpService
    {
        private readonly HttpClient _httpClient;
        private readonly AsyncRetryPolicy _retryPolicy;

        public ProdutoHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            _retryPolicy = Policy
                .Handle<HttpRequestException>()
                .Or<TaskCanceledException>() // Timeout
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        public async Task<List<ProdutoOutput>> ObterProdutosAsync()
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                var response = await _httpClient.GetAsync("produtos"); // endpoint da API externa

                response.EnsureSuccessStatusCode();

                var produtos = await response.Content.ReadFromJsonAsync<List<ProdutoOutput>>();
                return produtos ?? new List<ProdutoOutput>();
            });
        }

        public async Task<ProdutoDto?> ObterProdutoPorIdAsync(int id)
        {
            return await _retryPolicy.ExecuteAsync(async () =>
            {
                var response = await _httpClient.GetAsync($"Produtos/{id}");
                if (!response.IsSuccessStatusCode) return null;

                return await response.Content.ReadFromJsonAsync<ProdutoDto>();
            });
        }
    }
}
