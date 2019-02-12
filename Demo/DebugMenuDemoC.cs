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

namespace VARP.DebugMenus.Demo
{
    public class DebugMenuDemC : MonoBehaviour
    {
        public enum TrafficLight
        {
            Red,
            Gree,
            Blue
        }

        public TrafficLight enumValue;
        public bool toggleValue;
        public int integerValue;
        public float floatValue;
        
        private void OnEnable()
        {

            new DebugMenu("Edit/Preferences");
            new DebugMenuToggle("Edit/Preferences/Toggle", () => toggleValue, value => toggleValue = value, 1);
            new DebugMenuInteger("Edit/Preferences/Integer", () => integerValue, value => integerValue = value, 1);
            new DebugMenuFloat("Edit/Preferences/Float", () => floatValue, value => floatValue = value, 1);
            new DebugMenuAction("Edit/Preferences/Action", () => { Debug.Log("Action"); }, 1);
            new DebugMenuEnum<TrafficLight>("Edit/Preferences/TraficLight", () => enumValue, value => enumValue = value, 1);
            new DebugMenu("Edit/Preferences/Extra Preferences", 2);

        }
    }
}