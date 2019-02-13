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

namespace VARP.DebugMenus
{
    public class DebugMenuInteger : DebugMenuItem
    {
        private const string DEFAULT_FORMAT = "0";
        private readonly Func<int> getter;
        private readonly Action<int> setter;
        private string format;
        private int step;
        private int defaultValue;
        
        public DebugMenuInteger(string path, Func<int> getter, Action<int> setter  = null, int order = 0)
            : base(path, order)
        {
            this.getter = getter;
            this.setter = setter;
            format = DEFAULT_FORMAT;
            step = 1;
            defaultValue = getter();
            Render();
        }
        
        public DebugMenuInteger(DebugMenu menu, string path, Func<int> getter, Action<int> setter  = null, int order = 0)
            : base(menu, path, order)
        {
            this.getter = getter;
            this.setter = setter;
            format = DEFAULT_FORMAT;
            step = 1;
            defaultValue = getter();
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
                    setter?.Invoke(getter() + step);
                    Render();
                    break;
                case EvenTag.Dec:
                    setter?.Invoke(getter() - step);
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
        
        public DebugMenuInteger Step(int value)
        {
            step = value;
            return this;
        }
        
        public DebugMenuInteger Format(string value)
        {
            format = value ?? DEFAULT_FORMAT;
            return this;
        }
        
        public DebugMenuInteger Default(int value)
        {
            defaultValue = value;
            return this;
        }
    }
}