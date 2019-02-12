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
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace VARP.DebugMenus
{
    public class DebugMenuC : MonoBehaviour
    {
        [BoxGroup("Managed Objects")]
        public Canvas canvas;
        [BoxGroup("Managed Objects")]
        public TextMeshProUGUI menuText;
        [BoxGroup("Managed Objects")]
        public Transform backgroundTransform;
        [BoxGroup("Settings")]
        public int fontSize = 16;
        [BoxGroup("State")]
        public bool isVisible;
        [BoxGroup("State")]
        public bool isDirty;
    
        private Stack<MenuState> stack = new Stack<MenuState>(10);
        private float autoRefreshAt;
        
        // =============================================================================================================
        // Mono behaviour
        // =============================================================================================================
        
        public void OnValidate()
        {
            Debug.Assert(canvas != null);    
            Debug.Assert(menuText != null);    
        }


        private void OnEnable()
        {
            menuText.fontSize = fontSize;
            menuText.autoSizeTextContainer = true;
            SetVisible(false);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
                SetVisible(!isVisible);
            
            if (isVisible)
            {
                // Refresh menu time to time if autoRefreshPeriod is more than zero
                if (autoRefreshAt > 0 && Time.time > autoRefreshAt)
                    isDirty = true;
                
                if (Input.GetKeyDown(KeyCode.Escape))
                    CloseMenu();
                DebugMenu.EvenTag evt = DebugMenu.EvenTag.Null; 

                if (Input.GetKeyDown(KeyCode.W))
                    SendEvent(DebugMenu.EvenTag.Prev);
                if (Input.GetKeyDown(KeyCode.S))
                    SendEvent(DebugMenu.EvenTag.Next);
                if (Input.GetKeyDown(KeyCode.A))
                    SendEvent(DebugMenu.EvenTag.Inc);
                if (Input.GetKeyDown(KeyCode.D))
                    SendEvent(DebugMenu.EvenTag.Dec);
                if (Input.GetKeyDown(KeyCode.R))
                    SendEvent(DebugMenu.EvenTag.Reset);
            }
            
            if (isDirty)
                Render();
        }

        // =============================================================================================================
        // Manipulating by menu
        // =============================================================================================================
        
        public void SendEvent(DebugMenu.EvenTag tag)
        {
            var state = stack.Peek();
            state.OnEvent(tag);
            var menu = state.debugMenu;
            var line = state.line;
            var menuLine = menu[line];
            if (Input.GetKey(KeyCode.LeftShift))
                tag |= DebugMenu.EvenTag.Shift;
            menuLine.OnEvent(this, tag);
            Render();
        }
        
        // =============================================================================================================
        // Show/Hide/Toggle
        // =============================================================================================================
        
        public void SetVisible(bool state)
        {
            isVisible = state;
            if (isVisible)
            {
                canvas.enabled = true;
                if (stack.Count == 0)
                    OpenMenu(DebugMenu.RootDebugMenu);
                else
                    Render();
            }
            else
            {
                canvas.enabled = false;
            }
        }

        // =============================================================================================================
        // Render new or previous menu
        // =============================================================================================================

        public void OpenMenu(DebugMenu debugMenu)
        {
            stack.Push(new MenuState { debugMenu = debugMenu, line = 0 });
            Render();
        }
        
        private void CloseMenu()
        {
            if (stack.Count == 1)
            {
                SetVisible(false);
            }
            else
            {
                stack.Pop();
                Render();
            }
        }
        
        // =============================================================================================================
        // Render new or previous menu
        // =============================================================================================================
        
        private void Render()
        {
            isDirty = false;
            var state = stack.Peek();
            autoRefreshAt = Time.time + state.debugMenu.autoRefreshPeriod;
            menuText.text = MenuTextRenderer.RenderMenu(this, state.debugMenu, state.line);
        }

        public void MakeDirty()
        {
            isDirty = true;
        }
    }
}