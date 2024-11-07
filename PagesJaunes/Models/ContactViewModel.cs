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
    public string MondayStartTime { get; set; }
    public string MondayEndTime { get; set; }
    public string TuesdayStartTime { get; set; }
    public string TuesdayEndTime { get; set; }
    public string WednesdayStartTime { get; set; }
    public string WednesdayEndTime { get; set; }
    public string ThursdayStartTime { get; set; }
    public string ThursdayEndTime { get; set; }
    public string FridayStartTime { get; set; }
    public string FridayEndTime { get; set; }
    public string SaturdayStartTime { get; set; }
    public string SaturdayEndTime { get; set; }
    public string SundayStartTime { get; set; }
    public string SundayEndTime { get; set; }

    public void SetWorkingHours()
    {
        WorkingHours.Add(new WorkingHours(DayOfWeek.Monday, MondayStartTime, MondayEndTime));
        WorkingHours.Add(new WorkingHours(DayOfWeek.Tuesday, TuesdayStartTime, TuesdayEndTime));
        WorkingHours.Add(new WorkingHours(DayOfWeek.Wednesday, WednesdayStartTime, WednesdayEndTime));
        WorkingHours.Add(new WorkingHours(DayOfWeek.Thursday, ThursdayStartTime, ThursdayEndTime));
        WorkingHours.Add(new WorkingHours(DayOfWeek.Friday, FridayStartTime, FridayEndTime));
        WorkingHours.Add(new WorkingHours(DayOfWeek.Saturday, SaturdayStartTime, SaturdayEndTime));
        WorkingHours.Add(new WorkingHours(DayOfWeek.Sunday, SundayStartTime, SundayEndTime));
    }

    public void SetStaticWorkingHours()
    {
        MondayStartTime = WorkingHours[0].StartTimeString;
        MondayEndTime = WorkingHours[0].EndTimeString;

        TuesdayStartTime = WorkingHours[1].StartTimeString;
        TuesdayEndTime = WorkingHours[1].EndTimeString;

        WednesdayStartTime = WorkingHours[2].StartTimeString;
        WednesdayEndTime = WorkingHours[2].EndTimeString;

        ThursdayStartTime = WorkingHours[3].StartTimeString;
        ThursdayEndTime = WorkingHours[3].EndTimeString;

        FridayStartTime = WorkingHours[4].StartTimeString;
        FridayEndTime = WorkingHours[4].EndTimeString;

        SaturdayStartTime = WorkingHours[5].StartTimeString;
        SaturdayEndTime = WorkingHours[5].EndTimeString;

        SundayStartTime = WorkingHours[6].StartTimeString;
        SundayEndTime = WorkingHours[6].EndTimeString;
    }

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
            string start = data[1];
            string end = data[2];

            parsedList.Add(new WorkingHours(day, start, end));
        }

        return parsedList;
    }

    public string GetCurrentState()
    {

        var currentTime = DateTime.Now;

        foreach (var workingHour in WorkingHours.Where(workingHour => workingHour.Day == currentTime.DayOfWeek))
        {
            if (workingHour.StartTime < currentTime.TimeOfDay && currentTime.TimeOfDay < workingHour.EndTime)
            {
                if (workingHour.EndTime - currentTime.TimeOfDay >= TimeSpan.FromHours(1))
                    return $"Open till {workingHour.EndTime.Hours:00}:{workingHour.EndTime.Minutes:00}";
                var timeLeft = workingHour.EndTime - currentTime.TimeOfDay;
                return $"Closes in {timeLeft.Minutes} minutes";
            }
        }
        return "Closed";
    }
}