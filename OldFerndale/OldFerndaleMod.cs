﻿using System.Reflection;
using JetBrains.Annotations;
using MSCLoader;
using OldFerndale.Features;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

// ReSharper disable MemberCanBePrivate.Global

namespace OldFerndale
{
    [UsedImplicitly]
    public class OldFerndaleMod : Mod
    {
        public override string ID => "OldFerndale";
        public override string Name => "Old Ferndale";
        public override string Author => "アカツキ";
        public override string Version => "1.3.4";
        public override string Description => "Old Ferndale mod for My Summer Car.";

        internal SettingsSliderInt SettingOldSkin;
        internal SettingsSliderInt SettingOldWheels;
        internal SettingsSliderInt SettingTachometer;
        internal SettingsCheckBox SettingRemoveScoop;
        internal SettingsCheckBox SettingOldEngine;
        internal SettingsCheckBox SettingRemoveLinelockButton;
        internal SettingsCheckBox SettingOldLicensePlate;
        internal SettingsCheckBox SettingRemoveMudflaps;
        internal SettingsCheckBox SettingOldSuspension;
        internal SettingsCheckBox SettingRemoveRearAxle;
        internal SettingsCheckBox SettingRedInterior;

        public override void ModSetup()
        {
            SetupFunction(Setup.OnLoad, Mod_Load);
            base.ModSetup();
        }

        public override void ModSettings()
        {
            base.ModSettings();

            Settings.AddHeader(this, "Settings");
            SettingOldSkin = Settings.AddSlider(this, "skin", "Skin", 0, 5, 1,
                textValues: new[] { "Leave as is", "2016", "build 178", "Red", "Blue", "Black" });
            SettingOldWheels = Settings.AddSlider(this, "oldWheels", "Old Wheels", 0, 3, 1,
                textValues: new[] { "No change", "Wheels from 2016", "Wheels from Build 178", "Build <176" });
            SettingTachometer = Settings.AddSlider(this, "tachometer", "Tachometer", 0, 2, 1,
                textValues: new[] { "Unchanged", "Old", "Remove" });
            SettingRemoveScoop = Settings.AddCheckBox(this, "removeScoop", "Remove Scoop", true);
            SettingOldEngine = Settings.AddCheckBox(this, "oldEngine", "Old Engine", true);
            SettingRemoveLinelockButton = Settings.AddCheckBox(this, "linelock", "Remove Linelock Button", true);
            SettingRemoveMudflaps = Settings.AddCheckBox(this, "removeMudflaps", "Remove Mudflaps", true);
            SettingOldSuspension = Settings.AddCheckBox(this, "oldSuspension", "Old Suspension", true);
            SettingRemoveRearAxle = Settings.AddCheckBox(this, "removeRearAxle", "Remove Rear Axle", true);
            SettingRedInterior = Settings.AddCheckBox(this, "redInterior", "Red Interior");
            SettingOldLicensePlate = Settings.AddCheckBox(this, "oldLicPlate", "Old License Plate");

            Settings.AddButton(this, "Suggest new features",
                () =>
                {
                    ModUI.ShowYesNoMessage("Open website: https://mldkyt.com/suggestions?type=oldferndale",
                        () => { Application.OpenURL("https://mldkyt.com/suggestions?type=oldferndale"); });
                });
            Settings.AddHeader(this, "Thanks to");
            Settings.AddText(this,
                "<b>wojskoda</b> - Suggested most of the features and sent some of the assets that are used in this mod");
            Settings.AddText(this,
                "<b>Amistech</b> - All of the models and textures used in this mod were originally made by Amistech");
        }

        private void Mod_Load()
        {
            var resource = LoadResource();
            
            OldSkin.ApplyOldSkin(resource, SettingOldSkin);
            RemoveScoop.ApplyRemoveScoop(SettingRemoveScoop);
            OldEngine.ApplyOldEngine(SettingOldEngine);
            RemoveLinelock.ApplyRemoveLinelock(SettingRemoveLinelockButton);
            OldLicensePlate.ApplyOldLicensePlate(SettingOldLicensePlate);
            RemoveMudflaps.ApplyRemoveMudflaps(resource, SettingRemoveMudflaps);
            OldSuspension.ApplyOldSuspension(SettingOldSuspension);
            OldRims.ApplyOldRims(resource, SettingOldWheels);
            OldTachometer.ApplyOldTachometer(resource, SettingTachometer);
            RemoveRearAxle.ApplyRemoveRearAxle(SettingRemoveRearAxle);
            RedInterior.ApplyRedInterior(resource, SettingRedInterior);

            resource.Unload(false);
        }

        private AssetBundle LoadResource()
        {
            byte[] data;

            using (var stream = Assembly.GetExecutingAssembly()
                       .GetManifestResourceStream("OldFerndale.Resources.oldferndale.unity3d"))
            {
                Debug.Assert(stream != null, nameof(stream) + " != null");
                data = new byte[stream.Length];
                _ = stream.Read(data, 0, data.Length);
            }

            return AssetBundle.CreateFromMemoryImmediate(data);
        }
    }
}