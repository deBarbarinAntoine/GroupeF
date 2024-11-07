using System.Text.Json.Serialization;

namespace PagesJaunes.Models;

public class MigrateWorkingHours
{
    public DayOfWeek Day { get; }
    public TimeSpan StartTime { get; }
    public TimeSpan EndTime { get; }


    [JsonConstructor]
    public MigrateWorkingHours(DayOfWeek day, TimeSpan startTime, TimeSpan endTime)
    {
        Day = day;

        if (endTime >= startTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }
        else
        {
            EndTime = startTime;
            StartTime = endTime;
        }
    }

    public static List<WorkingHours> ToWorkingHours(List<MigrateWorkingHours> migrateWorkingHours)
    {
        List<WorkingHours> workingHours = [];
        workingHours.AddRange(
            migrateWorkingHours.Select(
                migrateWorkingHour => new WorkingHours(migrateWorkingHour.Day, migrateWorkingHour.StartTime.ToString(), migrateWorkingHour.EndTime.ToString())
                )
            );

        return workingHours;
    }
}