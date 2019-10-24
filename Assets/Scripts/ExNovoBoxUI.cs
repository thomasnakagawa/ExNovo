using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExNovoBoxUI : ExNovoUI
{
    public Transform actionBoxPlaceholders;
    /*
    public Transform runtimeActionBoxRoot;
    public GameObject actionBoxPrefab;

    private RectTransform row0Position;
    private List<RectTransform> row1Positions;
    private List<RectTransform> row2Positions;
    // private List<RectTransform> row3Positions;


    private ActionTreeNode currentAction;
    private ActionBox row0Action;
    private ActionBox[] row1Actions;
    private ActionBox[] row2Actions;
    // private ActionBox[] row3Actions;


    // Start is called before the first frame update
    void Awake()
    {
        if (actionBoxPrefab == null || actionBoxPrefab.GetComponent<ActionBox>() == null)
        {
            throw new UnityException("Error with action box prefab");
        }
        if (runtimeActionBoxRoot == null || runtimeActionBoxRoot.childCount > 0)
        {
            throw new UnityException("Runtime action root should exist and be empty at start");
        }
        if (actionBoxPlaceholders == null || actionBoxPlaceholders.childCount != 13)
        {
            throw new UnityException("Action box placeholder root should exist and have 13 action placeholders");
        }

        getActionPlaceholderPositions();

        // initialize the actionbox arrays
        row1Actions = new ActionBox[3];
        row2Actions = new ActionBox[9];
        // row3Actions = new ActionBox[27];
    }

    private void getActionPlaceholderPositions()
    {
        row0Position = actionBoxPlaceholders.GetChild(0).GetComponent<RectTransform>();
        row1Positions = new List<RectTransform>();
        for (int i = 1; i < 4; i++)
        {
            row1Positions.Add(actionBoxPlaceholders.GetChild(i).GetComponent<RectTransform>());
        }
        row2Positions = new List<RectTransform>();
        for (int i = 4; i < 13; i++)
        {
            row2Positions.Add(actionBoxPlaceholders.GetChild(i).GetComponent<RectTransform>());
        }
        // row3Positions = new List<RectTransform>();
        // for (int i = 13; i < 40; i++)
        // {
        //     row2Positions.Add(actionBoxPlaceholders.GetChild(i).GetComponent<RectTransform>());
        // }

        foreach (Transform placeholder in actionBoxPlaceholders)
        {
            placeholder.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // called when the currently selected action changes
    public override void onActionChanged(ActionTreeNode nextAction)
    {
        if (currentAction != null && isImmediateChildOf(currentAction, nextAction) > -1)
        {
            moveInto(currentAction, nextAction);
            currentAction = nextAction;
        }
        else if (currentAction != null && isParentOf(currentAction, nextAction))
        {
            moveBack(nextAction);
            currentAction = nextAction;
        }
        else
        {
            // there is no animation set for this change
            currentAction = nextAction;
            hardSet(currentAction);
        }
    }


    // Show the action boxes with no animation. Most basic update, delete the boxes and instantiate new ones
    private void hardSet(ActionTreeNode action)
    {
        // delete all existing action boxes
        destroyActionBox(row0Action);

        foreach (ActionBox actionBox in row1Actions)
        {
            destroyActionBox(actionBox);
        }

        foreach (ActionBox actionBox in row2Actions)
        {
            destroyActionBox(actionBox);
        }

        // foreach (ActionBox actionBox in row3Actions)
        // {
        //     destroyActionBox(actionBox);
        // }

        // create action boxes for the action and children
        if (action.isRoot == false)
        {
            row0Action = instantiateActionBox(action, row0Position);
        } // TODO: should there be something shown for when at the root?


        // make the row1 actions
        for (int i = 0; i < action.transform.childCount; i++)
        {
            ActionTreeNode childAction = action.transform.GetChild(i).GetComponent<ActionTreeNode>();
            ActionBox newRow1ActionBox = instantiateActionBox(childAction, row1Positions[i]);
            row1Actions[i] = newRow1ActionBox;

            // make the row2 actions
            for (int j = 0; j < childAction.transform.childCount; j++)
            {
                ActionTreeNode subChildAction = childAction.transform.GetChild(j).GetComponent<ActionTreeNode>();
                ActionBox newRow2ActionBox = instantiateActionBox(subChildAction, row2Positions[(i * 3) + j]);
                row2Actions[(i * 3) + j] = newRow2ActionBox;

                //make the row3 actions
                // for(int k = 0; k < childAction.transform.childCount; k++)
                // {
                //     ActionTreeNode subsubChildAction = subChildAction.transform.GetChild(k).GetComponent<ActionTreeNode>();
                //     ActionBox newRow3ActionBox = instantiateActionBox(subChildAction, row3Positions[((i*3) + j) + k]);
                //     row3Actions[((i * 3) + j) + k] = newRow3ActionBox;
                // }
            }
        }
    }

    // update the ui by moving down a branch of the action tree, animating the boxes
    private void moveInto(ActionTreeNode parent, ActionTreeNode child)
    {
        int childNumber = isImmediateChildOf(parent, child);

        // delete the selected box
        destroyActionBox(row0Action);

        // delete row1 except the one thats being selected
        for (int i = 0; i < 3; i++)
        {
            if (i == childNumber)
            {
                row1Actions[i].moveTo(row0Position, true /* skipDelay *//*);
                row0Action = row1Actions[i];
            }
            else
            {
                destroyActionBox(row1Actions[i]);
            }
        }

        // delete row2 except the 3 that are moving up to row1
        for (int i = 0; i < 9; i++)
        {
            if (row2Actions[i] != null)
            {
                if (Mathf.FloorToInt(i / 3) == childNumber)
                {
                    int row1Place = i % 3;
                    row2Actions[i].moveTo(row1Positions[row1Place]);
                    row1Actions[row1Place] = row2Actions[i];
                    row2Actions[i] = null;
                }
                else
                {
                    destroyActionBox(row2Actions[i]);
                }
            }
        }

        /* TODO: this must be implemented. For now theres not enough actions to show it
        // make the new row2
        if (child.transform.childCount > 0)
        {
            // make the row2 actions
            for (int j = 0; j < childAction.transform.childCount; j++)
            {
                ActionBox newSubActionBox = instantiateActionBox(childAction.transform.GetChild(j).GetComponent<ActionTreeNode>(), row2Positions[(i * 3) + j]);
                row2Actions.Add(newSubActionBox);
            }

        }
        *//*
    }

    // move up the tree. Animate the boxes moving
    private void moveBack(ActionTreeNode action)
    {
        // TODO: implement this method
        hardSet(action);
    }

    private int isImmediateChildOf(ActionTreeNode inputParent, ActionTreeNode inputChild)
    {
        for (int i = 0; i < inputParent.transform.childCount; i++)
        {
            if (inputParent.transform.GetChild(i).GetComponent<ActionTreeNode>() == inputChild)
            {
                return i;
            }
        }
        return -1;
    }

    private bool isParentOf(ActionTreeNode inputChild, ActionTreeNode inputParent)
    {
        return inputChild.transform.parent.GetComponent<ActionTreeNode>() == inputParent;
    }

    private ActionBox instantiateActionBox(ActionTreeNode action, RectTransform position)
    {
        GameObject newActionBoxGameObject = Instantiate(actionBoxPrefab, runtimeActionBoxRoot);
        ActionBox newActionBox = newActionBoxGameObject.GetComponent<ActionBox>();
        if (UISettings.instance.uiContentMode == UIContentMode.COLORS)
        {
            // on color mode, only show the color and no text
            newActionBox.initialize(position, "", action.color);
        }
        else if (UISettings.instance.uiContentMode == UIContentMode.LABELS)
        {
            // on labels mode, show color and text
            newActionBox.initialize(position, action.actionName, action.color);
        }
        else
        {
            throw new System.NotImplementedException("Actionbox instantiate not implemented for ui content mode " + UISettings.instance.uiContentMode);
        }
        return newActionBox;
    }

    private void destroyActionBox(ActionBox actionBox)
    {
        if (actionBox != null)
        {
            actionBox.destroyIfNotAnimating();
        }
    }

    public override void onEvent(EventType eventType, EventResult eventResult, ActionTreeNode action)
    {
        if (eventResult == EventResult.FAIL)
        {
            foreach (Flincher flincher in GetComponents<Flincher>())
            {
                flincher.flinch();
            }
        }

        if (eventResult == EventResult.PASS && eventType == EventType.CONFIRM)
        {
            row0Action.doConfirmAnimation();
        }

        if (eventType == EventType.RESET)
        {
            hardSet(action);
        }
    }
    */
}
