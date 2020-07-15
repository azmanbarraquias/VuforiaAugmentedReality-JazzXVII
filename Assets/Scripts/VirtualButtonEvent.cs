using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Vuforia;

public class VirtualButtonEvent : MonoBehaviour
{
    #region PUBLIC_MEMBERS
    public UnityEvent OnVirtualButtonPressed;
    public UnityEvent OnVirtualButtonReleased;

    public float buttonReleaseTimeDelay;

    #endregion // PUBLIC_MEMBERS

    #region PRIVATE_MEMBERS

    private VirtualButtonBehaviour virtualButtonBehaviours;

    #endregion // PRIVATE_MEMBERS

    #region MONOBEHAVIOUR_METHODS
    void Start()
    {
        // Register with the virtual buttons TrackableBehaviour
        virtualButtonBehaviours = GetComponent<VirtualButtonBehaviour>();

        virtualButtonBehaviours.RegisterOnButtonPressed(OnButtonPressed);
        virtualButtonBehaviours.RegisterOnButtonReleased(OnButtonReleased);

    }

    #endregion // MONOBEHAVIOUR_METHODS


    #region PUBLIC_METHODS

    /// Called when the virtual button has just been pressed:
    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        Debug.Log("OnButtonPressed: " + vb.VirtualButtonName);

        if (OnVirtualButtonPressed != null)
            OnVirtualButtonPressed.Invoke();

        //SetVirtualButtonMaterial(m_VirtualButtonPressed);

        //if (vb.VirtualButtonName == "AstronautVB")
        //{
        //    Debug.Log("Pressed WOHOO");
        //}
        //else
        //{
        //    throw new UnityException(vb.VirtualButtonName + " Virtual Button Not Supported");
        //}

        StopAllCoroutines();

        BroadcastMessage("HandleVirtualButtonPressed", SendMessageOptions.DontRequireReceiver);
    }


    /// Called when the virtual button has just been released:
    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        Debug.Log("OnButtonReleased: " + vb.VirtualButtonName);

        if (OnVirtualButtonReleased != null)
            OnVirtualButtonReleased.Invoke();

        //SetVirtualButtonMaterial(m_VirtualButtonDefault);

        StartCoroutine(DelayOnButtonReleasedEvent(buttonReleaseTimeDelay, vb.VirtualButtonName));
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