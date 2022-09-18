using UnityEngine;

public class FocusResoiver : MonoBehaviour
{
    private IFocus focus;
    public IFocus Focus { get { return focus; }  }

    void Awake()
    {
        foreach  (Component component in GetComponents(typeof(Component)))
        {
            if (typeof(IFocus).IsAssignableFrom(component.GetType()))
            {
                focus = (IFocus) component;
                // Debug.Log($"FocusResolver Resolved IFocus on {component.name} ({component.GetType().Name})");
            }
        }

        // Debug.Log($"Should Receiver Pointer : {ShouldReceivePointer()}");
    }

    public bool ShouldReceivePointer() => (focus != null)  ? focus.ShouldReceivePointer() : true;
}