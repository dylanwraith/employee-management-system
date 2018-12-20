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
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace COMPE361_Project
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FirstTimeSetup : Page
    {
        ProgramParams employee = new ProgramParams();
        Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        Windows.Storage.StorageFile employeeFile;
        public FirstTimeSetup()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var currentEmployee = (ProgramParams)e.Parameter;
            employee = currentEmployee;
        }

        async void Finish_Click(object sender, RoutedEventArgs e)
        {
            if (FirstName.Text == "" || LastName.Text == "" || Address.Text == "" || PhoneNumber.Text == "")
                ErrorMessage.Text = "ERROR: PLEASE FILL IN ALL BOXES";
            else
            {
                employee.FoundEmployee.FirstName = FirstName.Text;
                employee.FoundEmployee.LastName = LastName.Text;
                employee.FoundEmployee.Address = Address.Text;
                employee.FoundEmployee.CellNumber = PhoneNumber.Text;
                Dictionary<string, Employee> tempEmployee = new Dictionary<string, Employee>
                {
                    { employee.FoundEmployee.EmailAddress, employee.FoundEmployee }
                };
                string[] employees = new string[1] { employee.FoundEmployee.EmailAddress };
                string tempJson = $"{{ \"employees\": [], "; //"\"{employee.FoundEmployee.EmailAddress}\"], ";
                string employeeString = JsonConvert.SerializeObject(tempEmployee, Formatting.Indented);
                employeeString = employeeString.Remove(0, 1);
                string json = $"{tempJson} {employeeString}";

                employeeFile = await storageFolder.CreateFileAsync("testEmployeeFileWrite.json", Windows.Storage.CreationCollisionOption.OpenIfExists);
                await Windows.Storage.FileIO.WriteTextAsync(employeeFile, json);//employeeString);
                this.Frame.Navigate(typeof(PayrollSystem), employee);
            }
        }
    }
}