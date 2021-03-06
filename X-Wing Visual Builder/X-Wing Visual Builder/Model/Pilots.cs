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
    static class Pilots
    {
        public static Dictionary<int, Pilot> pilots = new Dictionary<int, Pilot>();

        static Pilots()
        {
            Dictionary<int, int> numberOfPilotsOwned = LoadPilotsOwned();
            using (TextFieldParser parser = new TextFieldParser(new StringReader(Properties.Resources.PilotDatabase)))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters("£");
                parser.HasFieldsEnclosedInQuotes = false;
                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    Dictionary<UpgradeType, int> possibleUpgrades = new Dictionary<UpgradeType, int>();
                    if (fields[6].Length > 0)
                    {
                        string[] possibleUpgradesSplit = fields[6].Split(',');
                        foreach (string possibleUpgrade in possibleUpgradesSplit)
                        {
                            if (possibleUpgrades.ContainsKey((UpgradeType)Int32.Parse(possibleUpgrade)))
                            {
                                possibleUpgrades[(UpgradeType)Int32.Parse(possibleUpgrade)]++;
                            }
                            else
                            {
                                possibleUpgrades[(UpgradeType)Int32.Parse(possibleUpgrade)] = 1;
                            }
                        }
                    }
                    int numberOwned = 0;
                    if(numberOfPilotsOwned != null && numberOfPilotsOwned.ContainsKey(Int32.Parse(fields[0])))
                    {
                        numberOwned = numberOfPilotsOwned[Int32.Parse(fields[0])];
                    }
                    possibleUpgrades.Add(UpgradeType.Title, 1);
                    possibleUpgrades.Add(UpgradeType.Modification, 1);
                    List<ExpansionType> inExpansion = new List<ExpansionType>();
                    if (fields[11].Length > 0)
                    {
                        string[] inExpansionSplit = fields[11].Split(',');
                        foreach (string inSingleExpansion in inExpansionSplit)
                        {
                            inExpansion.Add((ExpansionType)Int32.Parse(inSingleExpansion));
                        }
                    }
                    List<string> faq = new List<string>();
                    if (fields[8].Length > 0)
                    {
                        string[] possibleFaqSplit = fields[8].Split('|');
                        foreach (string possibleFaq in possibleFaqSplit)
                        {
                            faq.Add(possibleFaq);
                        }
                    }
                    pilots.Add(Int32.Parse(fields[0]), new Pilot(Int32.Parse(fields[0]), (ShipType)Int32.Parse(fields[1]), Convert.ToBoolean(Int32.Parse(fields[2])), fields[3],
                                Int32.Parse(fields[4]), fields[5], possibleUpgrades, Int32.Parse(fields[7]), faq, (Faction)Int32.Parse(fields[9]), Convert.ToBoolean(Int32.Parse(fields[10])), numberOwned, inExpansion, fields[12]));
                }
            }
            // Remove Huge Ship cards
            List<int> pilotsToRemove = new List<int>();
            foreach (KeyValuePair<int, Pilot> pilot in pilots)
            {
                if (pilot.Value.ship.shipSize == ShipSize.Huge) { pilotsToRemove.Add(pilot.Key); }
            }
            foreach (int pilotToRemove in pilotsToRemove)
            {
                pilots.Remove(pilotToRemove);
            }
            SaveNumberOfPilotsOwned();
        }

        private static Dictionary<int, int> LoadPilotsOwned()
        {
            string[] allPilotsOwned = FileHandler.LoadFile("pilotsowned.txt");
            if (allPilotsOwned == null) { return null; }

            Dictionary<int, int> pilotKeyOwned = new Dictionary<int, int>();            
            if (allPilotsOwned != null)
            {
                if (allPilotsOwned.Count() > 0)
                {
                    foreach (string pilotOwnedString in allPilotsOwned)
                    {
                        string[] pilotOwnedInfo = pilotOwnedString.Split(',');
                        pilotKeyOwned[Int32.Parse(pilotOwnedInfo[0])] = Int32.Parse(pilotOwnedInfo[2]);
                    }
                }
            }
            return pilotKeyOwned;
        }
        public static void SaveNumberOfPilotsOwned()
        {
            string numberOfPilotsOwned = "";
            foreach (Pilot pilot in pilots.Values.OrderBy(pilot => pilot.faction).ThenBy(pilot => pilot.ship.name).ThenByDescending(pilot => pilot.pilotSkill).ThenByDescending(pilot => pilot.cost).ThenBy(pilot => pilot.name).ToList())
            {
                numberOfPilotsOwned += pilot.id.ToString() + "," + pilot.name + "," + pilot.numberOwned.ToString();
                numberOfPilotsOwned += System.Environment.NewLine;
            }
            FileHandler.SaveFile("pilotsowned.txt", numberOfPilotsOwned);
        }

        public static Pilot GetRandomPilot()
        {
            List<int> keyList = new List<int>(pilots.Keys);
            Pilot randomPilot = pilots[keyList[Rng.Next(keyList.Count)]];
            while (true)
            {
                if (randomPilot.hasAbility == false)
                {
                    randomPilot = pilots[keyList[Rng.Next(keyList.Count)]];
                }
                else
                {
                    break;
                }
            }
            return randomPilot;
        }

        public static List<Pilot> GetPilots(Build build)
        {
            List<Pilot> pilotsToReturn = new List<Pilot>();
            List<Pilot> uniquePilotsInBuild = new List<Pilot>();
            foreach(UniquePilot uniquePilot in build.pilots.Values)
            {
                if(uniquePilot.pilot.isUnique) { uniquePilotsInBuild.Add(uniquePilot.pilot); }
            }

            foreach (Pilot pilot in pilots.Values)
            {
                
                if (pilot.faction == build.faction && uniquePilotsInBuild.Contains(pilot) == false)
                {
                    pilotsToReturn.Add(pilot);
                }
            }
            return pilotsToReturn;
        }
        public static List<Pilot> GetPilots(Build build, UniquePilot uniquePilot)
        {
            List<Pilot> pilotsToReturn = new List<Pilot>();
            List<Pilot> uniquePilotsInBuild = new List<Pilot>();
            foreach (UniquePilot uniquePilotToTest in build.pilots.Values)
            {
                if (uniquePilotToTest.pilot.isUnique && uniquePilotToTest.pilot != uniquePilot.pilot) { uniquePilotsInBuild.Add(uniquePilotToTest.pilot); }
            }
            
            foreach (Pilot pilot in pilots.Values)
            {
                if (pilot.faction == build.faction && pilot.ship.shipType == uniquePilot.pilot.ship.shipType && uniquePilotsInBuild.Contains(pilot) == false)
                {
                    pilotsToReturn.Add(pilot);
                }
            }
            return pilotsToReturn;
        }
    }
}
