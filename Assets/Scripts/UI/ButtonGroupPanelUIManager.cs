using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using UnityButton = UnityEngine.UI.Button;

namespace UI
{
    public abstract class ButtonGroupPanelUIManager : BaseButtonGroupPanelUIManager
    {
        [Header("Components")]
        [SerializeField] protected GameObject group;

        protected override List<ButtonContainer> ResolveButtons()
        {
            List<ButtonContainer> containers = new List<ButtonContainer>();

            foreach (UnityButton button in group.GetComponentsInChildren<UnityButton>().ToList())
            {
                containers.Add(new ButtonContainer
                {
                    button = button,
                    originalScale = button.transform.localScale
                });
            }

            return containers;
        }
    }
}