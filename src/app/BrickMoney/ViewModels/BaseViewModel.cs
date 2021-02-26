using System;
using System.Threading.Tasks;
using BrickMoney.Services;
using Prism.Mvvm;
using Prism.Navigation;
using WD.Logging.Abstractions;

namespace BrickMoney.ViewModels
{
    public abstract class BaseViewModel : BindableBase, INavigatedAware
    {
        protected BaseViewModel(ILogger logger, IAppNavigationService navService)
        {
            Logger = logger;
            NavService = navService;
        }
        protected bool FirstRun { get; private set; } = true;
        protected ILogger Logger { get; }
        protected IAppNavigationService NavService { get; }

        private bool _isLoading = false;
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        void INavigatedAware.OnNavigatedFrom(INavigationParameters parameters)
        {
            // No basic implementation
        }

        async void INavigatedAware.OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                if (FirstRun)
                {
                    FirstRun = false;
                    await OnFirstNavigatedToAsync(parameters);
                }

                await OnNavigatedToAsync(parameters);
            } catch (Exception ex)
            {
                Logger.Error(ex, "Navigation to page {0} failed", GetType().Name);
            }
        }

        protected virtual Task OnFirstNavigatedToAsync(INavigationParameters parameters)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            return Task.CompletedTask;
        }
    }
}
