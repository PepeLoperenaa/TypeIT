using Microsoft.Win32;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TypeIT.Commands;
using TypeIT.Stores;

namespace TypeIT.ViewModels
{
    class DocumentsViewModel : ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; }
        public ICommand UploadDocumentCommand { get; set; }
        public ICommand NavigateMyCollectionCommand { get; set; }
        public UserStore currentUser { get; set; }

        public DocumentsViewModel(NavigationStore navigationStore, UserStore userStore)
        {
           NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore, userStore));
           NavigateMyCollectionCommand = new NavigateCommand<MyCollectionViewModel>(navigationStore, () => new MyCollectionViewModel(navigationStore, userStore));
           UploadDocumentCommand = new DelegateCommand(ClickedUploadDocument);

           //Current user
           currentUser = userStore;
        }
        private void ClickedUploadDocument()
        {
            //Custom messagebox
            OpenFileDialog openFileDialog = new OpenFileDialog();

            Nullable<bool> result = openFileDialog.ShowDialog();
            //if (result == true)
            //{
            //    FileNameTextBox.Text = openFileDialog.FileName;
            //    TextBlock1.Text = System.IO.File.ReadAllText(openFileDialog.FileName);
            //}
        }
    }
}
