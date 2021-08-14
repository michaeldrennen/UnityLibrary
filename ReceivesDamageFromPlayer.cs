using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceivesDamageFromPlayer : MonoBehaviour {
    private IEnumerator                                      _coroutine;
    private Damageable                                       _damageable;
    private Dictionary<DamageType.Types, AbstractResistance> _resistances;
    //private GameObject                                       _go;

    public void Awake() {
        
        _damageable  = gameObject.GetComponent<Damageable>();
        _resistances = _damageable.Resistances;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        if (other.GetComponent<DoesDamageToEnemy>()) {
            var otherGo = other.gameObject;
            var damages = otherGo.GetComponent<DoesDamage>().Damages;
            
            _damageable.DoDamage(
                                 damages, 
                                 _resistances
                                 );
            
            // Deletes the bullet.
            // @TODO Maybe don't do this here, in case the bullet should penetrate.
            other.gameObject.SetActive(false);
        }
    }
}
