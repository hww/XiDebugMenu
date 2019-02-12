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
            var bucketTree = new TreeViewItem { id = 0, displayName = $"[{debugMenu.order}] {debugMenu.name}..." };
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
                        var itemTree = new TreeViewItem { id = 0, displayName = $"[{item.order}] '{item.name}'={item.value}" };
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