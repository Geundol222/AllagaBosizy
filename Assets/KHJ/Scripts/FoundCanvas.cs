using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FoundCanvas : MonoBehaviour
{
    [SerializeField] TMP_InputField answerInputField;
    [SerializeField] LoginCanvas LC;
    [SerializeField] LogImage answer;
    [SerializeField] Animator anim;

    private MySqlConnection con;
    private MySqlDataReader reader;

    private void OnEnable()
    {
        con = LC.con;
        anim.SetTrigger("IsOpen");
        answer.gameObject.SetActive(false);
    }

    public void FoundID()
    {
        try
        {
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
                    string text = ("����� ���̵�� '{0}' �Դϴ�.", readID).ToString();
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
        }
    }

    public void FoundPW()
    {
        try
        {
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
                    string text = ("����� ��й�ȣ�� '{0}' �Դϴ�.", readPass).ToString();
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
