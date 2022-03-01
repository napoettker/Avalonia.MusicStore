using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.MusicStore.ViewModels;
using Avalonia.ReactiveUI;
using NetSparkleUpdater;
using NetSparkleUpdater.SignatureVerifiers;
using ReactiveUI;
using System.Threading.Tasks;

namespace Avalonia.MusicStore.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {

        private SparkleUpdater _sparkle;

        public MainWindow()
        {
            InitializeComponent();
            this.WhenActivated(d => d(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));


            // AutoUpdater
            string manifestModuleName = System.Reflection.Assembly.GetEntryAssembly().ManifestModule.FullyQualifiedName;
            _sparkle = new SparkleUpdater("https://netsparkleupdater.github.io/NetSparkle/files/sample-app/appcast.xml", new DSAChecker(NetSparkleUpdater.Enums.SecurityMode.Strict))
            {
                UIFactory = new NetSparkleUpdater.UI.Avalonia.UIFactory(Icon),
                ShowsUIOnMainThread = true,
            };

            _sparkle.SecurityProtocolType = System.Net.SecurityProtocolType.Tls12;
            _sparkle.StartLoop(true, true);
        }

      
        private async Task DoShowDialogAsync(InteractionContext<MusicStoreViewModel, AlbumViewModel?> interaction)
        {
            var dialog = new MusicStoreWindow();
            dialog.DataContext = interaction.Input;

            var result = await dialog.ShowDialog<AlbumViewModel?>(this);
            interaction.SetOutput(result);
        }

        public async void ManualUpdateCheck_Click(object sender, RoutedEventArgs e)
        {
            await _sparkle.CheckForUpdatesAtUserRequest();
        }
    }
}