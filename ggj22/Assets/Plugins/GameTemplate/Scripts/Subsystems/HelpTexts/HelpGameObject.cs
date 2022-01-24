using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HelpGameObject : MonoBehaviour {
	[Header("Data"), Space]
	[SerializeField] bool isUseAsWhenShow = true;
	[SerializeField] int levelWhenShow = 0;

	[Header("Refs"), Space]
	[SerializeField] Image image;
	[SerializeField] TextMeshProUGUI textField;
	[SerializeField] CanvasGroup cg;

#if UNITY_EDITOR
	private void OnValidate() {
		if (!textField)
			textField = GetComponent<TextMeshProUGUI>();
		if (!cg)
			cg = GetComponent<CanvasGroup>();
		if (!image)
			image = GetComponent<Image>();
	}
#endif

	void Awake() {
		TemplateGameManager.Instance.OnHelpModeChange += OnHelpModeChange;

		OnHelpModeChange(TemplateGameManager.Instance.HelpLevelMode);
	}

	void OnDestroy() {
		TemplateGameManager.Instance.OnHelpModeChange -= OnHelpModeChange;
	}

	void OnHelpModeChange(int mode) {
		LeanTween.cancel(gameObject, false);

		bool isNeeded = isUseAsWhenShow ? (mode == levelWhenShow) : (mode != levelWhenShow) ;

		if (textField) {
			LeanTweenEx.ChangeAlpha(textField, isNeeded ? 1.0f : 0.0f, 0.2f);
		}
		else if (cg) {
			LeanTweenEx.ChangeAlpha(cg, isNeeded ? 1.0f : 0.0f, 0.2f);
		}
		else if (image) {
			LeanTweenEx.ChangeAlpha(image, isNeeded ? 1.0f : 0.0f, 0.2f);
		}
		else {
			gameObject.SetActive(isNeeded);
		}
	}
}
