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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace COMPE361_Project
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewSchedule : Page
    {
        Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        Windows.Storage.StorageFile employeeFile;
        public Employee receivedEmployee;
        public ViewSchedule()
        {
            this.InitializeComponent();
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var currentEmployee = (ProgramParams)e.Parameter;

            receivedEmployee = currentEmployee.FoundEmployee;
            try
            {
                for (int i = 0; i < receivedEmployee.ScheduleStart.Length; i++)
                {
                    EmployeeSchedule.Items.Add(new ListViewItem { Content = receivedEmployee.ScheduleDate[i] + " " + receivedEmployee.ScheduleStart[i] + " " + receivedEmployee.ScheduleEnd[i] + '\n' });
                }
            }
            catch {
                EmployeeSchedule.Items.Add(new ListViewItem { Content = "No schedule found." });
            }
        }
    }
}
