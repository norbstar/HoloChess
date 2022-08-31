using System.Linq;
using System.Collections.Generic;

using UnityEngine;

namespace UI.Panels
{
    public abstract class ButtonGroupPanelUIManager : BaseButtonGroupPanelUIManager
    {
        [Header("Components")]
        [SerializeField] protected GameObject group;

        protected override List<ButtonUIManager> ResolveInstances()
        {
            List<ButtonUIManager> instances = new List<ButtonUIManager>();

            foreach (ButtonUIManager manager in group.GetComponentsInChildren<ButtonUIManager>().ToList())
            {
                instances.Add(manager);
            }

            return instances;
        }
    }
}