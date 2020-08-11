using Prism.Events;
using Prism.Mvvm;

using SmartBudget.Core.Events;

namespace SmartBudget.ViewModels
{
    public class ShellWindowViewModel : BindableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ToastViewModel _toastViewModel;
        private string _title = "Smart Budget";

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ShellWindowViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _toastViewModel = new ToastViewModel();

            eventAggregator.GetEvent<NavigationEvent>().Subscribe(OnNavigationReceived);
        }

        private void OnNavigationReceived(string obj)
        {
            if (obj != "Dashboard")
                _toastViewModel.ShowInformation("Test");
        }
    }
}