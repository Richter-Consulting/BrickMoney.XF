using System.Globalization;

namespace BrickMoney.Services
{
    public interface IAppSettings
    {
        CultureInfo UserCulture { get; set; }
        int LastUpdateTimeStamp { get; set; }
        string LastUpdateEndpoint { get; set; }
    }
}
