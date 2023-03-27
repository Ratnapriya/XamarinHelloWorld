using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using PatientApp.iOS;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(CustomNaviPageRenderer))]
namespace PatientApp.iOS
{
    public class CustomNaviPageRenderer : NavigationRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
            {
                OverrideUserInterfaceStyle = UIUserInterfaceStyle.Light;
            }
        }
    }
}