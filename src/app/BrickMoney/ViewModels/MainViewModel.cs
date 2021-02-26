using System.Collections.Generic;
using System.Threading.Tasks;
using BrickMoney.Commands;
using BrickMoney.Events;
using BrickMoney.Models.Data;
using BrickMoney.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using WD.Logging.Abstractions;

namespace BrickMoney.ViewModels
{
    public sealed class MainViewModel : BaseViewModel, IDestructible
    {
        private readonly IDataService _dataService;
        private readonly IEventAggregator _eventAggregator;

        public MainViewModel(ILogger<MainViewModel> logger, IAppNavigationService navService, IDataService dataService, IEventAggregator eventAggregator) : base(logger, navService)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<SyncFinished>().Subscribe(async hasNew => await LoadData(), hasNew => hasNew);

            SetSelectedCommand = new ListItemSelectedCommand<SimpleUserSet>(async item => await SetSelectedAsync(item));
        }

        private IList<SimpleUserSet> _userDataList = new List<SimpleUserSet>();
        public IList<SimpleUserSet> UserDataList
        {
            get => _userDataList;
            set => SetProperty(ref _userDataList, value);
        }

        public DelegateCommand<SimpleUserSet> SetSelectedCommand { get; }
        private async Task SetSelectedAsync(SimpleUserSet userSet)
        {
            IsLoading = true;
            var basicInfo = await Task.Run(() => _dataService.GetBasicInfoForUserInfo(userSet.Id));
            await NavService.ShowInfoDialog($"- {basicInfo.NameDE}\n- {basicInfo.NameEN}\n- {basicInfo.RrPrice}\n- {basicInfo.Year}");
            IsLoading = false;
        }

        protected override async Task OnFirstNavigatedToAsync(INavigationParameters parameters)
        {
            Logger.Info("First entry");
            await LoadData();
            await Task.Run(_dataService.SyncDataAsync);
        }

        protected override Task OnNavigatedToAsync(INavigationParameters parameters)
        {
            Logger.Info("Every time");
            return base.OnNavigatedToAsync(parameters);
        }

        private async Task LoadData()
        {
            try
            {
                IsLoading = true;
                await Task.Delay(2500);
                var loadedData = await Task.Run(_dataService.GetLocalDataAsync);
                UserDataList = loadedData;
            }
            catch (System.Exception ex)
            {
                Logger.Error("Faild loading data", ex);
            }
            finally
            {
                IsLoading = false;
            }
        }

        public void Destroy()
        {
            _dataService.Dispose();
        }
    }
}
