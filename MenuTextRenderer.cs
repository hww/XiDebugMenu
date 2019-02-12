using System;
using System.Collections.Generic;
using UnityEngine;
using VARP.Keyboard;
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
        
     
        public static string RenderMenu(DebugMenuC sender, DebugMenu debugMenu, int selected, MenuOptions options = MenuOptions.Default)
        {
            stringBuilder.Clear();
            // update texts and the width of the fields
            debugMenu.SendToChildren(sender, DebugMenu.EvenTag.Render);
            var menuCount = debugMenu.Count;

            debugMenu.UpdateWidth(SPACE.Length);
            
            var lineWidth = debugMenu.widthOfNameAnValue + SUFFIX.Length + PREFIX.Length;

            string singleLine = null;
            string justLine = new string(CHAR_LIGHT_HORIZONTAL, lineWidth);
            
            
            string itemFormat1 = $"{{0}}<color=#{{3}}>{{1,-{debugMenu.widthOfNameAnValue}}}</color>{{2}}";
            string itemFormat2 = $"{{0}}<color=#{{4}}>{{1,-{debugMenu.widthOfName}}}</color>{SPACE}<color=#{{5}}>{{2,{debugMenu.widthOfValue}}}</color>{{3}}";

            stringBuilder.AppendLine(string.Format( upperLineFormat, justLine));
            var menuName = string.Format(itemFormat1, PREFIX, debugMenu.name, SUFFIX, debugMenu.nameColor);
            stringBuilder.AppendLine(string.Format(menuLineFormat, menuName));
            stringBuilder.AppendLine(string.Format(middleLineFormat, justLine));

            var order = -1;
            
            for (var i = 0; i < menuCount; i++)
            {
                var isSelected = i == selected;
                var meniItem = debugMenu[i];

                if (order < 0)
                {
                    order = meniItem.order;
                }
                else if (order != meniItem.order)
                {
                    order = meniItem.order;
                    if (singleLine == null) 
                        singleLine = string.Format(middleLineFormat,justLine);
                    stringBuilder.AppendLine(singleLine);   
                }
   
                var prefix = isSelected ? PREFIX_SELECTED : PREFIX;
                string item;
                if (meniItem is DebugMenu)
                {
                    item = string.Format(itemFormat1, prefix, meniItem.name + "...", SUFFIX, meniItem.nameColor);
                }
                else
                {
                    if (meniItem.name != null)
                    {
                        if (meniItem.value != null)
                            item = string.Format(itemFormat2, prefix, meniItem.name, meniItem.value, SUFFIX, meniItem.nameColor, meniItem.valueColor);
                        else
                            item = string.Format(itemFormat1, prefix, meniItem.name, SUFFIX, meniItem.nameColor);
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