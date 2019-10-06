
using UnityEngine;
public interface IPlayerManager
{
    void StartCombat(GameManager manager);
    void StartTurn();
    void EndTurn();
    void EndCombat(bool won);
    bool IsVulnerable(BoardTarget pos);
}
