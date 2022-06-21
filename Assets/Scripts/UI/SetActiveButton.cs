using UnityEngine;
using UnityEngine.UI;
public class SetActiveButton : MonoBehaviour
{
    private void OnEnable()
    {
        var firstButton = gameObject.GetComponentsInChildren<Button>()[0];
        firstButton.Select();
        firstButton.OnSelect(null);
    }
}
