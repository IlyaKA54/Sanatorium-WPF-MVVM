using Sanatorium.View;
using Sanatorium.ViewModel.Base;
using System;
using System.Windows.Input;

namespace Sanatorium.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        public ICommand Show { get; private set; }

        public HomeViewModel()
        {
            Show = new ViewModelCommand(ExecuteShowWindow);
        }

        private void ExecuteShowWindow(object obj)
        {
            var newWindow = new CustomerSelectionView();
            newWindow.Show();
        }
    }
}
