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

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace VARP.DebugMenus
{
    class DebugMenuTreeView : TreeView
    {
        public DebugMenuTreeView(TreeViewState treeViewState)
            : base(treeViewState)
        {
            Reload();
        }
        
        protected override TreeViewItem BuildRoot ()
        {
            var root = new TreeViewItem      { id = 0, depth = -1, displayName = "Scene" };
            var rootDebugMenu = DebugMenu.RootDebugMenu;
            var rootDebugTree = BuildTree(rootDebugMenu);
            root.AddChild(rootDebugTree);
            SetupDepthsFromParentsAndChildren(root);
            return root;
        }

        
        TreeViewItem BuildTree(DebugMenu debugMenu)
        {
            var bucketTree = new TreeViewItem { id = 0, displayName = $"[{debugMenu.order}] {debugMenu.label}..." };
            for (var i = 0; i < debugMenu.Count; i++)
            {
                var item = debugMenu[i];
                if (item != null)
                {
                    if (item is DebugMenu)
                    {
                        var submenuTree = BuildTree(item as DebugMenu);
                        bucketTree.AddChild(submenuTree);
                    }
                    else
                    {
                        var itemTree = new TreeViewItem { id = 0, displayName = $"[{item.order}] '{item.label}'={item.value}" };
                        bucketTree.AddChild(itemTree);
                    }
                }
            }
            return bucketTree;
        }
        
        // Selection
        protected override void SelectionChanged (IList<int> selectedIds)
        {
            Selection.instanceIDs = selectedIds.ToArray();
        }

    }
}