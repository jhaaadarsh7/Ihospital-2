using System.Net.Http.Json;
using System.Text.Json;

namespace Ihospital.Web.Services
{
    public interface IApiService
    {
        Task<(bool success, string message)> SubmitSurveyAsync(object surveyData);
        Task<List<T>> GetSurveyResponsesAsync<T>();
        Task<List<QuestionDto>> GetQuestionsAsync();
    }

    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(bool success, string message)> SubmitSurveyAsync(object surveyData)
        {
            try
            {
                // Use relative path - HttpClient will combine with BaseAddress correctly
                var endpoint = "Survey/submit";
                Console.WriteLine($"BaseAddress: {_httpClient.BaseAddress}");
                Console.WriteLine($"Endpoint: {endpoint}");
                Console.WriteLine($"Survey Data: {JsonSerializer.Serialize(surveyData)}");

                var response = await _httpClient.PostAsJsonAsync(endpoint, surveyData);

                Console.WriteLine($"Response Status Code: {(int)response.StatusCode} {response.ReasonPhrase}");
                Console.WriteLine($"Is Success: {response.IsSuccessStatusCode}");

                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Content: {responseContent}");
                Console.WriteLine($"Request Uri: {response.RequestMessage?.RequestUri}");

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        using var doc = JsonDocument.Parse(responseContent);
                        if (doc.RootElement.TryGetProperty("message", out var msg))
                        {
                            return (true, msg.GetString() ?? "Survey submitted successfully");
                        }
                    }
                    catch { }

                    return (true, "Survey submitted successfully");
                }

                return (false, $"Server error: {(int)response.StatusCode} {response.ReasonPhrase} - {responseContent}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return (false, $"Connection error: {ex.Message}");
            }
        }

        public async Task<List<T>> GetSurveyResponsesAsync<T>()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<T>>("survey/responses");
                return response ?? new List<T>();
            }
            catch
            {
                return new List<T>();
            }
        }

        public async Task<List<QuestionDto>> GetQuestionsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<QuestionDto>>("questions");
                return response ?? new List<QuestionDto>();
            }
            catch
            {
                return new List<QuestionDto>();
            }
        }
    }
}
