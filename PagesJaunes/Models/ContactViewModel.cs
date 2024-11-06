namespace PagesJaunes.Models;

public class ContactViewModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string Street { get; set; }
    public string ZipCode { get; set; }
    public List<WorkingHours> WorkingHours { get; set; } = new(7);

    public static List<string> ParseToList(List<WorkingHours> workingHours)
    {
        List<string> strList = new List<string>();
        foreach (var workingHour in workingHours)
        {
            strList.Add(workingHour.ToString());
        }

        return strList;
    }

    public static List<WorkingHours> ParseToWorkingHours(List<string> workingHours)
    {
        List<WorkingHours> parsedList = new List<WorkingHours>();
        foreach (var workingHour in workingHours)
        {
            var data = workingHour.Split(',');
            DayOfWeek day = Models.WorkingHours.ParseToDayOfWeek(data[0]);
            TimeSpan start = TimeSpan.Parse(data[1]);
            TimeSpan end = TimeSpan.Parse(data[2]);

            parsedList.Add(new WorkingHours(day, start, end));
        }

        return parsedList;
    }
}