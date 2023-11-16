using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Loyufei.UI;
using UnityEngine.EventSystems;

namespace InputDemo
{
    public class InputButton : Selectable, ISubmitHandler
    {
        [SerializeField]
        private InputOption _Option;
        [SerializeField]
        private UnSelectableButton _Button;

        public InputOption Option => _Option;

        public UnSelectableButton Button => _Button;

        public virtual void OnSubmit(BaseEventData eventData)
        {
            // if we get set disabled during the press
            // don't run the coroutine.
            if (!IsActive() || !IsInteractable())
                return;

            _Button.OnSubmit(eventData);

            DoStateTransition(SelectionState.Pressed, false);
            StartCoroutine(OnFinishSubmit());
        }

        private IEnumerator OnFinishSubmit()
        {
            var fadeTime = colors.fadeDuration;
            var elapsedTime = 0f;

            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.unscaledDeltaTime;
                yield return null;
            }

            DoStateTransition(currentSelectionState, false);
        }
    }
}