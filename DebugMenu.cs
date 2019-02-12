using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VARP.DebugMenus
{
    /// <summary>
    /// 
    /// </summary>
    /// <example>
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
                var currentItem = currentMenu.itemsList.Find(item => item.name == itemName);
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
                currentMenu = menu;
            }
            return currentMenu;
        }

        // =============================================================================================================
        // Menu creation
        // =============================================================================================================
        
        /// <summary>
        /// Deliver message to all children
        /// </summary>
        /// <param name="tag"></param>
        public void SendToChildren(DebugMenuC sender, EvenTag tag)
        {
            for (var i = 0; i < itemsList.Count; i++)
            {
                itemsList[i].OnEvent(sender, tag);
            }
        }

        /// <summary>
        /// Perform actions for events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="tag"></param>
        public override void OnEvent(DebugMenuC sender, EvenTag tag)
        {
            // do not send message to children, just do only what this instance need
            // update color or text
            if (tag.HasFlag(EvenTag.Increment))
                sender.OpenMenu(this);
        }
        
        public int widthOfName;
        public int widthOfValue;
        public int widthOfNameAnValue;

        /// <summary>
        /// Calculate menu, name, value columns width 
        /// </summary>
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
                        widthOfName = Math.Max(widthOfName, item.name.Length);
                        widthOfValue = Math.Max(widthOfValue, item.value.Length);
                    }
                    else
                    {
                        widthOfNameAnValue = Math.Max(widthOfNameAnValue, item.name.Length);
                    }
                }
            }
            widthOfNameAnValue = Math.Max(widthOfNameAnValue, widthOfName + widthOfValue + space);
            widthOfName = Math.Max(widthOfName, widthOfNameAnValue - widthOfValue - space);
        }
                

    }
    

}

