using System;
using UnityEngine;

namespace VARP.DebugMenus
{
    public class DebugMenuInteger : DebugMenuItem
    {
        private readonly Func<int> getter;
        private readonly Action<int> setter;
        private readonly string format;
        private readonly int increment;
        
        public DebugMenuInteger(string path, Func<int> getter, Action<int> setter, int order = 0,
            int increment = 1, string format = null)
            : base(path, order)
        {
            this.getter = getter;
            this.setter = setter;
            this.format = format ?? "0";
            this.increment = increment;
            Render();
        }
        
        public override void OnEvent(DebugMenuC sender, DebugMenu.EvenTag tag)
        {
            switch (tag)
            {
                case DebugMenu.EvenTag.Render:
                    Render();
                    break;
                case DebugMenu.EvenTag.Increment:
                    setter(getter() + increment);
                    Render();
                    break;
                case DebugMenu.EvenTag.Decrement:
                    setter(getter() - increment);
                    Render();
                    break;
            }
        }

        private void Render()
        {
            value = getter().ToString(format);
        }
    }
}