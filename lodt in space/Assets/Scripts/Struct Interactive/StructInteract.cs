using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StructType
{ 
    Storage,
    WaterCollector,
    Campfire
}

public class StructInteract : MonoBehaviour
{
    public int RangeToInteract;
    public Transform Player;
    public GameObject StructUI;
    public GameObject GameControllUI;

    public void SetUse(Transform player)
    {
        GameControllUI = GameObject.FindGameObjectWithTag("GameControll");
        Player = player;
        StartCoroutine("UseStruct");
    }

    private IEnumerator UseStruct()
    {
        bool playerIsFarToStruct = true;

        while (playerIsFarToStruct)
        {
            if (Vector3.Distance(Player.position, transform.position) <= RangeToInteract)
            {
                Player.GetComponent<Animator>().SetFloat("Moving", 0f);
                playerIsFarToStruct = false;
                Use();
            }
            else
            {
                Vector3 vectorToTarget = transform.position - Player.transform.position;
                while (Vector3.Distance(Player.position, transform.position) > RangeToInteract)
                {
                    Quaternion lookDir = Quaternion.LookRotation(vectorToTarget);
                    Quaternion targetRot = Quaternion.Slerp(Player.rotation, lookDir, 0.025f);
                    Player.rotation = targetRot;

                    Player.GetComponent<Animator>().SetFloat("Moving", 1f);
                    yield return null;
                }
            }
        }
    }

    public void CloseStructUI()
    {
        StructUI.SetActive(false);
        GameControllUI.SetActive(true);
    }

    private void Use()
    {
        StructUI.SetActive(true);
        GameControllUI.SetActive(false);
    }
}
