using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;

using SmartBudget.Core.Events;

namespace SmartBudget.Main.ViewModels
{
    public class TopBarViewModel : BindableBase, INavigationAware
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _imagePath;

        public string ImagePath
        {
            get { return _imagePath; }
            set { SetProperty(ref _imagePath, value); }
        }

        public TopBarViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<NavigationEvent>().Subscribe(OnNavigationReceived);
        }

        private void OnNavigationReceived(string message)
        {
            Title = message;
            ImagePath = $"pack://application:,,,/SmartBudget.Main;component/Resources/Icons/{message.ToLower()}.png";
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters.ContainsKey("Title"))
            {
                Title = navigationContext.Parameters.GetValue<string>("Title");
                ImagePath = $"pack://application:,,,/SmartBudget.Main;component/Resources/Icons/{Title}.png";
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}