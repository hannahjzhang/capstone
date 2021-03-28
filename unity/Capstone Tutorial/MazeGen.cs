using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGen : MonoBehaviour
{
    public TextAsset mazeFile;
    public GameObject wall;
    [HideInInspector] public Vector3 scale = new Vector3(3, 5, 3);
    // Start is called before the first frame update
    void Start()
    {
        string mapStr = mazeFile.text;
        Debug.Log(mapStr);

        mapStr = mapStr.Replace("\n", "");
        mapStr = mapStr.Replace("\r", "");
        char[,] map = new char[21, 43];
        int strIndex = 0;
        for (int row = 0; row < map.GetLength(0); ++row)
        {
            for (int col = 0; col < map.GetLength(1); ++col)
            {
                map[row, col] = mapStr[strIndex++];
            }
        }
        for (int row = 0; row < map.GetLength(0); ++row)
        {
            for (int col = 0; col < map.GetLength(1); ++col)
            {
                char c = map[row, col];
                // position a wall if the 2d map is a #
                if (c == '#')
                {
                    GameObject w = Instantiate(wall);
                    w.transform.position = new Vector3(scale.x*row, transform.position.y, scale.z*col);
                    w.transform.localScale = scale;
                    w.transform.SetParent(this.transform);
                }
                else
                {
                    GameObject w = Instantiate(wall);
                    w.transform.position = new Vector3(scale.x * row, transform.position.y - 0.5f * scale.y, scale.z * col);
                    w.transform.localScale = new Vector3(scale.x, 0.5f, scale.z);
                    w.transform.SetParent(this.transform);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}