using UnityEngine;

namespace UI
{
    public class ButtonGroupUIManager : MonoBehaviour
    {
        public void Enable()
        {
            for (int itr = 0; itr < transform.childCount; itr++)
            {
                GameObject child = transform.GetChild(itr).gameObject;
                
                if (child.TryGetComponent<PointerEventHandler>(out PointerEventHandler handler))
                {
                    handler.EnableCallbacks = true;
                }
            }
        }

        public void Disable()
        {
            for (int itr = 0; itr < transform.childCount; itr++)
            {
                GameObject child = transform.GetChild(itr).gameObject;
                
                if (child.TryGetComponent<PointerEventHandler>(out PointerEventHandler handler))
                {
                    handler.EnableCallbacks = false;
                }
            }
        }
    }
}