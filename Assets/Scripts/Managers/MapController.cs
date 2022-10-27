using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    [SerializeField]
    public List<SpawnPoint> blueteam;
	[SerializeField]
    public List<SpawnPoint> redteam;
    void Start()
    {
		//if(PlayerController.Instantiate.status == 1){
        NetworkManager.instance.OnPointTeamInMap(blueteam,redteam);
		//}
    }

    // public class PointJSON
	// {
	// 	public float[] position;
	// 	public PointJSON(SpawnPoint spawnPoint)
	// 	{
	// 		position = new float[] {
	// 			spawnPoint.transform.position.x,
	// 			spawnPoint.transform.position.y,
	// 			spawnPoint.transform.position.z
	// 		};
	// 	}
	// }
}
