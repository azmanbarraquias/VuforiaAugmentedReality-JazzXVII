using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Vuforia;

public class MultiVirtualButtons : MonoBehaviour
{
    #region PUBLIC_MEMBERS

    public VirtualButtonEvents[] virtualButtonEvents;
    [Space]

    public float buttonReleaseTimeDelay;

    #endregion // PUBLIC_MEMBERS

    #region PRIVATE_MEMBERS

    private VirtualButtonBehaviour[] virtualButtonBehaviours;

    #endregion // PRIVATE_MEMBERS

    #region MONOBEHAVIOUR_METHODS
    void Start()
    {
        // Register with the virtual buttons TrackableBehaviour
        virtualButtonBehaviours = GetComponentsInChildren<VirtualButtonBehaviour>();

        for (int i = 0; i < virtualButtonBehaviours.Length; ++i)
        {
            virtualButtonBehaviours[i].RegisterOnButtonPressed(OnButtonPressed);
            virtualButtonBehaviours[i].RegisterOnButtonReleased(OnButtonReleased);
        }
    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS

    /// Called when the virtual button has just been pressed:
    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        Debug.Log("OnButtonPressed: " + vb.VirtualButtonName);

        var vbe = virtualButtonEvents.Single(v => v.virtualButtonName == vb.VirtualButtonName);

        if (vbe != null)
        {
            if (vbe.OnVirtualButtonPressed != null)
            {
                vbe.OnVirtualButtonPressed.Invoke();
            }
        }
        else
        {
            Debug.LogError($"Virtual button with {vb.VirtualButtonName} name not found in a VirtualButtonEvents");
        }

        StopAllCoroutines();

        BroadcastMessage("HandleVirtualButtonPressed", SendMessageOptions.DontRequireReceiver);
    }


    /// Called when the virtual button has just been released:
    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        Debug.Log("OnButtonReleased: " + vb.VirtualButtonName);

        var vbe = virtualButtonEvents.Single(v => v.virtualButtonName == vb.VirtualButtonName);

        if (vbe != null)
        {
            if (vbe.OnVirtualButtonReleased != null)
            {
                StartCoroutine(DelayOnButtonReleasedEvent(buttonReleaseTimeDelay, vb.VirtualButtonName));

                vbe.OnVirtualButtonReleased.Invoke();
            }
        }
        else
        {
            Debug.LogError($"Virtual button with {vb.VirtualButtonName} name not found in a VirtualButtonEvents");
        }

        //SetVirtualButtonMaterial(m_VirtualButtonDefault);

    }
    #endregion //PUBLIC_METHODS


    #region PRIVATE_METHODS

    IEnumerator DelayOnButtonReleasedEvent(float waitTime, string buttonName)
    {
        yield return new WaitForSeconds(waitTime);

        BroadcastMessage($"{buttonName}, HandleVirtualButtonReleased", SendMessageOptions.DontRequireReceiver);
    }
    #endregion // PRIVATE METHODS
}

[System.Serializable]
public class VirtualButtonEvents
{
    public string virtualButtonName;
    public UnityEvent OnVirtualButtonPressed;
    public UnityEvent OnVirtualButtonReleased;
}