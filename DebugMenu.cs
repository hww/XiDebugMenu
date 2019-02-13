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
using System.Collections.Generic;
using UnityEngine;

namespace VARP.DebugMenus
{
    public class DebugMenu : DebugMenuItem
    {
        private const int DEFAULT_CAPACITY = 10;

        // =============================================================================================================
        // Root menu for all game
        // =============================================================================================================

        public static readonly DebugMenu RootDebugMenu = new DebugMenu(null, "Root", 0);

        // =============================================================================================================
        // Menu lines code
        // =============================================================================================================

        public float autoRefreshPeriod;
        public bool doRefresh;
        public int widthOfName;
        public int widthOfNameAnValue;
        public int widthOfValue;
        
        private readonly List<DebugMenuItem> itemsList;
        private Action<DebugMenu> onClose;
        private Action<DebugMenu> onOpen;
        private Action<DebugMenu> onClear;
        
        // =============================================================================================================
        // Dimensions of menu
        // =============================================================================================================

        public DebugMenu(string path, int order = 0) : base(path, order)
        {
            itemsList = new List<DebugMenuItem>(DEFAULT_CAPACITY);
        }

        public DebugMenu(DebugMenu menu, string label, int order = 0) : base(menu, label, order)
        {
            itemsList = new List<DebugMenuItem>(DEFAULT_CAPACITY);
        }

        // =============================================================================================================
        // DebugMenu main API
        // =============================================================================================================
        
        public int Count => itemsList.Count; // Get lines quantity
        public DebugMenuItem this[int idx] => itemsList[idx]; // Get line by index

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
                var currentItem = itemsList.Find(item => item.label == itemName);
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
            onClear?.Invoke(this);
        }
        
        public void ClearDownTree()
        {
            for (var i=0 ;i<itemsList.Count; i++)
                (itemsList[i] as DebugMenu)?.ClearDownTree();
            itemsList.Clear();
            onClear?.Invoke(this);
        }
        
        public void RequestRefresh()
        {
            doRefresh = true;
        }
        
        public override string ToString()
        {
            return $"Menu[{label}]";
        }
        
        // =============================================================================================================
        // Sorting & ordering
        // =============================================================================================================
        
        public void Sort()
        {
            itemsList.Sort(CompareItem);
        }

        private static int CompareItem(DebugMenuItem x, DebugMenuItem y)
        {
            Debug.Assert(x != null);
            Debug.Assert(y != null);
            return x.order.CompareTo(y.order);
        }

        // =============================================================================================================
        // Menu find or creation
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
                var currentItem = currentMenu.itemsList.Find(item => item.label == itemName);
                if (currentItem == null) currentItem = new DebugMenu(currentMenu, itemName, order);
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
        ///     Deliver message to all children
        /// </summary>
        /// <param name="tag"></param>
        public void SendToChildren(DebugMenuC sender, EvenTag tag)
        {
            for (var i = 0; i < itemsList.Count; i++) itemsList[i].OnEvent(sender, tag);
        }

        /// <summary>
        ///     Perform actions for events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="tag"></param>
        public override void OnEvent(DebugMenuC sender, EvenTag tag)
        {
            // do not send message to children, just do only what this instance need
            // update color or text
            switch (tag)
            {
                case EvenTag.Inc:
                    sender.OpenMenu(this);
                    break;
                case EvenTag.Dec:
                    sender.CloseMenu();
                    break;
                case EvenTag.OpenMenu:
                    onOpen?.Invoke(this);
                    break;
                case EvenTag.CloseMenu:
                    onClose?.Invoke(this);
                    break;
            }
        }

        // =============================================================================================================
        // Dimensions of menu
        // =============================================================================================================
        
        /// <summary>
        ///     Calculate menu, name, value columns width
        /// </summary>
        public void UpdateWidth(int space)
        {
            widthOfName = 0;
            widthOfValue = 0;
            widthOfNameAnValue = label.Length; // this menu name

            for (var i = 0; i < itemsList.Count; i++)
            {
                var item = itemsList[i];
                if (item is DebugMenu)
                {
                    widthOfNameAnValue = Math.Max(widthOfNameAnValue, item.label.Length + 3); // 3 for "..."
                }
                else
                {
                    if (item.label != null && item.value != null)
                    {
                        widthOfName = Math.Max(widthOfName, item.label.Length);
                        widthOfValue = Math.Max(widthOfValue, item.value.Length);
                    }
                    else
                    {
                        widthOfNameAnValue = Math.Max(widthOfNameAnValue, item.label.Length);
                    }
                }
            }

            widthOfNameAnValue = Math.Max(widthOfNameAnValue, widthOfName + widthOfValue + space);
            widthOfName = Math.Max(widthOfName, widthOfNameAnValue - widthOfValue - space);
        }



        // =============================================================================================================
        // Syntax sugar
        // =============================================================================================================

        public DebugMenu OnOpen(Action<DebugMenu> onOpen)
        {
            this.onOpen = onOpen;
            return this;
        }

        public DebugMenu OnClose(Action<DebugMenu> onClose)
        {
            this.onClose = onClose;
            return this;
        }

        public DebugMenu AutoRefresh(float period)
        {
            autoRefreshPeriod = period;
            return this;
        }
    }
}