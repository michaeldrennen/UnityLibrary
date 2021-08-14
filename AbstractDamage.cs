using System;
using UnityEngine;

public abstract class AbstractDamage {
    
    /**
     * The Type of Damage to apply to the target immediately.
     */
    [SerializeField] public DamageType.Types BurstDamageType;
    
    /**
     * The Type of damage to be applied over time.
     */
    [SerializeField] public DamageType.Types DamageOverTimeType;

    
    /**
     * How much damage should be "immediately" applied to the target.
     */
    [SerializeField] public int BurstDamage;

    
    /**
     * If this does extended damage, this is the total amount that could be applied over the duration. 
     */
    [SerializeField] public int TotalDamageOverTime = 0;

 
    /**
     * The duration over which the damage should be applied.
     */
    [SerializeField] public int TotalTimeToDeliverDamage = 0;


    public bool DoesBurstDamage() {
        return BurstDamage > 0;
    }


    public bool DoesDamageOverTime() {
        return TotalDamageOverTime > 0;
    }

    
    /**
     * Returns the amount of damage that should be applied to the target each second.
     */
    public int GetDamagePerSecond() {
        var damagePerSecond = (double) TotalDamageOverTime / TotalTimeToDeliverDamage;
        var rounded         = (int) Math.Round(damagePerSecond, 0);
        return rounded;
    }
}