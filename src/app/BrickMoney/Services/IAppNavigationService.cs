using System.Threading.Tasks;

namespace BrickMoney.Services
{
    public interface IAppNavigationService
    {
        Task BackAsync();

        Task ShowInfoDialog(string message);
    }
}
