using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class RoundStartUI : GameSceneUI
{
    Animator animator;
    [SerializeField] TMP_Text subtitle;
    [SerializeField] Camera stackCamera;
    [SerializeField] Image letterBox;
    [SerializeField] GameObject climberAvatar;
    [SerializeField] GameObject trollerAvatar;
    [SerializeField] Canvas canvasOfLetterBox;
    string[] subtitles;

    protected override void Awake()
    {
        base.Awake();
        if (!photonView.IsMine)
        {
            return;
        }
        // ĵ���� ���� ī�޶� ����
        canvasOfLetterBox.worldCamera = Camera.main;
        // �ִϸ�����
        animator = GetComponent<Animator>();
        // �ؽ�Ʈ
        subtitles = new string[] { "���ǿ� ������ ��ġ�� ����� �����ϼ���!","��Ե� ���� �ö󰡼���!"};
        // ���� ī�޶� Stack overlayī�޶� �߰�
        var cameraData = Camera.main.GetUniversalAdditionalCameraData();
        cameraData.cameraStack.Add(stackCamera);
    }

    private void Start()
    {
        RoundStart();
    }

    public void Init()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        canvasOfLetterBox.renderMode = RenderMode.WorldSpace;
        canvasOfLetterBox.transform.position = new Vector3(canvasOfLetterBox.transform.position.x, canvasOfLetterBox.transform.position.y, 0);
        climberAvatar.SetActive(false);
        trollerAvatar.SetActive(false);
        subtitle.text = "";
        if (GameManager.Team.GetTeam() == PlayerTeam.Troller)
        {
            trollerAvatar.SetActive(true);
            subtitle.text = subtitles[0];

        }
        else if (GameManager.Team.GetTeam() == PlayerTeam.Climber)
        {
            climberAvatar.SetActive(true);
            subtitle.text = subtitles[1];
        }
        else
        {
            Debug.Log("����");
        }

        stackCamera.transform.position = new Vector3(0, stackCamera.transform.position.y, stackCamera.transform.position.z);
        letterBox.rectTransform.offsetMin = new Vector2( -1920, letterBox.rectTransform.offsetMin.y);
       
    }

    public void RoundStart()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        Init();
        animator.SetTrigger("Start");
    }
}
