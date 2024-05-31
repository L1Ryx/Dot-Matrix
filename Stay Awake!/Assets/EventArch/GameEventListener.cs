using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent Event;
    public UnityEvent Response;

    private void OnEnable()
    {
        if (Event != null)
        {
            Event.RegisterListener(this);
            // Debug.Log($"Registered listener for event: {Event.name}");
        }
    }

    private void OnDisable()
    {
        if (Event != null)
        {
            Event.UnregisterListener(this);
            // Debug.Log($"Unregistered listener for event: {Event.name}");
        }
    }

    public void Register()
    {
        if (Event != null)
        {
            Event.RegisterListener(this);
            //Debug.Log($"Dynamically registered listener for event: {Event.name}");
        }
    }

    public void Unregister()
    {
        if (Event != null)
        {
            Event.UnregisterListener(this);
            //Debug.Log($"Dynamically unregistered listener for event: {Event.name}");
        }
    }

    public virtual void OnEventRaised()
    {
        // Debug.Log($"Event raised: {Event.name}");
        Response.Invoke();
    }
}
