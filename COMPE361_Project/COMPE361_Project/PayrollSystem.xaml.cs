using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238



namespace COMPE361_Project
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class PayrollSystem : Page
    {
        ProgramParams employee = new ProgramParams();
        public PayrollSystem()
        {
            this.InitializeComponent();
        }
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            Display.IsPaneOpen = true;
        }
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            var sendEmployee = new ProgramParams();
            sendEmployee.FoundEmployee = employee.FoundEmployee;
            Content.Navigate(typeof(ProfilePage), sendEmployee);
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var sendEmployee = new ProgramParams();
            sendEmployee.FoundEmployee = employee.FoundEmployee;
            this.Frame.Navigate(typeof(LoginPage), sendEmployee);
        }
        private void Clock_Click(object sender, RoutedEventArgs e)
        {
            var sendEmployee = new ProgramParams();
            sendEmployee.FoundEmployee = employee.FoundEmployee;
            if (employee.FoundEmployee.IsAdmin) Content.Navigate(typeof(ClockLogs), sendEmployee);
            else Content.Navigate(typeof(EmployeeClock), sendEmployee);
        }
        private void Schedule_Click(object sender, RoutedEventArgs e)
        {
            var sendEmployee = new ProgramParams();
            sendEmployee.FoundEmployee = employee.FoundEmployee;
            if (employee.FoundEmployee.IsManager || employee.FoundEmployee.IsAdmin) Content.Navigate(typeof(EditSchedule), sendEmployee);
            else Content.Navigate(typeof(ViewSchedule), sendEmployee);
        }
        private void PTO_Click(object sender, RoutedEventArgs e)
        {
            var sendEmployee = new ProgramParams();
            sendEmployee.FoundEmployee = employee.FoundEmployee;
            if (employee.FoundEmployee.IsAdmin || employee.FoundEmployee.IsManager) Content.Navigate(typeof(ManagePTO), sendEmployee);
            else Content.Navigate(typeof(PTORequest), sendEmployee);
        }
        private void Manage_Employee(object sender, RoutedEventArgs e)
        {
            var sendEmployee = new ProgramParams();
            sendEmployee.FoundEmployee = employee.FoundEmployee;
            Content.Navigate(typeof(EmployeeList), sendEmployee);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //Receive employee
            base.OnNavigatedTo(e);
            var currentEmployee = (ProgramParams)e.Parameter;

            //Update global variable
            employee = currentEmployee;

            Content.Navigate(typeof(ProfilePage), employee);

            //Update menu for employee
            if (currentEmployee.FoundEmployee.IsAdmin) Manage_Employees.Visibility = Visibility.Visible;
            else Manage_Employees.Visibility = Visibility.Collapsed;
            if (currentEmployee.FoundEmployee.IsAdmin)  Clock_Title.Content = Clock_Title.Content = "Clock Logs";
            else   Clock_Title.Content = "Clock In/Out";
            if (currentEmployee.FoundEmployee.IsAdmin|| currentEmployee.FoundEmployee.IsManager)
            {
                Calendar_Title.Content = "Edit Schedule";
                PTO_Title.Content = "Manage PTO";
            }
            else
            {
                Calendar_Title.Content = "View Schedule";
                PTO_Title.Content = "PTO Request";
            }
        }
    }
}
