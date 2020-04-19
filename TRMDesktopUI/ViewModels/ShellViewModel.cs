
using Caliburn.Micro;
using TRMDesktopUI.EventModels;

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
        private IEventAggregator _events;
        //private SimpleContainer _container;

        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM)
        {
            //_loginVM = loginVM;
            _salesVM = salesVM;
            _events = events;
            //_container = container;

            _events.Subscribe(this);

            //ActivateItem(_loginVM);
            //ActivateItem(_container.GetInstance<LoginViewModel>());
            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public void Handle(LogOnEvent message)
        {
            //throw new System.NotImplementedException();

            ActivateItem(_salesVM);
            //_loginVM = _container.GetInstance<LoginViewModel>();
        }



    }
}