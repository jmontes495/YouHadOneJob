using UnityEngine;
using UnityEngine.UI;

namespace YouHadOneJob
{
    public class Mail : MonoBehaviour
    {
        [SerializeField]
        private Text mailLabel;
        [SerializeField]
        private Text mailWordsLabel;
        [SerializeField]
        private Text[] wordsLabels;
        [SerializeField]
        private Image currentWordHighlight;

        private const float wordChangeTime = 0.7f;
        private const int completedMailWordsCount = 50;
        private readonly string mailWordsLabelFormat = "Words : {0}/" + completedMailWordsCount;

        private int wordsCount;
        private int indexForCurrent;
        private string[] words;
        private float wordElapsedTime;
        private int mailWordsCount;
        private bool isCompleted;

        private void Start ()
        {
            mailLabel.text = string.Empty;
            mailWordsCount = 0;
            mailWordsLabel.text = string.Format (mailWordsLabelFormat, mailWordsCount);
            GenerateWords ();
        }

        private void GenerateWords ()
        {
            wordsCount = wordsLabels.Length;
            indexForCurrent = Mathf.FloorToInt (wordsCount / 2f);
            words = new string[wordsCount];
            for (int i = 0; i < indexForCurrent; i++)
            {
                string word = string.Empty;
                words[i] = word;
                wordsLabels[i].text = word;
            }
            for (int i = indexForCurrent; i < wordsCount; i++)
            {
                string word = GetRandomWord ();
                words[i] = word;
                wordsLabels[i].text = word;
            }
        }

        private void Update ()
        {
            if (isCompleted)
                return;

            wordElapsedTime += Time.deltaTime;
            if (wordElapsedTime > wordChangeTime)
                ChangeWord ();
            if (Input.GetKeyDown (KeyCode.Space))
                ChooseWord ();
        }

        private void ChangeWord ()
        {
            wordElapsedTime = 0;
            for (int i = 0; i < wordsCount - 1; i++)
            {
                string word = words[i + 1];
                words[i] = word;
                wordsLabels[i].text = word;
            }
            string newWord = GetRandomWord ();
            words[wordsCount - 1] = newWord;
            wordsLabels[wordsCount - 1].text = newWord;
        }

        private void ChooseWord ()
        {
            string currentWord = words[indexForCurrent];
            if (currentWord == "meow")
            {
                mailLabel.text = string.Empty;
                mailWordsCount = 0;
                mailWordsLabel.text = string.Format (mailWordsLabelFormat, mailWordsCount);
                return;
            }
            mailWordsCount++;
            mailWordsLabel.text = string.Format (mailWordsLabelFormat, mailWordsCount);
            if (mailWordsCount == completedMailWordsCount)
            {
                isCompleted = true;
                return;
            }
            ChangeWord ();
            mailLabel.text += " " + currentWord;
        }

        private string GetRandomWord ()
        {
            if (Random.Range (0f, 1f) > 0.5f)
                return "woof";
            return "meow";
        }
    }
}