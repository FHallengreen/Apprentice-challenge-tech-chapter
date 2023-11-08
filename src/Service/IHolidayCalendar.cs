using Domain;
using Shared;

namespace Service;

public interface IHolidayCalendar
{
    Task<bool> IsHoliday(DateTime date);
    
    Task<ICollection<HolidayResponse>> GetHolidays(DateTime startDate, DateTime endDate);
    
}
