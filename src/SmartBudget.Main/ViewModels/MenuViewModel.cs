using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace SmartBudget.Main.ViewModels
{
    public class MenuViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        public DelegateCommand<string> NavigateCommand { get; private set; }

        public MenuViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string navigatePath)
        {
            if (navigatePath != null)
                _regionManager.RequestNavigate("Content", navigatePath);
        }
    }
}