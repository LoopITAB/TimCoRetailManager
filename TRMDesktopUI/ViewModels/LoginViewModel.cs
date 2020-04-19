
using Caliburn.Micro;
using System;
using System.Threading.Tasks;
using TRMDesktopUI.EventModels;
using TRMDesktopUI.Library.Api;

namespace TRMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _userName = "Lennie.Fredriksson@gmail.com";
        private string _password;
        private IAPIHelper _apiHelper;
        //private bool _isErrorVisible;
        private IEventAggregator _events;

        public LoginViewModel(IAPIHelper apiHelper, IEventAggregator events)
        {
            _apiHelper = apiHelper;
            _events = events;
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        public bool IsErrorVisible
        {
            get
            {
                bool output = false;

                if (ErrorMessage?.Length > 0)
                {
                    output = true;
                }

                return output;
            }

            //get { return _isErrorVisible; }
            //set
            //{
            //    NotifyOfPropertyChange(() => IsErrorVisible);
            //    _isErrorVisible = value;
            //}
        }

        private string _errorMessage;

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => IsErrorVisible);
                NotifyOfPropertyChange(() => ErrorMessage);
            }
        }

        public bool CanLogIn
        {
            get
            {
                bool output = false;

                if (UserName?.Length > 0 && Password?.Length > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        //public bool CanLogIn(string userName, string password)
        //{
        //    bool output = false;

        //    if(userName.Length > 0 && password.Length > 0)
        //    {
        //        output = true;
        //    }

        //    return output;
        //}

        //public void LogIn(string userName, string password)
        //{
        //    Console.WriteLine();
        //}

        public async Task LogIn()
        {
            try
            {
                ErrorMessage = string.Empty;
                var result = await _apiHelper.Authenticate(UserName, Password);

                // Capture more information about the user:
                await _apiHelper.GetLoggedInUserInfo(result.Access_Token);

                _events.PublishOnUIThread(new LogOnEvent());
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.Message);
                ErrorMessage = ex.Message;
            }
        }


    }
}