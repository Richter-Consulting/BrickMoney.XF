using System.Threading.Tasks;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using Xamarin.Essentials.Interfaces;

namespace BrickMoney.Services.Impl
{
    public class AppNavigationService : IAppNavigationService
    {
        private readonly INavigationService _navigationService;
        private readonly IMainThread _mainThread;
        private readonly IDialogService _dialogService;
        private readonly IPageDialogService _pageDialogService;

        public AppNavigationService(INavigationService navigationService, IMainThread mainThread, IDialogService dialogService, IPageDialogService pageDialogService)
        {
            _navigationService = navigationService;
            _mainThread = mainThread;
            _dialogService = dialogService;
            _pageDialogService = pageDialogService;
        }

        public Task BackAsync()
        {
            return _mainThread.InvokeOnMainThreadAsync(_navigationService.GoBackAsync);
        }

        public Task ShowInfoDialog(string message)
        {
            return _mainThread.InvokeOnMainThreadAsync(() => _pageDialogService.DisplayAlertAsync("Info", message, "OK"));
        }
    }
}
