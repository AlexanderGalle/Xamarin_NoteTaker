using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Reflection;
using System.Linq;
using Java.Lang;
using System.Net.Http;
using Newtonsoft.Json;

namespace XamarinTutorial
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        string apiURL;
        bool isRead;
        string newNote;
        NoteItem selectedNote;
        ObservableCollection<NoteItem> allNotes = new ObservableCollection<NoteItem>();
        public event PropertyChangedEventHandler PropertyChanged;

        public MainPageViewModel()
        {
            MessagingCenter.Subscribe<NoteDetailsViewModel>(this, "Unselect", (sender) =>
            {
                SelectedNote = null;
            });

            AddCommand = new Command(() => 
            {
            if (NewNote != "" && NewNote != null)
                {
                    NoteItem item = new NoteItem();

                    if(AllNotes.Count == 0)
                    {
                        item.Id = 1;
                    }
                    else
                    {
                        item.Id = AllNotes.LastOrDefault().Id+1;
                    }

                    item.NoteText = NewNote;

                    AllNotes.Add(item);

                    AddToDB(item);

                    NewNote = string.Empty;
                }
            });

            SelectedNoteCommand = new Command(async () =>
            {
                if (SelectedNote != null)
                {
                    var noteDetailsVM = new NoteDetailsViewModel(SelectedNote, AllNotes,ApiURL);
                    var noteDetailsPage = new NoteDetails();
                    noteDetailsPage.BindingContext = noteDetailsVM;
                    await Application.Current.MainPage.Navigation.PushAsync(noteDetailsPage);
                }
            });

        }

        public ObservableCollection<NoteItem> AllNotes 
        { 
            get => allNotes;
            set
            {
                allNotes = value;
                var args = new PropertyChangedEventArgs(nameof(AllNotes));
                PropertyChanged?.Invoke(this, args);
            } 
        }

        public string ApiURL
        {
            get => apiURL;
            set
            {
                apiURL = value;
                var args = new PropertyChangedEventArgs(nameof(ApiURL));
                PropertyChanged?.Invoke(this, args);
            }
        }
       
        public bool IsRead
        {
            get => isRead;
            set
            {
                isRead = value;
                var args = new PropertyChangedEventArgs(nameof(IsRead));
                PropertyChanged?.Invoke(this, args);
            }
        }

        public string NewNote
        {
            get => newNote;
            set
            {
                newNote = value;
                var args = new PropertyChangedEventArgs(nameof(NewNote));
                PropertyChanged?.Invoke(this, args);
            }
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

        public Command SelectedNoteCommand { get; }

        public Command AddCommand { get; }

        public async void AddToDB(NoteItem item)
        {
            try
            {

                using (HttpClient hc = new HttpClient())
                {
                    var jsonString = JsonConvert.SerializeObject(item);
                    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                    var response = await hc.PostAsync(ApiURL, content);
                }

            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

    }
}
