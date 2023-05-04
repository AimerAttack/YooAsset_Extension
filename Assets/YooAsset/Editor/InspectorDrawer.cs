using System;
using System.Collections.Generic;
using System.IO;
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
        }

        static void OnPostHeaderGUI(UnityEditor.Editor editor)
        {
            if (editor.target != null)
            {
                if (TryGetPathAndGUIDFromTarget(editor.target, out var path, out var guid))
                {
                    //获取CollectorSetting
                    var found = false;
                    var setting = AssetBundleCollectorSettingData.Setting;
                    AssetBundleCollectorPackage targetPackage = null;
                    AssetBundleCollectorGroup targetGroup = null;
                    var targetCollector = -1;
                    foreach (var package in setting.Packages)
                    {
                        foreach (var group in package.Groups)
                        {
                            if (group.ActiveRuleName != nameof(EnableGroup))
                                continue;
                            for (int i = 0; i < group.Collectors.Count; i++)
                            {
                                var collector = group.Collectors[i];
                                if (string.IsNullOrEmpty(collector.CollectPath))
                                    continue;
                                if (AssetDatabase.IsValidFolder(collector.CollectPath))
                                {
                                    var folderPath = collector.CollectPath;
                                    folderPath = folderPath.Replace('\\', Path.DirectorySeparatorChar)
                                        .Replace('/', Path.DirectorySeparatorChar);
                                    if (path.StartsWith(folderPath, StringComparison.OrdinalIgnoreCase))
                                    {
                                        found = true;
                                        targetPackage = package;
                                        targetGroup = group;
                                        targetCollector = i;
                                    }
                                }
                                else
                                {
                                    var assetPath = collector.CollectPath;
                                    if (assetPath.StartsWith("Assets/") == false &&
                                        assetPath.StartsWith("Packages/") == false)
                                    {
                                        continue;
                                    }

                                    Type assetType = AssetDatabase.GetMainAssetTypeAtPath(assetPath);
                                    if (assetType == typeof(LightingDataAsset))
                                        continue;
                                    if (assetType == typeof(UnityEditor.DefaultAsset))
                                        continue;
                                    if (collector.CollectorGUID == guid)
                                    {
                                        found = true;
                                        targetPackage = package;
                                        targetGroup = group;
                                        targetCollector = i;
                                    }
                                }

                                if (found)
                                    break;
                            }

                            if (found)
                                break;
                        }

                        if (found)
                            break;
                    }

                    if (found)
                    {
                        GUILayout.Label(string.Format("Package: {0}", targetPackage.PackageName));
                        GUILayout.Label(string.Format("Group: {0}", targetGroup.GroupName));
                        GUILayout.Label(string.Format("CollectorIndex: {0}", targetCollector));
                    }
                }
            }
        }


        static HashSet<string> excludedExtensions = new HashSet<string>(new string[]
            {".cs", ".js", ".boo", ".exe", ".dll", ".meta", ".preset", ".asmdef"});

        static bool TryGetPathAndGUIDFromTarget(UnityEngine.Object target, out string path, out string guid)
        {
            guid = string.Empty;
            path = string.Empty;
            if (target == null)
                return false;
            path = AssetDatabase.GetAssetOrScenePath(target);
            path = path.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
            if (!IsPathValidForEntry(path))
                return false;
            if (!AssetDatabase.TryGetGUIDAndLocalFileIdentifier(target, out guid, out long id))
                return false;
            return true;
        }

        static bool IsPathValidForEntry(string path)
        {
            if (string.IsNullOrEmpty(path))
                return false;
            path = path.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
            if (!path.StartsWith("assets", StringComparison.OrdinalIgnoreCase) && !IsPathValidPackageAsset(path))
                return false;
            if (path == "Assets")
                return false;
            return !excludedExtensions.Contains(Path.GetExtension(path));
        }

        static bool IsPathValidPackageAsset(string path)
        {
            string[] splitPath = path.ToLower().Split(Path.DirectorySeparatorChar);

            if (splitPath.Length < 3)
                return false;
            if (splitPath[0] != "packages")
                return false;
            if (splitPath[2] == "package.json")
                return false;
            return true;
        }
    }
}