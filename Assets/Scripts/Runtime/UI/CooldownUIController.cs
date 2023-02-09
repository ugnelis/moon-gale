using MoonGale.Core;
using MoonGale.Runtime.Player;
using UnityEngine;
using UnityEngine.UI;

namespace MoonGale.Runtime.UI
{
    internal sealed class CooldownUIController : MonoBehaviour
    {
        [SerializeField]
        private Image weakAttackCooldownImage;

        [SerializeField]
        private Image strongAttackCooldownImage;

        [SerializeField]
        private Image dashCooldownImage;

        private float strongAttackUsedTimeSeconds;
        private float strongAttackRechargedTimeSeconds;

        private float weakAttackUsedTimeSeconds;
        private float weakAttackRechargedTimeSeconds;

        private float dashUsedTimeSeconds;
        private float dashRechargedTimeSeconds;

        private void OnEnable()
        {
            GameManager.AddListener<PlayerStrongAttackMessage>(OnStrongAttacked);
            GameManager.AddListener<PlayerWeakAttackMessage>(OnWeakAttacked);
            GameManager.AddListener<PlayerDashMessage>(OnDashed);
        }

        private void OnDisable()
        {
            GameManager.RemoveListener<PlayerStrongAttackMessage>(OnStrongAttacked);
            GameManager.RemoveListener<PlayerWeakAttackMessage>(OnWeakAttacked);
            GameManager.RemoveListener<PlayerDashMessage>(OnDashed);
        }

        private void Update()
        {
            UpdateImageFill(weakAttackCooldownImage, weakAttackUsedTimeSeconds, weakAttackRechargedTimeSeconds);
            UpdateImageFill(strongAttackCooldownImage, strongAttackUsedTimeSeconds, strongAttackRechargedTimeSeconds);
            UpdateImageFill(dashCooldownImage, dashUsedTimeSeconds, dashRechargedTimeSeconds);
        }

        private static void UpdateImageFill(Image image, float usedTimeSeconds, float rechargedTimeSeconds)
        {
            if (rechargedTimeSeconds <= Time.time)
            {
                image.fillAmount = 1f;
                return;
            }

            image.fillAmount = Mathf.InverseLerp(usedTimeSeconds, rechargedTimeSeconds, Time.time);
        }

        private void OnStrongAttacked(PlayerStrongAttackMessage message)
        {
            strongAttackUsedTimeSeconds = Time.time;
            strongAttackRechargedTimeSeconds = message.NextAttackTimeSeconds;
        }

        private void OnWeakAttacked(PlayerWeakAttackMessage message)
        {
            weakAttackUsedTimeSeconds = Time.time;
            weakAttackRechargedTimeSeconds = message.NextAttackTimeSeconds;
        }

        private void OnDashed(PlayerDashMessage message)
        {
            dashUsedTimeSeconds = Time.time;
            dashRechargedTimeSeconds = message.NextDashTimeSeconds;
        }
    }
}
