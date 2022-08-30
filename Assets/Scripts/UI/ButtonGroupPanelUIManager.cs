using System.Linq;
using System.Collections.Generic;

using UnityEngine;

namespace UI
{
    public abstract class ButtonGroupPanelUIManager : BaseButtonGroupPanelUIManager
    {
        [Header("Components")]
        [SerializeField] protected GameObject group;

        protected override List<ButtonAccessor> ResolveAccessors()
        {
            List<ButtonAccessor> containers = new List<ButtonAccessor>();

            foreach (ButtonUIManager manager in group.GetComponentsInChildren<ButtonUIManager>().ToList())
            {
                containers.Add(new ButtonAccessor(manager));
            }

            return containers;
        }
    }
}