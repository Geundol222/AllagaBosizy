using MySql.Data.MySqlClient;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class LoginCanvas : MonoBehaviour
{
    [SerializeField] TMP_InputField idInputField;
    [SerializeField] TMP_InputField PasswordInputField;
    [SerializeField] LogImage logImage;
    public GameObject SignUpCanvas;
    public GameObject FoundCanvas;
    public MySqlConnection con;
    public MySqlDataReader reader;
    Animator anim;
    public bool isConnected = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    //�����ͺ��̽��� ����Ǿ����� ������ �����ϰ� logImage�� SignUpCanvas, FoundCanvas�� �����ִٸ� ���ִ� �Լ�
    private void OnEnable()
    {
        if (!isConnected)
            ConnectDataBase();
        logImage.gameObject.SetActive(false);
        SignUpCanvas.SetActive(false);
        FoundCanvas.SetActive(false);
    }

    //������ ���̽��� �������ִ� �Լ�
    public void ConnectDataBase()
    {
        try
        {
            string serverInfo = "Server=3.34.182.2; Database=user_data; Uid=root; PWD=dhffkrkwh; Port=3306; CharSet=utf8;";
            con = new MySqlConnection(serverInfo);
            con.Open();
            isConnected = true;
        }
        catch (Exception e)
        {
            return; ;
        }
    }

    //������ �Է��� id�� pwd�� �޾� �����ͺ��̽����� ��ȸ�Ͽ� ������ �ش� ���̵��� �г����� �޾� ������ ���� ���ִ� �Լ�
    public void Login()
    {
        if(!isConnected)
            ConnectDataBase();
        try
        {
            string id = idInputField.text;
            string pass = PasswordInputField.text;

            string sqlCommand = string.Format("SELECT ID, PWD, NICKNAME, PWDANSWER FROM user_info WHERE ID='{0}';", id);
            MySqlCommand cmd = new MySqlCommand(sqlCommand, con);
            reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string readID = reader["ID"].ToString();
                    string readPass = reader["PWD"].ToString();
                    string readnick = reader["NICKNAME"].ToString();
                    string readPassANS = reader["PWDANSWER"].ToString();

                    if (pass == readPass)
                    {
                        PhotonNetwork.LocalPlayer.NickName = readnick;
                        StartCoroutine(CloseLoginCanvasRoutine());
                    }
                    else
                    {
                        logImage.gameObject.SetActive(true);
                        logImage.SetText("��й�ȣ�� Ʋ�Ƚ��ϴ�.");
                    }
                }
            }
            else
            {
                logImage.gameObject.SetActive(true);
                logImage.SetText("�ش� ���̵� �����ϴ�.");
            }
            if (!reader.IsClosed)
                reader.Close();
        }
        catch (Exception e)
        {
            return;
        }
    }
    
    IEnumerator CloseLoginCanvasRoutine()
    {
        anim.SetTrigger("IsClose");
        yield return new WaitForSeconds(1.0f);
        PhotonNetwork.ConnectUsingSettings();
        yield break;
    }
}