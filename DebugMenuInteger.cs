using System;
using UnityEngine;

namespace VARP.DebugMenus
{
    public class DebugMenuInteger : DebugMenuItem
    {
        private readonly Func<int> getter;
        private readonly Action<int> setter;

        public DebugMenuInteger(string path, Func<int> getter, Action<int> setter, int order = 0)
            : base(path, order)
        {
            this.getter = getter;
            this.setter = setter;
            Render();
        }
        
        public override void OnEvent(DebugMenu.EvenTag tag)
        {
            switch (tag)
            {
                case DebugMenu.EvenTag.Render:
                    Render();
                    break;
                case DebugMenu.EvenTag.Increment:
                    setter(getter() + 1);
                    Render();
                    break;
                case DebugMenu.EvenTag.Decrement:
                    setter(getter() - 1);
                    Render();
                    break;
            }
        }

        private void Render()
        {
            value = getter().ToString();
        }
    }
    
    public class DebugMenuFloat : DebugMenuItem
    {
        private readonly Func<float> getter;
        private readonly Action<float> setter;

        public DebugMenuFloat(string path, Func<float> getter, Action<float> setter, int order = 0)
            : base(path, order)
        {
            this.getter = getter;
            this.setter = setter;
            Render();
        }
        
        public override void OnEvent(DebugMenu.EvenTag tag)
        {
            switch (tag)
            {
                case DebugMenu.EvenTag.Render:
                    Render();
                    break;
                case DebugMenu.EvenTag.Increment:
                    setter(getter() + 1);
                    Render();
                    break;
                case DebugMenu.EvenTag.Decrement:
                    setter(getter() - 1);
                    Render();
                    break;
            }
        }

        private void Render()
        {
            value = getter().ToString();
        }
    }
}