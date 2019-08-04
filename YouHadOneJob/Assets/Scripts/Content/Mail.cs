using System;
using UnityEngine;
using UnityEngine.UI;

namespace YouHadOneJob
{
    public enum MailingState
    {
        Writting = 0,
        Sending = 1,
        Cancelling = 2,
        Completed = 3,
    }

    public class Mail : TabContent
    {
        [SerializeField]
        private Text mailLabel;
        [SerializeField]
        private GameObject possibleWordsContainer;
        [SerializeField]
        private Text[] possibleWordsLabels;
        [SerializeField]
        private GameObject wrongWordAlert;

        private const int mailsToWrite = 5;
        private const int wordsCountPerMail = 20;
        private const float possibleWordChangeTime = 0.7f;
        private const float sendDuration = 15f;

        private MailingState state;
        private int possibleWordsCount;
        private string[] possibleWords;
        private float possibleWordElapsedTime;
        private int wordsCount;
        private float sendStartTime;
        private int writtenMails;

        private void Awake ()
        {
            HideWordAlert ();
            mailLabel.text = string.Empty;
            wordsCount = 0;
            writtenMails = 0;
            GenerateWords ();
        }

        private void GenerateWords ()
        {
            possibleWordsCount = possibleWordsLabels.Length;
            possibleWords = new string[possibleWordsCount];
            for (int i = 0; i < possibleWordsCount; i++)
            {
                string possibleWord = GetRandomWord ();
                possibleWords[i] = possibleWord;
                possibleWordsLabels[i].text = possibleWord;
            }
        }

        protected override void Tick (bool isFocused)
        {
            switch (state)
            {
                case MailingState.Completed:
                    break;
                case MailingState.Sending:
                case MailingState.Cancelling:
                    if (Time.time - sendStartTime > sendDuration)
                    {
                        state = MailingState.Writting;
                        mailLabel.text = string.Empty;
                        wordsCount = 0;
                        possibleWordsContainer.SetActive (true);
                    }
                    break;
                case MailingState.Writting:
                    possibleWordElapsedTime += Time.deltaTime;
                    if (possibleWordElapsedTime > possibleWordChangeTime)
                        ChangePossibleWord ();

                    if (isFocused && Input.GetKeyDown (KeyCode.Space))
                    {
                        ChooseWord ();

                        if (wordsCount == wordsCountPerMail)
                        {
                            writtenMails++;
                            if (writtenMails == mailsToWrite)
                            {
                                state = MailingState.Completed;
                                possibleWordsContainer.SetActive (false);
                            }
                            else
                            {
                                state = MailingState.Sending;
                                sendStartTime = Time.time;
                                possibleWordsContainer.SetActive (false);
                                wordsCount = 0;
                            }
                        }
                        else if (wordsCount == 0)
                        {
                            state = MailingState.Cancelling;
                            sendStartTime = Time.time;
                            possibleWordsContainer.SetActive (false);
                        }
                    }
                    break;
            }
        }

        private void ChangePossibleWord ()
        {
            possibleWordElapsedTime = 0;
            for (int i = 0; i < possibleWordsCount - 1; i++)
            {
                string word = possibleWords[i + 1];
                possibleWords[i] = word;
                possibleWordsLabels[i].text = word;
            }
            string newWord = GetRandomWord ();
            possibleWords[possibleWordsCount - 1] = newWord;
            possibleWordsLabels[possibleWordsCount - 1].text = newWord;
        }

        private void ChooseWord ()
        {
            string currentWord = possibleWords[0];
            if (currentWord.IndexOf ("meow", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                ShowWrongWordAlert ();
                wordsCount = 0;
                mailLabel.text += currentWord;
                return;
            }
            wordsCount++;
            string connector = UnityEngine.Random.Range (0f, 1f) > 0.2f ? " " : "\n";
            mailLabel.text += currentWord + connector;
            ChangePossibleWord ();
        }

        private void ShowWrongWordAlert ()
        {
            wrongWordAlert.SetActive (true);
            if (IsInvoking ("HideWordAlert"))
                CancelInvoke ("HideWordAlert");
            Invoke ("HideWordAlert", sendDuration);
        }

        private void HideWordAlert ()
        {
            wrongWordAlert.SetActive (false);
        }

        private string GetRandomWord ()
        {
            float random = UnityEngine.Random.Range (0f, 1f);
            if (random > 0.3f)
            {
                float woofRandom = UnityEngine.Random.Range (0f, 1f);
                if (woofRandom > 0.4f)
                    return "woof";
                if (woofRandom > 0.2f)
                    return "woof,";
                if (woofRandom > 0.15f)
                    return "woof.";
                return "Woof";
            }
            float meowRandom = UnityEngine.Random.Range (0f, 1f);
            if (meowRandom > 0.5f)
                return "meow";
            if (meowRandom > 0.4f)
                return "MEOW!";
            if (meowRandom > 0.3f)
                return "meow,";
            if (meowRandom > 0.2f)
                return "meow?";
            return "Meow";
        }

        protected override string GetTabText ()
        {
            switch (state)
            {
                case MailingState.Completed:
                    return "Done";
                case MailingState.Writting:
                    return wordsCount + "/" + wordsCountPerMail;
                case MailingState.Sending:
                    return "Sending...";
                case MailingState.Cancelling:
                    return "Cancelling...";
            }
            throw new UnityException ();
        }

        protected override string GetInstructionsText ()
        {
            switch (state)
            {
                case MailingState.Completed:
                    return "Done, switch to another tab";
                case MailingState.Writting:
                    return "Press SPACE to write the selected word";
                case MailingState.Sending:
                case MailingState.Cancelling:
                    return "WAIT " + Mathf.CeilToInt (sendDuration - Time.time + sendStartTime) + "s";
            }
            throw new UnityException ();
        }
    }
}