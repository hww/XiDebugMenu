using System;
using System.Collections.Generic;
using UnityEngine;

namespace VARP.DebugMenus
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>
    /// new DebugMenu("Sample/ApplicationInfo", DrawApplicationInfo);
    ///
    /// new DebugMenuToggle("Sample/Toggle/flag",
    ///                     () => flag,
    ///                     f => flag = f);
    /// new DebugMenuButton("Sample/Button/DateTimeNow", PrintDateTimeNow);
    /// </example>
    public class DebugMenu : DebugMenuItem
    {
        public static readonly DebugMenu RootDebugMenu = new DebugMenu("Root", null, 0, 20);
        
        public override string ToString() { return $"Menu[{name}]"; }

        public DebugMenu(string path, int order = 0, int capacity = 10) : base(path, order)
        {
            itemsList = new List<DebugMenuItem>(capacity);
        }
        
        public DebugMenu(string name, DebugMenu menu, int order, int capacity = 10) : base(name, menu, order)
        {
            itemsList = new List<DebugMenuItem>(capacity);
        }
        
        // =============================================================================================================
        // Menu lines code
        // =============================================================================================================
        
        private readonly List<DebugMenuItem> itemsList;
        
        public int Count => itemsList.Count;                 // Get lines quantity
        public DebugMenuItem this[int idx] => itemsList[idx];     // Get line by index
        
        public DebugMenuItem Find(string path)
        {
            var names = path.Split('/');
            return Find(names);
        }

        public DebugMenuItem Find(string[] path)
        {
            var currentMenu = this;
            var lastIndex = path.Length - 1;
            for (var i = 0; i < path.Length; i++)
            {
                var itemName = path[i];
                var currentItem = itemsList.Find(item => item.name == itemName);
                if (currentItem == null)
                    return null;
                if (i == lastIndex)
                    return currentItem;
                currentMenu = currentItem as DebugMenu;
                if (currentMenu == null)
                    return null;
            }
            return currentMenu;
        }

        public void AddItem(DebugMenuItem item)
        {
            itemsList.Add(item);
            itemsList.Sort(CompareItem);
        }
        
        public void RemoveItem(DebugMenuItem item)
        {
            itemsList.Remove(item);
        }
                
        public void Clear()
        {
            itemsList.Clear();
        }
        
        static int CompareItem(DebugMenuItem x, DebugMenuItem y)
        {
            Debug.Assert(x != null);
            Debug.Assert(y != null);
            return x.order.CompareTo(y.order);
        }
        
        // =============================================================================================================
        // Menu creation
        // =============================================================================================================

        // Will return existing or create new menu
        public DebugMenu GetOrCreateMenu(string path, int order = 0, int capacity = 10)
        {
            if (string.IsNullOrEmpty(path))
                return this;
            return GetOrCreateMenu(path.Split('/'));
        }
        
        // Will return existing or create new menu
        public DebugMenu GetOrCreateMenu(string[] path, int order = 0, int capacity = 10)
        {
            var currentMenu = this;
            for (var i = 0; i < path.Length; i++)
            {
                var itemName = path[i];
                var currentItem = itemsList.Find(item => item.name == itemName);
                if (currentItem == null)
                {
                    currentItem = new DebugMenu(itemName, currentMenu, order);

                }
                var menu = currentItem as DebugMenu;
                if (menu == null)
                {
                    Debug.LogError($"Expected menu '{itemName}' in path {path} found {currentItem}");
                    return null;
                }
            }
            return currentMenu;
        }

        // =============================================================================================================
        // Menu creation
        // =============================================================================================================
        
        // pre render all menu items, and compute width of fields
        public override void OnEvent(EvenTag tag)
        {
            for (var i = 0; i < itemsList.Count; i++)
            {
                itemsList[i].OnEvent(tag);
            }
        }

        public int widthOfName;
        public int widthOfValue;
        public int widthOfNameAnValue;

        public void UpdateWidth(int space)
        {
            widthOfName = 0;
            widthOfValue = 0;
            widthOfNameAnValue = name.Length; // this menu name
            
            for (var i = 0; i < itemsList.Count; i++)
            {
                var item = itemsList[i];
                if (item is DebugMenu)
                {
                    widthOfNameAnValue = Math.Max(widthOfNameAnValue, item.name.Length + 3); // 3 for ...
                }
                else
                {
                    if (item.name != null && item.value != null)
                    {
                        var nameLength = item.name.Length;
                        var valueLength = item.value.Length;
                        widthOfName = Math.Max(widthOfName, nameLength);
                        widthOfValue = Math.Max(widthOfValue, valueLength);
                        widthOfNameAnValue = Math.Max(widthOfNameAnValue, nameLength + valueLength + space);
                    }
                    else
                    {
                        widthOfNameAnValue = Math.Max(widthOfNameAnValue, item.name.Length);
                    }
                }
            }
            
            Debug.Log($"{widthOfName} {space} {widthOfValue} <= {widthOfNameAnValue} ");
        }
                
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
    }
}

