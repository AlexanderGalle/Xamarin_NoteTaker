using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Linq;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;
using Android.Widget;

namespace XamarinTutorial
{
    public class NoteDetailsViewModel : INotifyPropertyChanged
    {
        string apiURL;
        NoteItem selectedNote;
        ObservableCollection<NoteItem> notes;
        public event PropertyChangedEventHandler PropertyChanged;

        public NoteDetailsViewModel()
        {

        }

        public NoteDetailsViewModel(NoteItem note, ObservableCollection<NoteItem> collection,string apiurl) 
        {
            selectedNote = note;
            notes = collection;
            apiURL = apiurl;

            
            MessagingCenter.Subscribe<EditNoteViewModel,NoteItem>(this, "Updated Note",(sender,arg) =>
            {
                MessagingCenter.Unsubscribe<EditNoteViewModel>(this, "Updated Note");

                UpdateDB(arg);
                SelectedNote = arg;
                

            });

            EditCommand = new Command(async () =>
            {
                var editNoteVM = new EditNoteViewModel(SelectedNote,Notes);
                var editNotePage = new EditNote();
                editNotePage.BindingContext = editNoteVM;
                await Application.Current.MainPage.Navigation.PushAsync(editNotePage);
            });

            DeleteCommand = new Command(async() =>
            {
                DeleteFromDB(Notes.ElementAt(Notes.IndexOf(SelectedNote)));

                Notes.Remove(Notes.ElementAt(Notes.IndexOf(SelectedNote)));

               

                //this.Notes.CollectionChanged += this.Notes.CollectionChanged;
                //NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)

                await Application.Current.MainPage.Navigation.PopAsync();
            });

            DismissCommand = new Command(async () =>
            {
                MessagingCenter.Send(this, "Unselect");
                await Application.Current.MainPage.Navigation.PopAsync();
                
            });

        }
        
        public NoteItem SelectedNote
        {
            get => selectedNote;
            set
            {
                selectedNote = value;
                var args = new PropertyChangedEventArgs(nameof(SelectedNote));
                PropertyChanged?.Invoke(this, args);
            }
        }

        public ObservableCollection<NoteItem> Notes
        {
            get => notes;
            set
            {
                notes = value;
                var args = new PropertyChangedEventArgs(nameof(Notes));
                PropertyChanged?.Invoke(this, args);
            }
        }

        public Command EditCommand { get; }

        public Command DeleteCommand { get; }

        public Command DismissCommand { get; }

        public async void DeleteFromDB(NoteItem item)
        {
            try
            {
                using (HttpClient hc = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Delete,
                        RequestUri = new Uri(apiURL),
                        Content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json")
                    };

                    var response = await hc.SendAsync(request);
                }

            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        public async void UpdateDB(NoteItem item)
        {
            try
            {
                using (HttpClient hc = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Put,
                        RequestUri = new Uri(apiURL),
                        Content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json")
                    };

                    var response = await hc.SendAsync(request);
                }

            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

       
    }
}
