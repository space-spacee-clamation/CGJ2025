    using System;
    using System.Collections;
    using DG.Tweening;
    using TMPro;
    using UnityEngine;
    public class DialogBox : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI massageTest;
        [SerializeField] private CanvasGroup _group;
        private Animator _animator;
        private Action callBack;
        public void Init(Vector3 pos, string massage,Action callBack = null)
        {
            _group.alpha = 0;
            transform.position = pos;
            massageTest.text = massage;
            this.callBack = callBack;
            _animator= GetComponent<Animator>();
            callBack +=()=> Destroy(gameObject);
        }
        public void Show(float  time)
        {
            _animator.SetBool("Show", true);
            _group.DOFade(0.8f, ConstClass.DIALOG_ANIMATION_TIME).OnComplete(() => StartCoroutine(WaitTimeHide(time)));
        }

        private IEnumerator WaitTimeHide(float time)
        {
            WaitForSeconds wait = new WaitForSeconds(time);
            yield return wait;
            Hide();
        }
        public void Hide()
        {
            _animator.SetBool("Show", false);
            _group.DOFade(0, ConstClass.DIALOG_ANIMATION_TIME);
            StartCoroutine(WaitTimeDelet(ConstClass.DIALOG_ANIMATION_TIME));
        }
        private IEnumerator WaitTimeDelet(float time)
        {
            WaitForSeconds wait = new WaitForSeconds(time);
            yield return wait;
            Destroy(gameObject);
        }
    }
