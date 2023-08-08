using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputDemo
{
    public interface IInteract 
    {
        public IContacts Contacts { get; }

        public void Interact(IContacts contacts);
    }

    public interface IContacts 
    {
        public List<IInteract> Interacts { get; }

        public void Contact(IInteract interact);
        public void DisContact(IInteract interact);
        public void Interact(IInteract interact);
    }
}