using System;
using System.Collections.Generic;

namespace ValheimPlus
{    
    /// <summary>
    /// PieceEntry
    /// </summary>
    [Serializable]
    public class PieceEntry
    {
        public string Name;
        public string CraftingStationType;
        public bool Enabled;
        public List<PieceEntryRequirement> Requirements = new List<PieceEntryRequirement>();
        
        /// <summary>
        /// Constructor
        /// </summary>
        public PieceEntry()
        {

        }
        
        /// <summary>
        /// Construct from an existing Piece
        /// </summary>
        /// <param name="from_piece"></param>
         public PieceEntry(Piece from_piece)
         {
            Name                        = from_piece.name;
            Enabled                     = from_piece.m_enabled;
            CraftingStationType         = from_piece.m_craftingStation != null ? from_piece.m_craftingStation.name : "";

            foreach (Piece.Requirement requirement in from_piece.m_resources)
            {
                Requirements.Add(new PieceEntryRequirement(requirement));
            }
         }
        
        /// <summary>
        /// Check if this piece is valid
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return  (Name != null && Name.Length > 0 );
        }
        
        /// <summary>
        /// Serialize the object into a ZPackage
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
        /// Unserialize from a ZPackage into this object
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
