using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using BrickMoney.DB;
using BrickMoney.Helpers;
using BrickMoney.i18n;
using BrickMoney.Pages;
using BrickMoney.Services;
using BrickMoney.Services.Impl;
using BrickMoney.ViewModels;
using DryIoc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Targets;
using Prism;
using Prism.Ioc;
using SQLitePCL;
using WD.Logging;
using WD.Logging.Abstractions;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;

namespace BrickMoney
{
    public partial class App
    {
        private ILogger<App> _logger = null;

        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Secrets.SyncfusionLicense);
            InitializeComponent();

            try
            {
                InitLogger();

                InitLocalization();

                InitDatabase();

                var navigationResult = await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(MainPage)}");

                if (!navigationResult.Success)
                {
                    _logger?.Fatal(navigationResult.Exception, "Failed to navigate to main page");
                }
            } catch (Exception ex)
            {
                _logger?.Fatal(ex, "Failed to start app");
                throw;
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Xamarin Essentials
            containerRegistry.RegisterSingleton<IMainThread, MainThreadImplementation>();
            containerRegistry.RegisterSingleton<IFileSystem, FileSystemImplementation>();
            containerRegistry.RegisterSingleton<IPreferences, PreferencesImplementation>();
            containerRegistry.RegisterSingleton<ISecureStorage, SecureStorageImplementation>();

            // Register app services
            containerRegistry.RegisterSingleton(typeof(ILogger<>), typeof(NLogLoggerAdapter<>));
            containerRegistry.RegisterScoped<IAppNavigationService, AppNavigationService>();
            containerRegistry.RegisterSingleton<IAppSettings, AppSettingsService>();

            // Database
            RegisterDatabase(containerRegistry);

            // Navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainViewModel>();

            // Dialogs
        }

        private void RegisterDatabase(IContainerRegistry containerRegistry)
        {
            // Create DB folder if not exists
            containerRegistry.RegisterSingleton<DbContextOptions<BrickMoneyDB>>(container =>
            {
                var dbPath = Path.Combine(container.Resolve<IFileSystem>().AppDataDirectory, "databases");
                if (!Directory.Exists(dbPath))
                {
                    Directory.CreateDirectory(dbPath);
                }
                var dbFileName = Path.Combine(dbPath, "brick_money.db");
                var con = new SqliteConnectionStringBuilder
                {
                    DataSource = dbFileName
                };
                return new DbContextOptionsBuilder<BrickMoneyDB>()
                    .UseSqlite(con.ConnectionString).Options;
            });
            containerRegistry.Register<BrickMoneyDB>();
            containerRegistry.Register<IApiService, AppApiService>();
            containerRegistry.Register<IDataService, AppDataService>();
        }

        private void InitLocalization()
        {
            try
            {
                // Get user culture
                var userCulture = Container.Resolve<IAppSettings>().UserCulture;
                if (userCulture != null)
                {
                    // Set culture for translations and UI
                    AppLocalization.Culture = userCulture;
                    Thread.CurrentThread.CurrentCulture = userCulture;
                    Thread.CurrentThread.CurrentUICulture = userCulture;

                    _logger?.Debug("App language set to {0}", userCulture.Name);
                }
                LocalizationResourceManager.Current.Init(AppLocalization.ResourceManager);
            } catch (Exception ex)
            {
                _logger?.Fatal(ex, "Initialization of the localization failed");
            }
        }

        private void InitLogger()
        {
            try
            {
                // Create log directory
                var fileService = Container.Resolve<IFileSystem>();
                var logFilePath = Path.Combine(fileService.CacheDirectory, "logs");
                if (!Directory.Exists(logFilePath))
                {
                    Directory.CreateDirectory(logFilePath);
                }
                var logFileName = Path.Combine(logFilePath, "brick_money.log");

                // Create target to log into
                var fileTarget = new NLog.Targets.FileTarget("FileLog")
                {
                    FileName = logFileName,
                    ArchiveNumbering = ArchiveNumberingMode.DateAndSequence,
                    ArchiveOldFileOnStartup = true,
                    EnableArchiveFileCompression = true,
                    ArchiveAboveSize = 10 * 1024 * 1024 * 1024L,
                    MaxArchiveFiles = 5
                };

                // Create new configuration
                var configuration = new NLog.Config.LoggingConfiguration();
                configuration.AddTarget(fileTarget);

                // Configure rule
                var fileRule =
#if DEBUG
                new NLog.Config.LoggingRule("*", NLog.LogLevel.Debug, fileTarget);
#else
            new NLog.Config.LoggingRule("*", LogLevel.Warn, fileTarget);
#endif
                configuration.LoggingRules.Add(fileRule);

                // Debug output
#if DEBUG
                var debugTarget = new DebuggerTarget("DebugLog");
                configuration.AddTarget(debugTarget);
                var debugRule = new NLog.Config.LoggingRule("*", NLog.LogLevel.Debug, debugTarget);
                configuration.LoggingRules.Add(debugRule);
#endif

                // Set configuration
                LogManager.Configuration = configuration;
                _logger = Container.Resolve<ILogger<App>>();
            } catch(Exception ex)
            {
                Debug.WriteLine("Logger initialization failed" + Environment.NewLine + ex);
            }
        }

        private void InitDatabase()
        {
            try
            {
                Batteries_V2.Init();
                using (var db = Container.Resolve<BrickMoneyDB>())
                {
                    var migrations = db.Database.GetPendingMigrations();
                    db.Database.Migrate();
                    if (migrations.Any())
                    {
                        _logger?.Debug("Database migrated to version {0}", string.Join(", ", migrations));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Failed to initialize database");
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
