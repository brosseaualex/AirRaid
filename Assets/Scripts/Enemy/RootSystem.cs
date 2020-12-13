using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootSystem
{
    public static bool DEBUG_preventRootSpawning = false;
    public int numberOfRoots = 0;
    RootNode masterRootNode;

    public RootSystem(Vector3 firstPosition)
    {
        if (!DEBUG_preventRootSpawning)
        {
            GameObject newRootNodeObj = GameObject.Instantiate(EnemyManager.rootPrefab);
            newRootNodeObj.transform.position = firstPosition;
            newRootNodeObj.transform.SetParent(EnemyManager.Instance.rootNodeParent);
            masterRootNode = newRootNodeObj.GetComponent<RootNode>();
            masterRootNode.Initialize(null, this);
            GameObject.Destroy(masterRootNode.GetComponent<LineRenderer>()); //first one is at center, does not need line renderer
        }
    }

    public void RootSystemDied()
    {
        masterRootNode.DestroyRootSystem();
    }

    public void Refresh()
    {
        if (!DEBUG_preventRootSpawning)
        {
            masterRootNode.RefreshRootNode();
        }
    }
}
