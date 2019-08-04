using UnityEngine;

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

        // Start is called before the first frame update
        void Start()
        {
            currentStamina = 100f;
            panchito.ComeFromDirection(BossController.CurrentDirection.Left);
        }

        // Update is called once per frame
        void Update()
        {
            switch(tabManager.GetCurrentTab())
            {
                case TabType.Snakeing:
                    if(currentStamina <= 100f)
                        currentStamina += Time.deltaTime * sanityGain;

                    if (panchito.HasLookedOnHisWay())
                        Lost();

                    break;

                case TabType.Arting:
                    if (panchito.HasLookedOnHisWay() && art.GetState() == PaintingState.Cancelling)
                        Lost();

                    if (currentStamina >= 0f)
                        currentStamina -= Time.deltaTime * sanityLoss;

                    break;

                case TabType.Mailing:
                    if (panchito.HasLookedOnHisWay() && mail.GetState() == MailingState.Cancelling)
                        Lost();

                    if (currentStamina >= 0f)
                        currentStamina -= Time.deltaTime * sanityLoss;

                    break;

                case TabType.Coding:
                    if (panchito.HasLookedOnHisWay() && code.GetState() == CodingState.ReRunning)
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
        }
    }
}


