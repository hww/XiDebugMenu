# Debug Menu

It is easy to use, lightweight library forked from [wataru-ito/DebugMenu](https://github.com/wataru-ito/DebugMenu) but deeply modifyed.

Renders text only in game debug menu.  

```C#

// The fields to modify by menu
public bool toggleValue;
public int integerValue;
public float floatValue;
        
// Create new menu
new DebugMenu("Edit/Preferences");
new DebugMenuToggle("Edit/Preferences/Toggle", () => toggleValue, value => toggleValue = value, 1);
new DebugMenuInteger("Edit/Preferences/Integer", () => integerValue, value => integerValue = value, 1);
new DebugMenuFloat("Edit/Preferences/Float", () => floatValue, value => floatValue = value, 1);
new DebugMenuAction("Edit/Preferences/Action", () => { Debug.Log("Action"); }, 1);
new DebugMenu("Edit/Preferences/Extra Preferences", 2);
```
