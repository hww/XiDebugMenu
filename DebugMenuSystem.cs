using UnityEngine;
using VARP.MVC;

namespace VARP.DebugMenus
{
    public class DebugMenuSystem
    {
        public enum Menu
        {
            
        }
        public static readonly DebugMenu RootDebugMenu = new DebugMenu(null, "Debug Menu");
        public static readonly DebugMenu RootQuickMenu = new DebugMenu(null, "Quick Menu");
        public static DebugMenuRepresentation representation;
        public static DebugMenu currentMenu;
        public static bool isVisible;
        private static float autoRefreshAt;
        private static bool autoRefresh;
        private static DebugMenuController controller;

        /// <summary>
        /// Initialize menu system
        /// </summary>
        public static void Initialize()
        {
            controller = new DebugMenuController();
            representation = Object.FindObjectOfType<DebugMenuRepresentation>();
            Debug.Assert(representation != null);
        }

        /// <summary>
        /// DeInitialize menu system
        /// </summary>
        public static void DeInitialize()
        {
            SetVisible(false);
            RootDebugMenu.ClearDownTree();
            representation = null;
            currentMenu = null;
        }

        public static void Update()
        {

            // Refresh menu time to time if autoRefreshPeriod is more than zero
            if (autoRefresh && Time.time > autoRefreshAt)
                currentMenu.RequestRefresh();

            var evt = controller.GetEvet();
            if (evt != DebugMenuItem.EvenTag.Null)
                SendEvent(evt);
            
            if (currentMenu == null)
                return;
            
            if (currentMenu.doRefresh)
                Render();
        }
        
        public static void SendEvent(DebugMenuItem.EvenTag tag)
        {
            if (tag == DebugMenuItem.EvenTag.ToggleMenu)
            {
                var root = currentMenu?.GetRootMenu();
                if (RootDebugMenu == root)
                {
                    SetVisible(!isVisible);
                }
                else
                {
                    currentMenu = RootDebugMenu;
                    SetVisible(true);
                }
            }
            else if (tag == DebugMenuItem.EvenTag.ToggleQuickMenu)
            {
                var root = currentMenu?.GetRootMenu();
                if (RootQuickMenu == root)
                {
                    SetVisible(!isVisible);
                }
                else
                {
                    currentMenu = RootQuickMenu;
                    SetVisible(true);
                }
            }
            else
            {
                if (currentMenu != null)
                {
                    currentMenu.OnEvent(tag);

                    Render();
                }
            }
        }
        
        // =============================================================================================================
        // Show/Hide/Toggle
        // =============================================================================================================
        
        public static void SetVisible(bool state)
        {
            isVisible = state;
            representation?.SetVisible(state);
            Render();
        }

        // =============================================================================================================
        // Render new or previous menu
        // =============================================================================================================

        public static void OpenMenu(DebugMenu debugMenu)
        {
            currentMenu = debugMenu;
            debugMenu.OnEvent(DebugMenuItem.EvenTag.OpenMenu);
            Render();
        }
        
        public static void CloseMenu()
        {
            if (currentMenu.parentMenu == null)
            {
                SetVisible(false);
            }
            else
            {
                currentMenu.OnEvent(DebugMenuItem.EvenTag.CloseMenu);
                OpenMenu(currentMenu.parentMenu);
                Render();
            }
        }
        
        // =============================================================================================================
        // Render new or previous menu
        // =============================================================================================================
        
        private static void Render()
        {
            if (currentMenu == null)
                return;
            currentMenu.doRefresh = false;
            autoRefresh = currentMenu.autoRefreshPeriod > 0;
            autoRefreshAt = Time.time + currentMenu.autoRefreshPeriod;
            representation.SetText(MenuTextRenderer.RenderMenu(currentMenu, currentMenu.currentLine));
        }


    }
}