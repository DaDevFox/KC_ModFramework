using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Reflection;
using Harmony;

namespace ClassLibrary1
{
    class ExampleMod : MonoBehaviour
    {
        public static KCModHelper helper;
        public static Mesh houseMesh;

        void Preload(KCModHelper _helper)
        {

            helper = _helper;

            var harmony = HarmonyInstance.Create("harmony");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            AssetBundle bundle = KCModHelper.LoadAssetBundle(helper.modPath, "buildingreplacemodeltest");
            GameObject meshObj = bundle.LoadAsset("Assets/KCAssets/Models/ExampleHouse.fbx") as GameObject;
            houseMesh = meshObj.transform.Find("Cube").GetComponent<MeshFilter>().mesh;

        }

    }

    [HarmonyPatch(typeof(GameState))]
    [HarmonyPatch("Start")]
    public static class GameStatePatch
    {
        static void Postfix()
        { 
            BuildingFramework.Code.BuildingFramework.ReplaceBuildingModelBase("archerschool", ExampleMod.houseMesh);
        }
    }


}
