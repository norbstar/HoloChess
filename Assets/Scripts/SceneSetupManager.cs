using UnityEngine;

using UI;

public class SceneSetupManager : MonoBehaviour
{
    [SerializeField] GameObject menuPrefab;

    void Awake()
    {
        var menuManger = GameObject.FindObjectOfType<MenuUIManager>() as MenuUIManager;

        if (menuManger == null)
        {
            Instantiate(menuPrefab);
        }
    }
}