using HexTecGames.Basics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class TileHighlighter : MonoBehaviour
    {
        public Color Color
        {
            get
            {
                return sr.color;
            }
        }

        [SerializeField] private SpriteRenderer sr = default;

        [SerializeField] private float startDelay = default;
        [SerializeField] private bool hasDuration = default;
        [DrawIf(nameof(hasDuration), true)][SerializeField] private float duration = 1;
        [SerializeField] private float fadeIn = 1;
        [SerializeField] private float fadeOut = 1;


        private bool isFadingIn;
        private bool isFadingOut;


        private void Reset()
        {
            sr = GetComponent<SpriteRenderer>();
        }
        private void OnDisable()
        {
            isFadingIn = false;
            isFadingOut = false;
        }
        public void Activate(Vector3 position, HighlightData data)
        {
            sr.color = data.color;
            startDelay = data.startDelay;
            hasDuration = data.hasDuration;
            duration = data.duration;
            fadeIn = data.fadeIn;
            fadeOut = data.fadeOut;
            Activate(position);
        }
        public void Activate(Vector3 position)
        {
            gameObject.SetActive(true);
            StartCoroutine(ShowHighlight(position));
        }
        public void Activate(Vector3 position, Color color)
        {
            sr.color = color;
            Activate(position);
        }
        public void Deactivate()
        {
            if (!gameObject.activeSelf)
            {
                return;
            }
            else if (!gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
                return;
            }
            StartCoroutine(FadeOut(sr.color, fadeOut));
        }
        public IEnumerator ShowHighlight(Vector2 position)
        {
            yield return ShowHighlight(position, startDelay, duration, fadeIn, fadeOut);
        }
        public IEnumerator ShowHighlight(Vector2 position, float startDelay, float duration, float fadeIn, float fadeOut)
        {
            sr.enabled = false;
            Color startCol = sr.color;
            transform.position = position;
            yield return new WaitForSeconds(startDelay);
            yield return FadeIn(startCol, fadeOut);

            if (hasDuration == false)
            {
                yield break;
            }
            yield return new WaitForSeconds(duration);

            yield return FadeOut(startCol, fadeIn);
            sr.color = startCol;
            gameObject.SetActive(false);
        }

        private IEnumerator FadeColor(Color startCol, Color endCol, float duration)
        {
            float timer = 0;
            while (timer < duration)
            {
                sr.color = Color.Lerp(startCol, endCol, timer / duration);
                yield return null;
                timer += Time.deltaTime;
            }
            sr.color = endCol;
        }
        private IEnumerator FadeOut(Color startCol, float duration)
        {
            isFadingOut = true;
            Color endColor = startCol;
            endColor.a = 0;
            yield return FadeColor(startCol, endColor, duration);
            gameObject.SetActive(false);
            sr.color = startCol;
        }
        private IEnumerator FadeIn(Color startCol, float duration)
        {
            sr.enabled = true;
            isFadingIn = true;
            Color endColor = startCol;
            endColor.a = 0;
            yield return FadeColor(endColor, startCol, duration);
        }
    }
}