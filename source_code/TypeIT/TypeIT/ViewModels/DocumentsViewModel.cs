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
        private void ClickedUploadDocument()
        {
            //Custom messagebox
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                int posOfDot = filePath.LastIndexOf(".");
                int posOfSlash = filePath.LastIndexOf("\\") + 1;

                string fileName = filePath.Substring(posOfSlash, posOfDot - posOfSlash);

                FileParser fileParser = new FileParser();
                fileParser.ParseFile(filePath,fileName);

            }
        }
    }
}
