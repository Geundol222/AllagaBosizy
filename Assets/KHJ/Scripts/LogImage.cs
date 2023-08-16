using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//�������� �������� �˷��� ���� �ִٸ� �˷��ִ� image
public class LogImage : MonoBehaviour
{
    private Animator anim;          //Logimage�� �ִϸ�����
    public TMP_Text text;           //�������� �˷��� �͵��� ���� text
    LobbyManager manager;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        manager = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
    }

    private void OnEnable()
    {
        anim.SetTrigger("IsOpen");
    }

    public void SetText(string str)
    {
        text.text = str;
    }

    public void Close()
    {
        StartCoroutine(CloseRoutine());
    }

    IEnumerator CloseRoutine()
    {
        manager.PlayUIButtonClickSound();
        anim.SetTrigger("IsClose");
        yield return new WaitForSeconds(0.5f);
        text.text = "";
        gameObject.SetActive(false);
        yield return null;
    }
}
