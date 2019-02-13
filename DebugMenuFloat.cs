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
using UnityEditor.ShaderGraph;
using UnityEngine;

namespace VARP.DebugMenus
{
    public class DebugMenuFloat : DebugMenuItem
    {
        private const int DEFAULT_PRECISION = 2;
        private const int MAX_PRECISION = 8;
        private const int MIN_PRECISION = 0;
        private readonly Func<float> getter;
        private readonly Action<float> setter;
        private int increment;
        private int floatingPointScale;        //< multiply value before increment, divide after
        private string format;
        private float defaultValue;
        
        private static readonly string[] formats = new[]
        {
            "0", "0.0", "0.00", "0.000", "0.0000", "0.00000", "0.000000", "0.0000000", "0.00000000"
        };
        
        public DebugMenuFloat(string path, Func<float> getter, Action<float> setter = null, int order = 0)
            : base(path, order)
        {
            this.getter = getter;
            this.setter = setter;
            defaultValue = getter();
            increment = 1;
            Precision(DEFAULT_PRECISION);    
            Render();
        }
        
        public DebugMenuFloat(string label, DebugMenu menu, Func<float> getter, Action<float> setter = null, int order = 0)
            : base(menu, label, order)
        {
            this.getter = getter;
            this.setter = setter;
            defaultValue = getter();
            increment = 1;
            Precision(DEFAULT_PRECISION);    
            Render();
        }
        
        public override void OnEvent(DebugMenuC sender, EvenTag tag)
        {
            switch (tag)
            {
                case EvenTag.Render:
                    Render();
                    break;
                case EvenTag.Inc:
                    setter?.Invoke((float)(Math.Floor(getter() * floatingPointScale + 0.1f) + increment) / floatingPointScale);
                    Render();
                    break;
                case EvenTag.Dec:
                    setter?.Invoke((float)(Math.Floor(getter() * floatingPointScale + 0.1f) - increment) / floatingPointScale);
                    Render();
                    break;
                case EvenTag.Reset:
                    setter?.Invoke(defaultValue);
                    Render();
                    break;
            }
        }

        private void Render()
        {
            var val = getter();
            var def = val == defaultValue;
            value = val.ToString(format);
            valueColor = def ? Colors.ValueDefault : Colors.ValueModified;
            labelColor = def ? Colors.LabelDefault : Colors.LabelModified;
        }
        
        public DebugMenuFloat Precision(int value)
        {
            Debug.Assert(value >= MIN_PRECISION && value <= MAX_PRECISION);
            format = format ?? formats[value];
            floatingPointScale = (int)Math.Pow(10, value); // precision is digits after dot
            return this;
        }
        
        public DebugMenuFloat Format(string value)
        {
            format = value; 
            return this;
        }
        
        public DebugMenuFloat Increment(int value)
        {
            increment = value;
            return this;
        }
        
        public DebugMenuFloat Default(float value)
        {
            defaultValue = value;
            return this;
        }
    }
}