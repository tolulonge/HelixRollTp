using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Takes in swipe inputs to control the helix
public class HelixController : MonoBehaviour
{
    private Vector2 lastTapPos;
    private Vector2 startRotation;

    public Transform topTransform;
    public Transform goalTransform;

    public GameObject helixLevelPrefab;

    public GameObject helixCylinder;

    public List<Stage> allStages = new List<Stage>();
    private float helixDistance;
    private List<GameObject> spawnedLevels = new List<GameObject>();




    void Awake()
    {
        startRotation = transform.localEulerAngles;
        helixDistance = topTransform.localPosition.y - (goalTransform.localPosition.y + 0.1f);
        LoadStage(0);
       
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.singleton.isGameActive)
        {
            // Interprets left click or screen touch and acts in response
            if (Input.GetMouseButton(0))
            {
                Vector2 curTapPos = Input.mousePosition;

                if (lastTapPos == Vector2.zero)
                {
                    lastTapPos = curTapPos;
                }

                float delta = lastTapPos.x - curTapPos.x;
                lastTapPos = curTapPos;

                transform.Rotate(Vector3.up * delta);

            }

            if (Input.GetMouseButtonUp(0))
            {
                lastTapPos = Vector2.zero;
            }
        }
      
    }



    public void LoadStage(int stageNumber)
    {

        Stage stage = allStages[Mathf.Clamp(stageNumber, 0, allStages.Count - 1)];

        if(stage == null)
        {
            Debug.LogError("No stage " + stageNumber + " found in allStages List. Are all stages assigned in the list?");
            return;

        }

        Camera.main.backgroundColor = allStages[stageNumber].stageBackgroundColor;
        FindObjectOfType<BallController>().GetComponent<Renderer>().material.color = allStages[stageNumber].stageBallColor;

        // Reset helix rotation
        transform.localEulerAngles = startRotation;

        // destroy old levels if any
        foreach(GameObject go in spawnedLevels)
        {
            Destroy(go);
        }

        // Create new level / platforms
        float levelDistance = helixDistance / stage.levels.Count;
        float spawnPosY = topTransform.localPosition.y;

        for(int i = 0; i < stage.levels.Count; i++)
        {
            spawnPosY -= levelDistance;
            GameObject level = Instantiate(helixLevelPrefab, transform);
            level.transform.localPosition = new Vector3(0, spawnPosY, 0);
            spawnedLevels.Add(level);

            // creating the gaps
            int partsToDisable = 12 - stage.levels[i].partCount;
         
            List<GameObject> disabledParts = new List<GameObject>();

            while(disabledParts.Count < partsToDisable)
            {
                GameObject randomPart = level.transform.GetChild(Random.Range(0, level.transform.childCount)).gameObject;
                if (!disabledParts.Contains(randomPart))
                {
                    randomPart.SetActive(false);
                    disabledParts.Add(randomPart);
                }
            }

            
            List<GameObject> leftParts = new List<GameObject>();
            foreach (Transform t in level.transform)
            {
                t.GetComponent<Renderer>().material.color = allStages[stageNumber].stageLevelPartColor;
                if (t.gameObject.activeInHierarchy)
                {
                    leftParts.Add(t.gameObject);
                }
            }
            // creating the deathparts
            List<GameObject> deathParts = new List<GameObject>();

            while(deathParts.Count < stage.levels[i].deathPartCount)
            {
                GameObject randomPart = leftParts[(Random.Range(0, leftParts.Count))];
                if (!deathParts.Contains(randomPart))
                {
                    randomPart.gameObject.AddComponent<DeathPart>();
                    deathParts.Add(randomPart);
                }
            } 
        }
    }
}
