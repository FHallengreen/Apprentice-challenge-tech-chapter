using Microsoft.AspNetCore.Mvc;
using Service;

namespace WebHoliday;

[ApiController]
[Route("holidays")]
public class HolidaysController : ControllerBase
{
    private readonly HolidayCalendar _holidayCalendar;
    
    public HolidaysController(HolidayCalendar holidayCalendar)
    {
        _holidayCalendar = holidayCalendar;
    }

    [HttpGet("is-holiday/{date}")]
    public async Task<ActionResult> IsHoliday(DateTime date)
    {
        try
        {
            var isHoliday = await _holidayCalendar.IsHoliday(date);
            return Ok(isHoliday);
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("get-holidays")]
    public async Task<ActionResult> GetHolidays(DateTime startDate,DateTime endDate)
    {
        try
        {
            var holidays = await _holidayCalendar.GetHolidays(startDate, endDate);
            if (!holidays.Any())
            {
                return NoContent();
            }

            return Ok("Danish national holidays between: " + string.Join(", ", holidays.Select(h => h.Name)));
        }
        catch (HttpRequestException ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}