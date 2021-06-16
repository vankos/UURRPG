using Engine.Services;
using System.Windows;

namespace UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string exceptionMessageText = $"Exception occurred:{e.Exception.Message}\r\n\r\nat:{e.Exception.StackTrace}";
            LoggingService.Log(e.Exception);
            MessageBox.Show(exceptionMessageText, "UnhandledException", MessageBoxButton.OK);
        }
    }
}
