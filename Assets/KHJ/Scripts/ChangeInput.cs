using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using Unity.VisualScripting;

public class ChangeInput : MonoBehaviour
{
    EventSystem system;
    public Selectable IDInput;
    public Selectable PWDInput;
    public Button submitButton;


    void OnEnable()
    {
        system = EventSystem.current;
        IDInput.Select();
    }

    //tap�� ���� ��� inputfield�� ���õǾ� �ִٸ� �ٸ� inputfield�� �Ѿ�� ���ִ� �Լ�
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (EventSystem.current.currentSelectedGameObject == PWDInput.gameObject)
            {
                Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
                if (next != null)
                {
                    next.Select();
                }
            }
            else if (EventSystem.current.currentSelectedGameObject == IDInput.gameObject)
            {
                Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
                if (next != null)
                {
                    next.Select();
                }
            }
            else
                return;
        }
    }
}