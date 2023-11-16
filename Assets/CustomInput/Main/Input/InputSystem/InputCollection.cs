using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Loyufei.InputSystem
{
    [CreateAssetMenu(fileName = "Input Collection", menuName = "Input System/Input Collection", order = 1)]
    public class InputCollection : ScriptableObject, IInputCollection
    {
        [SerializeField]
        private List<InputSet> _List;

        public List<IInputSet> this[EInputMode inputMode]
        {
            get => this.ToList().FindAll(f => f.InputMode.HasFlag(inputMode));
        }

        public Dictionary<string, List<IInputUnit>> Dictionary
        {
            get
            {
                var dic = new Dictionary<string, List<IInputUnit>>();

                this.ForEach(set =>
                {
                    set.OnUse.ForEach(unit =>
                    {
                        if (dic.TryGetValue(unit.Name, out var pair))
                        {
                            pair.Add(unit);
                        }

                        else
                        {
                            dic.Add(unit.Name, new List<IInputUnit>() { unit });
                        }
                    });
                });

                return dic;
            }
        }

        public IEnumerator<IInputSet> GetEnumerator() => this._List.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }

    public interface IInputCollection : IEnumerable<IInputSet>
    {
        public List<IInputSet> this[EInputMode inputMode] { get; }

        public Dictionary<string, List<IInputUnit>> Dictionary { get; }
    }
}