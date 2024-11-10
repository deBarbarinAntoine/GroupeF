namespace YellowDirectory.Models;

/// <summary>
/// WorkingHours is the model associated to all ContactViewModels
/// for the WorkingHours attribute (business weekly schedule)
/// </summary>
public class WorkingHours
{
    public DayOfWeek Day { get; }
    public TimeSpan StartTime { get; }
    public TimeSpan EndTime { get; }
    public string StartTimeString { get; }
    public string EndTimeString { get; }

    /// <summary>
    /// Basic WorkingHours constructor taking TimeStamp-parsed strings
    /// for the opening and closing hours.
    /// </summary>
    /// <param name="day">the day of the week</param>
    /// <param name="startTimeString">the opening time in 'HH:mm:ss' format</param>
    /// <param name="endTimeString">the closing time in 'HH:mm:ss' format</param>
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

    /// <summary>
    /// Static small factory that creates a list of WorkingHours
    /// with the same startTime and endTime each day of the week.
    /// </summary>
    /// <param name="startTime">the daily opening time.</param>
    /// <param name="endTime">the daily closing time.</param>
    /// <returns>the list of WorkingHours.</returns>
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

    /// <summary>
    /// Static function that parses a string representing a day of the week
    /// into the corresponding entry in the DayOfWeek enum.
    /// </summary>
    /// <param name="day">the string representing a day of the week.</param>
    /// <returns>the corresponding DayOfWeek entry.</returns>
    /// <exception cref="InvalidDataException">
    /// when the string representing the day doesn't match any entry of the DayOfWeek enum.
    /// </exception>
    public static DayOfWeek ParseToDayOfWeek(string day)
    {
        if (Enum.IsDefined(typeof(DayOfWeek), day))
        {
            return (DayOfWeek)Enum.Parse(typeof(DayOfWeek), day, true);
        }

        throw new InvalidDataException($"Invalid data: {day}");
    }

    /// <summary>
    /// ToString implementation to convert the WorkingHours
    /// to a simple CSV string used in the database.
    /// </summary>
    /// <returns>the string representing the WorkingHours in the CSV format</returns>
    public override string ToString()
    {
        return Day + "," + StartTime + "," + EndTime;
    }
}