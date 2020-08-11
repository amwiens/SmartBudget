using Prism.Events;
using Prism.Mvvm;

using SmartBudget.Core.Events;

using System;

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

            eventAggregator.GetEvent<ExceptionEvent>().Subscribe(OnErrorReceived);
        }

        private void OnErrorReceived(Exception ex)
        {
            _toastViewModel.ShowError(ex.Message);
        }
    }
}