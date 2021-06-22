using Microsoft.Win32;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TypeIT.Commands;
using TypeIT.Models;
using TypeIT.Stores;
using TypeIT.Utilities;

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
        /// <summary>
        /// Open File dialog so that a document can be chosen. 
        /// TODO: Create File
        /// </summary>
        private async void ClickedUploadDocument()
        {
            //Custom messagebox
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PDF files (*.pdf)|*.pdf|Text files (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                int posOfDot = filePath.LastIndexOf(".");
                int posOfSlash = filePath.LastIndexOf("\\") + 1;

                string fileName = filePath.Substring(posOfSlash, posOfDot - posOfSlash);

                FileParser fileParser = new FileParser();
                DocumentModel document = await fileParser.ParseFile(filePath,fileName);

                XmlHandler.AddingADocumentIntoUserXml(currentUser.CurrentUser.Name, document.Name, document.GetNumberOfPages(), document.UserPageNumber);

                Xceed.Wpf.Toolkit.MessageBox.Show(
                    "Your document was successfully added !",
                    "Ok",
                    MessageBoxButton.OK
                );

                currentUser.CurrentUser.loadUserDocuments();
            }
        }

    }
}
