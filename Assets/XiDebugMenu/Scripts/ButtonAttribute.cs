using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace XiDebugMenu
{
#if UNITY_EDITOR
public class ButtonDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        string methodName = (attribute as ButtonAttribute).MethodName;
        Object target = property.serializedObject.targetObject;
        System.Type type = target.GetType();
        System.Reflection.MethodInfo method = type.GetMethod(methodName);
        if (method == null)
        {
            GUI.Label(position, "Method could not be found. Is it public?");
            return;
        }
        if (method.GetParameters().Length > 0)
        {
            GUI.Label(position, "Method cannot have parameters.");
            return;
        }
        if (GUI.Button(position, method.Name))
        {
            method.Invoke(target, null);
        }
    }
}
#endif

public class ButtonAttribute : PropertyAttribute
{
    public string MethodName { get; }

    public ButtonAttribute(string methodName)
    {
        MethodName = methodName;
    }
}

}