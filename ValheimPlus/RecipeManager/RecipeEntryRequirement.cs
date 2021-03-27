using System;

namespace ValheimPlus
{
    /// <summary>
    /// RecipeEntryRequirement
    /// </summary>
    [Serializable]
    public class RecipeEntryRequirement
    {
        public string ItemName;
        public int Amount;
        public int AmountPerLevel;
        public bool Recover;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public RecipeEntryRequirement()
        {

        }

        /// <summary>
        /// Construct an instance from a Piece.Requirement object
        /// </summary>
        /// <param name="from_requirement"></param>
        public RecipeEntryRequirement(Piece.Requirement from_requirement)
        {
            ItemName        = from_requirement.m_resItem != null ? from_requirement.m_resItem.name : "";
            Amount          = from_requirement.m_amount;
            AmountPerLevel  = from_requirement.m_amountPerLevel;
            Recover         = from_requirement.m_recover;
        }

        /// <summary>
        /// Check if this requirement is valid
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return ItemName != null && ItemName.Length > 0 &&
                   Amount >= 0 &&
                   AmountPerLevel >= 0;
        }

        /// <summary>
        /// Serialize the object into a ZPackage
        /// </summary>
        /// <param name="package"></param>
        public void Serialize(ZPackage package)
        {
            package.Write(ItemName);
            package.Write(Amount);
            package.Write(AmountPerLevel);
            package.Write(Recover);
        }

        /// <summary>
        /// Unserialize from a ZPackage into this object
        /// </summary>
        /// <param name="package"></param>
        public void Unserialize(ZPackage package)
        {
            ItemName        = package.ReadString();
            Amount          = package.ReadInt();
            AmountPerLevel  = package.ReadInt();
            Recover         = package.ReadBool();
        }
    }
}
