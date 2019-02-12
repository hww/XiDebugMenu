using System;
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
        public bool isDirty;


    
        private Stack<MenuState> stack = new Stack<MenuState>(10);

        // =============================================================================================================
        // Mono behaviour
        // =============================================================================================================
        
        public void OnValidate()
        {
            Debug.Assert(canvas != null);    
            Debug.Assert(menuText != null);    
        }

        public bool toggleValue;
        public int integerValue;
        public float floatValue;

        private void OnEnable()
        {

            menuText.fontSize = fontSize;
            menuText.autoSizeTextContainer = true;

            new DebugMenu("Edit/Preferences");
            new DebugMenuToggle("Edit/Preferences/Toggle", () => toggleValue, value => toggleValue = value, 1);
            new DebugMenuInteger("Edit/Preferences/Integer", () => integerValue, value => integerValue = value, 1);
            new DebugMenuFloat("Edit/Preferences/Float", () => floatValue, value => floatValue = value, 1);
            new DebugMenuAction("Edit/Preferences/Action", () => { Debug.Log("Action"); }, 1);
            new DebugMenu("Edit/Preferences/Extra Preferences", 2);

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
                SetVisible(!isVisible);
            if (isVisible)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                    CloseMenu();
                DebugMenu.EvenTag evt = DebugMenu.EvenTag.Null; 

                if (Input.GetKeyDown(KeyCode.W))
                    SendEvent(DebugMenu.EvenTag.Previous);
                if (Input.GetKeyDown(KeyCode.S))
                    SendEvent(DebugMenu.EvenTag.Next);
                if (Input.GetKeyDown(KeyCode.A))
                    SendEvent(DebugMenu.EvenTag.Decrement);
                if (Input.GetKeyDown(KeyCode.D))
                    SendEvent(DebugMenu.EvenTag.Increment);
            }
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
                tag |= DebugMenu.EvenTag.Alternate;
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
            menuText.text = MenuTextRenderer.RenderMenu(this, state.debugMenu, state.line);

     
            //Vector2 textSize = menuText.GetPreferredValues(menuText.text);
     //
            //// Adjust the button size / scale.
            //backgroundTransform.localScale = textSize + Vector2.one;
        }

    }
}