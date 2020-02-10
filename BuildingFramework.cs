using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Assets;
using Assets.Code;


namespace BuildingFramework.Code
{




    public class BuildingFramework : MonoBehaviour
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

        public static void ReplaceBuildingModelBase(string buildingUniqueName, Mesh newModel)
        {
            Building b = null;
            List<Building> internalPrefabs = typeof(GameState).GetField("internalPrefabs", BindingFlags.NonPublic).GetValue(GameState.inst) as List<Building>;
            for (int i = 0; i < internalPrefabs.Count; i++)
            {
                if (internalPrefabs[i].UniqueName == buildingUniqueName)
                {
                    b = internalPrefabs[i];
                }
            }
            if (b == null)
            {
                helper.Log("Building with UniqueName " + buildingUniqueName + " not found, make sure it is registered in GameState");
            }
            GameObject model = b.gameObject.transform.GetChild(0).gameObject;
            model.GetComponent<MeshFilter>().mesh = newModel;

        }

        public static MeshFilter[] GetBuildingMeshes(string buildingUniqueName)
        {
            Building b = null;
            List<Building> internalPrefabs = typeof(GameState).GetField("internalPrefabs", BindingFlags.NonPublic).GetValue(GameState.inst) as List<Building>;
            for (int i = 0; i < internalPrefabs.Count; i++)
            {
                if (internalPrefabs[i].UniqueName == buildingUniqueName)
                {
                    b = internalPrefabs[i];
                }
            }
            return b.GetComponentsInChildren<MeshFilter>();

        }



    }
}