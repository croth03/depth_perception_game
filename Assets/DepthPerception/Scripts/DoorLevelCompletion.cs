using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DepthPerception
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class DoorLevelCompletion : MonoBehaviour
    {
        [Header("Completion")]
        [SerializeField] private string completionText = "LEVEL COMPLETE";
        [SerializeField, Min(0.1f)] private float fadeDuration = 0.85f;
        [SerializeField, Min(0f)] private float postFadeHoldDuration = 0.45f;
        [SerializeField] private string nextSceneName = SceneNavigator.EndScreenScene;
        [SerializeField] private UnityEvent completed = new UnityEvent();

        [Header("Optional explicit player")]
        [SerializeField] private DepthPlayerController player;

        private bool isComplete;
        private CanvasGroup completionCanvasGroup;

        public bool IsComplete => isComplete;

        private void Reset()
        {
            GetComponent<BoxCollider>().isTrigger = true;
        }

        private void OnValidate()
        {
            BoxCollider trigger = GetComponent<BoxCollider>();
            if (trigger != null)
                trigger.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            DepthPlayerController enteringPlayer = other.GetComponentInParent<DepthPlayerController>();
            if (enteringPlayer == null || (player != null && enteringPlayer != player))
                return;

            Complete(enteringPlayer);
        }

        public void Complete(DepthPlayerController completingPlayer)
        {
            if (isComplete || completingPlayer == null)
                return;

            isComplete = true;
            player = completingPlayer;

            // Lock first so animation velocity is cleared, then stop the controller entirely.
            // This leaves the player safely held in place while a later scene loader can react
            // to the completed event.
            player.SetControlLocked(true);
            player.enabled = false;

            BuildCompletionOverlay();
            StartCoroutine(FadeToCompletion());
        }

        private void BuildCompletionOverlay()
        {
            GameObject root = new GameObject(
                "Level Completion UI (Runtime)",
                typeof(RectTransform),
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(GraphicRaycaster),
                typeof(CanvasGroup));

            Canvas canvas = root.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 1000;

            CanvasScaler scaler = root.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            completionCanvasGroup = root.GetComponent<CanvasGroup>();
            completionCanvasGroup.alpha = 0f;
            completionCanvasGroup.interactable = false;
            completionCanvasGroup.blocksRaycasts = true;

            GameObject fade = new GameObject("Restrained Fade", typeof(RectTransform), typeof(Image));
            fade.transform.SetParent(root.transform, false);
            RectTransform fadeRect = fade.GetComponent<RectTransform>();
            fadeRect.anchorMin = Vector2.zero;
            fadeRect.anchorMax = Vector2.one;
            fadeRect.offsetMin = Vector2.zero;
            fadeRect.offsetMax = Vector2.zero;
            Image fadeImage = fade.GetComponent<Image>();
            fadeImage.color = new Color(0.027f, 0.035f, 0.075f, 0.86f);

            GameObject label = new GameObject("Completion Label", typeof(RectTransform), typeof(Text));
            label.transform.SetParent(root.transform, false);
            RectTransform labelRect = label.GetComponent<RectTransform>();
            labelRect.anchorMin = new Vector2(0.5f, 0.5f);
            labelRect.anchorMax = new Vector2(0.5f, 0.5f);
            labelRect.pivot = new Vector2(0.5f, 0.5f);
            labelRect.anchoredPosition = Vector2.zero;
            labelRect.sizeDelta = new Vector2(760f, 120f);

            Text labelText = label.GetComponent<Text>();
            labelText.text = completionText;
            labelText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            labelText.fontSize = 52;
            labelText.fontStyle = FontStyle.Bold;
            labelText.alignment = TextAnchor.MiddleCenter;
            labelText.color = new Color(0.51f, 0.91f, 0.93f, 1f);
            labelText.raycastTarget = false;
        }

        private IEnumerator FadeToCompletion()
        {
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / Mathf.Max(0.01f, fadeDuration));
                completionCanvasGroup.alpha = Mathf.SmoothStep(0f, 1f, t);
                yield return null;
            }

            completionCanvasGroup.alpha = 1f;
            completed.Invoke();

            if (postFadeHoldDuration > 0f)
                yield return new WaitForSecondsRealtime(postFadeHoldDuration);

            if (!string.IsNullOrWhiteSpace(nextSceneName))
                SceneNavigator.LoadScene(nextSceneName);
        }
    }
}
