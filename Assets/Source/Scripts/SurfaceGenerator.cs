using UnityEngine;

using static GameData;

public class SurfaceGenerator : MonoBehaviour
{
    [SerializeField] private GameObject surfacePrefab;
    [SerializeField] private Vector3 Grid = new Vector3(40, 0, 40);
    [SerializeField] private int checkFrames = 4;

    // ~~~~ Round to nearest Grid point ~~~~
    public Vector3 SnapCalculate(Vector3 playerPos)
    {
        float x = playerPos.x - playerPos.x % Grid.x;
        float z = playerPos.z - playerPos.z % Grid.z;

        return new Vector3(x, 0, z);
    }
    private void Update()
    {
        if (Time.frameCount % checkFrames == 0)
        {
            if (!_gameSystem.Playing)
            {
                return;
            }

            CheckGround();
        }
    }
    private void CheckGround()
    {
        var newPos = SnapCalculate(_gameSystem.player.transform.position);
        transform.position = newPos;
    }
}