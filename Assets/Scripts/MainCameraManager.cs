using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

public class MainCameraManager : TrackingMainCameraManager
{
    private static string className = MethodBase.GetCurrentMethod().DeclaringType.Name;

    [Header("Components")]
    [SerializeField] HandController leftHandController;
    [SerializeField] HandController rightHandController;

    [Header("Tracking")]
    [SerializeField] float scanRadius = 2f;
    [SerializeField] Color scanVolumeColor = new Color(0f, 0f, 0f, 0.5f);

    [Header("Movement")]
    [SerializeField] float speed = 10.0f;

    public HandController LeftHandController { get { return leftHandController; } }
    public HandController RightHandController { get { return rightHandController; } }

    private List<IInteractable> trackedInteractables;
    
    public override void Awake()
    {
        base.Awake();
        trackedInteractables = new List<IInteractable>();
    }
}