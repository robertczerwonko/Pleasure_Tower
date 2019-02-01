using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_Spawner : MonoBehaviour {

    [Header("Tower Elements")]
    [SerializeField] Transform TowerBodyTransform;

    [SerializeField] GameObject RedE;
    [SerializeField] GameObject FallE;

    private List<Vector3> SpawnPoints = new List<Vector3>();

    private int howManyRows;

    private float amountOfPrefabsInRow;

    [HideInInspector] public float _towerHeight;
    [HideInInspector] public float _towerScale;

    public void StartTower(float theTowerHeight, float theTowerScale, float theRotatingSpeed)
    {
        _towerHeight = theTowerHeight;
        _towerScale = theTowerScale;
        GetComponent<Tower_Movement>()._rotateSpeed = theRotatingSpeed;
        TowerBodyTransform.localScale = new Vector3(_towerScale, _towerHeight, _towerScale);
        GatherInformation();
        CountSpawnPoints();
        SpawnFallingElements();
    }

    #region Private methods
    private void GatherInformation()
    {
        float amoutPre = (_towerScale * 2);
        amountOfPrefabsInRow = amoutPre > 3 ? amoutPre + 4 : amoutPre + 3;
     

        if (_towerScale > 1)
            Camera.main.orthographicSize = _towerScale + 1f;
        else
            Camera.main.orthographicSize = _towerScale;

        Camera.main.orthographicSize = 5f;
        howManyRows = (int)_towerHeight * 2;
    }

    private void CountSpawnPoints()
    {

        SpawnPoints = new List<Vector3>();
        float r = _towerScale / 2;
        float angSpace = 360 / amountOfPrefabsInRow;
        for (int i = 0; i < amountOfPrefabsInRow; i++)
        {
            SpawnPoints.Add(positionOnCircle(transform.position, r,angSpace * i));
        }
        Tower_Movement.instance.AllowRotate = true;
    }

    private void SpawnFallingElements()
    {
        GameObject tempObject;
        int maxRedInRow = (SpawnPoints.Count - 1) / 2;
        //int maxRedInRow = 1;
        int counterRed = 0;
        for (int i = 1; i < howManyRows - 1; i++)
        {
            foreach (Vector3 pos in SpawnPoints)
            {
                Vector3 tempPos = new Vector3(pos.x, i - howManyRows / 2, pos.z);
                
                if (Random.Range(0, 6) == 0 && i < howManyRows - 3 && counterRed < maxRedInRow)
                {
                    tempObject = Instantiate(RedE) as GameObject;
                    counterRed++;
                }
                else
                {
                    tempObject = Instantiate(FallE) as GameObject;
                }

                tempObject.transform.position = tempPos;
                tempObject.transform.LookAt(new Vector3(transform.position.x, i - howManyRows / 2, transform.position.z));
                tempObject.transform.parent = transform;
            }
            counterRed = 0;
        }
    }

    private Vector3 positionOnCircle(Vector3 center, float r, float ang)
    {
        Vector3 pos;
        pos.x = center.x + r * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        pos.z = center.z + r * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }

    #endregion





}
