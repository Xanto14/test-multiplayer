using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class HighscoreTable : MonoBehaviour
{
    [SerializeField] private Transform highscoreContainer;
    [SerializeField] private Transform templatePrefab;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI playerName;
    private Transform container;
    private Transform template;
    private List<Transform> templateTransformList;
    private void Awake()
    {
        container= highscoreContainer;
        template = templatePrefab;

        //Transforme la string json sauvegardé en liste
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        //sort la list de joueurs par score
        for (int i = 0; i < highscores.highscoresPlayerList.Count; i++)
        {
            for (int j = i + 1; j < highscores.highscoresPlayerList.Count; j++)
            {
                if (highscores.highscoresPlayerList[j].score > highscores.highscoresPlayerList[i].score)
                {
                    HighscorePlayer temporaire = highscores.highscoresPlayerList[i];
                    highscores.highscoresPlayerList[i] = highscores.highscoresPlayerList[j];
                    highscores.highscoresPlayerList[j] = temporaire;
                }

            }
        }

        templateTransformList = new List<Transform>();

        //met chaque prefab de joueur dans le scroll view
        foreach (HighscorePlayer player in highscores.highscoresPlayerList)
        {
            CreateHighscorePlayerTransform(player, container, templateTransformList);
        }
    }

    public void OnClick_AddPlayerScore()
    {
        string finalName;
        finalName = playerName.text;
        //Si le nom n'est pas 3 charactère, l,algorythme va compléter le nom en écrivant BOB
        if (finalName.ToCharArray().Length < 3||finalName.IsNullOrEmpty())
            finalName = "BOB";


        AddPlayerHighscore(int.Parse(score.text.ToString()), finalName.ToUpper());
    }
    private void CreateHighscorePlayerTransform(HighscorePlayer highscorePlayer,Transform container, List<Transform> listTransform)
    {
            Transform playerTemplate = Instantiate(template, container);
            //RectTransform templateRectTransform = templateTransform.GetComponent<RectTransform>();
            //templateRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i);

            int rank = listTransform.Count + 1;
            int score = highscorePlayer.score;
            string name = highscorePlayer.name;
            string rankString;

            switch (rank)
            {
                default:
                    rankString = rank + "TH"; break;

                case 1: rankString = "1ST"; break;
                case 2: rankString = "2ND"; break;
                case 3: rankString = "3RD"; break;
            }


            playerTemplate.Find("PositionText").GetComponent<TextMeshProUGUI>().text = rankString;
            playerTemplate.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = score.ToString();
            playerTemplate.Find("NameText").GetComponent<TextMeshProUGUI>().text = name;

        playerTemplate.GetComponent<RawImage>().enabled= (rank % 2 == 1);
        //highlight le premier
        if (rank == 1)
        {
            playerTemplate.Find("PositionText").GetComponent<TextMeshProUGUI>().color = Color.green;
            playerTemplate.Find("ScoreText").GetComponent<TextMeshProUGUI>().color = Color.green;
            playerTemplate.Find("NameText").GetComponent<TextMeshProUGUI>().color = Color.green;
        }

        //met un badge pour les 3 premiers
        switch (rank)
        {
            default: break;
            case 1: playerTemplate.Find("Gold").gameObject.SetActive(true); break;
            case 2: playerTemplate.Find("Silver").gameObject.SetActive(true); break;
            case 3: playerTemplate.Find("Bronze").gameObject.SetActive(true); break;
        }

        templateTransformList.Add(template);


    }
    private void AddPlayerHighscore(int score, string name)
    {
        HighscorePlayer highscorePlayer = new HighscorePlayer { score=score,name=name};

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        highscores.highscoresPlayerList.Add(highscorePlayer);

        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }
    //pour passer un gameobject a transformer en json au lieu d'une liste
    private class Highscores
    {
        public List<HighscorePlayer> highscoresPlayerList;
    }

    [System.Serializable]
    private class HighscorePlayer
    {
       public int score;
       public string name;
    }

}
