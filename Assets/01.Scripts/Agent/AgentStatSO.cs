using UnityEngine;

[CreateAssetMenu(menuName = "SO/AgentStat")]
public class AgentStatSO : ScriptableObject
{
    public float speed;
    public float health;
    public float attackDelay;
    public float defensive;
    public float criticalPercent;
    public float criticalDamage;
}
