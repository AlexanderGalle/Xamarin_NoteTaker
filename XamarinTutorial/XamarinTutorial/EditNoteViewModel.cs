using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.Linq;


namespace XamarinTutorial
{
    public class EditNoteViewModel : INotifyPropertyChanged
    {
        NoteItem editText;
        string previousText;
        int previousId;
        NoteItem updatedItem;
        ObservableCollection<NoteItem> notes;
        public event PropertyChangedEventHandler PropertyChanged;
        
        public EditNoteViewModel(){}

        public EditNoteViewModel(NoteItem oldText, ObservableCollection<NoteItem> collection)
        {
            editText = oldText;
            notes = collection;
            previousText = oldText.NoteText;
            previousId = oldText.Id;
           
            ConfirmCommand = new Command(async () =>
            {
                foreach(NoteItem item in Notes)
                {
                    if(item.Id == previousId)
                    {
                        updatedItem = item;
                    }
                }

                Notes[Notes.IndexOf(updatedItem)] = EditText;

                MessagingCenter.Send(this, "Updated Note",EditText);


               /* var noteDetailsVM = new NoteDetailsViewModel(EditText, Notes);
                var noteDetailsPage = new NoteDetails();
                noteDetailsPage.BindingContext = noteDetailsVM;
                Application.Current.MainPage.Navigation
                    .InsertPageBefore(noteDetailsPage, Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault());

                Application.Current.MainPage.Navigation
                    .RemovePage(Application.Current.MainPage.Navigation.NavigationStack.LastOrDefault());*/

                await Application.Current.MainPage.Navigation.PopAsync();

            });

            CancelCommand = new Command(async () =>
            {
                EditText.NoteText = previousText;
                await Application.Current.MainPage.Navigation.PopAsync();
            });
        }

        public NoteItem EditText
        {
            get => editText;
            set
            {
                editText = value;
                var args = new PropertyChangedEventArgs(nameof(EditText));
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

        public Command CancelCommand { get; }

        public Command ConfirmCommand { get; }

    }
}
