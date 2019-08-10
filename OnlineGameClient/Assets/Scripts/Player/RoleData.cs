using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
public class RoleData
{
    public RoleType RoleType { get; private set; }
    public GameObject RolePrefab { get; private set; }
    public GameObject ArrowPrefab { get; private set; }
    public Vector3 SpawnPosition { get; private set; }
    public GameObject ExplostionEffect { get; private set; }
    public RoleData(RoleType roleType, string rolePath, string arrowPath,string explosionPath,Transform spawnPosition)
    {
        this.RoleType = roleType;
        this.RolePrefab = Resources.Load("Prefabs/" + rolePath) as GameObject;
        this.ArrowPrefab = Resources.Load("Prefabs/" + arrowPath) as GameObject;
        this.ExplostionEffect = Resources.Load("Prefabs/" + explosionPath) as GameObject;
        ArrowPrefab.GetComponent<Arrow>().explosionEffect = ExplostionEffect;
        this.SpawnPosition = spawnPosition.position;
        
    }
	
}
