using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Domain;
public class Holiday
{
    
    public DateTime Date { get; set; }
    public string Name { get; set; }
    public bool IsHoliday { get; set; }
    
}
