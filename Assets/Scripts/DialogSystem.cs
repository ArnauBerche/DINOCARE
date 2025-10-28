using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogSystem : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField, TextArea(2, 4)] private string[] dialogueLines;
    private bool isTyping;

    private float typingTime;
    [SerializeField] private float defaultTypingTime = 0.05f;

    private int lineIndex;

    private void Start()
    {
        SetDialogue(dialogueLines);
    }


    void Update()
    {
        if (lineIndex == dialogueLines.Length)
        {
            this.enabled = false;
        }
    }

    private void StartDialogue()
    {
        isTyping = true;
        dialoguePanel.SetActive(true);
        lineIndex = 0;
        StartCoroutine(ShowLine());
    }

    private void NextDialogueLine()
    {
        lineIndex++;
        if (lineIndex < dialogueLines.Length)
        {
            StartCoroutine(ShowLine());
        }
        else
        {
            isTyping = false;
            dialoguePanel.SetActive(false);
        }
    }

    public void SkipDialogue()
    {
        if (!isTyping) return;
        if (dialogueText.text == dialogueLines[lineIndex])
        {
            typingTime = defaultTypingTime;
            NextDialogueLine();
        }
        else
        {
            typingTime = 0;
        }
    }
    public void SetDialogue(string[] lines)
    {
        dialogueLines = lines;
        StartDialogue();
    }

    private IEnumerator ShowLine()
    {
        dialogueText.text = string.Empty;

        foreach (char ch in dialogueLines[lineIndex])
        {
            dialogueText.text += ch;
            yield return new WaitForSecondsRealtime(typingTime);
        }
    }
}