using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class Damageable : MonoBehaviour {
    public int hitPoints;
    public int armorPoints;
    public int shieldPoints;

    
    /**
     * Holds the name of the coroutine used to process damage.
     */
    private                 IEnumerator                                        _coroutine;
    
    
    /**
     * A flag to prevent the damage from getting re-added to the _damageTicker in the coroutine.
     */
    private bool _currentlyProcessingDamage = false;
    
    
    /**
     * All of the resistances that might lower damage.
     */
    [SerializeField] public Dictionary<DamageType.Types, AbstractResistance> Resistances;


    /**
     * Indexed by seconds, this holds all damage that should be applied.
     */
    private List<Dictionary<DamageType.Types, int>> _damageTicker;

    
    /**
     * 
     */
    public void Awake() {
        Resistances   = new Dictionary<DamageType.Types, AbstractResistance>();
        _damageTicker = new List<Dictionary<DamageType.Types, int>>();
    }


    /**
     * 
     */
    public void DoDamage(
        Dictionary<DamageType.Types, AbstractDamage>     damages,
        Dictionary<DamageType.Types, AbstractResistance> resistances) {
        _coroutine = ProcessDamage(damages, resistances);
        StartCoroutine(_coroutine);
    }
    

    /**
     * 
     */
    private IEnumerator ProcessDamage(
        Dictionary<DamageType.Types, AbstractDamage>     damages,
        Dictionary<DamageType.Types, AbstractResistance> resistances) {
        if (_currentlyProcessingDamage == false) {
            SetDamageTicker(damages, resistances);
        }

        // All damage has been queued up in the damageTicker.
        // Now it's time to loop through the damageTicker and apply it to this target.
        var damageTickerLength = _damageTicker.Count;
        for (var i = 0; i < damageTickerLength; i++) {
            var damageThisSecond = _damageTicker[i];

            foreach (var damage in damageThisSecond) {
                RemovePoints(damage.Key, damage.Value);
            }

            if (AmDead()) {
                yield break;
            }

            _currentlyProcessingDamage = true;
            yield return new WaitForSeconds(1f);
        }
    }


    /**
     * 
     */
    private void SetDamageTicker(
        Dictionary<DamageType.Types, AbstractDamage>     damages,
        Dictionary<DamageType.Types, AbstractResistance> resistances) {
        foreach (KeyValuePair<DamageType.Types, AbstractDamage> damage in damages) {
            // Process any Burst damage that this should be applied, and add to the damageTicker.
            if (damage.Value.DoesBurstDamage()) {
                var damageType   = damage.Value.BurstDamageType;
                var damageAmount = damage.Value.BurstDamage;
                var damageAmountAfterResistance = ApplyResistanceToDamageAmount(resistances,
                                                                                damageType,
                                                                                damageAmount);

                var damageDict = new Dictionary<DamageType.Types, int>
                                 {{damage.Value.BurstDamageType, damageAmountAfterResistance}};
                _damageTicker.Insert(0, damageDict);
            }


            // If this does any Damage Over Time, process that and add to the ticker.
            if (damage.Value.DoesDamageOverTime()) {
                var damageType      = damage.Value.DamageOverTimeType;
                var damagePerSecond = damage.Value.GetDamagePerSecond();
                var damagePerSecondAfterResistance = ApplyResistanceToDamageAmount(resistances,
                                                                                   damageType,
                                                                                   damagePerSecond);

                var numSeconds = damage.Value.TotalTimeToDeliverDamage;

                for (var i = 0; i < numSeconds; i++) {
                    var damageTickerIndex = i + 1;
                    var damageDict = new Dictionary<DamageType.Types, int>
                                     {{damage.Value.DamageOverTimeType, damagePerSecondAfterResistance}};

                    _damageTicker.Insert(damageTickerIndex, damageDict);
                }
            }
        }
    }


    /**
     * 
     */
    private AbstractResistance GETResistanceByType(DamageType.Types type) {
        if (Resistances.ContainsKey(type)) {
            return Resistances[type];
        }

        return null;
    }


    /**
     * 
     */
    private int ApplyResistanceToDamageAmount(Dictionary<DamageType.Types, AbstractResistance> resistances,
                                              DamageType.Types                                 damageType,
                                              int                                              damageAmount) {
        var resistance = GETResistanceByType(damageType);

        if (resistance == null) {
            return damageAmount;
        }

        var reduceDamageBy = (double) resistance.DamageReduction / 100;

        return (int) (damageAmount * reduceDamageBy);
    }


    /**
     * 
     */
    private void RemovePoints(DamageType.Types type,
                              int              damageToBeDone) {
        var remainingDamageToBeDone = damageToBeDone;
        remainingDamageToBeDone = RemoveShieldPoints(remainingDamageToBeDone);
        remainingDamageToBeDone = RemoveArmorPoints(remainingDamageToBeDone);
        remainingDamageToBeDone = RemoveHitPoints(remainingDamageToBeDone);
    }


    /**
     * 
     */
    private int RemoveShieldPoints(int damageToBeDone) {
        // If shield points are 0 (or less, safety check)...
        if (shieldPoints < 1) {
            return damageToBeDone;
        }

        if (shieldPoints >= damageToBeDone) {
            shieldPoints -= damageToBeDone;
            return 0;
        }

        damageToBeDone -= shieldPoints;
        shieldPoints   =  0;
        return damageToBeDone;
    }


    /**
     * 
     */
    private int RemoveArmorPoints(int damageToBeDone) {
        // If shield points are 0 (or less, safety check)...
        if (armorPoints < 1) {
            return damageToBeDone;
        }

        if (armorPoints >= damageToBeDone) {
            armorPoints -= damageToBeDone;
            return 0;
        }

        damageToBeDone -= armorPoints;
        armorPoints    =  0;
        return damageToBeDone;
    }


    /**
     * 
     */
    private int RemoveHitPoints(int damageToBeDone) {
        // If shield points are 0 (or less, safety check)...
        if (hitPoints < 1) {
            return damageToBeDone;
        }

        if (hitPoints >= damageToBeDone) {
            hitPoints -= damageToBeDone;
            return 0;
        }

        damageToBeDone -= hitPoints;
        hitPoints      =  0;
        return damageToBeDone;
    }


    /**
     * 
     */
    public bool AmDead() {
        if (hitPoints == 0) {
            return true;
        }

        return false;
    }
}