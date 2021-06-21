﻿using Microsoft.Win32;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TypeIT.Commands;
using TypeIT.FileTypes;
using TypeIT.Models;
using TypeIT.Stores;

namespace TypeIT.ViewModels
{
    public class MyCollectionViewModel : ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateTypingCommand { get; set; }
        public ICommand NavigateDocumentsCommand { get; }
        public ICommand OpenBookCommand { get; set; }
        public ICommand DeleteBookCommand { get; set; }
        public UserStore currentUser { get; set; }
        public DocumentModel currentDocument { get; set; }

        public MyCollectionViewModel(NavigationStore navigationStore, UserStore userStore)
        {
            NavigateHomeCommand = new NavigateCommand<DashboardViewModel>(navigationStore, () => new DashboardViewModel(navigationStore, userStore));
            NavigateDocumentsCommand = new NavigateCommand<DocumentsViewModel>(navigationStore, () => new DocumentsViewModel(navigationStore, userStore));
            NavigateTypingCommand = new NavigateCommand<TypingViewModel>(navigationStore, () => new TypingViewModel(navigationStore, userStore, currentDocument));
            currentUser = userStore;
            DeleteBookCommand = new DelegateCommand<string>(ClickedDeleteBook);
            OpenBookCommand = new DelegateCommand<string>(ClickedOpenBook);
        }

        private void ClickedDeleteBook(string BookTitle)
        {
            XmlHandler.DeleteDocument(currentUser.CurrentUser.Name, BookTitle);
            currentUser.CurrentUser.RefreshUserModel();
        }

        private void ClickedOpenBook(string BookTitle)
        {
            currentDocument = XmlHandler.GetUserDocument(currentUser.CurrentUser.Name, $"../../../Documents/{BookTitle}");
            NavigateTypingCommand.Execute(currentDocument);
        }


    }
}
