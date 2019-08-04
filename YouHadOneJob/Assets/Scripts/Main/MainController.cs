using UnityEngine;
using UnityEngine.SceneManagement;

namespace YouHadOneJob
{
    public class MainController : MonoBehaviour
    {
        [SerializeField]
        private BossController panchito;
        [SerializeField]
        private GaugePointer sanityGauge;
        [SerializeField]
        private TabsManager tabManager;
        [SerializeField]
        private Arting art;
        [SerializeField]
        private Coding code;
        [SerializeField]
        private Mailing mail;
        [SerializeField]
        private float sanityGain = 8f;
        [SerializeField]
        private float sanityLoss = 5f;

        private float currentStamina;
        private bool lost;

        // Start is called before the first frame update
        void Start()
        {
            currentStamina = 100f;
            lost = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (lost)
                return;

            switch (tabManager.GetCurrentTab())
            {
                case TabType.Snakeing:
                    if(currentStamina <= 100f)
                        currentStamina += Time.deltaTime * sanityGain;

                    if (panchito.IsStaring())
                        Lost();

                    break;

                case TabType.Arting:
                    if (panchito.IsStaring() && art.GetState() == PaintingState.Cancelling)
                        Lost();

                    if (currentStamina >= 0f)
                        currentStamina -= Time.deltaTime * sanityLoss;

                    break;

                case TabType.Mailing:
                    if (panchito.IsStaring() && mail.GetState() == MailingState.Cancelling)
                        Lost();

                    if (currentStamina >= 0f)
                        currentStamina -= Time.deltaTime * sanityLoss;

                    break;

                case TabType.Coding:
                    if (panchito.IsStaring() && code.GetState() == CodingState.Fixing)
                        Lost();

                    if (currentStamina >= 0f)
                        currentStamina -= Time.deltaTime * sanityLoss;

                    break;
            }

            if (currentStamina <= 0f)
                Lost();

            UpdateStamina();
        }

        private void UpdateStamina()
        {
            sanityGauge.SetSanity(currentStamina);
        }

        private void Lost()
        {
            Debug.LogError("Lost");
            lost = true;
            Invoke("GoToGameOver", 2f);
        }

        private void GoToGameOver()
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}


