using System.Collections.Generic;
using UnityEngine;

public enum BuildingType
{ 
    Floor,
    Construction,
    Wall,
    Door
}

public class Building : MonoBehaviour
{
    public BuildingType Type;
    public Renderer MainRenderer;
    public Vector2Int Size = Vector2Int.one;
    public int NeedFloorLevel;
    public List<GameObject> Walls;
    public GameObject Slot;
    public List<GameObject> Colliders;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Ground" && other.transform.tag != "WallPoint")
            Colliders.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag != "Ground" && other.transform.tag != "WallPoint")
            Colliders.Remove(other.gameObject);
    }

    public void SetTransparent(bool available)
    {
        if (available)
        {
            MainRenderer.material.color = Color.green;
        }
        else
        {
            MainRenderer.material.color = Color.red;
        }
    }

    public void SetNormal()
    {
        MainRenderer.material.color = Color.white;
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                if ((x + y) % 2 == 0) Gizmos.color = new Color(0.88f, 0f, 1f, 0.3f);
                else Gizmos.color = new Color(1f, 0.68f, 0f, 0.3f);

                Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1, .1f, 1));
            }
        }
    }
}