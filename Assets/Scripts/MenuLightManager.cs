using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLightController: MonoBehaviour
{
    // List to hold all the area lights in the scene
    private List<Light> areaLights;

    // Time delay between each light turning on (in seconds)
    public float delay = 8.0f;

    private void Start()
    {
        // Find all area lights in the scene
        areaLights = new List<Light>();

        // Assuming all area lights have a specific tag, like "MenuLight"
        GameObject[] lightObjects = GameObject.FindGameObjectsWithTag("MenuLight");

        // Add each light component from the found objects
        foreach (GameObject obj in lightObjects)
        {
            Light light = obj.GetComponent<Light>();
            if (light != null && light.type == LightType.Area)
            {
                light.enabled = false;  // Start each light as off
                areaLights.Add(light);
            }
        }

        // Start the coroutine to turn on lights one by one
        StartCoroutine(TurnOnLights());
    }

    private IEnumerator TurnOnLights()
    {
        // Loop through each light in the list
        for (int i = 0; i < areaLights.Count; i++)
        {
            // Enable the current light
            areaLights[i].enabled = true;

            // Wait for the specified delay before continuing
            yield return new WaitForSeconds(delay);
        }
    }
}
