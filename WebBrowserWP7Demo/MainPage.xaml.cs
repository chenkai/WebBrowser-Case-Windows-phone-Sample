using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Resources;

namespace WebBrowserWP7Demo
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            //string navigateUrl = @"http://www.163.com";
            //this.ComponentContent_WB.Navigate(new Uri(navigateUrl, UriKind.RelativeOrAbsolute));

            SaveFilesToIsoStore();
            ComponentContent_WB.Navigate(new Uri("CreateProduct.html", UriKind.Relative));

            string defineHtmlStr = @"<html>
<head>
<script>
   function DefineExistFun(elementStr)
      {
       var getElems=document.getElementByTag(elementStr);
       alert(elementStr);
      }
</script>
<body>
<a href="+"http://chenkai.cnblogs.com"+">Test</a></body></head></html>";
        }

        private void ExcuteScript_BT_Click(object sender, RoutedEventArgs e)
        {
            //Button Client Event Excute InvokeJavaScript Method
            try
            {
                this.ComponentContent_WB.InvokeScript("DefineExistFun");
            }
            catch (Exception se)
            {
                MessageBox.Show("Excute JavaScript Have Exception:" + se.Message);
            }
        }

        private void SaveFilesToIsoStore()
        {
            //These files must match what is included in the application package,
            //or BinaryStream.Dispose below will throw an exception.
            string[] files = {
            "CreateProduct.html"
        };

            IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();

            if (false == isoStore.FileExists(files[0]))
            {
                foreach (string f in files)
                {
                    StreamResourceInfo sr = Application.GetResourceStream(new Uri(f, UriKind.Relative));
                    using (BinaryReader br = new BinaryReader(sr.Stream))
                    {
                        byte[] data = br.ReadBytes((int)sr.Stream.Length);
                        SaveToIsoStore(f, data);
                    }
                }
            }
        }

        private void SaveToIsoStore(string fileName, byte[] data)
        {
            string strBaseDir = string.Empty;
            string delimStr = "/";
            char[] delimiter = delimStr.ToCharArray();
            string[] dirsPath = fileName.Split(delimiter);

            //Get the IsoStore.
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication();

            //Re-create the directory structure.
            for (int i = 0; i < dirsPath.Length - 1; i++)
            {
                strBaseDir = System.IO.Path.Combine(strBaseDir, dirsPath[i]);
                isoStore.CreateDirectory(strBaseDir);
            }

            //Remove the existing file.
            if (isoStore.FileExists(fileName))
            {
                isoStore.DeleteFile(fileName);
            }

            //Write the file.
            using (BinaryWriter bw = new BinaryWriter(isoStore.CreateFile(fileName)))
            {
                bw.Write(data);
                bw.Close();
            }
        }

    }
}