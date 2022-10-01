using UnityEngine;

namespace Tests
{
    [AddComponentMenu("Tests/Quit Handler Test")]
    public class QuitHandlerTest : MonoBehaviour
    {
        public void Quit() => Application.Quit();
    }
}