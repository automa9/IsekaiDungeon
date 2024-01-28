using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightToggle : MonoBehaviour
{
    public Light targetLight;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has a specific tag (you can customize this)
        if (other.CompareTag("Player"))
        {
            ToggleLight(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object exiting the trigger has a specific tag (you can customize this)
        if (other.CompareTag("Player"))
        {
            ToggleLight(false);
        }
    }

    private void ToggleLight(bool enable)
    {
        // Toggle the state of the light based on the trigger events
        if (targetLight != null)
        {
            targetLight.enabled = enable;
        }
    }
}
