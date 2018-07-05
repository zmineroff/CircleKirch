using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPlay : MonoBehaviour {

	// Later: use this to change what order player can click on components
	// E.g., for RuleTargetArg, only rules are active at first. Then, target becomes active
	public enum RuleExecutionMode {RuleTargetArg, TargetRuleArg}
	public RuleExecutionMode ruleExecMode = RuleExecutionMode.RuleTargetArg;

	public enum RuleStage {SelectRule, SelectTarget, SelectArguments}
	public RuleStage ruleStage;

	public enum Rule {None=0, Ohm=1, KVL=2, KCL=3}
	public Rule rule;
	public Rating ruleTarget;
	public List<Rating> ruleArguments = new List<Rating> ();


	// Use this for initialization
	void Start () {
		ruleStage = RuleStage.SelectRule;

		// switch (ruleExecMode) {
		// 	case RuleExecutionMode.RuleTargetArg:
		// 		ruleStage = RuleStage.SelectRule;
		// 		break;
		// 	case RuleExecutionMode.TargetRuleArg:
		// 		ruleStage = RuleStage.SelectTarget;
		// 		break;
		// 	default:
		// 		Debug.Log("unreognized rule execution mode");
		// 		break;   
		// }
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (hit.collider != null) {
				Transform objectHit = hit.transform;
				
				Rating ratingHit = objectHit.GetComponent<Rating>();
				if (ratingHit != null) {
					if (ruleStage==RuleStage.SelectTarget) {
						if (ratingHit.known) {
							Debug.Log("Target cannot be already known");
						} else {
							ratingHit.isTarget = true;
							ruleTarget = ratingHit;
							Debug.Log("Target Set");
							ruleStage = RuleStage.SelectArguments; //next rule
						}
					} else if (ruleStage==RuleStage.SelectArguments) {
						if (ratingHit.isTarget) {
							Debug.Log("Target cannot be argument");
						} else {
							ratingHit.isArgument = !ratingHit.isArgument;
							ruleArguments.Add(ratingHit);
						}
					}
				}
			}
		}
	}

	public void SetRule (int ruleToSet) {
		rule = (Rule)ruleToSet;
		ruleStage = RuleStage.SelectTarget; //next rule
	}

	public void ExecuteRule () {
		switch (rule) {
			case Rule.Ohm:
				OhmsLaw();
				break;
			case Rule.KVL:
				KVL();
				break;
			case Rule.KCL:
				KCL();
				break;
			default:
				Debug.Log("unreognized rule");
				break;   
		}
	}

	void OhmsLaw () {
		Debug.Log("Executing Ohms");
		if (ruleArguments.Count!=2) {
			Debug.Log("Wrong number of arguments. Ohm should have exactly 2");
			resetRules();
			return;
		}

		foreach (Rating r in ruleArguments) {
			if (r.circuitElement != ruleTarget.circuitElement) {
				Debug.Log("Args and target must have same parent");
				resetRules();
				return;
			}

			if (!r.known) {
				Debug.Log("Args must be known");
				resetRules();
				return;
			}
		}

		ruleTarget.known = true;
		resetRules();
	}

	void KVL () {
		Debug.Log("Executing KVL");
	}

	void KCL () {
		Debug.Log("Executing KCL");
	}

	void resetRules () {
		foreach (Rating r in ruleArguments) {
			r.isArgument = false;
		}
		ruleArguments.Clear();
		ruleTarget.isTarget = false;
		ruleTarget = null;
		rule = Rule.None;
		ruleStage = RuleStage.SelectRule; //first rule
	}



	// Can have function "NextRule" which selects the next rule in the sequence
	// based on ruleExecutionMode and current rule
	
	// for modes where arg selction is not last, you need a button to
	// indicate that you are done selecting arguments
}
