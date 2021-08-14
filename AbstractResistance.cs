using UnityEngine;

public abstract class AbstractResistance  {
    
    /**
     * The type of damage that should be reduced.
     */
    [SerializeField]
    public DamageType.Types Type;

    
    /**
     * 100 means remove all damage. 50 == 50%, etc
     */
    [SerializeField]
    public int DamageReduction;






}