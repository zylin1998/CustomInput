using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Custom.InputSystem
{
    [CreateAssetMenu(fileName = "Input Setting", menuName = "Custum Input/Input Setting", order = 1)]
    public class InputSetting : ScriptableObject, IInputSetting
    {
        [SerializeField]
        private List<InputSet> _List;

        public IEnumerator<IInputSet> GetEnumerator() => this._List.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }

    public interface IInputSetting : IEnumerable<IInputSet>
    {
        public List<IInputSet> this[RuntimePlatform platform]
        {
            get => this.ToList().FindAll(f => f.Platform.Contains(platform));
        }

        public TInputSet GetSet<TInputSet>() where TInputSet : IInputSet
        {
            return this.OfType<TInputSet>().First();
        }

        public List<TInputSet> GetSets<TInputSet>() where TInputSet : IInputSet
        {
            return this.OfType<TInputSet>().ToList();
        }

        public IInputSet GetSetbyName(string name)
        { 
            return this.ToList().Find(f => f.Name == name); 
        }

        public TInputSet GetSetbyName<TInputSet>(string name) where TInputSet : IInputSet
        {
            return this.GetSetbyName(name) is TInputSet list ? list : default;
        }

        public Dictionary<string, List<IInputUnit>> Dictionary 
        {
            get 
            {
                var dic = new Dictionary<string, List<IInputUnit>>();

                foreach(var set in this) 
                {
                    set.OnUse.ToList().ForEach(unit => 
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
                }

                return dic;
            }
        }
    }
}