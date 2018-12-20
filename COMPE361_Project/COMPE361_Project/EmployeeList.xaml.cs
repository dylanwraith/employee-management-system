using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace COMPE361_Project
{
    public sealed partial class EmployeeList : Page
    {
        public EmployeeList()
        {
            this.InitializeComponent();
            displayEmployeeList();
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var currentEmployee = (ProgramParams)e.Parameter;

            activeEmployee = currentEmployee.FoundEmployee;


        }
        private void AdminClick(object sender, RoutedEventArgs e)
        {
            
            bool ManagerBool = ManagerCheck.IsChecked.HasValue ? ManagerCheck.IsChecked.Value : false;
            bool EmployeeBool = EmployeeCheck.IsChecked.HasValue ? EmployeeCheck.IsChecked.Value : false;
            if (ManagerBool)
            {
                ManagerCheck.IsChecked = false;
            }
            else if (EmployeeBool)
            {
                EmployeeCheck.IsChecked = false;
            }
            AdminCheck.IsChecked = true;
            if (AdminCheck.IsChecked == true) EmpType.Text = "Admin";
        }
        
        private void ManagerClick(object sender, RoutedEventArgs e)
        {
            bool AdminBool = AdminCheck.IsChecked.HasValue ? AdminCheck.IsChecked.Value : false;
            bool EmployeeBool = EmployeeCheck.IsChecked.HasValue ? EmployeeCheck.IsChecked.Value : false;
            if (AdminBool)
            {
                AdminCheck.IsChecked = false;
            }
            else if (EmployeeBool)
            {
                EmployeeCheck.IsChecked = false;
            }
            ManagerCheck.IsChecked = true;
            if (ManagerCheck.IsChecked == true) EmpType.Text = "Manager";
        }

        private void EmployeeClick(object sender, RoutedEventArgs e)
        {
            bool AdminBool = AdminCheck.IsChecked.HasValue ? AdminCheck.IsChecked.Value : false;
            bool ManagerBool = ManagerCheck.IsChecked.HasValue ? ManagerCheck.IsChecked.Value : false;
            if (AdminBool)
            {
                AdminCheck.IsChecked = false;
            }
            else if (ManagerBool)
            {
                ManagerCheck.IsChecked = false;
            }
            EmployeeCheck.IsChecked = true;
            if (EmployeeCheck.IsChecked == true) EmpType.Text = "Employee";
        }
        public async void displayEmployeeList()
        {
            EmpType.Visibility = Visibility.Collapsed;
            Type.Visibility = Visibility.Collapsed;
            LoadButton.Visibility = Visibility.Collapsed;
            try
            {
                //get json into string
                employeeFile = await storageFolder.CreateFileAsync("testEmployeeFileWrite.json", Windows.Storage.CreationCollisionOption.OpenIfExists);
                string newFile = await Windows.Storage.FileIO.ReadTextAsync(employeeFile);
                JObject json = JObject.Parse(newFile);

                //access employees
                JArray employeeList = (JArray)json["employees"];

                List<string> employees = JsonConvert.DeserializeObject<List<string>>(employeeList.ToString());

                for (int i = 0; i < employees.Count; i++)
                {
                    Employees.Items.Add(new ListViewItem { Content = (string)json[employees[i]]["FirstName"] + " " + (string)json[employees[i]]["LastName"] });
                    EmployeeEmails.Items.Add(new ListViewItem { Content = employees[i], Tag = "test" });


                }
            }
            catch { TestStatus.Text = "Click Update again"; }

        }

        Employee activeEmployee = new Employee();
        


        //Create temporary files
        Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

        Windows.Storage.StorageFile employeeFile;
        public bool employeeExists = true;
        
        public async void AddEmployee(string username, Employee newEmployee)
        {
            //Check if employee already exists
            CheckIfEmployeeExists(username);
            //If employee doesn't exist
            if (!employeeExists)
            {
                //get json into string
                employeeFile = await storageFolder.CreateFileAsync("testEmployeeFileWrite.json", Windows.Storage.CreationCollisionOption.OpenIfExists);
                string newFile = await Windows.Storage.FileIO.ReadTextAsync(employeeFile);
                JObject json = JObject.Parse(newFile);

                //access employees
                JArray employeeList = (JArray)json["employees"];

                //add employee email to list
                employeeList.Add(username);
                

                //update json
                await Windows.Storage.FileIO.WriteTextAsync(employeeFile, json.ToString());

                //Make JSON string from dictionary
                string newEmployeeJSON = CreateEmployeeJSON(username, newEmployee);
                //Write to employee JSON
                WriteToJSON(newEmployeeJSON);

                //**Write Employee Successfully Added**
                TestStatus.Text = "SUCCESS - EMPLOYEE ADDED";

                

            }
            //If employee already exists
            else
                TestStatus.Text = "ERROR - EMPLOYEE ALREADY EXISTS";


        }

        public async void WriteToJSON(string newJSON)
        {
            employeeFile = await storageFolder.CreateFileAsync("testEmployeeFileWrite.json", Windows.Storage.CreationCollisionOption.OpenIfExists);
            string newFile = await Windows.Storage.FileIO.ReadTextAsync(employeeFile);
            if (newFile == "")
                newFile = newJSON;
            else
            {
                newFile = newFile.Remove(newFile.Length - 1, 1);
                newJSON = newJSON.Remove(0, 1);
                newJSON = newJSON.Remove(newJSON.Length - 1, 1);
                newFile = newFile + ',' + newJSON + '}';
            }
            await Windows.Storage.FileIO.WriteTextAsync(employeeFile, newFile);
        }

        public async void CheckIfEmployeeExists(string username)
        {
            ReadFile();
            string employeeList = await Windows.Storage.FileIO.ReadTextAsync(employeeFile);

            //Check for empty file
            if (employeeList == "") employeeExists = false;
            else
            {

                //Check if employee already exists
                JObject employeeChecker = JObject.Parse(employeeList);

                

                //If employee does not exist
                try
                {
                    if (employeeChecker[username] == null) employeeExists = false;
                    else employeeExists = true;
                }
                catch { employeeExists = false; }
            }
        }

        public async void ReadFile()
        {
            employeeFile = await storageFolder.CreateFileAsync("testEmployeeFileWrite.json", Windows.Storage.CreationCollisionOption.OpenIfExists);
            //employeeFile = await storageFolder.GetFileAsync("testEmployeeFileWrite.json");
        }

        public string CreateEmployeeJSON(string username, Employee newEmployee)
        {
            //Create employee dictionary
            Dictionary<string, Employee> tempEmployee = new Dictionary<string, Employee>
                {
                    { username, newEmployee }
                };


            //Make JSON string from dictionary
            return JsonConvert.SerializeObject(tempEmployee, Formatting.Indented);
        }
        public async void UpdateEmployee(string email)
        {

            //get json into string
            employeeFile = await storageFolder.CreateFileAsync("testEmployeeFileWrite.json", Windows.Storage.CreationCollisionOption.OpenIfExists);
            string newFile = await Windows.Storage.FileIO.ReadTextAsync(employeeFile);
            JObject json = JObject.Parse(newFile);
            try
            {
                if (FirstName.Text != "" && LastName.Text != "" && PhoneNumber.Text != "" && Address.Text != "" && Username.Text != "")
                {
                    //access employees
                    json[email]["FirstName"] = FirstName.Text;
                    json[email]["LastName"] = LastName.Text;
                    json[email]["CellNumber"] = PhoneNumber.Text;
                    json[email]["Address"] = Address.Text;
                    //json[email]["employeeType"] = EmpType.Text;
                    if (AdminCheck.IsChecked == true)
                    {
                        json[email]["IsAdmin"] = true;
                        json[email]["IsManager"] = false;

                    }
                    else if (ManagerCheck.IsChecked == true)
                    {
                        json[email]["IsManager"] = true;
                        json[email]["IsAdmin"] = false;
                    }
                    else
                    {
                        json[email]["IsAdmin"] = false;
                        json[email]["IsManager"] = false;

                    }
                }
                else {

                    TestStatus.Text = "Please Fill Out All Fields.";
                }
                // if (ManagerCheck.IsChecked == true) EmpType.Text = "Manager"
                //Dont Do this
                // json[email]["EmailAddress"] = Username.Text; 
                // json[email]["IsAdmin"] = isAdmin.Text;
            }
            catch {
                FirstName.Text = "";
                LastName.Text = "";
                Username.Text = "";
                PhoneNumber.Text = "";
                Address.Text = "";
                EmpType.Text = "";
                AdminCheck.IsChecked = false;
                ManagerCheck.IsChecked = false;
                EmployeeCheck.IsChecked = false;
                TestStatus.Text = "UPDATED!!!";

            }

            //update json
            await Windows.Storage.FileIO.WriteTextAsync(employeeFile, json.ToString());
            //update EmployeeList
            //Employees.ItemsSource = null;
            //EmployeeEmails.ItemsSource = null;
            Employees.Items.Clear();
            Employees.Header = "Employees";
            EmployeeEmails.Items.Clear();
            EmployeeEmails.Header = "Select To Edit";
            displayEmployeeList();
        }
        public Employee CreateEmployee()
        {
            Employee employee = new Employee();

            if (FirstName.Text != "" && LastName.Text != "" && PhoneNumber.Text != "" && Address.Text != "" && Username.Text != "")
            {
                //userName is email address since that is how we login
                employee.FirstName = FirstName.Text;
                employee.LastName = LastName.Text;
                employee.CellNumber = PhoneNumber.Text;
                employee.EmailAddress = Username.Text.ToLower(); //keep it lowercase cuz email
                employee.Address = Address.Text;
                if (AdminCheck.IsChecked == true)
                {
                    employee.IsAdmin = true;
                    employee.IsManager = false;
                }
                else if (ManagerCheck.IsChecked == true)
                {
                    employee.IsManager = true;
                    employee.IsAdmin = false;
                }
                else
                {
                    employee.IsAdmin = false;
                    employee.IsManager = false;
                }
            }
            else {
                TestStatus.Text = "Please fill out all fields";
                employee = null;
            }
               
            // employee.employeeType = EmployeeType.Text;
            return employee;
        }

        //Create new employee
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Employee employee = CreateEmployee();
            try
            {
                if ((employee != null))
                    AddEmployee(Username.Text, employee);
                else
                    TestStatus.Text = "Employee not created. Check all fields.";
            }
            catch { }
            }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            CheckIfEmployeeExists(Username.Text);
            if (!employeeExists) TestStatus.Text = "CANNOT UPDATE - EMPLOYEE DOES NOT EXIST";
            else { try {UpdateEmployee(Username.Text); }

                catch {
                    FirstName.Text = "";
                    LastName.Text = "";
                    Username.Text = "";
                    PhoneNumber.Text = "";
                    Address.Text = "";
                    EmpType.Text = "";
                    AdminCheck.IsChecked = false;
                    ManagerCheck.IsChecked = false;
                    EmployeeCheck.IsChecked = false;

                    TestStatus.Text = "ERROR - EMPLOYEE DOES NOT EXIST"; }
            }
            
        }

        //Loads employee data
        private async void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            ReadFile();
            CheckIfEmployeeExists(Username.Text);
            if (!employeeExists) TestStatus.Text = "ERROR - EMPLOYEE DOES NOT EXIST";
            else
            {
                try
                {
                    string employeeListString = await Windows.Storage.FileIO.ReadTextAsync(employeeFile);
                    JObject employeeList = JObject.Parse(employeeListString);

                    //NEW
                    JObject employeeTarget = (JObject)employeeList[Username.Text];

                    string EmployeeJSON = employeeTarget.ToString();

                    //Taken out temporarily
                    //string EmployeeJSON = employeeList[Username.Text][Password.Text].ToString();

                    Employee foundEmployee = new Employee();
                    Newtonsoft.Json.JsonConvert.PopulateObject(EmployeeJSON, foundEmployee);
                    FirstName.Text = foundEmployee.FirstName;
                    LastName.Text = foundEmployee.LastName;
                    Username.Text = foundEmployee.EmailAddress;
                    PhoneNumber.Text = foundEmployee.CellNumber;
                    Address.Text = foundEmployee.Address;
                //    EmployeeType.Text = foundEmployee.employeeType;
                    TestStatus.Text = "SUCCESS - EMPLOYEE FOUND";
                }
                catch { TestStatus.Text = "ERROR - EMPLOYEE DOES NOT EXIST"; }
            }
        }

        public async void RemoveEmployee(string username)
        {
            //Check if employee already exists
            //If employee doesn't exist
            try
            {
                //get json into string
                employeeFile = await storageFolder.CreateFileAsync("testEmployeeFileWrite.json", Windows.Storage.CreationCollisionOption.OpenIfExists);
                string newFile = await Windows.Storage.FileIO.ReadTextAsync(employeeFile);
                JObject json = JObject.Parse(newFile);

                //access employees
                if (!(username == activeEmployee.EmailAddress))
                {
                    json.Remove(username);
                    JArray employeeOldList = (JArray)json["employees"];
                    List<string> temp = employeeOldList.ToObject<List<string>>();
                    temp.Remove(username);
                    JArray employeeNewList = JArray.FromObject(temp);
                    json["employees"] = (JToken)employeeNewList;

                    //var result = (JArray)json.Remove(username);


                    //json["employees"] = employeeNewList;


                    //update json
                    await Windows.Storage.FileIO.WriteTextAsync(employeeFile, json.ToString());

                    //**Write Employee Successfully Added**
                    TestStatus.Text = "SUCCESS - EMPLOYEE REMOVED";
                }
                else { TestStatus.Text = "Cannot delete yourself, silly"; }
            }
            catch { }

        }
        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
            {

            }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBlock_SelectionChanged_1(object sender, RoutedEventArgs e)
        {

        }

        private void First_Copy1_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void Username_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Employees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void EmployeeEmails_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e) {

            string test = Username.Text;
            RemoveEmployee(Username.Text);
            Employees.Items.Clear();
            Employees.Header = "Employees";
            EmployeeEmails.Items.Clear();
            EmployeeEmails.Header = "Select To Edit";
            displayEmployeeList();
        }


        private async void Employees_ItemClick(object sender, ItemClickEventArgs e) {
            string email = "";

            if (e.ClickedItem.ToString() != "Windows.UI.Xaml.Controls.ListViewHeaderItem")
            {
                try { 
                email = e.ClickedItem.ToString();
                string employeeListString = await Windows.Storage.FileIO.ReadTextAsync(employeeFile);
                JObject employeeList = JObject.Parse(employeeListString);

                //NEW
                JObject employeeTarget = (JObject)employeeList[email];
                string EmployeeJSON = employeeTarget.ToString();


                Employee foundEmployee = new Employee();
                Newtonsoft.Json.JsonConvert.PopulateObject(EmployeeJSON, foundEmployee);
                FirstName.Text = foundEmployee.FirstName;
                LastName.Text = foundEmployee.LastName;
                Username.Text = foundEmployee.EmailAddress;
                PhoneNumber.Text = foundEmployee.CellNumber;
                Address.Text = foundEmployee.Address;
                    if (foundEmployee.IsAdmin == true) { 
                        AdminCheck.IsChecked = true;
                        ManagerCheck.IsChecked = false;
                        EmployeeCheck.IsChecked = false;
                    }
                    else if (foundEmployee.IsManager == true) { 
                        ManagerCheck.IsChecked = true;
                        AdminCheck.IsChecked = false;
                        EmployeeCheck.IsChecked = false;
                    }
                    else { 
                        AdminCheck.IsChecked = false;
                        ManagerCheck.IsChecked = false;
                        EmployeeCheck.IsChecked = true;
                    }

                }
                catch { TestStatus.Text = "Employee Does Not Exist. Click UPDATE"; }

            }

        }

        private void Type_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }
    }
    
}