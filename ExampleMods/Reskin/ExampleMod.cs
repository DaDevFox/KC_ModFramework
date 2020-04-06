using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Reflection;
using Harmony;

namespace Sample_Mods
{
    class ExampleMod : MonoBehaviour
    {
        public static KCModHelper helper;
        public static GameObject houseMesh;

        void Preload(KCModHelper _helper)
        {

            helper = _helper;


            AssetBundle bundle = KCModHelper.LoadAssetBundle(helper.modPath, "buildingreplacemodeltest");
            houseMesh = bundle.LoadAsset("Assets/AssetBundles/ExampleHouse.prefab") as GameObject;

            BuildingFramework.HospitalBuildingModelInfo modelInfo = new BuildingFramework.HospitalBuildingModelInfo();
            modelInfo.model = houseMesh;

            BuildingFramework.ModelHelper.ReskinBuildingBase(modelInfo);
        }
    }

    


}
