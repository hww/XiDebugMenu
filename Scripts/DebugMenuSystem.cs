using UnityEngine;

namespace XiDebugMenu
{
    public class DebugMenuSystem
    {
        public enum Menu
        {
            
        }
        public static readonly DebugMenu RootDebugMenu = new DebugMenu(null, "Debug Menu");
        public static readonly DebugMenu RootQuickMenu = new DebugMenu(null, "Quick Menu");
        public static DebugMenuRepresentation debugMenuRepresentation;
        public static AlertBoxRepresentation alertBoxRepresentation;
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
            debugMenuRepresentation = Object.FindObjectOfType<DebugMenuRepresentation>();
            Debug.Assert(debugMenuRepresentation != null);
            alertBoxRepresentation = Object.FindObjectOfType<AlertBoxRepresentation>();
            Debug.Assert(alertBoxRepresentation != null);
        }

        /// <summary>
        /// DeInitialize menu system
        /// </summary>
        public static void DeInitialize()
        {
            SetVisible(false);
            RootDebugMenu.ClearDownTree();
            debugMenuRepresentation = null;
            currentMenu = null;
        }

        public static void Update()
        {

            // Refresh menu time to time if autoRefreshPeriod is more than zero
            if (autoRefresh && Time.unscaledTime > autoRefreshAt)
                currentMenu.RequestRefresh();

            var evt = controller.GetEvet();
            if (evt != DebugMenuItem.EvenTag.Null)
                SendEvent(evt);
            
            if (currentMenu == null)
                return;
            
            if (currentMenu.DoRefresh)
                Render();
        }
        
        public static void SendEvent(DebugMenuItem.EvenTag tag)
        {
            if (tag == DebugMenuItem.EvenTag.ToggleMenu)
            {
                if (isVisible)
                {
                    SetVisible(false);
                }
                else
                {
                    currentMenu = RootDebugMenu;
                    SetVisible(true);
                }
            }
            else if (tag == DebugMenuItem.EvenTag.ToggleQuickMenu)
            {
                if (isVisible)
                {
                    SetVisible(false);
                }
                else
                {
                    currentMenu = RootQuickMenu;
                    SetVisible(true);
                }
            }
            else if (tag == DebugMenuItem.EvenTag.CloseMenu)
            {
                CloseMenu();
            }
            else
            {
                if (currentMenu != null)
                {
                    currentMenu.OnEvent(tag);
                    if (isVisible)
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
            debugMenuRepresentation?.SetVisible(state);
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
            if (currentMenu == null)
                return;
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
            currentMenu.DoRefresh = false;
            autoRefresh = currentMenu.autoRefreshPeriod > 0;
            autoRefreshAt = Time.unscaledTime + currentMenu.autoRefreshPeriod;
            debugMenuRepresentation.SetText(MenuTextRenderer.RenderMenu(currentMenu, currentMenu.currentLine));
        }

        // =============================================================================================================
        // Representation 
        // =============================================================================================================

        public static void FlashText(string text)
        {
            alertBoxRepresentation?.FlashText(text);
        }

    }
}