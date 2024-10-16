using UnityEngine;

//temp standin

public class FloorboardBreak : MonoBehaviour
{
    public GameObject Floorboard_full;
    public GameObject Floorboard_breaking;
    public GameObject Floorboard_broken;

    private int currentStage = 1;

    void Start()
    {
        Floorboard_full.SetActive(true);
        Floorboard_breaking.SetActive(false);
        Floorboard_broken.SetActive(false);
    }

    public void BreakFloorboard()
    {
        if (currentStage == 1)
        {
            Floorboard_full.SetActive(false);
            Floorboard_breaking.SetActive(true);
        }
        else if (currentStage == 2)
        {
            Floorboard_breaking.SetActive(false);
            Floorboard_broken.SetActive(true);
        }
    }
}