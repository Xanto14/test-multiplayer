#if UNITY_EDITOR
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class ResourcesPrefabPathBuilder : IPreprocessBuildWithReport
{
    public int callbackOrder {get { return 0; } }


    public void OnPreprocessBuild(BuildReport report)
    {
        MasterManager.PopulatedNetworkedPrefabs();
    }

}
#endif