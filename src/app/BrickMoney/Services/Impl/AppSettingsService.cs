﻿using System.Globalization;
using Xamarin.Essentials.Interfaces;

namespace BrickMoney.Services.Impl
{
    public class AppSettingsService : IAppSettings
    {
        private readonly IPreferences _settings;
        private readonly ISecureStorage _securSettings;

        public AppSettingsService(IPreferences settings, ISecureStorage securSettings)
        {
            _settings = settings;
            _securSettings = securSettings;
        }

        private const string USER_CULTURE_KEY = "AppSettings_UserCulture";
        private CultureInfo _userCulture = null;
        public CultureInfo UserCulture
        {
            get
            {
                if (_userCulture == null)
                {
                    var cultureName = _settings.Get(USER_CULTURE_KEY, null);
                    _userCulture = string.IsNullOrEmpty(cultureName) ? null : new CultureInfo(cultureName);
                }
                return _userCulture;
            }
            set
            {
                if(string.Equals(_userCulture?.Name, value?.Name))
                {
                    return;
                }

                var cultureName = value?.Name;
                if (string.IsNullOrEmpty(cultureName))
                {
                    _settings.Remove(USER_CULTURE_KEY);
                } else
                {
                    _settings.Set(USER_CULTURE_KEY, cultureName);
                }

                _userCulture = null;
            }
        }

        private const string LAST_UPDATE_TIMESTAMP_KEY = "AppSettings_LastUpdateTimestamp";
        private int? _lastUpdateTime = null;
        public int LastUpdateTimeStamp
        {
            get
            {
                if(_lastUpdateTime == null)
                {
                    _lastUpdateTime = _settings.Get(LAST_UPDATE_TIMESTAMP_KEY, int.MinValue);
                }

                return _lastUpdateTime ?? int.MinValue;
            }
            set
            {
                if(value == _lastUpdateTime)
                {
                    return;
                }

                _settings.Set(LAST_UPDATE_TIMESTAMP_KEY, value);
                _lastUpdateTime = null;
            }
        }

        private const string LAST_UPDATE_ENDPOINT_KEY = "AppSettings_LastUpdateEndpoint";
        private string _lastUpdateEndpoint = null;
        public string LastUpdateEndpoint
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_lastUpdateEndpoint))
                {
                    _lastUpdateEndpoint = _settings.Get(LAST_UPDATE_ENDPOINT_KEY, string.Empty);
                }

                return _lastUpdateEndpoint;
            }
            set
            {
                if(string.Equals(value, _lastUpdateEndpoint))
                {
                    return;
                }

                _settings.Set(LAST_UPDATE_ENDPOINT_KEY, value);
                _lastUpdateEndpoint = null;
            }
        }
    }
}
