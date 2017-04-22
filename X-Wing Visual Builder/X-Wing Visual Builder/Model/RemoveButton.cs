﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace X_Wing_Visual_Builder.Model
{
    class RemoveButton : Image, IUpgradeId, IGeneralId
    {
        public int uniqueBuildId { get; set; }
        public int uniquePilotId { get; set; }
        public int upgradeId { get; set; }
        public int pilotId { get; set; }
        private int _id;
        public int id
        {
            get
            {
                if (upgradeId == 0)
                {
                    return pilotId;
                }
                else
                {
                    return upgradeId;
                }
            }
            set
            {
                _id = value;
            }
        }
        public RemoveButton(double width, double height)
        {
            BitmapImage webImage = new BitmapImage(new Uri(@"D:\Documents\Game Stuff\X-Wing\removebutton.png"));
            RenderOptions.SetBitmapScalingMode(this, BitmapScalingMode.HighQuality);
            this.Source = webImage;
            this.Height = height;
            this.Width = width;
            this.UseLayoutRounding = true;
        }
    }
}