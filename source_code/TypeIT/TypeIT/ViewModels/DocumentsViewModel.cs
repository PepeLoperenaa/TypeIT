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

        public DocumentsViewModel(NavigationStore navigationStore)
        {
           NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore));
           UploadDocumentCommand = new DelegateCommand(ClickedUploadDocument);

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
