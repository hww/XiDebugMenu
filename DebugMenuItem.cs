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

namespace VARP.DebugMenus
{
    public abstract class DebugMenuItem
    {

        
        public DebugMenu menu;
        public int order;
        public string name;
        public string value;
        public string nameColor;
        public string valueColor;
     
        public enum EvenTag
        {
            Null, 
            Render,
            Increment, 
            Decrement, 
            Previous,
            Next,
            Alternate = 1024
        }
        
        protected DebugMenuItem(string path, int order)
        {
            var pathOnly = DebugMenuTools.GetDirectoryName(path);
            name = DebugMenuTools.GetFileName(path);
            menu = DebugMenu.RootDebugMenu.GetOrCreateMenu(pathOnly);
            this.order = order;
            nameColor = Tango.WhiteBright;
            valueColor = Tango.WhiteBright;
            menu.AddItem(this);
        }

        protected DebugMenuItem(string name, DebugMenu menu, int order)
        {
            this.name = name;
            this.menu = menu;
            this.order = order;
            nameColor = Tango.WhiteBright;
            valueColor = Tango.WhiteBright;
            menu?.AddItem(this);
        }

        public virtual void OnEvent(DebugMenuC sender, DebugMenu.EvenTag tag)
        {
            // override this method to make specific menu item
        }

        public override string ToString() { return $"MenuItem[{name}]"; }


        // =============================================================================================================
        // Syntax sugar (Can be removed)
        // =============================================================================================================

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
