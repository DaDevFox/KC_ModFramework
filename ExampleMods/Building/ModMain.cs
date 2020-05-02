using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Assets;
using Assets.Code;


public class ModMain: MonoBehaviour
{
    public static KCModHelper helper;
    public static AssetBundle assetBundle;
    public static GameObject cannonPrefab;
    public static GameObject cannonBallPrefab;

    public void Preload(KCModHelper _helper)
    {
        helper = _helper;
        String modpath = helper.modPath;
        assetBundle = KCModHelper.LoadAssetBundle(modpath + "\\AssetBundle", "cannonbundle");

        if(assetBundle != null)
        {
            cannonPrefab = assetBundle.LoadAsset("assets/workspace/Cannon.prefab") as GameObject;
            cannonBallPrefab = assetBundle.LoadAsset("assets/workspace/CannonBall.prefab") as GameObject;
            
            if(cannonPrefab == null)
                helper.Log("Cannon Prefab Could Not Be Loaded");
            if(cannonBallPrefab == null)
                helper.Log("CannonBall Prefab Could Not Be Loaded");

            Projectile cannonP = cannonBallPrefab.AddComponent<Projectile>();
            cannonP.attackDamage = 85f;
            cannonP.hitRadius = 1f;

            Building cannonB = cannonPrefab.AddComponent<Building>();
            ProjectileDefense cannonPD = cannonPrefab.AddComponent<ProjectileDefense>();
            MaxRangeDisplay cannonMRD = cannonPrefab.AddComponent<MaxRangeDisplay>();
            BuildingCollider cannonCOL = cannonPrefab.transform.Find("Offset").Find("cannon").gameObject.AddComponent<BuildingCollider>();
            Cannon cannon = cannonPrefab.AddComponent<Cannon>();
            
            cannon.b = cannonB;
            cannon.pd = cannonPD;
            cannon.flag = cannonPrefab.transform.Find("Offset").Find("cannon").Find("flag").gameObject;
            cannon.veteranDecor = cannonPrefab.transform.Find("Offset").Find("cannon").Find("cannon_veteran");
            cannon.RotateParent = cannonPrefab.transform.Find("Offset").Find("cannon").Find("cannon_top");
            cannonPD.b = cannonB;
            cannonPD.projectilePrefab = cannonP;
            cannonPD.AttackTime = 10f;
            cannonPD.AttackRange = 5f;
            cannonPD.TrackingRange = 7f;
            cannonPD.rangeIncreasePerHeight = 3f;
            cannonCOL.Building = cannonB;
 
            BuildingInfo cannonInfo = new BuildingInfo(cannonPrefab, "cannon");
            cannonInfo.building = cannonB;
            cannonInfo.customName = "Cannon";
            cannonInfo.descOverride = "Cannon that goes boom boom...";
            cannonInfo.workersForFullYield = 4;
            cannonInfo.buildAllowedWorkers = 6;
            cannonInfo.placementSounds = new string[] {"castleplacement"};
            cannonInfo.selectionSounds = new string[] {"BuildingSelectCastleGate"};
            cannonInfo.skillUsed = "Ballisteer";
            cannonInfo.jobCategory = JobCategory.Ballista;
            cannonInfo.categoryName = "projectiletopper";
            cannonInfo.ignoreRoadCoverageForPlacement = true;

            ResourceAmount cannonResourceAmount = default(ResourceAmount);
            cannonResourceAmount.Add(ResourceAmount.Make(FreeResourceType.Tree, 45));
            cannonResourceAmount.Add(ResourceAmount.Make(FreeResourceType.Stone, 10));
            cannonResourceAmount.Add(ResourceAmount.Make(FreeResourceType.Gold, 35));
            cannonResourceAmount.Add(ResourceAmount.Make(FreeResourceType.IronOre, 10));

            cannonInfo.buildingCost = cannonResourceAmount;
            cannonInfo.personPositions = new Transform[4];
            cannonInfo.preqBuilding = "chamberofwar";
            cannonInfo.tabCategory = "Castle";

            Transform cannon_offset = cannonPrefab.transform.Find("Offset");
            Transform[] cannon_peepo_positions = new Transform[4];
            for(int i = 1; i < 5; i++)
            {
                
                Transform peep = cannon_offset.Find(("p"+ i.ToString()));
                cannon_peepo_positions[i-1] = peep;
            }

            cannonInfo.personPositions = cannon_peepo_positions;

            BuildingHelper.RegisterBuilding(cannonInfo);

            
            // Initalizing Harmony Patches
            var harmony = HarmonyInstance.Create("harmony");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        else
        {
            helper.Log("Bundle Not Loaded");
        }

        
    }


    public void SceneLoaded(KCModHelper _helper)
    {
    }
    


    [HarmonyPatch(typeof(GameState))]
    [HarmonyPatch("Start")]
    public static class CopyingMaxRangeDisplayPatch
    {
        static void Postfix(GameState __instance)
        {   
            Building b = __instance.GetPlaceableByUniqueName("ballista");
            MaxRangeDisplay mrd = b.transform.GetComponent<MaxRangeDisplay>();

            MaxRangeDisplay cMRD = cannonPrefab.GetComponent<MaxRangeDisplay>();

            cMRD.ring = mrd.ring;
            cMRD.cylinder = mrd.cylinder;
            cMRD.material = mrd.material;
            cMRD.inRangeMat = mrd.inRangeMat;
            cMRD.disableMaterial = mrd.disableMaterial;
            cMRD.inDisableMaterial = mrd.inDisableMaterial;
        }
    }

}
