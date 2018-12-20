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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace COMPE361_Project
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PTORequest : Page
    {
        private Employee currentEmployee;
        public PTORequest()
        {
            this.InitializeComponent();
            DateSelector.MinDate = DateTime.Today;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ProgramParams employee = (ProgramParams)e.Parameter;
            currentEmployee = employee.FoundEmployee;
        }
        private void DateSelected(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs e)
        {
            DatesRequestedBox.Items.Clear();
            foreach(DateTimeOffset date in DateSelector.SelectedDates)
            {
                DatesRequestedBox.Items.Add(date.ToString("MM/dd/yyyy"));
            }
        }
        private void Clear(object sender, RoutedEventArgs e)
        {
            DateSelector.SelectedDates.Clear();
        }
        private void Send(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> listOfDates = DatesRequestedBox.Items.Cast<ListViewItem>().Select(x => x.ToString()).ToList();
                //currentEmployee. newRequest = new PTORequestForm(currentEmployee.FirstName + currentEmployee.LastName, listOfDates, ReasonBox.Text);
                string saveJSON = JsonConvert.SerializeObject(listOfDates);
            }
            catch(Exception ex)
            {
                Status.Text = ex.Message;
            }
            Status.Text = "Request Successful";
        }
    }
}
