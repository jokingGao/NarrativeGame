using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TreeSharpPlus;
using UnityEngine.SceneManagement;

public class textBoxController : MonoBehaviour
{

    //public Text Name;
    public Text Body;
    public Button button;
    public Canvas Box;

    private int counter;
    public bool dialogFinished;
    public bool gameOver;
    private bool ready;
    // Use this for initialization
    void Start()
    {
        counter = 0;
        gameOver = false;
        dialogFinished = false;
        Box.enabled = false;
        //Name.enabled = false;
        Body.enabled = false;
        button.enabled = false;
        ready = true;
        button.onClick.AddListener(delegate { buttonPressed(); });
    }

    // Update is called once per frame
    void Update()
    {
        if (ready)
        {
            switch (counter)
            {
                case 1:
                    Dialog1();
                    break;
                case 2:
                    Dialog2();
                    break;
                case 3:
                    Dialog3();
                    break;
                case 4:
                    Dialog4();
                    break;
                case 5:
                    Dialog5();
                    break;
            }
        }
    }

    public RunStatus startCounter()
    {
        counter = 1;
        return RunStatus.Success;
    }

    private void buttonPressed()
    {
        counter = counter + 1;
        dialogFinished = true;
        //Name.enabled = false;
        Body.enabled = false;
        Box.enabled = false;
        button.enabled = false;

        if (gameOver)
        {
            SceneManager.LoadScene(0);
        }
    }

    public bool isDialogFinished()
    {
        return dialogFinished;
    }

    public RunStatus resetDialogFinished()
    {
        dialogFinished = false;
        return RunStatus.Success;
    }

    void buttonEnabled()
    {
        button.enabled = true;
    }

    private void Dialog1()
    {
        //Name.text = "Sam:";
        Body.text = "HERO:Where is my princess?";

        button.enabled = false;
        //Name.enabled = true;
        Body.enabled = true;
        Box.enabled = true;
        Invoke("buttonEnabled", 2);
    }

    private void Dialog2()
    {
        //Name.text = "Dave:";
        Body.text = "NPC:Your princess has been taken away by a infamous monster....";

        button.enabled = false;
        //Name.enabled = true;
        Body.enabled = true;
        Box.enabled = true;
        Invoke("buttonEnabled", 2);
    }

    private void Dialog3()
    {
        //Name.text = "Sam:";
        Body.text = "HERO:Damn!How can I find her?";

        button.enabled = false;
        //Name.enabled = true;
        Body.enabled = true;
        Box.enabled = true;
        Invoke("buttonEnabled", 2);
    }

    private void Dialog4()
    {
        //Name.text = "Dave:";
        Body.text = "NPC:They are in the outside world..But before this, you may need a weapon..";

        button.enabled = false;
        //Name.enabled = true;
        Body.enabled = true;
        Box.enabled = true;
        Invoke("buttonEnabled", 2);
    }

    private void Dialog5()
    {
        //Name.text = "Sam:";
        Body.text = "HERO:Thanks for telling me this..I will save her!";

        button.enabled = false;
        //Name.enabled = true;
        Body.enabled = true;
        Box.enabled = true;
        Invoke("buttonEnabled", 2);
    }

    public RunStatus PrincessDialog()
    {
        //Name.text = "Sam:";
        Body.text = "Princess:Thank you!I miss you so much!";

        button.enabled = false;
        button.GetComponentInChildren<Text>().text = "Press to restart";
        //Name.enabled = true;
        Body.enabled = true;
        Box.enabled = true;
        gameOver = true;
        Invoke("buttonEnabled", 2);
        return RunStatus.Success;
    }


    public RunStatus TreasureDialog()
    {
        //Name.text = "Sam:";
        Body.text = "Hmm... I wonder what's behind this Treasure Chest...";

        button.enabled = false;
        //Name.enabled = true;
        Body.enabled = true;
        Box.enabled = true;
        Invoke("buttonEnabled", 2);

        return RunStatus.Success;
    }


    public RunStatus PosterDialog()
    {
        //Name.text = "Sam:";
        Body.text = "Hmm... I wonder what's behind this Poster...";

        button.enabled = false;
        //Name.enabled = true;
        Body.enabled = true;
        Box.enabled = true;
        Invoke("buttonEnabled", 2);

        return RunStatus.Success;
    }

    public RunStatus PosterDialogComplete()
    {
        //Name.text = "Sam:";
        Body.text = "Sweet! A sword! I can take out the guards with this!";

        button.enabled = false;
        //Name.enabled = true;
        Body.enabled = true;
        Box.enabled = true;
        Invoke("buttonEnabled", 2);

        return RunStatus.Success;
    }

    public RunStatus gotKilled()
    {
        //Name.text = "Cop:";
        Body.text = "You are killed by the monster....";

        button.enabled = false;
        button.GetComponentInChildren<Text>().text = "Press to restart";
        gameOver = true;
        //Name.enabled = true;
        Body.enabled = true;
        Box.enabled = true;
        Invoke("buttonEnabled", 2);

        return RunStatus.Success;
    }
}
