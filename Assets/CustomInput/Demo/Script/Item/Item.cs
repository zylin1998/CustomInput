using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InputDemo
{
    public class Item : MonoBehaviour, IInteract
    {
        public GameObject Sphere;

        private static void PickUp() => InputDemo.Score += 1;

        public IContacts Contacts { get; private set; }

        public void Interact(IContacts contacts)
        {
            this.Contacts = contacts;

            PickUp();

            Destroy(this.gameObject);
        }

        private void OnTriggerEnter(Collider collider)
        {
            var interact = collider.GetComponent<IContacts>();

            if (interact != null) 
            {
                interact.Contact(this);
            }
        }

        private void OnTriggerExit(Collider collider)
        {
            var interact = collider.GetComponent<IContacts>();

            if (interact != null)
            {
                interact.DisContact(this);
            }
        }

        private void OnDestroy()
        {
            if (this.Contacts != null) 
            {
                this.Contacts.DisContact(this);
            }   
        }
    }
}