namespace PagesJaunes.Models;

public class WorkingHours
{
    public DayOfWeek Day { get; }
    public TimeSpan StartTime { get; }
    public TimeSpan EndTime { get; }

    public WorkingHours(DayOfWeek day, TimeSpan startTime, TimeSpan endTime)
    {
        Day = day;

        if (EndTime < StartTime)
        {
            StartTime = endTime;
            EndTime = startTime;
        }
        else
        {
            StartTime = startTime;
            EndTime = endTime;
        }
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