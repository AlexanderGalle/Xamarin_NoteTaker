using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinTutorial
{
    public class ImprovedListView : ListView
    {
        public static BindableProperty SelectedItemChangedCommandProperty = BindableProperty.Create(nameof(SelectedItemChangedCommand), typeof(Command),typeof(ImprovedListView),null);
        public Command SelectedItemChangedCommand
        {
            get
            {
                return (Command)this.GetValue(SelectedItemChangedCommandProperty);
            }
            set
            {
                this.SetValue(SelectedItemChangedCommandProperty, value);
            }
        }

        public ImprovedListView()
        {
            this.ItemTapped += OnItemTapped;
        }

        private void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if(e.Item != null)
            {
                SelectedItemChangedCommand?.Execute(e.Item);
                SelectedItem = null;
            }
        }
    }
}
