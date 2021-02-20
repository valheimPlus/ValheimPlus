using System.Collections.Generic;

namespace ValheimPlus.ConsolePlus
{
    interface IValheimPlusCommand
    {
        List<string> Arguments { get; }
        string CommandName { get; }
        bool RequiresAdmin { get; }
        void Execute(params object[] args);
    }
}
