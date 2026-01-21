using CommunityToolkit.Mvvm.ComponentModel;

namespace SongbookManagerMaui.Controls;

public partial class DateControl : ContentView
{
    #region Properties
    public DateTime Date
    {
        get { return (DateTime)GetValue(DateProperty); }
        set { SetValue(DateProperty, value); }
    }

    public static readonly BindableProperty DateProperty =
        BindableProperty.Create("Date", typeof(DateTime), typeof(DateControl), DateTime.Today);
    #endregion

    #region Constructor
    public DateControl()
	{
		InitializeComponent();

        LoadDates();
    }
    #endregion

    #region Methods
    private void LoadDates()
    {
        DateTime today = DateTime.Now;

        if (today.DayOfWeek.Equals(DayOfWeek.Monday))
        {
            MondayRadio.Content = today;
            TuesdayRadio.Content = today.AddDays(1);
            WednesdayRadio.Content = today.AddDays(2);
            ThursdayRadio.Content = today.AddDays(3);
            FridayRadio.Content = today.AddDays(4);
            SaturdayRadio.Content = today.AddDays(5);
            SundayRadio.Content = today.AddDays(6);

            MondayRadio.IsChecked = true;
        }
        else if (today.DayOfWeek.Equals(DayOfWeek.Tuesday))
        {
            MondayRadio.Content = today.AddDays(-1);
            TuesdayRadio.Content = today;
            WednesdayRadio.Content = today.AddDays(1);
            ThursdayRadio.Content = today.AddDays(2);
            FridayRadio.Content = today.AddDays(3);
            SaturdayRadio.Content = today.AddDays(4);
            SundayRadio.Content = today.AddDays(5);

            TuesdayRadio.IsChecked = true;
        }
        else if (today.DayOfWeek.Equals(DayOfWeek.Wednesday))
        {
            MondayRadio.Content = today.AddDays(-2);
            TuesdayRadio.Content = today.AddDays(-1);
            WednesdayRadio.Content = today;
            ThursdayRadio.Content = today.AddDays(1);
            FridayRadio.Content = today.AddDays(2);
            SaturdayRadio.Content = today.AddDays(3);
            SundayRadio.Content = today.AddDays(4);

            WednesdayRadio.IsChecked = true;
        }
        else if (today.DayOfWeek.Equals(DayOfWeek.Thursday))
        {
            MondayRadio.Content = today.AddDays(-3);
            TuesdayRadio.Content = today.AddDays(-2);
            WednesdayRadio.Content = today.AddDays(-1);
            ThursdayRadio.Content = today;
            FridayRadio.Content = today.AddDays(1);
            SaturdayRadio.Content = today.AddDays(2);
            SundayRadio.Content = today.AddDays(3);

            ThursdayRadio.IsChecked = true;
        }
        else if (today.DayOfWeek.Equals(DayOfWeek.Friday))
        {
            MondayRadio.Content = today.AddDays(-4);
            TuesdayRadio.Content = today.AddDays(-3);
            WednesdayRadio.Content = today.AddDays(-2);
            ThursdayRadio.Content = today.AddDays(-1);
            FridayRadio.Content = today;
            SaturdayRadio.Content = today.AddDays(1);
            SundayRadio.Content = today.AddDays(2);

            FridayRadio.IsChecked = true;
        }
        else if (today.DayOfWeek.Equals(DayOfWeek.Saturday))
        {
            MondayRadio.Content = today.AddDays(-5);
            TuesdayRadio.Content = today.AddDays(-4);
            WednesdayRadio.Content = today.AddDays(-3);
            ThursdayRadio.Content = today.AddDays(-2);
            FridayRadio.Content = today.AddDays(-1);
            SaturdayRadio.Content = today;
            SundayRadio.Content = today.AddDays(1);

            SaturdayRadio.IsChecked = true;
        }
        else
        {
            MondayRadio.Content = today.AddDays(-6);
            TuesdayRadio.Content = today.AddDays(-5);
            WednesdayRadio.Content = today.AddDays(-4);
            ThursdayRadio.Content = today.AddDays(-3);
            FridayRadio.Content = today.AddDays(-2);
            SaturdayRadio.Content = today.AddDays(-1);
            SundayRadio.Content = today;

            SundayRadio.IsChecked = true;
        }
    }

    private void MondayRadio_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (MondayRadio is not null)
        {
            Date = (DateTime)MondayRadio.Content;
        }
    }

    private void TuesdayRadio_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (TuesdayRadio is not null)
        {
            Date = (DateTime)TuesdayRadio.Content;
        }
    }

    private void WednesdayRadio_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (WednesdayRadio is not null)
        {
            Date = (DateTime)WednesdayRadio.Content;
        }
    }

    private void ThursdayRadio_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (ThursdayRadio is not null)
        {
            Date = (DateTime)ThursdayRadio.Content;
        }
    }

    private void FridayRadio_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (FridayRadio is not null)
        {
            Date = (DateTime)FridayRadio.Content;
        }
    }

    private void SaturdayRadio_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (SaturdayRadio is not null)
        {
            Date = (DateTime)SaturdayRadio.Content;
        }
    }

    private void SundayRadio_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (SundayRadio is not null)
        {
            Date = (DateTime)SundayRadio.Content;
        }
    }
    #endregion
}