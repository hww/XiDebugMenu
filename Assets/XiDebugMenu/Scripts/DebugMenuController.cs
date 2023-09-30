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

using System.Runtime.CompilerServices;
using UnityEngine;

namespace XiDebugMenu
{
    public class DebugMenuController
    {
        private DebugMenuItem.EvenTag lastEvent;
        private float repeatTimer;
        private const float REPEAT_DELAY = 0.75f;
        private const float REPEAT_INTERVAL = 0.1f;

        public DebugMenuItem.EvenTag GetEvet()
        {
            var evt = GetEvetInternal();
            if (lastEvent != evt)
            {
                lastEvent = evt;
                repeatTimer = REPEAT_DELAY;
                return lastEvent;
            }
            else
            {
                if (lastEvent == DebugMenuItem.EvenTag.Null)
                    return DebugMenuItem.EvenTag.Null;

                repeatTimer -= Time.deltaTime;
                if (repeatTimer < 0)
                {
                    repeatTimer = REPEAT_INTERVAL;
                    return lastEvent;
                }
                return DebugMenuItem.EvenTag.Null;
            }

        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private DebugMenuItem.EvenTag GetEvetInternal()
        { 
            if (Input.GetKey(KeyCode.E))
                return DebugMenuItem.EvenTag.ToggleMenu;
            if (Input.GetKey(KeyCode.F))                    
                return DebugMenuItem.EvenTag.ToggleQuickMenu;
            if (Input.GetKey(KeyCode.Q))
                return DebugMenuItem.EvenTag.CloseMenu;

            if (DebugMenuSystem.isVisible)
            {
                if (Input.GetKey(KeyCode.W))
                    return DebugMenuItem.EvenTag.Up;
                if (Input.GetKey(KeyCode.S))
                    return DebugMenuItem.EvenTag.Down;
                if (Input.GetKey(KeyCode.A))
                    return DebugMenuItem.EvenTag.Left;
                if (Input.GetKey(KeyCode.D))
                    return DebugMenuItem.EvenTag.Right;
                if (Input.GetKey(KeyCode.R))
                    return DebugMenuItem.EvenTag.Reset;
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (Input.GetKey(KeyCode.A))
                        return DebugMenuItem.EvenTag.Left;
                    if (Input.GetKey(KeyCode.D))
                        return DebugMenuItem.EvenTag.Right;
                    if (Input.GetKey(KeyCode.R))
                        return DebugMenuItem.EvenTag.Reset;
                }
            }
            return DebugMenuItem.EvenTag.Null;
        }
    }
}