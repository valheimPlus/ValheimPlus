using System;
using System.IO;
using System.Collections.Generic;
using fastJSON;
using UnityEngine;

namespace ValheimPlus
{
    /// <summary>
    /// RecipeConfig
    /// Structure represents the configuration and the recipes for RecipeManager
    /// </summary>
    [Serializable]
    public class RecipeConfig
    {
        public List<RecipeEntry> Recipes = new List<RecipeEntry>();
        public List<PieceEntry> Pieces = new List<PieceEntry>();

        /// <summary>
        /// Constructor
        /// </summary>
        public RecipeConfig()
        {

        }

        /// <summary>
        /// Constructor from existing game Recipe
        /// </summary>
        /// <param name="from_recipes"></param>
        public RecipeConfig(List<Recipe> from_recipes)
        {
            foreach(Recipe recipe in from_recipes)
            {
                Recipes.Add(new RecipeEntry(recipe));
            }
        }

        /// <summary>
        /// Serialize the object into a ZPackage
        /// </summary>
        /// <param name="package"></param>
        public void Serialize(ZPackage package)
        {
            package.Write(Recipes.Count);
            package.Write(Pieces.Count);

            foreach(RecipeEntry entry in Recipes)
            {
                entry.Serialize(package);
            }

            foreach(PieceEntry entry in Pieces)
            {
                entry.Serialize(package);
            }
        }

        /// <summary>
        /// Unserialize from a ZPackage into this object
        /// </summary>
        /// <param name="package"></param>
        public void Unserialize(ZPackage package)
        {
            Recipes.Clear();
            Pieces.Clear();
            
            int recipe_count = package.ReadInt();
            int piece_count = package.ReadInt();

            for (int i = 0; i < recipe_count; i++)
            {
                RecipeEntry entry = new RecipeEntry();

                entry.Unserialize(package);

                if (entry.IsValid())
                {
                    Recipes.Add(entry);
                }
            }

            for (int i = 0; i < piece_count; i++)
            {
                PieceEntry entry = new PieceEntry();

                entry.Unserialize(package);

                if (entry.IsValid())
                {
                    Pieces.Add(entry);
                }
            }
        }
        
        /// <summary>
        /// Save the current configuration to json
        /// </summary>
        /// <param name="out_json_path"></param>
        /// <returns></returns>
        public bool SaveTo(String out_json_path)
        {
            try
            {
                JSONParameters parameters = new JSONParameters();
                parameters.UseExtensions = false;
                parameters.UseFastGuid = false;
   
                File.WriteAllText(out_json_path, JSON.ToNiceJSON(this, parameters));
            }
            catch (System.Exception e)
            {
                Debug.Log($"Error parsing json: {e.Message}");
                return false;
            }

            return true;
        }
    }
}
