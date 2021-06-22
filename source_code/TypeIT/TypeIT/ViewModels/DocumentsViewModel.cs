using System;
using Microsoft.Win32;
using Prism.Commands;
using System.Windows;
using System.Windows.Input;
using TypeIT.Commands;
using TypeIT.Models;
using TypeIT.Stores;
using TypeIT.Utilities;

namespace TypeIT.ViewModels
{
    internal class DocumentsViewModel : ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; }
        public ICommand UploadDocumentCommand { get; set; }
        public ICommand NavigateMyCollectionCommand { get; set; }
        public UserStore CurrentUser { get; set; }

        public DocumentsViewModel(NavigationStore navigationStore, UserStore userStore)
        {
            NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore, userStore));
            NavigateMyCollectionCommand = new NavigateCommand<MyCollectionViewModel>(navigationStore, () => new MyCollectionViewModel(navigationStore, userStore));
            UploadDocumentCommand = new DelegateCommand(ClickedUploadDocument);

            //Current user
            CurrentUser = userStore;
        }
        /// <summary>
        /// Open File dialog so that a document can be chosen. 
        /// </summary>
        private async void ClickedUploadDocument()
        {
            //Custom messagebox
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF files (*.pdf)|*.pdf|Text files (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                int posOfDot = filePath.LastIndexOf(".", StringComparison.Ordinal);
                int posOfSlash = filePath.LastIndexOf("\\", StringComparison.Ordinal) + 1;

                string fileName = filePath.Substring(posOfSlash, posOfDot - posOfSlash);

                FileParser fileParser = new FileParser();
                DocumentModel document = await fileParser.ParseFile(filePath, fileName);

                XmlHandler.AddingADocumentIntoUserXml(CurrentUser.CurrentUser.Name, document.Name, document.GetNumberOfPages(), document.UserPageNumber);

                Xceed.Wpf.Toolkit.MessageBox.Show(
                    "Your document was successfully added!",
                    "Ok",
                    MessageBoxButton.OK
                );

                CurrentUser.CurrentUser.LoadUserDocuments();
            }
        }

    }
}
