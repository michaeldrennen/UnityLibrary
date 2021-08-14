
public class DamageHot : AbstractDamage {
    public DamageHot(
        int              burstDamage              = 1,
        int              totalDamageOverTime      = 0,
        int              totalTimeToDeliverDamage = 0) {
        BurstDamageType          = DamageType.Types.Hot;
        DamageOverTimeType       = DamageType.Types.Hot;
        BurstDamage              = burstDamage;
        TotalDamageOverTime      = totalDamageOverTime;
        TotalTimeToDeliverDamage = totalTimeToDeliverDamage;
    }
}