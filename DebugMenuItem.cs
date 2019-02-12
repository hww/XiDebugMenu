using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace VARP.DebugMenus
{
    public abstract class DebugMenuItem
    {
        public const string BLACK = "272727";
        public const string WHITE = "FFFFFF";
        public const string GRAY = "707070";
        public const string YELLOW = "FFE400";
        public const string ORANGE = "FF652F";
        public const string GREEN = "12A76C";
        public const string BLUE = "2E9CCA";
        
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
            nameColor = WHITE;
            valueColor = WHITE;
            menu.AddItem(this);
        }

        protected DebugMenuItem(string name, DebugMenu menu, int order)
        {
            this.name = name;
            this.menu = menu;
            this.order = order;
            nameColor = WHITE;
            valueColor = WHITE;
            menu?.AddItem(this);
        }

        public virtual void OnEvent(DebugMenuC sender, DebugMenu.EvenTag tag)
        {
            // override this method to make specific menu item
        }

        public override string ToString() { return $"MenuItem[{name}]"; }
    }
}
