
using Caliburn.Micro;
using TRMDesktopUI.EventModels;
using TRMDesktopUI.Library.Api;
using TRMDesktopUI.Library.Models;

namespace TRMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        // 1. Implement IHandle
        // 2. Subscribe
        // 3. ActivateItem

        // LoginVM can be refreshed each time we use it.
        //private LoginViewModel _loginVM;
        private SalesViewModel _salesVM;
        private readonly ILoggedInUserModel _user;
        private readonly IAPIHelper _apiHelper;
        private IEventAggregator _events;
        //private SimpleContainer _container;

        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM, ILoggedInUserModel user, IAPIHelper apiHelper)
        {
            //_loginVM = loginVM;
            _salesVM = salesVM;
            _user = user;
            _apiHelper = apiHelper;
            _events = events;
            //_container = container;

            _events.Subscribe(this);

            //ActivateItem(_loginVM);
            //ActivateItem(_container.GetInstance<LoginViewModel>());
            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public bool IsLoggedIn
        {
            get
            {
                bool output = false;

                if (string.IsNullOrWhiteSpace(_user.Token) == false)
                {
                    output = true;
                }

                return output;
            }
        }

        public void Handle(LogOnEvent message)
        {
            //throw new System.NotImplementedException();

            ActivateItem(_salesVM);
            //_loginVM = _container.GetInstance<LoginViewModel>();
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

        public void ExitApplication()
        {
            //this.ExitApplication();
            TryClose();
        }

        public void LogOut()
        {
            _user.ResetUserModel();
            _apiHelper.LogOffUser();
            ActivateItem(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsLoggedIn);
        }

    }
}