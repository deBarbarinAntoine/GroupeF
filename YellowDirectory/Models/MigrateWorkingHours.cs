using System.Text.Json.Serialization;

namespace YellowDirectory.Models;

/// <summary>
/// MigrateWorkingHours is the WorkingHours model
/// associated to the MigrateContactViewModel for the migrations.
/// </summary>
public class MigrateWorkingHours
{
    public DayOfWeek Day { get; }
    public TimeSpan StartTime { get; }
    public TimeSpan EndTime { get; }


    /// <summary>
    /// Basic constructor used deserializing from JSON format.
    /// </summary>
    /// <param name="day">the day of the week</param>
    /// <param name="startTime">the opening hour</param>
    /// <param name="endTime">the closing hour</param>
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

    /// <summary>
    /// Statically converts a list of MigrateWorkingHours to a list of WorkingHours.
    /// </summary>
    /// <param name="migrateWorkingHours">the MigrateWorkingHours to convert.</param>
    /// <returns>the converted list of WorkingHours.</returns>
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