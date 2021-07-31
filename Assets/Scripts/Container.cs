using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public int capacity = 200;
    private Game.SupplyType supplyType;
    public float currentGrams = 0;
    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SupplyPackage>() != null && other.GetComponent<SupplyPackage>().supplyType == supplyType)
        {
            other.GetComponent<SupplyPackage>().Container = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<SupplyPackage>() != null && other.GetComponent<SupplyPackage>().supplyType == supplyType)
        {
            other.GetComponent<SupplyPackage>().Container = null;
        }
    }
}