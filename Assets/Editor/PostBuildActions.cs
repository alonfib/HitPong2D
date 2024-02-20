using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

public class PostBuildActions
{
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target == BuildTarget.iOS)
        {
            RemoveNestedFrameworks(pathToBuiltProject);
        }
    }

    private static void RemoveNestedFrameworks(string pathToBuiltProject)
    {
        string unityFrameworkPath = Path.Combine(pathToBuiltProject, "Frameworks/UnityFramework.framework/Frameworks");

        if (Directory.Exists(unityFrameworkPath))
        {
            Directory.Delete(unityFrameworkPath, true);
        }
    }
}
