using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Loyufei.InputSystem 
{
    public static class InputCollectionExtensions
    {
        public static TInputSet GetSet<TInputSet>(this IInputCollection self) where TInputSet : IInputSet
        {
            return self.OfType<TInputSet>().First();
        }

        public static List<TInputSet> GetSets<TInputSet>(this IInputCollection self) where TInputSet : IInputSet
        {
            return self.OfType<TInputSet>().ToList();
        }

        public static IInputSet GetSetbyMode(this IInputCollection self, EInputMode inputMode)
        {
            return self.Find(f => f.InputMode == inputMode);
        }

        public static TInputSet GetSetbyMode<TInputSet>(this IInputCollection self, EInputMode inputMode) where TInputSet : IInputSet
        {
            return self.GetSetbyMode(inputMode).IsType<TInputSet>();
        }
    }
}
