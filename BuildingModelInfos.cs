using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BuildingFramework
{
    #region BuildingModelInfo Definitions

    #region Base

    //Base
    public class BuildingModelInfo
    {
        public delegate void RemovalProcedure(Building b);
        public delegate void ReplaceProcedure(Building b);


        public RemovalProcedure removalProcedure
        {
            get
            {
                return _removalProcedure;
            }
        }
        public ReplaceProcedure replaceProcedure
        {
            get
            {
                return _replaceProcedure;
            }
        }

        protected RemovalProcedure _removalProcedure;
        protected ReplaceProcedure _replaceProcedure;


        public string buildingUniqueName
        {
            get
            {
                return _buildingUniqueName;
            }
        }

        protected string _buildingUniqueName = "";

    }

    //Generic
    public class GenericBuildingModelInfo : BuildingModelInfo
    {
        public GenericBuildingModelInfo()
        {
            _removalProcedure = delegate (Building b) { Procedures.rp_base(b); };
            _replaceProcedure = delegate (Building b) { Procedures.rep_base(b, this); };
        }


        public GameObject model;
    }

    #endregion

    //Keep
    public class KeepBuildingModelInfo : BuildingModelInfo
    {
        public KeepBuildingModelInfo()
        {
            _removalProcedure = delegate (Building b) { Procedures.rp_keep(b); };
            _replaceProcedure = delegate (Building b) { Procedures.rep_keep(b, this); };

            _buildingUniqueName = "keep";
        }

        public GameObject keepUpgrade1;
        public GameObject keepUpgrade2;
        public GameObject keepUpgrade3;
        public GameObject keepUpgrade4;
    }

    //Wood Castle Block
    public class WoodCastleBlockBuildingModelInfo : BuildingModelInfo
    {
        public WoodCastleBlockBuildingModelInfo()
        {
            _removalProcedure = delegate (Building b) { Procedures.rp_woodcastleblock(b); };
            _replaceProcedure = delegate (Building b) { Procedures.rep_woodcastleblock(b, this); };

            _buildingUniqueName = "woodcastleblock";
        }


        public GameObject Open;
        public GameObject Closed;
        public GameObject Single;
        public GameObject Opposite;
        public GameObject Adjacent;
        public GameObject Threeside;

        public GameObject doorPrefab;

    }

    //Wooden Gate
    public class WoodenGadeBuildingModelInfo
    {




        public GameObject gate;
        public GameObject porticulus;

    }


    //Hospital
    public class HospitalBuildingModelInfo : GenericBuildingModelInfo
    {
        public HospitalBuildingModelInfo()
        {
            _buildingUniqueName = "hospital";
        }
    }


    #endregion

}
