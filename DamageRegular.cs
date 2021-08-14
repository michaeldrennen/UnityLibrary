
public class DamageRegular : AbstractDamage {
    public DamageRegular(
        int burstDamage              = 1,
        int totalDamageOverTime      = 0,
        int totalTimeToDeliverDamage = 0) {
        BurstDamageType          = DamageType.Types.Regular;
        DamageOverTimeType       = DamageType.Types.Regular;
        BurstDamage              = burstDamage;
        TotalDamageOverTime      = totalDamageOverTime;
        TotalTimeToDeliverDamage = totalTimeToDeliverDamage;
    }
}