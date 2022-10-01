using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [AddComponentMenu("UI/Swap Button UI Manager")]
    [RequireComponent(typeof(Button))]
    public class SwapButtonUIManager : ButtonUIManager
    {
        [Header("Config")]
        [SerializeField] Sprite off;
        [SerializeField] Sprite offHightlight;
        [SerializeField] Sprite on;
        [SerializeField] Sprite onHightlight;
        [SerializeField] bool startOn = false;

        public delegate void OnSwapEvent(SwapButtonUIManager manager, bool isOn);
        public event OnSwapEvent SwapEventReceived;

        private bool isOn = false;
        public bool IsOn
        {
            get
            {
                return isOn;
             }
             
             set
             {
                isOn = value;

                if (isOn)
                {
                    swapButton.targetGraphic.GetComponent<Image>().sprite = on;

                    var spriteState = new SpriteState
                    {
                        highlightedSprite = onHightlight,
                        pressedSprite = onHightlight,
                        selectedSprite = onHightlight
                    };

                    swapButton.spriteState = spriteState;
                }
                else
                {
                    swapButton.targetGraphic.GetComponent<Image>().sprite = off;

                    var spriteState = new SpriteState
                    {
                        highlightedSprite = offHightlight,
                        pressedSprite = offHightlight,
                        selectedSprite = offHightlight
                    };

                    swapButton.spriteState = spriteState;
                }
            }
        }

        private Button swapButton;

        public override void Awake()
        {
            base.Awake();
            ResolveDependencies();

            IsOn = startOn;
        }

        private void ResolveDependencies() => swapButton = GetComponent<Button>() as Button;

        public override void OnClickButton(Button button)
	    {
            IsOn = !isOn;
            SwapEventReceived?.Invoke(this, isOn);
            base.OnClickButton(button);
        }
    }
}