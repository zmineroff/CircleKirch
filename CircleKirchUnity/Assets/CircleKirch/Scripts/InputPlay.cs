﻿using System.Collections;
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

                currentStep++;
            }

            switch (rule) {
                case Rule.Ohm:
                    OhmsLaw(ruleTarget, ratingArguments);
                    break;
                case Rule.KVL:
                    KVL(ruleTarget, ratingArguments, wireArguments);
                    break;
                case Rule.KCL:
                    KCL(ruleTarget, ratingArguments, wireArguments);
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

    void KVL(Rating ruleTarget, List<Rating> ratingArguments, List<Wire> wireArguments) {
        Debug.Log("Executing KVL");

        // Check that target is voltage
        if (ruleTarget.ratingType != Rating.RatingType.Voltage) {
            Debug.Log("Target must be voltage");
        }

        // Check that only voltages are selected
        foreach (Rating r in ratingArguments) {
            if (r.ratingType != Rating.RatingType.Voltage) {
                Debug.Log("All rating arguments must be voltage ratings");
                return;
            }

            if (!r.known) {
                Debug.Log("All rating arguments must be known");
                return;
            }
        }

        // Check Kirchhoff Loop
        CircuitElement currentElt = ruleTarget.circuitElement;
        Terminal currentTerminal = currentElt.terminals[0];
        Wire currentWire = currentTerminal.wires[0];
        if (!currentWire.isArgument) {
            Debug.Log("Invalid loop. Wire not set as argument.");
            return;
        }
        int iternum = 0;
        while (currentTerminal != ruleTarget.circuitElement.terminals[1]) {
            Debug.Log(iternum);
            if (iternum > 1000) {
                Debug.Log("Loop cap maxed reached");
                return;
            }
            iternum++;
            
            if (currentTerminal == currentWire.terminalA) {
                currentTerminal = currentWire.terminalB;
            } else {
                currentTerminal = currentWire.terminalA;
            }

            if (currentTerminal == ruleTarget.circuitElement.terminals[1]) {
                Debug.Log("Rating revealed!");
                ruleTarget.known = true;
                return;
            }

            currentElt = currentTerminal.circuitElement;
            bool eltIsJunction = currentElt.circuitElementType == CircuitElement.CircuitElementType.Junction;
            if (eltIsJunction) {
                Debug.Log("Junction");
                int numWireArgsInTerminal = 0;
                Wire nextWire = null;
                foreach (Wire w in currentTerminal.wires) {
                    if (w != currentWire && w.isArgument) {
                        numWireArgsInTerminal++;
                        nextWire = w;
                    }
                }
                currentWire = nextWire;
                if (numWireArgsInTerminal != 1) {
                    Debug.Log("Invalid loop. Junction should have only 2 wire arguments");
                    return;
                }
            } else {
                if (currentElt.circuitElementType == CircuitElement.CircuitElementType.Battery) {
                    Debug.Log("Battery");
                }
                if (currentElt.circuitElementType == CircuitElement.CircuitElementType.Resistor) {
                    Debug.Log("Resistor");
                }
                Wire nextWire = null;
                foreach (Terminal t in currentElt.terminals) {
                    if (t.wires.Count==0) {
                        Debug.Log("No wires on terminal");
                        return;
                    }
                    if (t.wires[0] != currentWire) {
                        nextWire = t.wires[0];
                        currentTerminal = t;
                    }
                }
                currentWire = nextWire;
            }

            if (currentWire == null) {
                Debug.Log("Bad");
                return;
            }

            if (!currentWire.isArgument) {
                Debug.Log("Invalid loop. Wire not set as argument.");
                return;
            }

            if (!eltIsJunction && !currentElt.voltage.isArgument && currentElt.voltage != ruleTarget) {
                Debug.Log("Invalid loop. Voltage not set as argument.");
                return;
            }
        }

        Debug.Log("Rating revealed!");
        ruleTarget.known = true;
    }


    void KCL(Rating ruleTarget, List<Rating> ratingArguments, List<Wire> wireArguments) {
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
