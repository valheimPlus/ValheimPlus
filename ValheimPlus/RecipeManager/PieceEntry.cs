using System;
using System.Collections.Generic;

namespace ValheimPlus
{    
    /// <summary>
    /// RecipeEntry
    /// </summary>
    [Serializable]
    public class PieceEntry
    {
        public string Name;
        public string CraftingStationType;
        public bool Enabled;

        public bool IsUpgrade;
        public int Comfort;
        public string ComfortGroup;
        public bool GroundPiece;
        public bool AllowAltGroundPlacement;
        public bool GroundOnly;
        public bool CultivatedGroundOnly;
        public bool WaterPiece;
        public bool ClipGround;
        public bool ClipEverything;
        public bool NoInWater;
        public bool NotOnWood;
        public bool NotOnTiltingSurface;
        public bool InCeilingOnly;
        public bool NotOnFloor;
        public bool NoClipping;
        public bool OnlyInTeleportArea;
        public bool AllowedInDungeons;
        public float SpaceRequirement;
        public bool RepairPiece;
        public bool CanBeRemoved;

        public List<PieceEntryRequirement> Requirements = new List<PieceEntryRequirement>();
        
        /// <summary>
        /// 
        /// </summary>
        public PieceEntry()
        {

        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="from_piece"></param>
         public PieceEntry(Piece from_piece)
         {
            Name                        = from_piece.name;
            Enabled                     = from_piece.m_enabled;
            CraftingStationType         = from_piece.m_craftingStation != null ? from_piece.m_craftingStation.name : "";
            IsUpgrade                   = from_piece.m_isUpgrade;
            Comfort                     = from_piece.m_comfort;
            ComfortGroup                = from_piece.m_comfortGroup.ToString();
            GroundPiece                 = from_piece.m_groundPiece;
            AllowAltGroundPlacement     = from_piece.m_allowAltGroundPlacement;
            GroundOnly                  = from_piece.m_groundOnly;
            CultivatedGroundOnly        = from_piece.m_cultivatedGroundOnly;
            WaterPiece                  = from_piece.m_waterPiece;
            ClipGround                  = from_piece.m_clipGround;
            ClipEverything              = from_piece.m_clipEverything;
            NoInWater                   = from_piece.m_noInWater;
            NotOnWood                   = from_piece.m_notOnWood;
            NotOnTiltingSurface         = from_piece.m_notOnTiltingSurface;
            InCeilingOnly               = from_piece.m_inCeilingOnly;
            NotOnFloor                  = from_piece.m_notOnFloor;
            NoClipping                  = from_piece.m_noClipping;
            OnlyInTeleportArea          = from_piece.m_onlyInTeleportArea;
            AllowedInDungeons           = from_piece.m_allowedInDungeons;
            SpaceRequirement            = from_piece.m_spaceRequirement;
            RepairPiece                 = from_piece.m_repairPiece;
            CanBeRemoved                = from_piece.m_canBeRemoved;

            foreach (Piece.Requirement requirement in from_piece.m_resources)
            {
                Requirements.Add(new PieceEntryRequirement(requirement));
            }
         }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return  (Name != null && Name.Length > 0 );
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="package"></param>
        public void Serialize(ZPackage package)
        {
            package.Write(Name);
            package.Write(CraftingStationType);
            package.Write(Enabled);
            package.Write(Requirements.Count);

            foreach(PieceEntryRequirement requirement in Requirements)
            {
                requirement.Serialize(package);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="package"></param>
        public void Unserialize(ZPackage package)
        {
            Name = package.ReadString();
            CraftingStationType = package.ReadString();
            Enabled = package.ReadBool();

            Requirements.Clear();

            int requirement_count = package.ReadInt();

            if ( requirement_count > 0 )
            {
                for ( int i = 0; i < requirement_count; i++)
                {
                    PieceEntryRequirement requirement = new PieceEntryRequirement();

                    requirement.Unserialize(package);

                    Requirements.Add(requirement);
                }
            }
        }
    }
}
