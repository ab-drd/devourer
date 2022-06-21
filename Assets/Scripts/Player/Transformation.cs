using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Transformation : MonoBehaviour
{
    [SerializeField] private GameObject[] forms;
    [SerializeField] private CameraFollow cam;
    [SerializeField] public UIManager uiManager;
    [SerializeField] private float transformCooldown;
    public int unlockedFormCount;

    Controls controls;
    private float transformTimer;

    private void Awake()
    {
        transformTimer = Mathf.Infinity;
        controls = new Controls();
        uiManager.SetTransformationIconHolderSize();
    }

    private void OnEnable()
    {
        controls.Player.Transformation.performed += TransformActive;
        controls.Player.Transformation.Enable();

        FindActiveForm();
    }

    private void OnDisable()
    {
        controls.Player.Transformation.Disable();
    }

    private void Update()
    {
        if (forms.Length == 0) return;
        
        transformTimer += Time.deltaTime;
        if (transformTimer <= transformCooldown + 0.1f)
            uiManager.SetTransformationCooldownScale(transformTimer / transformCooldown);
    }

    private void FindActiveForm()
    {
        for (int i = 0; i < unlockedFormCount; i++)
        {
            if (forms[i].activeInHierarchy)
            {
                if (forms[i].GetComponent<AnglerfishMovement>())
                    uiManager.ActivateBurstBar();
                else
                    uiManager.DeactivateBurstBar();
                break;
            }
        }
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

                if (forms[i].GetComponent<AnglerfishMovement>())
                    uiManager.DeactivateBurstBar();

                forms[i].SetActive(false);

                var index = i + 1;
                if (index == unlockedFormCount)
                    index = 0;

                forms[index].SetActive(true);
                forms[index].transform.position = tempPos;
                forms[index].GetComponent<Rigidbody2D>().velocity = tempVelocity;
                
                cam.ChangeFollowTarget(index);
                uiManager.ChangeActiveForm(index);
                if (forms[index].GetComponent<AnglerfishMovement>())
                    uiManager.ActivateBurstBar();

                break;
            }
        }
    }

    public void SetFormCount(int _forms)
    {
        unlockedFormCount = _forms;

        uiManager.RefreshTransformationIcons();
    }

    public void ResetFormPositions()
    {
        for (var i = 0; i < forms.Length; i++)
        {
            forms[i].transform.localPosition = new Vector3(0, 0, 0);
        }
    }
}
