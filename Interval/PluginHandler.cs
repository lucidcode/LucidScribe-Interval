using System;

namespace lucidcode.LucidScribe.Plugin.Interval
{
    public class PluginHandler : Interface.LucidPluginBase
    {
        private int interval = 20;

        public override string Name
        {
            get
            {
                return "Interval REM";
            }
        }

        public override bool Initialize()
        {
            return true;
        }

        public override double Value
        {
            get
            {
                if ((DateTime.Now.Second - 1) % interval == 0)
                {
                    return 888;
                }
                return 1;
            }
        }

    }

}