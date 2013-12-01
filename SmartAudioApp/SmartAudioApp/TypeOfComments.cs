using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Runtime.Serialization;

namespace SmartAudioApp
{
    [DataContract]
    public class TypeOfComments
    {
        #region .:.Prorpriedades.:.
        [DataMember]
        public int id;

        [DataMember]
        public string name;

        [DataMember]
        public string description;
        #endregion
    }
}
