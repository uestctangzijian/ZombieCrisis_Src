using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject bloodPrefab;

    public void spawnBlood(Transform transform)
    {
        Vector3 spawnPos = transform.position;
        spawnPos.y = Random.Range(0.01f, 0.05f);
        float zRotation = Random.Range(0f, 180f);
        Transform bloodTrans = Instantiate(bloodPrefab, spawnPos, Quaternion.Euler(90f, 0, zRotation)).transform;
        bloodTrans.parent = transform;
        Vector3 randomizedScale = bloodTrans.transform.localScale;
        randomizedScale.x *= Random.Range(1.0f, 1.2f);
        randomizedScale.y *= Random.Range(1.0f, 1.2f);
        bloodTrans.transform.localScale = randomizedScale;

        Destroy(bloodTrans.gameObject, 60f);
    }
}
