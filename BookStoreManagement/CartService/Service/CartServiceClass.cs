using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using CartService.Entity;
using System.Net.Http;
using System.Text.Json;

namespace CartService.Service
{
    public class CartServiceClass
    {
        private readonly string _connectionString;
        private readonly HttpClient _httpClient;

        public CartServiceClass(IConfiguration configuration, HttpClient httpClient)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _httpClient = httpClient;
        }

        private async Task<bool> BookExists(int bookId)
        {
            var response = await _httpClient.GetAsync($"http://bookservice/api/books/{bookId}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<int>> GetCartItems(int customerId)
        {
            var books = new List<int>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT BookId FROM Cart WHERE CustomerId = @CustomerId";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customerId);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            books.Add(reader.GetInt32("BookId"));
                        }
                    }
                }
            }
            return books;
        }

        public async Task<bool> AddToCart(int customerId, int bookId, int quantity)
        {
            if (!await BookExists(bookId))
                return false;

            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "INSERT INTO Cart (CustomerId, BookId, Quantity) VALUES (@CustomerId, @BookId, @Quantity)";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    command.Parameters.AddWithValue("@BookId", bookId);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    return await command.ExecuteNonQueryAsync() > 0;
                }
            }
        }

        public async Task<bool> RemoveFromCart(int customerId, int bookId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "DELETE FROM Cart WHERE CustomerId = @CustomerId AND BookId = @BookId";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    command.Parameters.AddWithValue("@BookId", bookId);
                    return await command.ExecuteNonQueryAsync() > 0;
                }
            }
        }
    }
}
