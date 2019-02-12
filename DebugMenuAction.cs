using System;

namespace VARP.DebugMenus
{
    public class DebugMenuAction : DebugMenuItem
    {
        private readonly Action action;

        public DebugMenuAction(string path, Action action, int order = 0)
            : base(path, order)
        {
            this.action = action;
            this.value = null;    // do not have value, wil display it by color
        }
        
        public override void OnEvent(DebugMenuC sender, DebugMenu.EvenTag tag)
        {
            switch (tag)
            {
                case DebugMenu.EvenTag.Render:
                    Render();
                    break;
                case DebugMenu.EvenTag.Increment:
                    action();
                    break;
                case DebugMenu.EvenTag.Decrement:
                    break;
            }
        }

        private void Render()
        {
            nameColor = action != null ? WHITE : GRAY;
        }
    }
}