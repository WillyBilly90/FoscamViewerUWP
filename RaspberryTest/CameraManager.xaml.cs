using RaspberryTest.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Windows.ApplicationModel.Appointments.DataProvider;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace RaspberryTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CameraManager : Page
    {
        public CameraManager()
        {
            this.InitializeComponent();
        }

        public void FillList()
        {
            //Clear listview
            CameraLvw.Items.Clear();
            //Clear textboxes
            CameraIpTbx.Text = "";
            CameraNameTbx.Text = "";
            CameraPasswordTbx.Text = "";
            CameraPortTbx.Text = "";
            CameraUsernameTbx.Text = "";
            //Load current cameralist from file
            List<Camera> currentCameraList = new List<Camera>();
            currentCameraList = LoadCameraXmlAsync().Result;
            foreach (var cam in currentCameraList)
            {
                CameraLvw.Items.Add(cam.Name);
            }
        }
        
        private void AddCameraBtn_ClickAsync(object sender, RoutedEventArgs e)
        {
            //Load current cameralist from file
            List<Camera>currentCameraList=new List<Camera>();
            currentCameraList = LoadCameraXmlAsync().GetAwaiter().GetResult();
            //Add new camera to list
            Camera newCam = new Camera();
            newCam.IpAddress = CameraIpTbx.Text;
            newCam.Name = CameraNameTbx.Text;
            newCam.Password = CameraPasswordTbx.Text;
            newCam.Port = CameraPortTbx.Text;
            newCam.Username = CameraUsernameTbx.Text;
            currentCameraList.Add(newCam);
            //Save updated list to file
            SaveCameraXmlAsync(currentCameraList).Wait();
            //Update list on screeen
            FillList();
        }


        public async System.Threading.Tasks.Task SaveCameraXmlAsync(List<Camera>cameraList)
        {
            //save list of camera's to xml file
            StorageFile fileForWrite =
                await ApplicationData.Current.LocalFolder.CreateFileAsync("cameras.xml",
                    CreationCollisionOption.ReplaceExisting).AsTask().ConfigureAwait(false);
            using (var stream = await fileForWrite.OpenStreamForWriteAsync())
            {
                // write xml into the writer
                var serializer = new XmlSerializer(typeof(List<Camera>),new XmlRootAttribute("CameraList"));
                serializer.Serialize(stream, cameraList);
            }
        }

        public static async System.Threading.Tasks.Task<List<Camera>> LoadCameraXmlAsync()

        {
            //load list of camera's from xml file
            List<Camera> cameraList = new List<Camera>();
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            bool fileExists = File.Exists(storageFolder.Path + "\\cameras.xml");
            try
            {
                if (fileExists)
                {
                    StorageFile fileToRead = await storageFolder.GetFileAsync("cameras.xml").AsTask().ConfigureAwait(false);
                    using (var stream = await fileToRead.OpenStreamForReadAsync())
                    {
                        // read xml from the writer
                        var serializer = new XmlSerializer(typeof(List<Camera>), new XmlRootAttribute("CameraList"));
                        cameraList = (List<Camera>)serializer.Deserialize(stream);
                    }

                }
            }
            catch
            {
            }
            return cameraList;
        }

        
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            FillList();
        }

        private void RemoveCameraBtn_Click(object sender, RoutedEventArgs e)
        {
            //remove selected camera from cameraList, save changes to XML and refresh ListView
            if (CameraLvw.SelectedItems.Count > 0)
            {
                int selectedCam = CameraLvw.SelectedIndex;
                //Load current cameralist from file
                List<Camera> currentCameraList = new List<Camera>();
                currentCameraList = LoadCameraXmlAsync().GetAwaiter().GetResult();
                //remove camera from list
                currentCameraList.RemoveAt(selectedCam);
                //Save updated list to file
                SaveCameraXmlAsync(currentCameraList).Wait();
                //Update list on screeen
                FillList();
            }
        }

        private void CloseManagerBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
