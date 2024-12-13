using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CKFrame;

public enum PlayerState
{
    Normal,
    ReLoad,
    GetHit,
    Die
}
public class Player_Controller : SingletonMono<Player_Controller>
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform firePoint;
    private PlayerState playerState;

    #region 参数

    private float moveSpeed;
    private int currBulletNum;
    private int maxbulletNum;
    private float shootIntervel;
    private float bulletMovePower;
    private int attack;
    private bool canShoot = true;

    #endregion
    private int groundLayerMask;

    public PlayerState PlayerState
    {
        get => playerState;
        set
        {
            playerState = value;
            switch (playerState)
            {
                case PlayerState.Normal:
                    break;
                case PlayerState.ReLoad:
                    StartCoroutine(DoReload());
                    break;
                case PlayerState.GetHit:
                    break;
                case PlayerState.Die:
                    break;
            }
        }
    }

    public void Init(Player_Config config)
    {
        moveSpeed = config.MoveSpeed;
        maxbulletNum = config.MaxBulletNum;
        currBulletNum = maxbulletNum;
        shootIntervel = config.ShootInterval;
        bulletMovePower = config.BulletMovePower;
        attack = config.Attack;
        
        groundLayerMask = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        StateOnUpdate();
    }

    private void StateOnUpdate()
    {
        switch (playerState)
        {
            case PlayerState.Normal:
                Move();
                Shoot();
                if (currBulletNum < maxbulletNum && Input.GetKeyDown(KeyCode.R))
                {
                    PlayerState = PlayerState.ReLoad;
                }
                break;
            case PlayerState.ReLoad:
                Move();
                break;
            case PlayerState.GetHit:
                break;
            case PlayerState.Die:
                break;
        }
    }

    private void Move()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        Vector3 moveDir = new Vector3(h, -5, v);
        characterController.Move(moveDir * moveSpeed * Time.deltaTime);
        
        Ray ray = Camera_Controller.Instance.camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo,1000,groundLayerMask))
        {
            if (hitInfo.point.z < transform.position.z)
            {
                v *= -1;
                h *= -1;
            }
            Vector3 dir = new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z) - transform.position;
            Quaternion rotation = Quaternion.FromToRotation(Vector3.forward, dir);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, rotation, Time.deltaTime * 20f);
        }

        animator.SetFloat("MoveX", h);
        animator.SetFloat("MoveY", v);
    }

    private void Shoot()
    {
        if (canShoot && Input.GetMouseButton(0))
        {
            StartCoroutine(DoShoot());
            Debug.Log("mouse");
        }
        else
        {
            animator.SetBool("Shoot",false);
        }
    }

    private IEnumerator DoShoot()
    {
        currBulletNum--;
        // 修改UI
        EventManager.EventTrigger<int,int>("UpdateBullet", currBulletNum,maxbulletNum);
        animator.SetBool("Shoot",true);
        canShoot = false;
        AudioManager.Instance.PlayOnShot("Audio/Shoot/laser_01", transform.position);
        // 生成子弹
        Bullet bullet = ResManager.Load<Bullet>("Bullet");
        bullet.transform.position = firePoint.position;
        bullet.Init(firePoint.forward,bulletMovePower,attack);
        
        yield return new WaitForSeconds(shootIntervel);
        canShoot = true;
        //子弹打完，需要换弹
        if (currBulletNum == 0)
        {
            PlayerState = PlayerState.ReLoad;
        }
    }

    private IEnumerator DoReload()
    {
        animator.SetBool("Reload",true);
        AudioManager.Instance.PlayOnShot("Audio/Shoot/ReLoad", this);
        yield return new WaitForSeconds(1.9f);
        animator.SetBool("Reload",false);
        PlayerState = PlayerState.Normal;
        currBulletNum = maxbulletNum;
        EventManager.EventTrigger<int,int>("UpdateBullet", currBulletNum,maxbulletNum);
    }
}
