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

namespace XiDebugMenu
{
    public class DebugMenuToggle : DebugMenuItem
    {
        private readonly Func<bool> getter;
        private readonly Action<bool> setter;
        private bool defaultValue;
        
        public DebugMenuToggle(string path, Func<bool> getter, Action<bool> setter, int order = 0)
            : base(path, order)
        {
            this.getter = getter;
            this.setter = setter;
            value = null;    // do not have value, wil display it by color
            defaultValue = this.getter();
        }
        
        public DebugMenuToggle(DebugMenu parentMenu, string label, Func<bool> getter, Action<bool> setter,
            int order = 0)
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
                    setter(!getter());
                    Render();
                    OnModified();
                    break;
                case EvenTag.Left:
                    setter(!getter());
                    Render();
                    OnModified();
                    break;
                case EvenTag.Reset:
                    setter(defaultValue);
                    Render();
                    OnModified();
                    break;
            }
        }

        private void Render()
        {
            var val = getter();
            labelColor = val ? Colors.ToggleLabelEnabled : Colors.ToggleLabelDisabled;
        }
        
        private void OnModified()
        {
            if (DebugMenuSystem.isVisible)
                return;
            DebugMenuSystem.FlashText($"<color={labelColor}>{label}</color>");
        }
    }
}