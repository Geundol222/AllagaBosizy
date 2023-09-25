using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FoundCanvas : MonoBehaviour
{
    [SerializeField] TMP_InputField answerInputField;               //������ �亯�� ���� inputfield
    [SerializeField] LoginCanvas LC;                                //LoginCanvas�� �����ͺ��̽��� �޾ƿ��� ����
    [SerializeField] LogImage answer;                               //�������� �̰����� �˷� �� ���� ��� image
    [SerializeField] Animator anim;                                 //FoundCanvas�� animator

    private MySqlConnection con;
    private MySqlDataReader reader;

    private void OnEnable()
    {
        //������ �� �α����˹������� ������ �޾ƿ�
        con = LC.con;
        anim.SetTrigger("IsOpen");
        answer.gameObject.SetActive(false);
    }

    //�޾ƿ� �������� ������ �Է��� PWDANSWER�� äũ�� �� ������ ID�� ��ȯ���ִ� �Լ�
    public void FoundID()
    {
        try
        {
            if (!LC.isConnected)
                LC.ConnectDataBase();
            string id = answerInputField.text;

            string sqlCommand = string.Format("SELECT ID FROM user_info WHERE PWDANSWER='{0}';", id);
            MySqlCommand cmd = new MySqlCommand(sqlCommand, con);
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string readID = reader["ID"].ToString();
                    answer.gameObject.SetActive(true);
                    string printText = "����� ���̵�� " + readID + "�Դϴ�.";
                    string text = (printText).ToString();
                    answer.SetText(text);
                    if (!reader.IsClosed)
                        reader.Close();
                }
            }
            else
            {
                answer.gameObject.SetActive(true);
                answer.SetText("�׷� �亯�� �����ϴ�.");
            }
            if (!reader.IsClosed)
                reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return;
        }
    }

    //�޾ƿ� �������� ������ �Է��� PWDANSWER�� äũ�� �� ������ PASSWORD�� ��ȯ���ִ� �Լ�
    public void FoundPW()
    {
        try
        {
            if (!LC.isConnected)
                LC.ConnectDataBase();
            string id = answerInputField.text;

            string sqlCommand = string.Format("SELECT PWD FROM user_info WHERE PWDANSWER='{0}';", id);
            MySqlCommand cmd = new MySqlCommand(sqlCommand, con);
            reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    string readPass = reader["PWD"].ToString();
                    answer.gameObject.SetActive(true);
                    string printText = "����� ��й�ȣ�� " + readPass + "�Դϴ�.";
                    string text = (printText).ToString();
                    answer.SetText(text);
                    if (!reader.IsClosed)
                        reader.Close();
                }
            }
            else
            {
                answer.gameObject.SetActive(true);
                answer.SetText("�׷� �亯�� �����ϴ�.");
            }
            if (!reader.IsClosed)
                reader.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return;
        }
    }

    public void CloseCanvas()
    {
        StartCoroutine(CloseCanvasRoutine());
    }

    IEnumerator CloseCanvasRoutine()
    {
        answerInputField.text = "";
        anim.SetTrigger("IsClose");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        yield return null;
    }
}
