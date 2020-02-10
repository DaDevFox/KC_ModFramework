using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Assets;
using Assets.Code;

namespace BuildingFramework.Code {
   public class BuildingFramework: MonoBehaviour
   {

        public static KCModHelper helper;

        public struct ModBuilding
        {
            public GameObject buildingPrefab;
            
        }

        void Preload(KCModHelper _helper)
        {
            helper = _helper;
            String modpath = helper.modPath;
        }

        public static void registerBuilding()
        {
            
        }


   }
}