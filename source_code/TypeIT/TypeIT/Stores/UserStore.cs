using System;
using TypeIT.Models;

namespace TypeIT.Stores
{
    public class UserStore
    {
        public event Action CurrentUserChanged;

        private UserModel _currentUser;

        public UserModel CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                OnCurrentUserChanged();
            }
        }

        private void OnCurrentUserChanged()
        {
            CurrentUserChanged?.Invoke();
        }
    }
}
