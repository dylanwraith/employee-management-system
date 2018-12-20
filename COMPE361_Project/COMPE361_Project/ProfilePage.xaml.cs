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
    public sealed partial class ProfilePage : Page
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var currentEmployee = (ProgramParams)e.Parameter;

            employee = (ProgramParams)currentEmployee;

            employeeData = employee.FoundEmployee;

            Welcome.Text = $"Welcome Back {currentEmployee.FoundEmployee.FirstName} {currentEmployee.FoundEmployee.LastName}";
            if (currentEmployee.FoundEmployee.IsAdmin)
                Position.Text = $"Admin";
            else if (currentEmployee.FoundEmployee.IsManager)
                Position.Text = $"Manager";
            else
                Position.Text = $"General Employee";
            /*
            var employeeSend = new ProgramParams();
            employeeSend = employee;
            this.Frame.Navigate(typeof(LoginPage), employeeSend);
            */
        }
        ProgramParams employee = new ProgramParams();
        Employee employeeData = new Employee();
        public ProfilePage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }
    }
}
