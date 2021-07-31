using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyPackage : MonoBehaviour
{
    private Container container;
    public Container Container
    {
        get => container;
        set
        {
            container = value;
            StopAllCoroutines();
            StartCoroutine(RotatingPackage(container == null));
        }
    }

    public float rotationSpeed;
    public float pouringOutSpeed = 10;
    public float capacity = 250;
    public float currentGrams;
    private Coroutine pouringOutCoroutine;
    private GameObject progressBar;
    private float maxProgressBarScale;
    public Game.SupplyType supplyType;

    private void Start()
    {
        currentGrams = capacity;
        progressBar = transform.GetChild(0).GetChild(0).gameObject;
        maxProgressBarScale = progressBar.transform.localScale.x;
    }

    IEnumerator RotatingPackage(bool reverse)
    {
        float needAngle = reverse ? 360 : 200;
        while (Mathf.Abs(transform.rotation.eulerAngles.z - needAngle) > 0.25f)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y,
                Mathf.LerpAngle(transform.rotation.eulerAngles.z, needAngle, Time.fixedDeltaTime * rotationSpeed));
            if (transform.rotation.eulerAngles.z <= 240 && pouringOutCoroutine == null)
            {
                pouringOutCoroutine = StartCoroutine(PouringOutPackage());
            }

            if (transform.rotation.eulerAngles.z > 240 && pouringOutCoroutine != null)
            {
                StopCoroutine(pouringOutCoroutine);
                pouringOutCoroutine = null;
            }
            yield return new WaitForFixedUpdate();
        }
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, needAngle);
        if (currentGrams == 0)
        {
            Destroy(this);
        }
    }

    IEnumerator PouringOutPackage()
    {
        while (Container != null)
        {
            float grams = Mathf.Clamp(Mathf.Clamp(pouringOutSpeed * Time.fixedDeltaTime, 0, currentGrams), 0,
                container.capacity - container.currentGrams);
            currentGrams -= grams;
            container.currentGrams += grams;
            progressBar.transform.localScale = new Vector3(maxProgressBarScale * currentGrams / capacity, progressBar.transform.localScale.y, progressBar.transform.localScale.z);
            if (container.currentGrams == container.capacity)
            {
                Container = null;
            }
            if (currentGrams == 0)
            {
                Container = null;
            }
            yield return new WaitForFixedUpdate();
        }
    }
}