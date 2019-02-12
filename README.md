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

![Picture1](Documentation/menu-picture1.png)
![Picture2](Documentation/menu-picture2.png)
![Picture3](Documentation/menu-picture3.png)

## Enum values

```C#
public enum TrafficLight { Red,Green, Blue }
public TrafficLight enumValue;

new DebugMenuEnum<TrafficLight>("Edit/Preferences/TraficLight", () => enumValue, value => enumValue = value, 1);
```

## Keyboard Shortcuts

E show hide menu
ESC close current menu and display previous, or hide menu
W,S move previous and next menu item
A,D edit menu item
R reset value to default

## Colors
- booleans
-- yellow _color for label for enabed feture_
-- white _color for label for disabled feature_
- integers,floats,enums
-- bright green _color for value for default_ 
-- yellow _color for value and label for not default value_
- actions
-- gray _color for inactive action_
-- other _color for active action_ 
