using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace RaspberryTest.Classes
{
    [XmlRoot("CameraList")]
    public class CameraList
    {
        public CameraList() { List<Camera> Items = new List<Camera>(); }

        [XmlElement("Camera")]
        public List<Camera> Items { get; set; }
    }


    [XmlType("Camera")]
    public class Camera
    {
        [XmlElement("Name")]
        public string Name { get; set; } //name of camera

        [XmlElement("IpAddress")]
        public string IpAddress { get; set; } //IP address of camera

        [XmlElement("Port")]
        public string Port { get; set; } //Port of camera

        [XmlElement("UserName")]
        public string Username { get; set; } //username of camera

        [XmlElement("Password")]
        public string Password { get; set; } //password of camera
    }
}
