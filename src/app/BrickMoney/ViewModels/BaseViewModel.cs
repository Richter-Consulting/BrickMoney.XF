using System;
using Prism.Mvvm;
using Prism.Navigation;

namespace BrickMoney.ViewModels
{
    public abstract class BaseViewModel : BindableBase, INavigatedAware
    {
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}
