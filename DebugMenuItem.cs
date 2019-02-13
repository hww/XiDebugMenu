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

using UnityEditor;

namespace VARP.DebugMenus
{
    public abstract class DebugMenuItem
    {
        public DebugMenu menu;           //< the menu of this item 
        public int order;                //< sorting menu items and group them by similarities
        public readonly string label;    //< at left side of menu item
        public string value;             //< at right side of menu item
        public string labelColor;        //< label color
        public string valueColor;        //< value color
     
        public enum EvenTag
        {
            Null,               //< Nothing 
            Render,             //< Render item, update label, value and colors
            Dec,                //< Decrease value or call action
            Inc,                //< Increase value or call action
            Prev,               //< Go to previous item 
            Next,               //< Go to next item
            Reset,              //< Reset value to default
            OpenMenu,           //< When menu open    
            CloseMenu,          //< When menu closed
        }
        
        protected DebugMenuItem(string path, int order)
        {
            var pathOnly = DebugMenuTools.GetDirectoryName(path);
            label = DebugMenuTools.GetFileName(path);
            menu = DebugMenu.RootDebugMenu.GetOrCreateMenu(pathOnly);
            this.order = order;
            labelColor = Colors.ToggleLabelDisabled;
            valueColor = Colors.ToggleLabelDisabled;
            menu.AddItem(this);
        }

        protected DebugMenuItem(string label, DebugMenu menu, int order)
        {
            this.label = label;
            this.menu = menu;
            this.order = order;
            labelColor = Colors.ToggleLabelDisabled;
            valueColor = Colors.ToggleLabelDisabled;
            menu?.AddItem(this);
        }

        public virtual void OnEvent(DebugMenuC sender, DebugMenu.EvenTag tag)
        {
            // override this method to make specific menu item
        }

        public override string ToString() { return $"MenuItem[{label}]"; }

        // =============================================================================================================
        // Syntax sugar (Can be removed)
        // =============================================================================================================

        /// <summary>
        /// Menu will group items with equal or +1 difference to single block
        /// </summary>
        public DebugMenuItem Order(int order)
        {
            this.order = order;
            menu.Reorder();
            return this;
        }
        
        public DebugMenuItem AddToMenu(DebugMenu menu)
        {
            this.menu?.RemoveItem(this);
            this.menu = menu;
            menu.AddItem(this);
            return this;
        }
    }
}
