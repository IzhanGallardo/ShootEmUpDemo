using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag == "TimerText")
        {
            timerText();
        } else if (gameObject.tag == "StageText")
        {
            stageText();
        } else if(gameObject.tag == "HpText")
        {
            hpText();
        } else if(gameObject.tag == "ScoresText")
        {
            scoresText();
        }
    }

    void timerText()
    {
        gameObject.GetComponent<Text>().text = SpaceshipController.stageTimer.ToString() + " seconds left";
    }

    void stageText()
    {
        gameObject.GetComponent<Text>().text = "Stage " + SpaceshipController.stage.ToString();
    }

    void hpText()
    {
        gameObject.GetComponent<Text>().text = SpaceshipController.curHP.ToString() + "/" + SpaceshipController.maxHP.ToString() + " HP";
    }

    void scoresText()
    {
        gameObject.GetComponent<Text>().text = "Your maximum Stage is: " + PlayerPrefs.GetInt("maxStage").ToString();

        if(PlayerPrefs.GetInt("maxStage") == 3)
        {
            gameObject.GetComponent<Text>().text += "\n Congratulations, you have shot them all!";
        }
    }
}
