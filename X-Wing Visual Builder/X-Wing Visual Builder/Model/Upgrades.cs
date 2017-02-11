﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.Resources;
using System.IO;

namespace X_Wing_Visual_Builder.Model
{
    class Upgrades
    {
        private Dictionary<int, Upgrade> upgrades = new Dictionary<int, Upgrade>();

        public Upgrades()
        {
            StringReader sr = new StringReader(Properties.Resources.UpgradeDatabase);
            using (TextFieldParser parser = new TextFieldParser(sr))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters("£");
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    upgrades.Add(Int32.Parse(fields[0]), new Upgrade(Int32.Parse(fields[0]), (UpgradeType)Int32.Parse(fields[1]), Int32.Parse(fields[2]), fields[3], fields[4], fields[5],
                                             (Faction)Int32.Parse(fields[6]), (ShipSize)Int32.Parse(fields[7]), (Ship)Int32.Parse(fields[8]),
                                             Convert.ToBoolean(Int32.Parse(fields[9])), Convert.ToBoolean(Int32.Parse(fields[10])), Convert.ToBoolean(Int32.Parse(fields[11])),
                                             Int32.Parse(fields[12]), Convert.ToBoolean(Int32.Parse(fields[13])), Convert.ToBoolean(Int32.Parse(fields[14])),
                                             Convert.ToBoolean(Int32.Parse(fields[15])), Convert.ToBoolean(Int32.Parse(fields[16])), Convert.ToBoolean(Int32.Parse(fields[17]))));
                }
            }
        }

        public Upgrade GetUpgrade(int id)
        {
            return upgrades[id];
        }
    }
}
