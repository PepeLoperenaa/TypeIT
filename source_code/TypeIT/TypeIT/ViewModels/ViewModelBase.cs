using System;
using System.ComponentModel;
using System.Windows.Input;

namespace TypeIT.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged, ICommand
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            //Not used
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            //Not used
            throw new NotImplementedException();
        }

        protected void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void PropertyChangedEvent(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        }
    }
}
