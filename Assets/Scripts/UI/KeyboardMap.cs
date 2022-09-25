using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Scriptables;

namespace UI
{
    public class KeyboardMap : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] List<KeyBindingButtonUIManager> buttons;

        [Header("Config")]
        [SerializeField] KeyboardProfileScriptable scriptable;

        void Awake()
        {
            ResolveDependencies();

            int id = 0;

            foreach (KeyBindingButtonUIManager button in buttons)
            {
                button.Id = ++id;
                button.AssignBinding(scriptable);
            }
        }

        private void ResolveDependencies() => buttons = GetComponentsInChildren<KeyBindingButtonUIManager>().ToList();

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}