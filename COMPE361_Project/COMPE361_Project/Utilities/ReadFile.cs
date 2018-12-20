using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Collections.ObjectModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CsvParse;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace COMPE361_Project
{
    class FileReader
    {
        /*

        //Create temporary files
        Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        Windows.Storage.StorageFile employeeFile;
        public async void AddEmployee(string username, string password, Employee newEmployee)
        {
            //Read employee file
            ReadFile();
            //employeeFile = await storageFolder.GetFileAsync("testEmployeeFileRead.json");
            string employeeList = await Windows.Storage.FileIO.ReadTextAsync(employeeFile);

            //Check if employee already exists
            JObject employeeChecker = JObject.Parse(employeeList);

            //If employee does not exist
            if (employeeChecker[username][password] == null)
            {
                //Create employee dictionary
                Dictionary<string, Employee> tempEmployee = new Dictionary<string, Employee>
                {
                    { password, newEmployee }
                };
                Dictionary<string, Dictionary<string, Employee>> newEmployeeDictionary = new Dictionary<string, Dictionary<string, Employee>>
                {
                    { username, tempEmployee }
                };

                //Make JSON string from dictionary
                string newEmployeeJSON = JsonConvert.SerializeObject(newEmployeeDictionary, Formatting.Indented);

                //Write to employee JSON
                employeeFile = await storageFolder.CreateFileAsync("testEmployeeFileWrite.json", Windows.Storage.CreationCollisionOption.OpenIfExists);
                string newFile = await Windows.Storage.FileIO.ReadTextAsync(employeeFile);
                if (newFile == "")
                    newFile = newEmployeeJSON;
                else
                {
                    newFile = newFile.Remove(newFile.Length - 1, 1);
                    newEmployeeJSON = newEmployeeJSON.Remove(0, 1);
                    newEmployeeJSON = newEmployeeJSON.Remove(newEmployeeJSON.Length - 1, 1);
                    newFile = newFile + ',' + newEmployeeJSON + '}';
                }
                await Windows.Storage.FileIO.WriteTextAsync(employeeFile, newFile);
                //**Write Employee Successfully Added**
                
            }

            //If employee already exists
            else
            {
                //**Write Employee Already Exists**
            }
        }
        public async void CheckIfEmployeeExists(string username, string password, Employee newEmployee)
        {
            ReadFile();
            string employeeList = await Windows.Storage.FileIO.ReadTextAsync(employeeFile);

            //Check if employee already exists
            JObject employeeChecker = JObject.Parse(employeeList);

            //If employee does not exist
            if (employeeChecker[username][password] == null)
        }
        public async void ReadFile()
        {
            employeeFile = await storageFolder.GetFileAsync("testEmployeeFileRead.json");
        }
        */
    }
}
