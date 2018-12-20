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
    public sealed partial class ClockLogs : Page
    {

        Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        Windows.Storage.StorageFile employeeFile;
        public Employee receivedEmployee;

        public ClockLogs()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            employeeFile = await storageFolder.CreateFileAsync("testEmployeeFileWrite.json", Windows.Storage.CreationCollisionOption.OpenIfExists);
            string newFile = await Windows.Storage.FileIO.ReadTextAsync(employeeFile);
            JObject json = JObject.Parse(newFile);
            JArray employee = (JArray)json["employees"];
            List<string> names = JsonConvert.DeserializeObject<List<string>>(employee.ToString());
            for (int i = 0; i < names.Count; i++)
            {
                EmployeeEmailList.Items.Add(new ListViewItem { Content = names[i] });
            }
        }
        private async void EmployeeEmailList_ItemClick(object sender, ItemClickEventArgs e)
        {
            string email = "";

            if (e.ClickedItem.ToString() != "Windows.UI.Xaml.Controls.ListViewHeaderItem")
            {
                try
                {
                    email = e.ClickedItem.ToString();
                    string employeeListString = await Windows.Storage.FileIO.ReadTextAsync(employeeFile);
                    JObject employeeList = JObject.Parse(employeeListString);

                    JObject employeeTarget = (JObject)employeeList[email];
                    string EmployeeJSON = employeeTarget.ToString();

                    Employee foundEmployee = new Employee();
                    Newtonsoft.Json.JsonConvert.PopulateObject(EmployeeJSON, foundEmployee);
                    ClockInList.Items.Clear();
                    ClockInList.Header = "ClockInList";
                    ClockOutList.Items.Clear();
                    ClockOutList.Header = "ClockOutList";
                    LunchInList.Items.Clear();
                    LunchInList.Header = "LunchInList";
                    LunchOutList.Items.Clear();
                    LunchOutList.Header = "LunchOutList";
                    for (int i = 0; i < foundEmployee.ClockIn.Length; i++)
                    {
                        ClockInList.Items.Add(new ListViewItem { Content = foundEmployee.ClockIn[i] });
                    }
                    for (int i = 0; i < foundEmployee.ClockOut.Length; i++)
                    {
                        ClockOutList.Items.Add(new ListViewItem { Content = foundEmployee.ClockOut[i] });
                    }
                    for (int i = 0; i < foundEmployee.LunchIn.Length; i++)
                    {
                        LunchInList.Items.Add(new ListViewItem { Content = foundEmployee.LunchIn[i] });
                    }
                    for (int i = 0; i < foundEmployee.LunchOut.Length; i++)
                    {
                        LunchOutList.Items.Add(new ListViewItem { Content = foundEmployee.LunchOut[i] });
                    }
                }
                catch { }
            }
        }
    }
}
