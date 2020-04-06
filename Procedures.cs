
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BuildingFramework
{
    class Procedures
    {


        #region Base

        //Generic removal procedure; Works for most buildings
        internal static void rp_base(Building b)
        {
            GameObject model = b.gameObject.transform.Find("Offset").GetChild(0).gameObject;
            model.GetComponent<MeshFilter>().mesh = null;
        }

        internal static void rep_base(Building b, GenericBuildingModelInfo modelInfo)
        {
            GameObject model = b.gameObject.transform.Find("Offset").GetChild(0).gameObject;
            GameObject obj = GameObject.Instantiate(modelInfo.model);
            obj.transform.SetParent(model.transform);
        }

        #endregion

        #region Keep

        internal static void rp_keep(Building b)
        {
            GameObject keepUpgrade1 = b.transform.Find("Offset/SmallerKeep").gameObject;
            GameObject keepUpgrade2 = b.transform.Find("Offset/SmallKeep").gameObject;
            GameObject keepUpgrade3 = b.transform.Find("Offset/Keep").gameObject;
            GameObject keepUpgrade4 = b.transform.Find("Offset/MediumKeep").gameObject;

            MeshFilter m_1 = keepUpgrade1.GetComponent<MeshFilter>();
            MeshFilter m_2 = keepUpgrade2.GetComponent<MeshFilter>();
            MeshFilter m_3 = keepUpgrade3.GetComponent<MeshFilter>();
            MeshFilter m_4 = keepUpgrade4.GetComponent<MeshFilter>();

            m_1.mesh = null;
            m_2.mesh = null;
            m_3.mesh = null;

        }

        internal static void rep_keep(Building b, KeepBuildingModelInfo modelInfo)
        {
            GameObject keepUpgrade1 = b.transform.Find("Offset/SmallerKeep").gameObject;
            GameObject keepUpgrade2 = b.transform.Find("Offset/SmallKeep").gameObject;
            GameObject keepUpgrade3 = b.transform.Find("Offset/Keep").gameObject;
            GameObject keepUpgrade4 = b.transform.Find("Offset/MediumKeep").gameObject;

            GameObject.Instantiate(modelInfo.keepUpgrade1, keepUpgrade1.transform);
            GameObject.Instantiate(modelInfo.keepUpgrade2, keepUpgrade2.transform);
            GameObject.Instantiate(modelInfo.keepUpgrade3, keepUpgrade3.transform);
            GameObject.Instantiate(modelInfo.keepUpgrade4, keepUpgrade4.transform);
        }

        #endregion

        #region Wood Castle Block
        internal static void rp_woodcastleblock(Building b)
        {
            CastleBlock cb = b.gameObject.GetComponent<CastleBlock>();
            cb.Adjacent = null;
            cb.Single = null;
            cb.Opposite = null;
            cb.Threeside = null;
            cb.doorPrefab = null;
            cb.Closed = null;
            cb.Open = null;


        }

        internal static void rep_woodcastleblock(Building b, WoodCastleBlockBuildingModelInfo modelInfo)
        {
            CastleBlock cb = b.gameObject.GetComponent<CastleBlock>();
            cb.Adjacent = modelInfo.Adjacent.transform;
            cb.Single = modelInfo.Single.transform;
            cb.Opposite = modelInfo.Opposite.transform;
            cb.Threeside = modelInfo.Threeside.transform;
            cb.doorPrefab = modelInfo.doorPrefab;
            cb.Closed = modelInfo.Closed.transform;
            cb.Open = modelInfo.Open.transform;
        }

        #endregion

        #region Orchard

        internal static void rp_orchard(Building b)
        {
            GameObject model = b.gameObject.transform.Find("Offset/Container").GetChild(0).gameObject;
            GameObject.Destroy(model);
        }

        #endregion

    }
}
