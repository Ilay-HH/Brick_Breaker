using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capsule_Controller : MonoBehaviour
{
    [SerializeField]
    private int id;
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime);
    }
    public int GetID()
    {
        return id;
    }

}
