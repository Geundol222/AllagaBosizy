using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class SignUpCanvas : MonoBehaviour
{
    [SerializeField] TMP_InputField idInputField;
    [SerializeField] TMP_InputField PasswordInputField;
    [SerializeField] TMP_InputField PasswordAgainInputField;
    [SerializeField] TMP_InputField NickNameInputField;
    [SerializeField] TMP_InputField answerInputField;
    [SerializeField] LoginCanvas LC;
    [SerializeField] GameObject answer;
    [SerializeField] LogImage logImage;
    [SerializeField] Animator anim;
    private MySqlConnection con;
    private MySqlDataReader reader;
    private bool isCheckID;
    private bool isCheckNickName;

    private void OnEnable()
    {
        con = LC.con;
        isCheckID = false;
        isCheckNickName = false;
        logImage.gameObject.SetActive(false);
        answer.SetActive(false);
        anim.SetTrigger("IsOpen");
    }

    public void CheckID()
    {
        try
        {
            string id = idInputField.text;

            string sqlCommand = string.Format("SELECT ID FROM user_info WHERE ID='{0}';", id);

            MySqlCommand cmd = new MySqlCommand(sqlCommand, con);
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                logImage.gameObject.SetActive(true);
                logImage.SetText("�ߺ� ���̵� �ֽ��ϴ�.");
                isCheckID = false;
            }
            else
            {
                logImage.gameObject.SetActive(true);
                logImage.SetText("���̵� ���� �� �ֽ��ϴ�.");
                isCheckID = true;
            }
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void CheckNickName()
    {
        try
        {
            string name = NickNameInputField.text;

            string sqlCommand = string.Format("SELECT NICKNAME FROM user_info WHERE NICKNAME='{0}';", name);

            MySqlCommand cmd = new MySqlCommand(sqlCommand, con);
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                logImage.gameObject.SetActive(true);
                logImage.SetText("�ߺ� �����̸��� �ֽ��ϴ�.");
                isCheckNickName = false;
            }
            else
            {
                logImage.gameObject.SetActive(true);
                logImage.SetText("�����̸��� ���� �� �ֽ��ϴ�.");
                isCheckNickName = true;
            }
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void OKButton()
    {
        try
        {
            string id = idInputField.text;
            string pwd = PasswordInputField.text;
            string pwdck = PasswordAgainInputField.text;
            string name = NickNameInputField.text;

            if (pwd == pwdck)
            {
                if (isCheckID)
                {
                    if (isCheckNickName)
                    {
                        answer.SetActive(true);
                    }
                    else
                    {
                        logImage.gameObject.SetActive(true);
                        logImage.SetText("�����̸� �ߺ��� Ȯ�����ּ���.");
                    }
                }
                else
                {
                    logImage.gameObject.SetActive(true);
                    logImage.SetText("���̵� �ߺ��� Ȯ�����ּ���.");
                }
            }
            else
            {
                logImage.gameObject.SetActive(true);
                logImage.SetText("��й�ȣ�� Ȯ�κ�й�ȣ�� �ٸ��ϴ�.");
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void CreateID()
    {
        string id = idInputField.text;
        string pwd = PasswordInputField.text;
        string name = NickNameInputField.text;
        string answerstring = answerInputField.text;

        string sqlCommand = string.Format("INSERT INTO `userdata`.`user_info` (`ID`, `PWD`, `NICKNAME`, `PWDANSWER`) VALUES ('{0}','{1}','{2}','{3}');", id, pwd, name, answerstring);
        Debug.Log(sqlCommand);
        MySqlCommand cmd = new MySqlCommand(sqlCommand, con);
        reader = cmd.ExecuteReader();
        reader.Close();
        answer.SetActive(false);
    }

    public void CloseCanvas()
    {
        StartCoroutine(CloseCanvasRoutine());
    }

    IEnumerator CloseCanvasRoutine()
    {
        idInputField.text = "";
        PasswordInputField.text = "";
        PasswordAgainInputField.text = "";
        NickNameInputField.text = "";
        answerInputField.text = "";
        anim.SetTrigger("IsClose");
        yield return new WaitForSeconds(0.4f);
        gameObject.SetActive(false);
        yield return null;
    }
}
