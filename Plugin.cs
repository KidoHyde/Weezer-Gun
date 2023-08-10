using BepInEx;
using CrashUtils.WeaponManager.WeaponSetup;
using HarmonyLib;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace WeezerGunUtils
{
    [BepInPlugin("Crash.WeezerGun", "Weezer Gun", "1.0.0")]
    public class WeezerGunLoader : BaseUnityPlugin
    {
        public static WeezerGunLoader CG;
        public static WeezerGun WeezerGun;

        public static AssetBundle Assets;
        public static Harmony Harmony = new Harmony("Crash.WeezerGun");


        private void Awake()
        {
            CG = this;
            Logger.LogInfo($"Plugin Crash.WeezerGun is loaded!");
            Harmony.PatchAll(typeof(WeezerGunLoader));
            Assets = AssetBundle.LoadFromFile(Path.Combine(ModPath(), "weezergun"));


            WeezerGun.LoadAssets();
            GunAdditives.Register(new WeezerGun());


        }
        

        public static string ModPath()
        {
            return Assembly.GetExecutingAssembly().Location.Substring(0, Assembly.GetExecutingAssembly().Location.LastIndexOf(Path.DirectorySeparatorChar));
        }



    }


}
