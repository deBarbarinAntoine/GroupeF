namespace YellowDirectory.Models;

public class WorkingHours
{
    public DayOfWeek Day { get; }
    public TimeSpan StartTime { get; }
    public TimeSpan EndTime { get; }
    public string StartTimeString { get; }
    public string EndTimeString { get; }

    public WorkingHours(DayOfWeek day, string startTimeString, string endTimeString)
    {
        Day = day;

        var startTime = TimeSpan.Parse(startTimeString);
        var endTime = TimeSpan.Parse(endTimeString);

        if (endTime >= startTime)
        {
            StartTimeString = startTimeString;
            EndTimeString = endTimeString;
            StartTime = startTime;
            EndTime = endTime;
        }
        else
        {
            EndTimeString = startTimeString;
            StartTimeString = endTimeString;
            EndTime = startTime;
            StartTime = endTime;
        }
    }

    public static List<WorkingHours> NewList(TimeSpan startTime, TimeSpan endTime)
    {
        List<WorkingHours> workingHours = [];
        foreach (DayOfWeek day in Enum.GetValues(typeof(DayOfWeek)))
        {
            if (day == DayOfWeek.Sunday)
                continue;
            workingHours.Add(new WorkingHours(day, startTime.ToString(), endTime.ToString()));
        }
        workingHours.Add(new WorkingHours(DayOfWeek.Sunday, startTime.ToString(), endTime.ToString()));

        return workingHours;
    }

    public static DayOfWeek ParseToDayOfWeek(string day)
    {
        if (Enum.IsDefined(typeof(DayOfWeek), day))
        {
            return (DayOfWeek)Enum.Parse(typeof(DayOfWeek), day, true);
        }

        throw new InvalidDataException($"Invalid data: {day}");
    }

    public override string ToString()
    {
        return Day + "," + StartTime + "," + EndTime;
    }
}