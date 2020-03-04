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

        public struct ModBuilding
        {
            public GameObject buildingPrefab;

        }

        

        public static void registerBuilding()
        {

        }

        public static void ReplaceBuildingModelBase(string buildingUniqueName, Mesh newModel)
        {
            
            Building b = null;
            List<Building> internalPrefabs = typeof(GameState).GetField("internalPrefabs", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(GameState.inst) as List<Building>;
            for (int i = 0; i < internalPrefabs.Count; i++)
            {
                if (internalPrefabs[i].UniqueName == buildingUniqueName)
                {
                    b = internalPrefabs[i];
                }
            }
                
            GameObject model = b.gameObject.transform.Find("Offset").GetChild(0).gameObject;
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