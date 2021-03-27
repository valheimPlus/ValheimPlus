using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using fastJSON;
using ValheimPlus.Configurations;
using BepInEx;

namespace ValheimPlus
{
    /// <summary>
    /// RecipeManager
    /// </summary>
    public class RecipeManager
    {
        public RecipeConfig Config { get; internal set; } = new RecipeConfig();
        public static RecipeManager instance = null;
        protected List<int> SeenTables = new List<int>();

        /// <summary>
        /// Constructor
        /// </summary>
        public RecipeManager()
        {

        }
        
        /// <summary>
        /// Initializes the global instance
        /// </summary>
        public static void Initialize()
        {
            if (RecipeManager.instance == null)
                RecipeManager.instance = new RecipeManager();
        }

        /// <summary>
        /// DeInitializes the global instance
        /// </summary>
        public static void DeInitialize()
        {
            if (RecipeManager.instance != null)
                //RecipeManager.instance.RestoreOriginal();
                RecipeManager.instance = null;
        }

        /// <summary>
        /// FindDatabaseRecipe
        /// 
        /// Look for a Recipe object from ObjectDB
        /// </summary>
        /// <param name="name">The recipe name</param>
        /// <returns></returns>
        public Recipe FindDatabaseRecipe(String name)
        {
            return ObjectDB.instance.m_recipes.Find(x => x.name == name);
        }

        /// <summary>
        /// Find a workbench recipe entry by its name
        /// </summary>
        /// <param name="name">The recipe name</param>
        /// <returns></returns>
        
        public RecipeEntry FindRecipe(String name)
        {
            return Config.Recipes.Find(x => x.Name == name);
        }
        
        /// <summary>
        /// Find a piece recipe entry by its name
        /// </summary>
        /// <param name="name">The recipe name</param>
        /// <returns></returns>
        public PieceEntry FindPiece(String name)
        {
            return Config.Pieces.Find(x => x.Name == name);
        }

        /// <summary>
        /// Adds a recipe from a config entry, replacing any existing entry
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool AddRecipe(RecipeEntry config)
        {
            if (!config.IsValid())
            {
                Debug.Log($"Invalid Config for Recipe `{config.Name}`");
                return false;
            }

            int removed = Config.Recipes.RemoveAll(x => x.Name == config.Name);

            if (removed > 0)
            {
                Debug.Log($"Replaced Recipe `{config.Name}`");
            }
            else
            {
                Debug.Log($"Added Recipe `{config.Name}`");
            }

            Config.Recipes.Add(config);

            return true;
        }

        /// <summary>
        /// Adds a recipe from a config entry, replacing any existing entry
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public bool AddPiece(PieceEntry config)
        {
            if (!config.IsValid())
            {
                Debug.Log($"Invalid Config for Piece `{config.Name}`");
                return false;
            }

            int removed = Config.Pieces.RemoveAll(x => x.Name == config.Name);

            if (removed > 0)
            {
                Debug.Log($"Replaced Piece `{config.Name}`");
            }
            else
            {
                Debug.Log($"Added Piece `{config.Name}`");
            }

            Config.Pieces.Add(config);

            return true;
        }

        /// <summary>
        /// Creates a new Recipe object from a config entry
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public Recipe CreateRecipe(RecipeEntry config)
        {
            GameObject item_object = null;
            ItemDrop item_drop = null;
            GameObject requirement_item_object = null;
            ItemDrop requirement_item_drop = null;

            if (!config.IsValid())
            {
                throw new Exception("Configuration Invalid");
            }

            if ((item_object = GetGameObject(config.ItemName)) == null)
            {
                throw new Exception(String.Format("Item Object `{0}` not found for recipe `{1}", config.ItemName, config.Name));
            }
            else if ((item_drop = item_object.GetComponent<ItemDrop>()) == null)
            {
                throw new Exception(String.Format("Item Object `{0}` not found for recipe `{1}", config.ItemName, config.Name));
            }
            
            Recipe recipe = ScriptableObject.CreateInstance<Recipe>();

            if (recipe == null)
            {
                throw new Exception("Error creating scriptable object `Recipe`");
            }
    
            recipe.name                 = config.Name;
            recipe.m_amount             = config.Amount;
            recipe.m_enabled            = config.Enabled;
            recipe.m_minStationLevel    = config.MinimumStationLevel;
            recipe.m_resources          = new Piece.Requirement[config.Requirements.Count];
            recipe.m_craftingStation    = null;
            recipe.m_repairStation      = null;
            recipe.m_item               = item_drop;

            if (config.CraftingStationType.Length > 0)
            {
                recipe.m_craftingStation = GetCraftingStation(config.CraftingStationType);
                
                if (recipe.m_craftingStation == null)
                {
                    throw new Exception(String.Format("Unable To Find Crafting Station Type: {0} For Recipe {1}", config.CraftingStationType, recipe.name));
                }
            }

            if (config.RepairStationType.Length > 0)
            {
                recipe.m_repairStation = GetCraftingStation(config.RepairStationType);

                if (recipe.m_repairStation == null)
                {
                    throw new Exception(String.Format("Unable To Find Crafting Station Type: {0} For Recipe {1}", config.RepairStationType, recipe.name)); 
                }
            }

            for (int i = 0; i < config.Requirements.Count; i++)
            {
                recipe.m_resources[i]                   = new Piece.Requirement();                
                recipe.m_resources[i].m_amount          = config.Requirements[i].Amount;
                recipe.m_resources[i].m_amountPerLevel  = config.Requirements[i].AmountPerLevel;
                recipe.m_resources[i].m_recover         = config.Requirements[i].Recover;
                
                if ((requirement_item_object = GetGameObject(config.Requirements[i].ItemName)) == null)
                {
                    throw new Exception(String.Format("Requirement Item Object `{0}` not found for recipe `{1}", config.Requirements[i].ItemName, config.Name));
                }
                else if ((requirement_item_drop = requirement_item_object.GetComponent<ItemDrop>()) == null)
                {
                    throw new Exception(String.Format("Requirement Item Object `{0}` not found for recipe `{1}", config.Requirements[i].ItemName, config.Name));
                }

                recipe.m_resources[i].m_resItem = requirement_item_drop;
            }

            return recipe;
        }

        /// <summary>
        /// Syncs ObjectDB recipe list with configuration
        /// </summary>
        public void Sync()
        {
            foreach (RecipeEntry entry in Config.Recipes)
            {
                Recipe recipe = null;
                
                if(!entry.IsValid())
                {
                    Debug.Log($"Skipping Recipe `{entry.Name} - Invalid");
                    continue;
                }

                try
                {
                    recipe = CreateRecipe(entry);
                } catch(Exception e)
                {
                    Debug.Log($"Skipping Recipe `{entry.Name} - Error creating recipe: {e.Message}");
                    continue;
                }

                if (recipe == null)
                {
                    Debug.Log($"Skipping Recipe `{entry.Name} - Invalid");
                    continue;
                }

                int removed = ObjectDB.instance.m_recipes.RemoveAll(x => x.name == entry.Name);

                if (removed > 0)
                {
                    Debug.Log($"Replaced recipe `{entry.Name}` - Remove count: {removed}");
                }
                else
                {
                    Debug.Log($"Added recipe `{entry.Name}`");
                }

                ObjectDB.instance.m_recipes.Add(recipe);
            }
        }

        /// <summary>
        /// Gets the crafting station object by its name
        /// </summary>
        /// <param name="type">The name of the station</param>
        /// <returns></returns>
        public CraftingStation GetCraftingStation(String type)
        {
            foreach (Recipe r in ObjectDB.instance.m_recipes)
            {
                if (r.m_craftingStation && r.m_craftingStation.name == type)
                {
                    return r.m_craftingStation;
                }
            }

            return null;
        }

        /// <summary>
        /// Updates the configuration from the specified json file path
        /// </summary>
        /// <param name="in_json_path">Full path to the json file</param>
        public void UpdateFrom(String in_json_path)
        {
            try
            {
                String json = File.ReadAllText(in_json_path); 
               
                JSONParameters parameters = new fastJSON.JSONParameters();
                parameters.UseExtensions = false;
                parameters.UseFastGuid = false;

                RecipeConfig read_config = JSON.ToObject<RecipeConfig>(json, parameters);              
                
                UpdateFrom(read_config.Recipes);
                UpdateFrom(read_config.Pieces);
            }
            catch (System.Exception e)
            {
                Debug.Log($"Error parsing json file `{in_json_path}` - {e.Message}");
            }
        }

        /// <summary>
        /// Updates the station recipes configuration with the provided list. Replacing any existing recipes.
        /// </summary>
        /// <param name="config_list"></param>
        public void UpdateFrom(List<RecipeEntry> config_list)
        {
            foreach (RecipeEntry recipe_config in config_list)
            {
                if (!AddRecipe(recipe_config))
                {
                    Debug.Log($"Error adding recipe `{recipe_config.Name}`");
                }
            }
        }

        /// <summary>
        /// Updates the piece recipe configuration with the provided list. Replacing any existing recipes.
        /// </summary>
        /// <param name="config_list"></param>
        public void UpdateFrom(List<PieceEntry> config_list)
        {
            foreach (PieceEntry piece_config in config_list)
            {
                if (!AddPiece(piece_config))
                {
                    Debug.Log($"Error adding piece `{piece_config.Name}`");
                }
            }
        }

        /// <summary>
        /// Get a game object by its name from ObjectDB
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObject GetGameObject(String name)
        {
            return ObjectDB.instance.GetItemPrefab(name);
        }

        public void ProcessPieceTable(PieceTable table)
        {
            UnityEngine.Debug.Log($"ProcessPieceTable [{Config.Pieces.Count}]");
            
            if (table == null)
            {
                UnityEngine.Debug.Log("Null Table");
                return;
            }
            else if (SeenTables.Contains(table.GetInstanceID()))
            {
                UnityEngine.Debug.Log("Seen Table");
                return;
            }
            
            foreach(GameObject pieceObject in table.m_pieces)
            {
                UpdatePiece(pieceObject.GetComponent<Piece>());                
            }

            SeenTables.Add(table.GetInstanceID());
        }

        public void UpdatePiece(Piece piece)
        {
            if (piece == null)  { UnityEngine.Debug.Log("Null Piece"); return; }
            
            PieceEntry config = FindPiece(piece.name);

            if (config == null) { UnityEngine.Debug.Log($"Null Conf {piece.name}"); return; }

            // First resolve requirement items are valid
            Piece.Requirement[] requirements = new Piece.Requirement[ config.Requirements.Count ];
                
            for (int i = 0; i < config.Requirements.Count; i++)
            {
                GameObject requirement_item_object = null;
                ItemDrop requirement_item_drop = null;

                requirements[i]                   = new Piece.Requirement();                
                requirements[i].m_amount          = config.Requirements[i].Amount;
                requirements[i].m_amountPerLevel  = config.Requirements[i].AmountPerLevel;
                requirements[i].m_recover         = config.Requirements[i].Recover;
                
                if ((requirement_item_object = GetGameObject(config.Requirements[i].ItemName)) == null)
                {
                    Debug.Log(String.Format("Requirement Item Object `{0}` not found for piece recipe `{1}", config.Requirements[i].ItemName, config.Name));
                    return;
                }
                else if ((requirement_item_drop = requirement_item_object.GetComponent<ItemDrop>()) == null)
                {
                    Debug.Log(String.Format("Requirement Item Object `{0}` not found for piece recipe `{1}", config.Requirements[i].ItemName, config.Name));
                    return;
                }

                requirements[i].m_resItem = requirement_item_drop;
            }

            // All are valid, apply all values to the piece
            piece.m_enabled                 = config.Enabled;
            piece.m_isUpgrade               = config.IsUpgrade;
            piece.m_comfort                 = config.Comfort;
            //from_piece.m_comfortGroup.ToString() = ComfortGroup;
            piece.m_groundPiece             = config.GroundPiece;
            piece.m_allowAltGroundPlacement = config.AllowAltGroundPlacement;
            piece.m_groundOnly              = config.GroundOnly;
            piece.m_cultivatedGroundOnly    = config.CultivatedGroundOnly;
            piece.m_waterPiece              = config.WaterPiece;
            piece.m_clipGround              = config.ClipGround;
            piece.m_clipEverything          = config.ClipEverything;
            piece.m_noInWater               = config.NoInWater;
            piece.m_notOnWood               = config.NotOnWood;
            piece.m_notOnTiltingSurface     = config.NotOnTiltingSurface;
            piece.m_inCeilingOnly           = config.InCeilingOnly;
            piece.m_notOnFloor              = config.NotOnFloor;
            piece.m_noClipping              = config.NoClipping;
            piece.m_onlyInTeleportArea      = config.OnlyInTeleportArea;
            piece.m_allowedInDungeons       = config.AllowedInDungeons;
            piece.m_spaceRequirement        = config.SpaceRequirement;
            piece.m_repairPiece             = config.RepairPiece;
            piece.m_canBeRemoved            = config.CanBeRemoved;
            piece.m_resources               = requirements;

            Debug.Log(String.Format("Updated Piece Recipe `{0}", config.Name));
        }

        /// <summary>
        /// Executed on game start
        /// </summary>
        public void OnGameStart()
        {
            if (ZNet.instance == null || (ZNet.instance.IsServerInstance() || ZNet.instance.IsLocalInstance()))
            {
                foreach (String _database in Configuration.Current.RecipeManager.databaseFile.Split(Path.PathSeparator))
                {
                    String database = _database.Trim();
                    String path = Path.Combine(Paths.ConfigPath, database);

                    if (Path.IsPathRooted(database))
                    {
                        Debug.Log($"Error loading recipe database @ {path} - Must be relative");
                        continue;
                    }

                    Debug.Log($"Loading Recipe Database: {path}");
                    
                    UpdateFrom(path);
                }

                Sync();
            }
        }

        /// <summary>
        /// Executed on new peer connection
        /// </summary>
        /// <param name="peer"></param>
        public void OnNewConnection(ZNetPeer peer)
        {
            if (ZNet.instance != null && ZNet.instance.IsClientInstance())
            {
                peer.m_rpc.Register("ValheimPlus_RecipeSync", new Action<ZRpc, ZPackage>(this.RPC_Sync));  
            }
        }

        /// <summary>
        /// Sync the current config with the client
        /// </summary>
        /// <param name="rpc"></param>
        public void Sync(ZRpc rpc)
        {
            if (ZNet.instance == null || (!ZNet.instance.IsServerInstance() && !ZNet.instance.IsLocalInstance()))
            {
                return;
            }    
            
            try
            {
                ZPackage package = new ZPackage();
                Config.Serialize(package);            
                rpc.Invoke("ValheimPlus_RecipeSync", (object) package);    
            } catch(Exception e)
            {
                Debug.Log($"RPC_Sync Error: {e.Message}");
                Debug.Log(Environment.StackTrace);
            }
        }

        /// <summary>
        /// Handles the client side sync of recipes recieved from RPC call `ValheimPlus_RecipeSync`
        /// </summary>
        /// <param name="rpc"></param>
        /// <param name="package"></param>
        protected void RPC_Sync(ZRpc rpc, ZPackage package)
        {
            if (ZNet.instance == null || !ZNet.instance.IsClientInstance())
            {
                return;
            }

            try
            {
                //RestoreOriginal();
                Config.Unserialize(package);
                Sync();
            } catch(Exception e)
            {
                Debug.Log($"RPC_Sync Error: {e.Message}");
                Debug.Log($"RPC_Sync Error: {Environment.StackTrace}");
            }
        }
    }
}
