using Domain;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using Shared;

namespace Service;

public class HolidayCalendar : IHolidayCalendar
{
    private readonly string _apiToken;
    private readonly HttpClient _httpClient;
    private readonly string apiBaseUrl = "https://api.sallinggroup.com/v1/holidays/";

    public HolidayCalendar(IConfiguration configuration)
    {
        _httpClient = new HttpClient();

        _apiToken = configuration["SALLING_GROUP_API_TOKEN"];
        if (_apiToken is null)
        {
            throw new MissingFieldException("API token is not set in the environment variables.");
        }

        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiToken);
    }

    public async Task<bool> IsHoliday(DateTime date)
    {
        var url = $"{apiBaseUrl}is-holiday?date={date:yyyy-MM-dd}";

        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException("An error occurred while calling the API.");
        }

        var content = await response.Content.ReadAsStringAsync();

        return JsonConvert.DeserializeObject<bool>(content);
    }

    
    public async Task<ICollection<HolidayResponse>> GetHolidays(DateTime startDate, DateTime endDate)
    {
        var url = $"{apiBaseUrl}?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException("An error occurred while calling the API.");
        }

        var content = await response.Content.ReadAsStringAsync();

        var holidays = JsonConvert.DeserializeObject<List<Holiday>>(content)!;
        var holidayDtos = holidays.Select(h => new HolidayResponse
        {
            Date = h.Date.ToShortDateString(),
            Name = h.Name,
        }).ToList();
        return holidayDtos;
    }
}