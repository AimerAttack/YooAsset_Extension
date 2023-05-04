using UnityEditor;
using UnityEngine;

namespace YooAsset.Editor
{
    [InitializeOnLoad]
    internal static class InspectorDrawer
    {
        static InspectorDrawer()
        {
            UnityEditor.Editor.finishedDefaultHeaderGUI += OnPostHeaderGUI;
            
            EventModule.AddListener(Event.AddPackage, OnAddPackage);
            EventModule.AddListener(Event.RemovePackage, OnRemovePackage);
            EventModule.AddListener(Event.ChangePackageName, OnChangePackageName);
            EventModule.AddListener(Event.AddGroup, OnAddGroup);
            EventModule.AddListener(Event.RemoveGroup, OnRemoveGroup);
            EventModule.AddListener(Event.ChangeGroupName, OnChangeGroupName);
            EventModule.AddListener(Event.AddCollector, OnAddCollector);
            EventModule.AddListener(Event.RemoveCollector, OnRemoveCollector);
            EventModule.AddListener(Event.ChangeCollector, OnChangeCollector);
            EventModule.AddListener(Event.Undo, OnUndo);
        }

        static void OnPostHeaderGUI(UnityEditor.Editor editor)
        {
            GUILayout.Button("test");
        }

        static void OnAddPackage()
        {
            Debug.Log("OnAddPackage");
        }
        
        static void OnRemovePackage()
        {
            Debug.Log("OnRemovePackage");
        }
        
        static void OnChangePackageName()
        {
            Debug.Log("OnChangePackageName");
        }
        
        static void OnAddGroup()
        {
            Debug.Log("OnAddGroup");
        }
        
        static void OnRemoveGroup()
        {
            Debug.Log("OnRemoveGroup");
        }
        
        static void OnChangeGroupName()
        {
            Debug.Log("OnChangeGroupName");
        }
        
        static void OnAddCollector()
        {
            Debug.Log("OnAddCollector");
        }
        
        static void OnRemoveCollector()
        {
            Debug.Log("OnRemoveCollector");
        }
        
        static void OnChangeCollector()
        {
            Debug.Log("OnChangeCollector");
        }
        
        static void OnUndo()
        {
            Debug.Log("OnUndo");
        }
    }
}