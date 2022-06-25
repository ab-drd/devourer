using UnityEngine;
using UnityEngine.UI;
public class SetActiveButton : MonoBehaviour
{
    private void OnEnable()
    {
        var firstButton = GetComponentsInChildren<Button>()[0];
        firstButton.Select();
        firstButton.OnSelect(null);
    }
}