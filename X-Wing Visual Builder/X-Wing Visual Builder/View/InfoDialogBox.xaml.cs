﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using X_Wing_Visual_Builder.Model;
using System.Diagnostics;
using System.Windows.Navigation;

namespace X_Wing_Visual_Builder.View
{
    /// <summary>
    /// Interaction logic for InfoDialogBox.xaml
    /// </summary>
    public partial class InfoDialogBox : Window
    {
        protected Grid pageStructureGrid = new Grid();
        private int upgradeCardWidth = 166;
        private int upgradeCardHeight = 255;
        private int pilotCardWidth = 292;
        private int pilotCardHeight = 410;

        public InfoDialogBox()
        {
            InitializeComponent();
            this.Height = 600;
            this.Width = 360;
            ImageBrush backgroundBrush = new ImageBrush(new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\main_background.png")));
            backgroundBrush.TileMode = TileMode.Tile;
            backgroundBrush.Stretch = Stretch.None;
            backgroundBrush.ViewportUnits = BrushMappingMode.Absolute;
            backgroundBrush.AlignmentX = AlignmentX.Left;
            backgroundBrush.AlignmentY = AlignmentY.Top;
            backgroundBrush.Viewport = new Rect(0, 0, 512, 699);
            Background = backgroundBrush;

            pageStructureGrid.ColumnDefinitions.Add(new ColumnDefinition());
            pageStructureGrid.ColumnDefinitions.Add(new ColumnDefinition());
            

            pageStructureGrid.RowDefinitions.Add(new RowDefinition());

            this.Content = pageStructureGrid;
        }

        public void AddUpgrade(Upgrade upgrade)
        {
            Width += upgradeCardWidth + 20;
            CardCanvas upgradeCanvas = upgrade.GetUpgradeCanvas(upgradeCardWidth, upgradeCardHeight, new Thickness(2, 2, 2, 2));
            upgradeCanvas.HideInfoButton();
            Grid.SetColumn(upgradeCanvas, 0);
            Grid.SetRow(upgradeCanvas, 0);
            pageStructureGrid.Children.Add(upgradeCanvas);
            pageStructureGrid.ColumnDefinitions[0].Width = GridLength.Auto;

            AlignableWrapPanel upgradeDetails = new AlignableWrapPanel();
            upgradeDetails.VerticalAlignment = VerticalAlignment.Center;
            upgradeDetails.Margin = new Thickness(50, 0, 0, 0);
            Grid.SetColumn(upgradeDetails, 1);
            Grid.SetRow(upgradeDetails, 0);
            pageStructureGrid.Children.Add(upgradeDetails);

            TextBlock upgradeInfo = new TextBlock();
            upgradeInfo.Width = 300;
            upgradeInfo.TextWrapping = TextWrapping.Wrap;
            upgradeInfo.VerticalAlignment = VerticalAlignment.Center;
            foreach (KeyValuePair<ExpansionType, int> expansionType in upgrade.inExpansion)
            {
                if (expansionType.Value == 1)
                {
                    upgradeInfo.Inlines.Add(Expansions.expansions[expansionType.Key].name + "\r\n");
                }
                else
                {
                    upgradeInfo.Inlines.Add(Expansions.expansions[expansionType.Key].name + " (" + expansionType.Value.ToString() + ")\r\n");
                }
            }
            upgradeInfo.Inlines.Add("\r\n");
            foreach (string faq in upgrade.faq)
            {
                upgradeInfo.Inlines.Add(faq + "\r\n\r\n");
            }
            

            Hyperlink hyperlink = new Hyperlink(new Run("Buy here"));
            hyperlink.NavigateUri = new Uri("http://stackoverflow.com");
            hyperlink.RequestNavigate += new RequestNavigateEventHandler(ClickedLink);

            upgradeInfo.Inlines.Add(hyperlink);
            upgradeDetails.Children.Add(upgradeInfo);
        }

        private void ClickedLink(object sender, RequestNavigateEventArgs e)
        {
            Hyperlink hyperlink = (Hyperlink)sender;
            System.Diagnostics.Process.Start(hyperlink.NavigateUri.ToString());
        }
    }
}
