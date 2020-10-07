using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[ExecuteAlways]
class SavableEntity : MonoBehaviour
{
    [SerializeField] private string uniqueIdentifier = "";
    private static readonly Dictionary<string, SavableEntity> GlobalLookup = new Dictionary<string, SavableEntity>();

#if UNITY_EDITOR
    private void Update()
    {
        if (Application.IsPlaying(gameObject)) return;
        if (string.IsNullOrEmpty(gameObject.scene.path)) return;

        SerializedObject serializedObject = new SerializedObject(this);
        SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

        if(string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
        {
            property.stringValue = Guid.NewGuid().ToString();
            serializedObject.ApplyModifiedProperties();
        }

        GlobalLookup[property.stringValue] = this;
    }

    private bool IsUnique(string candidate)
    {
        if (!GlobalLookup.ContainsKey(candidate)) return true;
            
        if (GlobalLookup[candidate] == this) return true;
            
        if (GlobalLookup[candidate] == null)
        {
            GlobalLookup.Remove(candidate);
            return true;
        }

        if (GlobalLookup[candidate].GetUniqueIdentifier() != candidate)
        {
            GlobalLookup.Remove(candidate);
            return true;
        }
            
        return false;
    }
#endif

    public string GetUniqueIdentifier()
    {
        return uniqueIdentifier;
    }

    public object CaptureState()
    {
        Dictionary<string, object> state = new Dictionary<string, object>();

        foreach (ISavable savable in GetComponents<ISavable>())
        {
            state[savable.GetType().ToString()] = savable.CaptureState();
        }
        return state;
    }

    public void RestoreState(object state)
    {
        Dictionary<string, object> stateDict = (Dictionary<string, object>)state;

        foreach (ISavable savable in GetComponents<ISavable>())
        {
            string typeString = savable.GetType().ToString();
            if (stateDict.ContainsKey(typeString))
            {
                savable.RestoreState(stateDict[typeString]);
            }
        }
    }
}
