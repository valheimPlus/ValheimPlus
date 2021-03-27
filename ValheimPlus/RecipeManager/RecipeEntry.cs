using System;
using System.Collections.Generic;

namespace ValheimPlus
{    
    /// <summary>
    /// RecipeEntry
    /// </summary>
    [Serializable]
    public class RecipeEntry
    {
        public string Name;
        public string ItemName;
        public int Amount;
        public int MinimumStationLevel;
        public string CraftingStationType;
        public string RepairStationType;
        public bool Enabled;
        public List<RecipeEntryRequirement> Requirements = new List<RecipeEntryRequirement>();
        
        /// <summary>
        /// Constructor
        /// </summary>
        public RecipeEntry()
        {

        }
        
        /// <summary>
        /// Constrc from an existing Recipe object
        /// </summary>
        /// <param name="from_recipe"></param>
         public RecipeEntry(Recipe from_recipe)
         {
             Name                = from_recipe.name;
             ItemName            = from_recipe.m_item != null ? from_recipe.m_item.name : "";
             Amount              = from_recipe.m_amount;
             MinimumStationLevel = from_recipe.m_minStationLevel;
             Enabled             = from_recipe.m_enabled;
             CraftingStationType = from_recipe.m_craftingStation != null ? from_recipe.m_craftingStation.name : "";
             RepairStationType   = from_recipe.m_repairStation != null ? from_recipe.m_repairStation.name : "";

             foreach (Piece.Requirement requirement in from_recipe.m_resources)
             {
                 Requirements.Add(new RecipeEntryRequirement(requirement));
             }
         }
        
        /// <summary>
        /// Check if this recipe is valid
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return  (Name != null && Name.Length > 0 )       &&
                 (ItemName != null && ItemName.Length > 0 )  &&
                 Amount >= 0                                 &&
                 MinimumStationLevel >= 0;
        }
        
        /// <summary>
        /// Serialize the object into a ZPackage
        /// </summary>
        /// <param name="package"></param>
        public void Serialize(ZPackage package)
        {
            package.Write(Name);
            package.Write(ItemName);
            package.Write(Amount);
            package.Write(MinimumStationLevel);
            package.Write(CraftingStationType);
            package.Write(RepairStationType);
            package.Write(Enabled);
            package.Write(Requirements.Count);

            foreach(RecipeEntryRequirement requirement in Requirements)
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
            Name                = package.ReadString();
            ItemName            = package.ReadString();
            Amount              = package.ReadInt();
            MinimumStationLevel = package.ReadInt();
            CraftingStationType = package.ReadString();
            RepairStationType   = package.ReadString();
            Enabled             = package.ReadBool();

            Requirements.Clear();

            int requirement_count = package.ReadInt();

            if (requirement_count > 0)
            {
                for (int i = 0; i < requirement_count; i++)
                {
                    RecipeEntryRequirement requirement = new RecipeEntryRequirement();

                    requirement.Unserialize(package);

                    Requirements.Add(requirement);
                }
            }
        }
    }
}
