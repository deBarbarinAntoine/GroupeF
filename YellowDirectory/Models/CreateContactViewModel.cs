namespace YellowDirectory.Models;

public class CreateContactViewModel
{
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

    public static CreateContactViewModel Empty()
    {
        var viewModel = new CreateContactViewModel();
        viewModel.Name = string.Empty;
        viewModel.Email = string.Empty;
        viewModel.Phone = string.Empty;
        viewModel.Country = string.Empty;
        viewModel.City = string.Empty;
        viewModel.Street = string.Empty;
        viewModel.ZipCode = string.Empty;

        viewModel.MondayStartTime = TimeSpan.FromHours(7).ToString();
        viewModel.MondayEndTime = TimeSpan.FromHours(19).ToString();

        viewModel.TuesdayStartTime = TimeSpan.FromHours(7).ToString();
        viewModel.TuesdayEndTime = TimeSpan.FromHours(19).ToString();

        viewModel.WednesdayStartTime = TimeSpan.FromHours(7).ToString();
        viewModel.WednesdayEndTime = TimeSpan.FromHours(19).ToString();

        viewModel.ThursdayStartTime = TimeSpan.FromHours(7).ToString();
        viewModel.ThursdayEndTime = TimeSpan.FromHours(19).ToString();

        viewModel.FridayStartTime = TimeSpan.FromHours(7).ToString();
        viewModel.FridayEndTime = TimeSpan.FromHours(19).ToString();

        viewModel.SaturdayStartTime = TimeSpan.FromHours(7).ToString();
        viewModel.SaturdayEndTime = TimeSpan.FromHours(19).ToString();

        viewModel.SundayStartTime = TimeSpan.FromHours(7).ToString();
        viewModel.SundayEndTime = TimeSpan.FromHours(19).ToString();

        return viewModel;
    }

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
}