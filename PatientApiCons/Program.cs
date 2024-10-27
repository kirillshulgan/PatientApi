using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PatientApiCons
{
    internal class Program
    {
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            Random rnd = new Random();

            for (int i = 0; i < 100; i++)
            {
                var patient = new
                {
                    name = new
                    {
                        use = i % 2 == 0 ? "official" : "unofficial",
                        family = $"Иванов_{i}",
                        given = new[] { $"Иван_{i}", $"Иванович_{i}" }
                    },
                    gender = rnd.Next(0,4),
                    birthDate = DateTime.UtcNow.AddDays(-i).ToString("yyyy-mm-dd"), // Дата рождения
                    active = rnd.Next(0, 2)
                };

                var json = JsonConvert.SerializeObject(patient);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Убедитесь, что URL соответствует вашему API
                var response = await client.PostAsync("http://localhost:5189/api/patient", content);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Запись {i + 1} создана.");
                }
                else
                {
                    Console.WriteLine($"Ошибка записи {i + 1}: {response.StatusCode}");
                }
            }
        }
    }
}
