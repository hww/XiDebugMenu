using System;
using UnityEngine;

namespace VARP.DebugMenus
{
    public class DebugMenuFloat : DebugMenuItem
    {
        private readonly Func<float> getter;
        private readonly Action<float> setter;
        private readonly int floatingPointScale;        // multiply value before increment
        private string format;

        private static readonly string[] formats = new[]
        {
            "0", "0.0", "0.00", "0.000", "0.0000", "0.00000", "0.000000", "0.0000000", "0.00000000"
        };
        
        public DebugMenuFloat(string path, Func<float> getter, Action<float> setter, int order = 0, 
            int precision = 2, string format = null)
            : base(path, order)
        {
            Debug.Assert(precision >= 0 && precision < 8);
            this.getter = getter;
            this.setter = setter;
            this.format = format ?? formats[precision];
            floatingPointScale = (int)Math.Pow(10, precision); // precision is digits after dot
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
                    setter((float)(Math.Floor(getter() * floatingPointScale + 0.1f) + 1) / floatingPointScale);
                    Render();
                    break;
                case DebugMenu.EvenTag.Decrement:
                    setter((float)(Math.Floor(getter() * floatingPointScale + 0.1f) - 1) / floatingPointScale);
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