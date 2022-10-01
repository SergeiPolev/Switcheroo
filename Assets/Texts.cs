using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Texts : MonoBehaviour
{
    [SerializeField] private string[] texts;
    [SerializeField] private float delayBetweenTexts;

	[SerializeField] private Button startButton;
	[SerializeField] private CanvasGroup startButtonGroup;

    private float timer;

    private int currentText = 0;

	public TextMeshProUGUI text;

	private bool ended;

	void Start()
	{
		StartCoroutine(RevealText());

		startButton.onClick.AddListener(StartGame);
	}

	private void Update()
    {
		if (ended && Time.time > timer)
        {
			StartCoroutine(RevealText());
		}    
    }

	private void StartGame()
    {
		SceneManager.LoadScene(1);
    }

	IEnumerator RevealText()
	{
		ended = false;

		var originalString = texts[currentText];
		text.text = "";

		var numCharsRevealed = 0;
		while (numCharsRevealed < originalString.Length)
		{
			++numCharsRevealed;
			text.text = originalString.Substring(0, numCharsRevealed);

			yield return new WaitForSeconds(0.07f);
		}

		yield return new WaitForSeconds(2f);

		if (currentText < texts.Length - 1)
        {
			ended = true;

			currentText++;

			timer = Time.time + delayBetweenTexts;
        }
	}
}