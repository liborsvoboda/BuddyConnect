using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using IeuanWalker.Maui.Switch;
using MauiIcons.Fluent;
using MauiIcons.FontAwesome;
using MauiIcons.FontAwesome.Solid;
using MauiIcons.Material;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using SimpleToolkit.Core;
using static Microsoft.Maui.ApplicationModel.Permissions;

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
                .UseMauiCompatibility()
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
           
            return builder.Build();
        }
    }
}
