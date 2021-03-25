namespace ValheimPlus.Configurations.Sections
{
    public class RecipeManagerConfiguration : BaseConfig<RecipeManagerConfiguration>
    {
        public string databaseFile { get; set; } = "recipes.json";
        public string databaseDumpFile { get; set; } = "";
    }
}
