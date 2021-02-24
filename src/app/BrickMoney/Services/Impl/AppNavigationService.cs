using System.Threading.Tasks;
using Prism.Navigation;
using Xamarin.Essentials.Interfaces;

namespace BrickMoney.Services.Impl
{
    public class AppNavigationService : IAppNavigationService
    {
        private readonly INavigationService _navigationService;
        private readonly IMainThread _mainThread;

        public AppNavigationService(INavigationService navigationService, IMainThread mainThread)
        {
            _navigationService = navigationService;
            _mainThread = mainThread;
        }

        public Task BackAsync()
        {
            return _mainThread.InvokeOnMainThreadAsync(_navigationService.GoBackAsync);
        }
    }
}
