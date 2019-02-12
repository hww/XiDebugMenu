using System;
using UnityEngine;

namespace VARP.DebugMenus
{
    public class DebugMenuToggle : DebugMenuItem
    {
        private readonly Func<bool> getter;
        private readonly Action<bool> setter;

        public DebugMenuToggle(string path, Func<bool> getter, Action<bool> setter, int order = 0)
            : base(path, order)
        {
            this.getter = getter;
            this.setter = setter;
            this.value = null;    // do not have value, wil display it by color
        }
        
        public override void OnEvent(DebugMenu.EvenTag tag)
        {
            switch (tag)
            {
                case DebugMenu.EvenTag.Render:
                    Render();
                    break;
                case DebugMenu.EvenTag.Increment:
                    setter(!getter());
                    Render();
                    break;
                case DebugMenu.EvenTag.Decrement:
                    setter(!getter());
                    Render();
                    break;
            }
        }

        private void Render()
        {
            nameColor = getter() ? YELLOW : WHITE;
        }
    }
}