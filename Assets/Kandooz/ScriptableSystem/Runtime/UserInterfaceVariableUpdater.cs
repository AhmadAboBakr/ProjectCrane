using System;
using TMPro;
using UniRx;
using UnityEngine;

namespace Kandooz.ScriptableSystem
{
    [AddComponentMenu(menuName:"Kandooz/Scriptable System/UI Variable Updated")]
    public class UserInterfaceVariableUpdater : MonoBehaviour
    {
        [SerializeField] private ScriptableVariable variable;
        [SerializeField] private TextMeshProUGUI text;
        private CompositeDisposable _disposable;

        private void OnEnable()
        {
            _disposable = new CompositeDisposable();
            text.text = variable.ToString();
            variable.Do(_ => UpdateText()).Subscribe().AddTo(this);
        }

        private void UpdateText()
        {
            text.text = variable.ToString();
        }

        private void OnDisable()
        {
            _disposable.Dispose();
        }
    }
}