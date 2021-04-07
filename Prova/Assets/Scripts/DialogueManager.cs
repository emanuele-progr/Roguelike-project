using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

	public Text nameText;
	public Text dialogueText;

	public Animator animator;
	public AudioClip tic;

	private Queue<string> sentences;

	public static DialogueManager instance = null;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
			sentences = new Queue<string>();

			//Initialization code goes here[/INDENT]
		}
		else
		{
			Destroy(gameObject);
		}

	}

	public void StartDialogue(Dialogue dialogue)
	{
		animator = GameObject.Find("DialogueBox").GetComponent<Animator>();
		nameText = GameObject.Find("DialogueBox").transform.GetChild(0).GetComponent<Text>();
		dialogueText = GameObject.Find("DialogueBox").transform.GetChild(1).GetComponent<Text>();
		Debug.Log(dialogue.name);
		animator.SetBool("isOpen", true);

		nameText.text = dialogue.name;

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		SoundManager.instance.PlaySingle(tic);
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence(string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	void EndDialogue()
	{
		animator.SetBool("isOpen", false);
	}

}