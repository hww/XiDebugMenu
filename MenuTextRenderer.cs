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
using VARP.StringTools;

namespace VARP.DebugMenus
{

    public class MenuTextRenderer
    {
        private const string SUFFIX = " "; 
        private const string PREFIX = " "; 
        private const string PREFIX_SELECTED = ">";
        private const string SPACE = "  ";
        private const char CHAR_SQUARE_DOT = '▪';
        private const char CHAR_DASHED_LINE = '-';
        private const char CHAR_NBSP = (char)0xA0; 
        private const char CHAR_LIGHT_HORIZONTAL = '─';
            
        private const  string upperLineFormat  = "┌{0}┐";
        private const  string middleLineFormat = "├{0}┤";
        private const  string bottomLineFormat = "└{0}┘";
        private const  string menuLineFormat   = "│{0}│";
        
        private static readonly BetterStringBuilder stringBuilder = new BetterStringBuilder(80*40);
        
    
        public enum MenuOptions
        {
            Default
        }


        public static string RenderMenu(DebugMenuC sender, DebugMenu debugMenu, int selected,
            MenuOptions options = MenuOptions.Default)
        {
            stringBuilder.Clear();
            // update texts and the width of the fields
            debugMenu.SendToChildren(sender, DebugMenu.EvenTag.Render);
            var menuCount = debugMenu.Count;

            debugMenu.UpdateWidth(SPACE.Length);
            
            var lineWidth = debugMenu.widthOfNameAnValue + SUFFIX.Length + PREFIX.Length;

            string singleLine = null;
            string justLine = new string(CHAR_LIGHT_HORIZONTAL, lineWidth);
            
            
            string itemFormat1 = $"{{0}}<color={{3}}>{{1,-{debugMenu.widthOfNameAnValue}}}</color>{{2}}";
            string itemFormat2 = $"{{0}}<color={{4}}>{{1,-{debugMenu.widthOfName}}}</color>{SPACE}<color={{5}}>{{2,{debugMenu.widthOfValue}}}</color>{{3}}";

            var menuName = string.Format(itemFormat1, PREFIX, debugMenu.label, SUFFIX, debugMenu.labelColor);
            stringBuilder.AppendLine(menuName);
            stringBuilder.AppendLine(justLine);

            var order = -1;
            
            for (var i = 0; i < menuCount; i++)
            {
                var isSelected = i == selected;
                var menuItem = debugMenu[i];

                order++;
                
                if (order >= 0 && Math.Abs(order - menuItem.order) > 1) 
                    stringBuilder.AppendLine(justLine);   
                order = menuItem.order;
                
                var prefix = isSelected ? PREFIX_SELECTED : PREFIX;
                string item;
                if (menuItem is DebugMenu)
                {
                    item = string.Format(itemFormat1, prefix, menuItem.label + "...", SUFFIX, menuItem.labelColor);
                }
                else
                {
                    if (menuItem.label != null)
                    {
                        if (menuItem.value != null)
                            item = string.Format(itemFormat2, prefix, menuItem.label, menuItem.value, SUFFIX, menuItem.labelColor, menuItem.valueColor);
                        else
                            item = string.Format(itemFormat1, prefix, menuItem.label, SUFFIX, menuItem.labelColor);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
                stringBuilder.AppendLine(item);
            }
            return stringBuilder.ToString();
        }
        
        public static string RenderMenu_WithFrame(DebugMenuC sender, DebugMenu debugMenu, int selected,
            MenuOptions options = MenuOptions.Default)
        {
            stringBuilder.Clear();
            // update texts and the width of the fields
            debugMenu.SendToChildren(sender, DebugMenu.EvenTag.Render);
            var menuCount = debugMenu.Count;

            debugMenu.UpdateWidth(SPACE.Length);
            
            var lineWidth = debugMenu.widthOfNameAnValue + SUFFIX.Length + PREFIX.Length;

            string singleLine = null;
            string justLine = new string(CHAR_LIGHT_HORIZONTAL, lineWidth);
            
            
            string itemFormat1 = $"{{0}}<color={{3}}>{{1,-{debugMenu.widthOfNameAnValue}}}</color>{{2}}";
            string itemFormat2 = $"{{0}}<color={{4}}>{{1,-{debugMenu.widthOfName}}}</color>{SPACE}<color={{5}}>{{2,{debugMenu.widthOfValue}}}</color>{{3}}";

            stringBuilder.AppendLine(string.Format( upperLineFormat, justLine));
            var menuName = string.Format(itemFormat1, PREFIX, debugMenu.label, SUFFIX, debugMenu.labelColor);
            stringBuilder.AppendLine(string.Format(menuLineFormat, menuName));
            stringBuilder.AppendLine(string.Format(middleLineFormat, justLine));

            var order = -1;
            
            for (var i = 0; i < menuCount; i++)
            {
                var isSelected = i == selected;
                var menuItem = debugMenu[i];

                if (order >= 0 && Math.Abs(order - menuItem.order) > 1) 
                {

                    if (singleLine == null) 
                        singleLine = string.Format(middleLineFormat,justLine);
                    stringBuilder.AppendLine(singleLine);   
                }
                
                order = menuItem.order;
                
                var prefix = isSelected ? PREFIX_SELECTED : PREFIX;
                string item;
                if (menuItem is DebugMenu)
                {
                    item = string.Format(itemFormat1, prefix, menuItem.label + "...", SUFFIX, menuItem.labelColor);
                }
                else
                {
                    if (menuItem.label != null)
                    {
                        if (menuItem.value != null)
                            item = string.Format(itemFormat2, prefix, menuItem.label, menuItem.value, SUFFIX, menuItem.labelColor, menuItem.valueColor);
                        else
                            item = string.Format(itemFormat1, prefix, menuItem.label, SUFFIX, menuItem.labelColor);
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
                stringBuilder.AppendLine(string.Format(menuLineFormat, item));
            }
            stringBuilder.AppendLine(string.Format( bottomLineFormat, justLine));
            return stringBuilder.ToString();
        }
    }
}