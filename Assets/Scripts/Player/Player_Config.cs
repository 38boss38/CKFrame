using CKFrame;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "配置/玩家配置", fileName = "Player_Config")]
public class Player_Config : ConfigBase
{
    [LabelText("移动速度")] public float MoveSpeed = 2f;
    [LabelText("生命值")] public float HP = 100f;
    [LabelText("最大子弹数量")] public float MaxBulletNum = 30f;
    [LabelText("射击间隔")] public float ShootInterval = 0.02f;
    [LabelText("子弹移动力量")] public float BulletMovePower = 1000f;
    [LabelText("攻击力")] public float Attack = 20f;
}