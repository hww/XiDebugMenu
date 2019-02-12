using UnityEngine;
using UnityEditor.IMGUI.Controls;
using UnityEditor;

namespace VARP.DebugMenus
{
    class DebugMenuTreeViewWindow : EditorWindow
    {
        // SerializeField is used to ensure the view state is written to the window 
        // layout file. This means that the state survives restarting Unity as long as the window
        // is not closed. If the attribute is omitted then the state is still serialized/deserialized.
        [SerializeField] TreeViewState m_TreeViewState;

        //The TreeView is not serializable, so it should be reconstructed from the tree data.
        DebugMenuTreeView m_TreeView;

        void OnEnable ()
        {
            if (m_TreeViewState == null)
                m_TreeViewState = new TreeViewState ();
            m_TreeView = new DebugMenuTreeView(m_TreeViewState);
        }

        void OnGUI ()
        {
            if (m_TreeView != null)
              m_TreeView.OnGUI(new Rect(0, 0, position.width, position.height));
        }

        [MenuItem ("/Window/Rocket/Debug Menu Tree Window")]
        static void ShowWindow ()
        {
            // Get existing open window or if none, make a new one:
            var window = GetWindow<DebugMenuTreeViewWindow> ();
            window.titleContent = new GUIContent ("Debug Menu Tree");
            window.Show ();
        }
        
        void OnSelectionChange ()
        {
            if (m_TreeView != null)
                m_TreeView.SetSelection (Selection.instanceIDs);
            Repaint ();
        }
        
        void OnHierarchyChange()
        {
            if (m_TreeView != null)
                m_TreeView.Reload();
            Repaint ();
        }
    }
}