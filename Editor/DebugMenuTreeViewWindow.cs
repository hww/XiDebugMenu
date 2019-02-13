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
        private float m_refreshAt;
        
        void OnEnable ()
        {
            if (m_TreeViewState == null)
                m_TreeViewState = new TreeViewState ();
            m_refreshAt = Time.time + 2f;
            m_TreeView = new DebugMenuTreeView(m_TreeViewState);
        }

        void OnGUI ()
        {
            if (Time.time > m_refreshAt)
            {
                m_refreshAt = Time.time + 2f;
                m_TreeView = new DebugMenuTreeView(m_TreeViewState);
            }
            m_TreeView?.OnGUI(new Rect(0, 0, position.width, position.height));
        }

        [MenuItem ("/Window/Rocket/Debug Menu Tree Window")]
        static void ShowWindow ()
        {
            var window = GetWindow<DebugMenuTreeViewWindow> ();
            window.titleContent = new GUIContent ("Debug Menu Tree");
            window.Show ();
        }
        
        void OnSelectionChange ()
        {
            m_TreeView?.SetSelection (Selection.instanceIDs);
            Repaint ();
        }
        
        void OnHierarchyChange()
        {
            m_TreeView?.Reload();
            Repaint ();
        }
    }
}