using Prism.Mvvm;

namespace SmartBudget.ViewModels
{
    public class ShellWindowViewModel : BindableBase
    {
        private string _title = "Smart Budget";

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ShellWindowViewModel()
        {
        }
    }
}