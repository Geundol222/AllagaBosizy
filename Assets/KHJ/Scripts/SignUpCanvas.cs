using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
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
    private MySqlConnection con;
    private MySqlDataReader reader;
    private bool isCheckID;
    private bool isCheckNickName;

    private void OnEnable()
    {
        con = LC.con;
        isCheckID = false;
        isCheckNickName = false;
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
                Debug.Log("�ߺ� ���̵� �ֽ�!");
            }
            else
            {
                Debug.Log("���̵� ���� �� �ֽ�!");
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
                Debug.Log("�ߺ� �����̸� �ֽ�!");
            }
            else
            {
                Debug.Log("�����̸� ���� �� �ֽ�!");
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
                        Debug.Log("�����̸� �ߺ� üũ�� ���ּ���.");
                }
                else
                    Debug.Log("���̵� �ߺ� üũ�� ���ּ���.");
            }
            else
                Debug.Log("�ٽ��Է��� ��й�ȣ�� ó�� ��й�ȣ�� ���� �ʽ��ϴ�.");
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
        MySqlCommand cmd = new MySqlCommand(sqlCommand, con);
        reader = cmd.ExecuteReader();
        answer.SetActive(false);
    }
}
