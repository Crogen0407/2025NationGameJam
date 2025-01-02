using Crogen.AgentFSM;
using Crogen.PowerfulInput;
using UnityEngine;

public class Player : Agent
{
    [field:SerializeField] public InputReader InputReader { get; private set; }
    [field:SerializeField] public AgentStatSO AgentStatSO { get; private set; }
    private void Awake()
    {
        //처음 State는 Idle입니다.
        Initialize<PlayerStateEnum>();
    }
    
    
}
