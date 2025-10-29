using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int CurrentScene = 0;

    //BG
    public GameObject BG1;
    public GameObject BG1_1;
    public GameObject BG2;
    public GameObject BG3;
    public GameObject BG4;

    //Minigame
    public GameObject Minigame1;
    public GameObject Minigame2;
    public GameObject Minigame3;
    public GameObject Minigame4;


    //Dialogue
    public GameObject Dialogue1;
    public GameObject Dialogue2;
    public GameObject Dialogue3;
    public GameObject Dialogue4;
    public GameObject Dialogue5;
    public GameObject Dialogue6;
    public GameObject Dialogue7;
    public GameObject Dialogue8;
    public GameObject Dialogue9;

    int _lastScene = -1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ApplyScene(CurrentScene);
        _lastScene = CurrentScene;
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentScene != _lastScene)
        {
            ApplyScene(CurrentScene);
            _lastScene = CurrentScene;
        }
    }

    void DeactivateAll()
    {
        BG1.SetActive(false);
        BG1_1.SetActive(false);
        BG2.SetActive(false);
        BG3.SetActive(false);
        BG4.SetActive(false);

        Minigame1.SetActive(false);
        Minigame2.SetActive(false);
        Minigame3.SetActive(false);
        Minigame4.SetActive(false);

        Dialogue1.SetActive(false);
        Dialogue2.SetActive(false);
        Dialogue3.SetActive(false);
        Dialogue4.SetActive(false);
        Dialogue5.SetActive(false);
        Dialogue6.SetActive(false);
        Dialogue7.SetActive(false);
        Dialogue8.SetActive(false);
        Dialogue9.SetActive(false);
    }

    void ApplyScene(int scene)
    {
        DeactivateAll();
        switch (scene)
        {
            case 0:
                BG1.SetActive(true);
                Dialogue1.SetActive(true);
                break;
            case 1:
                Minigame1.SetActive(true);
                break;
            case 2:
                BG1_1.SetActive(true);
                Dialogue2.SetActive(true);
                break;
            case 3:
                BG2.SetActive(true);
                Dialogue3.SetActive(true);
                break;
            case 4:
                Minigame2.SetActive(true);
                break;
            case 5:
                BG2.SetActive(true);
                Dialogue4.SetActive(true);
                break;
            case 6:
                Minigame3.SetActive(true);
                break;
            case 7:
                BG2.SetActive(true);
                Dialogue5.SetActive(true);
                break;
            case 8:
                BG3.SetActive(true);
                Dialogue6.SetActive(true);
                break;
            case 9:
                Minigame4.SetActive(true);
                break;
            case 10:
                BG3.SetActive(true);
                Dialogue7.SetActive(true);
                break;
            case 11:
                BG1_1.SetActive(true);
                Dialogue8.SetActive(true);
                break;
            case 12:
                BG4.SetActive(true);
                Dialogue9.SetActive(true);
                break;
            default:
                break;
        }
    }
}
