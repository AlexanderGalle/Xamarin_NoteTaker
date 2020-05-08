using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;
using System.Reflection;
using Android.Content.Res;
using Xamarin.Essentials;
using System.Net.Http;
using Google.Protobuf;
using Newtonsoft.Json;
using System.Net;
using System.Collections.ObjectModel;
using Xamarin.Forms.StyleSheets;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace XamarinTutorial
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

        }
        
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var vm = BindingContext as MainPageViewModel;

            if (vm.IsRead == false)
            {
                string apiUrl = null;
                
                if (Device.RuntimePlatform == Device.Android)
                {
                    apiUrl = "http://10.0.2.2:44385/api/Notes";
                }
                else if (Device.RuntimePlatform == Device.iOS)
                {
                    apiUrl = "http://localhost:44385/api/Notes";
                }
                else if (Device.RuntimePlatform == "Ooui")
                {
                    apiUrl = "http://localhost:44385/api/Notes";
                }

                try
                {

                    using (HttpClient hc = new HttpClient())
                    {
                        var response = await hc.GetStringAsync(apiUrl);
                        var result = JsonConvert.DeserializeObject<List<NoteItem>>(response);

                        foreach (NoteItem note in result)
                        {
                            vm.AllNotes.Add(note);
                        }
                        vm.ApiURL = apiUrl;
                        vm.IsRead = true;
                    }
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
        }
    }
}
