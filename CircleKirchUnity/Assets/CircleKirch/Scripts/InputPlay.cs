using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlay : MonoBehaviour {

    public enum InteractionStep { SelectRule, SelectTarget, SelectArguments, SearchForRule, SearchForArguments };
    public List<InteractionStep> interactionSteps = new List<InteractionStep>();

    public enum Rule { None = 0, Ohm = 1, KVL = 2, KCL = 3 }
    public Rule rule;
    public Rating ruleTarget;
    public List<Rating> ratingArguments = new List<Rating>();
    public List<Wire> wireArguments = new List<Wire>();

    public GameObject rulesPanel;

    private bool doneSelectingArguments = false;

    private int currentStep = 0;

    void Awake() {
        rulesPanel = GetComponent<InputManager>().playCanvas.transform.Find("LawsPanel").gameObject;
        rulesPanel.SetActive(false);
    }

    // Use this for initialization
    void Start() {
        IEnumerator coroutine = ExecuteRule();
        StartCoroutine(coroutine);
    }

    // Update is called once per frame
    void Update() {
        // if (Input.GetMouseButtonDown(0)) {
        //     RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        //     if (hit.collider != null) {
        //         Transform objectHit = hit.transform;

        //         // Do stuff
        //     }
        // }
    }


    IEnumerator ExecuteRule() {
        bool mainUnknownFound = false;
        while (!mainUnknownFound) {
            while (currentStep < interactionSteps.Count) {
                switch (interactionSteps[currentStep]) {
                    case InteractionStep.SelectRule:
                        Debug.Log("Selecting a rule");
                        yield return SelectRule();
                        break;
                    case InteractionStep.SelectTarget:
                        Debug.Log("Selecting a target");
                        yield return SelectTarget();
                        break;
                    case InteractionStep.SelectArguments:
                        Debug.Log("Selecting arguments");
                        yield return SelectArguments();
                        break;
                    case InteractionStep.SearchForRule:
                        yield return SearchForRule();
                        break;
                    case InteractionStep.SearchForArguments:
                        yield return SearchForArguments();
                        break;
                    default:
                        Debug.Log("unreognized interaction step mode");
                        break;
                }

                currentStep = currentStep + 1 % interactionSteps.Count;
                

                
            }

            switch (rule) {
                case Rule.Ohm:
                    OhmsLaw(ruleTarget, ratingArguments);
                    break;
                case Rule.KVL:
                    KVL(ruleTarget, ratingArguments);
                    break;
                case Rule.KCL:
                    KCL(ruleTarget, ratingArguments);
                    break;
                default:
                    Debug.Log("unreognized rule");
                    break;
            }

            cleanUp();

            yield return null;
        }
    }

    IEnumerator SelectRule() {
        rulesPanel.SetActive(true);
        while (rule == Rule.None) {
            yield return new WaitForEndOfFrame();
        }
        rulesPanel.SetActive(false);
        yield break;
    }

    IEnumerator SelectTarget() {
        // Rating ruleTarget = null;
        while (ruleTarget == null) {
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null) {
                    Transform objectHit = hit.transform;

                    Rating ratingHit = objectHit.GetComponent<Rating>();
                    if (ratingHit != null) {
                        if (ratingHit.known) {
                            Debug.Log("Target cannot be already known");
                        } else if (ratingHit.isArgument) {
                            Debug.Log("Argument cannot be target");
                        } else {
                            ratingHit.isTarget = true;
                            ruleTarget = ratingHit;
                            Debug.Log("Target Set");
                        }
                    }
                }
            }
            // yield return new WaitForEndOfFrame();
            // NOTE: need to use yield return null to detect button click
            yield return null;
        }
        yield break;
    }


    IEnumerator SelectArguments() {
        // List<Rating> ratingArguments = new List<Rating>();

        while (!doneSelectingArguments) {
            if (Input.GetMouseButtonDown(0)) {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null) {
                    Transform objectHit = hit.transform;

                    // Rating
                    Rating ratingHit = objectHit.GetComponent<Rating>();
                    if (ratingHit != null) {
                        if (ratingHit.isTarget) {
                            Debug.Log("Target cannot be argument");
                        } else {
                            if (ratingHit.isArgument) {
                                ratingArguments.Remove(ratingHit);
                                ratingHit.isArgument = false;
                                Debug.Log("Argument Unset");
                            } else {
                                ratingArguments.Add(ratingHit);
                                ratingHit.isArgument = true;
                                Debug.Log("Argument Set");
                            }
                        }
                    }

                    // Wire
                    Wire wireHit = objectHit.GetComponent<Wire>();
                    if (wireHit != null) {
                        if (wireHit.isArgument) {
                            wireArguments.Remove(wireHit);
                            wireHit.isArgument = false;
                            Debug.Log("Wire argument Unset");
                        } else {
                            wireArguments.Add(wireHit);
                            wireHit.isArgument = true;
                            Debug.Log("Wire argument Set");
                        }
                    }
                }
            }
            // yield return new WaitForEndOfFrame();
            // NOTE: need to use yield return null to detect button click
            yield return null;
        }
        yield break;
    }


    IEnumerator SearchForRule() {
        // TO DO
        Debug.Log("Search for rule");
        yield return new WaitForEndOfFrame();
        yield break;
    }

    IEnumerator SearchForArguments() {
        // TO DO
        Debug.Log("Search for arguments");
        yield return new WaitForEndOfFrame();
        yield break;
    }


    public void SetRule(int ruleToSet) {
        rule = (Rule)ruleToSet;
    }


    void OhmsLaw(Rating ruleTarget, List<Rating> ratingArguments) {
        Debug.Log("Executing Ohms");
        if (ratingArguments.Count != 2) {
            Debug.Log("Wrong number of arguments. Ohm should have exactly 2");
            return;
        }

        foreach (Rating r in ratingArguments) {
            if (r.circuitElement != ruleTarget.circuitElement) {
                Debug.Log("Args and target must have same parent");
                return;
            }

            if (!r.known) {
                Debug.Log("Args must be known");
                return;
            }
        }

        Debug.Log("Rating revealed!");
        ruleTarget.known = true;
    }

    void KVL(Rating ruleTarget, List<Rating> ratingArguments) {
        Debug.Log("Executing KVL");

        // Check if loop is valid
        // Starts with target
        // There is a wire connecting all
        // Should wires be arguments too?

    }


    void KCL(Rating ruleTarget, List<Rating> ratingArguments) {
        Debug.Log("Executing KCL");
    }

    public void setDoneSelectingArguments() {
        doneSelectingArguments = true;
    }

    void cleanUp() {
        Debug.Log("Cleaning up");
        foreach (Rating r in ratingArguments) {
            r.isArgument = false;
        }
        ratingArguments.Clear();

        foreach (Wire w in wireArguments) {
            w.isArgument = false;
        }
        wireArguments.Clear();

        ruleTarget.isTarget = false;
        ruleTarget = null;
        rule = Rule.None;
        currentStep = 0;
        doneSelectingArguments = false;
    }
}
