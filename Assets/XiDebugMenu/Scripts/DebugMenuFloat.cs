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
using XiCore.UnityExtensions;

namespace XiDebugMenu
{
    public class DebugMenuFloat : DebugMenuItem
    {
        private const int DEFAULT_STEP = 1;
        private const int DEFAULT_PRECISION = 2;
        private const int MAX_PRECISION = 8;
        private const int MIN_PRECISION = 0;
        private readonly Func<float> getter;
        private readonly Action<float> setter;
        private int step;
        private int floatingPointScale;        //< multiply value before increment, divide after
        private string format;
        private float defaultValue;
        private int precision;

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
            step = DEFAULT_STEP;
            Precision(DEFAULT_PRECISION);    
            Render();
        }
        
        public DebugMenuFloat(DebugMenu parentMenu, string label, Func<float> getter, Action<float> setter = null,
            int order = 0)
            : base(parentMenu, label, order)
        {
            this.getter = getter;
            this.setter = setter;
            defaultValue = getter();
            step = DEFAULT_STEP;
            Precision(DEFAULT_PRECISION);    
            Render();
        }
        
        public override void OnEvent(EvenTag tag)
        {
            switch (tag)
            {
                case EvenTag.Render:
                    Render();
                    break;
                case EvenTag.Right:
                    setter?.Invoke((float)(Math.Floor(getter() * floatingPointScale + 0.1f) + step * KeyboardModificationSpeed) / floatingPointScale);
                    Render();
                    OnModified();
                    break;
                case EvenTag.Left:
                    setter?.Invoke((float)(Math.Floor(getter() * floatingPointScale + 0.1f) - step * KeyboardModificationSpeed) / floatingPointScale);
                    Render();
                    OnModified();
                    break;
                case EvenTag.Reset:
                    setter?.Invoke(defaultValue);
                    Render();
                    OnModified();
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
        
        private void OnModified()
        {
            if (DebugMenuSystem.isVisible)
                return;
            DebugMenuSystem.FlashText($"<color={labelColor}>{label}</color> <color={valueColor}>{value}</color>");   
        }
        
        public DebugMenuFloat Precision(int value)
        {
            Debug.Assert(value >= MIN_PRECISION && value <= MAX_PRECISION);
            precision = value;
            format = format ?? formats[value];
            floatingPointScale = (int)Math.Pow(10, value); // precision is digits after dot
            return this;
        }
        
        public DebugMenuFloat Format(string value)
        {
            format = value ?? formats[precision];
            return this;
        }
        
        public DebugMenuFloat Step(int value)
        {
            step = value;
            return this;
        }
        
        public DebugMenuFloat Default(float value)
        {
            defaultValue = value;
            return this;
        }
    }


    public class DebugMenuVector3
    {
        private readonly DebugMenuFloat x;
        private readonly DebugMenuFloat y;
        private readonly DebugMenuFloat z;

        public DebugMenuVector3(string path, Func<Vector3> getter, Action<Vector3> setter = null, int order = 0)
        {
            x = new DebugMenuFloat(path + ".x", () => getter.Invoke().x, (x) => setter.Invoke(getter.Invoke().WithX(x)), order);
            y = new DebugMenuFloat(path + ".y", () => getter.Invoke().y, (y) => setter.Invoke(getter.Invoke().WithY(y)), order + 1);
            z = new DebugMenuFloat(path + ".z", () => getter.Invoke().z, (z) => setter.Invoke(getter.Invoke().WithZ(z)), order + 2);
        }

        public DebugMenuVector3(DebugMenu parentMenu, string label, Func<Vector3> getter, Action<Vector3> setter = null,
            int order = 0)
        {
            x = new DebugMenuFloat(parentMenu, label + ".x", () => getter.Invoke().x, (x) => setter.Invoke(getter.Invoke().WithX(x)), order);
            y = new DebugMenuFloat(parentMenu, label + ".y", () => getter.Invoke().y, (y) => setter.Invoke(getter.Invoke().WithY(y)), order + 1);
            z = new DebugMenuFloat(parentMenu, label + ".z", () => getter.Invoke().z, (z) => setter.Invoke(getter.Invoke().WithZ(z)), order + 2);
        }

        public DebugMenuVector3 Precision(int value)
        {
            x.Precision(value);
            y.Precision(value);
            z.Precision(value);
            return this;
        }

        public DebugMenuVector3 Format(string value)
        {
            x.Format(value);
            y.Format(value);
            z.Format(value);
            return this;
        }

        public DebugMenuVector3 Step(int value)
        {
            x.Step(value);
            y.Step(value);
            z.Step(value);
            return this;
        }

        public DebugMenuVector3 Default(float value)
        {
            x.Default(value);
            y.Default(value);
            z.Default(value);
            return this;
        }
    }
}