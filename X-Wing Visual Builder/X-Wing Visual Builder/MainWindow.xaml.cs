using System;
using System.Linq;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using X_Wing_Visual_Builder.Model;
using X_Wing_Visual_Builder.View;

namespace X_Wing_Visual_Builder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        BrowseCardsPage browseCardsPage = (BrowseCardsPage)Pages.pages[PageName.BrowseCards];
        bool isUpgradeCacheFull = false;
        bool isPilotCacheFull = false;

        public MainWindow()
        {
            string baseLocation = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
            string filteredLocation = System.IO.Path.GetDirectoryName(baseLocation).Replace("file:\\", "") + "\\Misc\\";
            Uri iconUri = new Uri(filteredLocation + "RebelIcon.ico");
            Icon = BitmapFrame.Create(iconUri);
            InitializeComponent();
            ResizeMode = ResizeMode.CanResize;
            WindowState = WindowState.Maximized;
            ComponentDispatcher.ThreadIdle += FillCardCache;
        }

        void FillCardCache(object sender, EventArgs e)
        {
            if (!isUpgradeCacheFull)
            {
                foreach (Upgrade upgrade in Upgrades.upgrades.Values.ToList())
                {
                    isUpgradeCacheFull = true;
                    if (!browseCardsPage.upgradeCache.ContainsKey(upgrade.id))
                    {
                        browseCardsPage.AddUpgradeToCache(upgrade);
                        isUpgradeCacheFull = false;
                        break;
                    }
                }
            }
            if (!isPilotCacheFull)
            {
                foreach (Pilot pilot in Pilots.pilots.Values.ToList())
                {
                    isPilotCacheFull = true;
                    if (!browseCardsPage.pilotCache.ContainsKey(pilot.id))
                    {
                        browseCardsPage.AddPilotToCache(pilot);
                        isPilotCacheFull = false;
                        break;
                    }
                }
            }
        }
    }
}
