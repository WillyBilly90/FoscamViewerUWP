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
using VLC;
using RaspberryTest.Classes;
using RaspberryTest;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RaspberryTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public int ActiveCameraId = 0;

        public MainPage()
        {
            this.InitializeComponent();
        }
        public void PlayFoscam()
        {
            List<Camera> currentCameraList = GetExistingCameraList();
            //Go to first camera if there is no next camera
            if (ActiveCameraId > currentCameraList.Count-1) ActiveCameraId = 0;
            if (currentCameraList.Count > 0)
            {
                //Create connectionstring
                string cameraConnectionString = "rtsp://" + currentCameraList[ActiveCameraId].Username + ":" +
                    currentCameraList[ActiveCameraId].Password + "@" +
                    currentCameraList[ActiveCameraId].IpAddress + ":" +
                    currentCameraList[ActiveCameraId].Port + "/videoSub";
                //Bind connectionstring
                MediaElement1.Source = cameraConnectionString;
            }
        }
            

        private void CameraSettings_Click(object sender, RoutedEventArgs e)
        {
                this.Frame.Navigate(typeof(CameraManager));
        }

        public List<Camera> GetExistingCameraList()
        {
            //Load existing cameralist
            List<Camera> currentCameraList = new List<Camera>();
            currentCameraList = CameraManager.LoadCameraXmlAsync().GetAwaiter().GetResult();
            return currentCameraList;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
                
                PlayFoscam();
        }

        private void mediaElement1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //if medialement is tapped, go to next camera in list, if there is no next camera, go to first camera
            ActiveCameraId++;
            PlayFoscam();
        }

        private void Grid_GotFocus(object sender, RoutedEventArgs e)
        {
            PlayFoscam();
        }

        private void Shutdown_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }
    }
}
