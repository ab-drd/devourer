using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Transformation : MonoBehaviour
{
    [SerializeField] private GameObject[] forms;
    [SerializeField] private CameraFollow cam;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private float transformCooldown;
    public int unlockedFormCount;

    Controls controls;
    private float transformTimer;

    private void Awake()
    {
        transformTimer = Mathf.Infinity;
        controls = new Controls();
        unlockedFormCount = 1;
        uiManager.SetTransformationIconHolderSize();
    }

    private void OnEnable()
    {
        controls.Player.Transformation.performed += TransformActive;
        controls.Player.Transformation.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Transformation.Disable();
    }

    private void Update()
    {
        if (forms.Length == 0) return;
        
        transformTimer += Time.deltaTime;
        uiManager.SetTransformationCooldownScale(transformTimer / transformCooldown);
    }

    private void TransformActive(InputAction.CallbackContext obj)
    {
        if (transformTimer < transformCooldown || unlockedFormCount < 2) return;

        transformTimer = 0;

        for (int i = 0; i < unlockedFormCount; i++)
        {
            if (forms[i].activeInHierarchy)
            {
                var tempVelocity = forms[i].GetComponent<Rigidbody2D>().velocity;
                var tempPos = forms[i].transform.position;
                forms[i].SetActive(false);

                var index = i + 1;
                if (index == unlockedFormCount)
                    index = 0;

                forms[index].SetActive(true);
                forms[index].transform.position = tempPos;
                forms[index].GetComponent<Rigidbody2D>().velocity = tempVelocity;
                
                cam.ChangeFollowTarget(index);
                uiManager.ChangeActiveForm(index);

                break;
            }
        }
    }

    public void SetFormCount(int _forms)
    {
        unlockedFormCount = _forms;

        uiManager.RefreshTransformationIcons();
    }
}
