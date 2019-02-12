// =============================================================================
// MIT License
// 
// Copyright (c) 2018 Valeriya Pudova (hww.github.io)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// =============================================================================

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
        private float defaultValue;
        
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
            this.defaultValue = getter();
            floatingPointScale = (int)Math.Pow(10, precision); // precision is digits after dot
            Render();
        }
        
        public override void OnEvent(DebugMenuC sender, EvenTag tag)
        {
            switch (tag)
            {
                case EvenTag.Render:
                    Render();
                    break;
                case EvenTag.Increment:
                    setter((float)(Math.Floor(getter() * floatingPointScale + 0.1f) + 1) / floatingPointScale);
                    Render();
                    break;
                case EvenTag.Decrement:
                    setter((float)(Math.Floor(getter() * floatingPointScale + 0.1f) - 1) / floatingPointScale);
                    Render();
                    break;
                case EvenTag.Reset:
                    setter(defaultValue);
                    Render();
                    break;
            }
        }

        private void Render()
        {
            var val = getter();
            value = val.ToString(format);
            valueColor = val == defaultValue ? Tango.WhiteBright : Tango.YellowBright;
        }
        
    }
}