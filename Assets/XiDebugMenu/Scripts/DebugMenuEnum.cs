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
using XiCore.Extensions;

namespace XiDebugMenu
{
    public class DebugMenuEnum<T> : DebugMenuItem where T : struct, Enum
    {
        private readonly Func<T> getter;
        private readonly Action<T> setter;
        private T defaultValue;
      
        public DebugMenuEnum(string path, Func<T> getter, Action<T> setter, int order = 0)
            : base(path, order)
        {
            this.getter = getter;
            this.setter = setter;
            value = null;    // do not have value, wil display it by color
            defaultValue = this.getter();
        }
        
        public DebugMenuEnum(DebugMenu parentMenu, string label, Func<T> getter, Action<T> setter, int order = 0)
            : base(parentMenu, label, order)
        {
            this.getter = getter;
            this.setter = setter;
            value = null;    // do not have value, wil display it by color
            defaultValue = this.getter();
        }

        
        public override void OnEvent(EvenTag tag)
        {
            switch (tag)
            {
                case EvenTag.Render:
                    Render();
                    break;
                case EvenTag.Right:
                    setter?.Invoke(EnumExtensions.Next(getter()));
                    Render();
                    OnModified();
                    break;
                case EvenTag.Left:
                    setter?.Invoke(EnumExtensions.Previous(getter()));
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
            var def = val.Equals(defaultValue);
            value = val.ToString();
            valueColor = def ? Colors.ValueDefault : Colors.ValueModified;
            labelColor = def ? Colors.LabelDefault : Colors.LabelModified;
        }

        private void OnModified()
        {
            if (DebugMenuSystem.isVisible)
                return;
            DebugMenuSystem.FlashText($"<color={labelColor}>{label}</color> <color={valueColor}>{value}</color>");     
        }
        
        public DebugMenuEnum<T> Default(T value)
        {
            defaultValue = value;
            return this;
        }
    }
}