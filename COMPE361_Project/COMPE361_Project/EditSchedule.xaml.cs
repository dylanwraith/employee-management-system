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
    public sealed partial class EditSchedule : Page
    {
        Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        Windows.Storage.StorageFile employeeFile;
        public Employee receivedEmployee;
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            employeeFile = await storageFolder.CreateFileAsync("testEmployeeFileWrite.json", Windows.Storage.CreationCollisionOption.OpenIfExists);
            string newFile = await Windows.Storage.FileIO.ReadTextAsync(employeeFile);
            JObject json = JObject.Parse(newFile);
            JArray employee = (JArray)json["employees"];
            List<string> names = JsonConvert.DeserializeObject<List<string>>(employee.ToString());
            for (int i = 0; i < names.Count; i++)
            {
                EmployeeScheduleList.Items.Add(new ListViewItem { Content = names[i] });
            }
            DateSelector.MinDate = DateTime.Today;
        }
        public string selectedEmployee = "";
        public EditSchedule()
        {
            string[] times = new string[96] {"0:00", "0:15", "0:30", "0:45", "1:00", "1:15", "1:30", "1:45", "2:00", "2:15", "2:30", "2:45",
                "3:00", "3:15", "3:30", "3:45", "4:00", "4:15", "4:30", "4:45", "5:00", "5:15", "5:30", "5:45", "6:00", "6:15", "6:30", "6:45",
                "7:00", "7:15", "7:30", "7:45", "8:00", "8:15", "8:30", "8:45", "9:00", "9:15", "9:30", "9:45", "10:00", "10:15", "10:30", "10:45",
                "11:00", "11:15", "11:30", "11:45", "12:00", "12:15", "12:30", "12:45", "13:00", "13:15", "13:30", "13:45",
                "14:00", "14:15", "14:30", "14:45", "15:00", "15:15", "15:30", "15:45", "16:00", "16:15", "16:30", "16:45",
                "17:00", "17:15", "17:30", "17:45", "18:00", "18:15", "18:30", "18:45", "19:00", "19:15", "19:30", "19:45",
                "20:00", "20:15", "20:30", "20:45", "21:00", "21:15", "21:30", "21:45", "22:00", "22:15", "22:30", "22:45",
                "23:00", "23:15", "23:30", "23:45" };
            this.InitializeComponent();
            for(int i = 0; i < 96; i ++)
            {
                
                    MenuFlyoutItem x = new MenuFlyoutItem();
                    MenuFlyoutItem y = new MenuFlyoutItem();
                x.Text = times[i];
                y.Text = times[i];
                    x.Click += new RoutedEventHandler(StartTimeSelect);
                    y.Click += new RoutedEventHandler(EndTimeSelect);
                    StartTimeOptions.Items.Add(x);
                    EndTimeOptions.Items.Add(y);
                
            }
        }
        private void DateSelect(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            dateSelected.Text = sender.Date.Value.ToString("MM/dd/yyyy");
        }
        private void StartTimeSelect(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem x = sender as MenuFlyoutItem;
            Start.Text = x.Text;
        }
        private void EndTimeSelect(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem x = sender as MenuFlyoutItem;
            End.Text = x.Text;
        }
        private async void Schedule(object sender, RoutedEventArgs e)
        {
            if (Start.Text == "" || End.Text == "" || dateSelected.Text == "" || selectedEmployee == "") Output.Text = "Please Select Every Parameter";
            else
            {
                employeeFile = await storageFolder.CreateFileAsync("testEmployeeFileWrite.json", Windows.Storage.CreationCollisionOption.OpenIfExists);
                string newFile = await Windows.Storage.FileIO.ReadTextAsync(employeeFile);
                JObject json = JObject.Parse(newFile);
                JArray employeeStart = (JArray)json[selectedEmployee]["ScheduleStart"];
                JArray employeeEnd = (JArray)json[selectedEmployee]["ScheduleEnd"];
                JArray employeeDate = (JArray)json[selectedEmployee]["ScheduleDate"];
                employeeStart.Add(Start.Text);
                employeeEnd.Add(End.Text);
                employeeDate.Add(dateSelected.Text);
                await Windows.Storage.FileIO.WriteTextAsync(employeeFile, json.ToString());
                Output.Text = "Successful Schedule";
            }
        }
        private void EmployeeScheduleList_ItemClick(object sender, ItemClickEventArgs e)
        {
            selectedEmployee = e.ClickedItem.ToString();
        }
    }
}
