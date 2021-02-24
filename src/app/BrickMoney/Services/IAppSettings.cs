using System.Globalization;

namespace BrickMoney.Services
{
    public interface IAppSettings
    {
        CultureInfo UserCulture { get; set; }
    }
}
