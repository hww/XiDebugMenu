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

using TMPro;
using UnityEngine;

namespace XiDebugMenu
{
    public class DebugMenuRepresentation : MonoBehaviour
    {
        [Header("Managed Objects")]
        public Canvas canvas;
        public TextMeshProUGUI menuText;
        public Transform backgroundTransform;
        [Header("Settings")]
        public int fontSize = 16;

        private float hideTextAt;
        
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
            DebugMenuSystem.Initialize();
        }

        private void OnDisable()
        {
            DebugMenuSystem.DeInitialize();
        }

        void Update()
        {
            DebugMenuSystem.Update();
            if (hideTextAt > 0 && Time.unscaledTime > hideTextAt)
            {
                hideTextAt = -1f;
                SetVisible(false);
            }
        }

        // =============================================================================================================
        // Manipulating by menu
        // =============================================================================================================

        public void SetVisible(bool state)
        {
            canvas.enabled = state;
        }

        public void SetText(string text)
        {
            menuText.text = text;
            hideTextAt = -1;
        }

        public void FlashText(string text)
        {
            menuText.text = text;
            hideTextAt = Time.unscaledTime + 1.5f;
            SetVisible(true);
        }
    }
}