using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace YouHadOneJob
{
    public class Coding : TabContent
    {
        [SerializeField]
        private Codeline codelinePrefab;
        [SerializeField]
        private Transform codelinesParent;
        [SerializeField]
        private ScrollRect codelinesScroll;

        private const int reportsCount = 4;
        private const int linesToTypePerRun = 20;
        private const int bugsPerReport = 4;
        private const int completeLinesToType = linesToTypePerRun * reportsCount;
        private const float runDuration = 15f;

        private string[] completeLines;
        private int currentLineIndex;
        private CodingState state;
        private int typedLines;
        private int bugsCount;
        private List<Codeline> buggableLines;
        private float runStartTime;

        private void Awake ()
        {
            string[] availableLines = CodeString.Value.Split ('\n');
            completeLines = new string[completeLinesToType];
            int availableIndex = 0;
            for (int i = 0; i < completeLines.Length; i++)
            {
                completeLines[i] = availableLines[availableIndex];
                availableIndex++;
                if (availableIndex == availableLines.Length)
                    availableIndex = 0;
            }

            buggableLines = new List<Codeline> ();
        }

        protected override void Tick (bool isFocused)
        {
            switch (state)
            {
                case CodingState.Completed:
                    break;
                case CodingState.Typing:
                    if (isFocused && Input.GetKeyDown (KeyCode.Space))
                    {
                        TypeLine ();
                        if (typedLines == linesToTypePerRun)
                        {
                            state = CodingState.Running;
                            runStartTime = Time.time;
                            typedLines = 0;
                        }
                    }
                    break;
                case CodingState.Running:
                    if (Time.time - runStartTime > runDuration)
                    {
                        for (int i = 0; i < bugsPerReport; i++)
                            BugLine ();
                        state = CodingState.Fixing;
                    }
                    break;
                case CodingState.Fixing:
                    if (bugsCount == 0)
                    {
                        state = CodingState.ReRunning;
                        runStartTime = Time.time;
                    }
                    break;
                case CodingState.ReRunning:
                    if (Time.time - runStartTime > runDuration)
                    {
                        if (currentLineIndex >= completeLinesToType)
                            state = CodingState.Completed;
                        else
                            state = CodingState.Typing;
                    }
                    break;
            }
        }

        private void TypeLine ()
        {
            Codeline codeline = Instantiate (codelinePrefab, codelinesParent);
            string text = completeLines[currentLineIndex];
            currentLineIndex++;
            codeline.Setup (text);
            if (text != string.Empty)
                buggableLines.Add (codeline);
            typedLines++;
            codelinesScroll.verticalNormalizedPosition = 0;
        }

        private void BugLine ()
        {
            int randomIndex = UnityEngine.Random.Range (0, buggableLines.Count);
            Codeline codeline = buggableLines[randomIndex];
            codeline.SetBugged (FixBug);
            buggableLines.Remove (codeline);
            bugsCount++;
        }

        private void FixBug (Codeline codeline)
        {
            buggableLines.Add (codeline);
            bugsCount--;
        }

        protected override string GetTabText ()
        {
            switch (state)
            {
                case CodingState.Completed:
                    return "Done";
                case CodingState.Typing:
                    return typedLines + "/" + linesToTypePerRun;
                case CodingState.Running:
                case CodingState.ReRunning:
                    return "Running...";
                case CodingState.Fixing:
                    return (bugsPerReport - bugsCount) + "/" + bugsPerReport;
            }
            throw new UnityException ();
        }

        protected override string GetInstructionsText ()
        {
            switch (state)
            {
                case CodingState.Completed:
                    return "Done, switch to another tab";
                case CodingState.Typing:
                    return "Press SPACE to code";
                case CodingState.Running:
                case CodingState.ReRunning:
                    return "WAIT " + Mathf.CeilToInt (runDuration - Time.time + runStartTime) + "s";
                case CodingState.Fixing:
                    return "Find and CLICK to fix bugs";
            }
            throw new UnityException ();
        }

        public CodingState GetState()
        {
            return state;
        }
    }
}