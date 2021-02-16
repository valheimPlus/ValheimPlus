namespace ValheimPlus
{
    class Helper
    {
        public static Character getPlayerCharacter()
        {
            foreach (Character character in Character.GetAllCharacters())
            {
                if (character.IsPlayer())
                {
                    return character;
                }
            }
            return null;
        }
    }
}
