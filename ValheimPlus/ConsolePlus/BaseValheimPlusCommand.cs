using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ValheimPlus.ConsolePlus
{
    public abstract class BaseValheimPlusCommand : IValheimPlusCommand
    {
        public virtual List<string> Arguments => throw new NotImplementedException();
        public virtual string CommandName => throw new NotImplementedException();
        public virtual bool RequiresAdmin => throw new NotImplementedException();

        public virtual void Execute(params object[] args)
        {
            if(ZNet.instance.IsServer())
            {

            }
        }

        protected abstract void ExecuteServerContext(params object[] args);
        protected abstract void ExecuteClientContext();
    }
}
