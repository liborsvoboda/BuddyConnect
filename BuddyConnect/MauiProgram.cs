using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using IeuanWalker.Maui.Switch;
using MauiIcons.Fluent;
using MauiIcons.FontAwesome;
using MauiIcons.FontAwesome.Solid;
using MauiIcons.Material;
using Microsoft.Extensions.Logging;
using SimpleToolkit.Core;

namespace BuddyConnect {

    /// <summary>
    /// Application Initiate
    /// Create Database if Not Exist
    /// </summary>
    public static class MauiProgram {
        public static MauiApp CreateMauiApp() {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                
                .UseSwitch()
                .UseSimpleToolkit()
                
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitCore()
                .UseMauiCommunityToolkitMediaElement()

                .UseMaterialMauiIcons()
                .UseFontAwesomeMauiIcons()
                .UseFluentMauiIcons()
                .UseFontAwesomeSolidMauiIcons()
                
                
                .ConfigureFonts()
                .ConfigureAnimations()
                .ConfigureDispatching()
                .ConfigureImageSources()
                .DisplayContentBehindBars()
                

#if WINDOWS
            .ConfigureMauiHandlers(handlers => handlers.AddHandler<WinUIRatingControl, WinUIRatingControlHandler>())
#endif
                .ConfigureFonts(fonts => {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            //HW Controls
            builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);

            //Services Controls
            builder.Services.AddSingleton<StatupControls>();
            builder.Services.AddSingleton<WinUIRatingControl>();
            return builder.Build();
        }
    }
}
