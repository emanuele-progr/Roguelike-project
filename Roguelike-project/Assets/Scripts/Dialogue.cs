using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
	public string name;

	[TextArea(3, 10)]
	public string[] sentences;
	public Dialogue(string n, string sentence)
    {
		name = n;
		sentences = new string[1];
		sentences[0] = sentence;
	}



}