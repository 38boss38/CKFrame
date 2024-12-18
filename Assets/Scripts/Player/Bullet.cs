using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CKFrame;

[Pool]
public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private new Collider collider;
    private int attack;

    public void Init(Vector3 dir,float movePower,int attack)
    {
        rb.AddForce(dir.normalized * movePower);
        trailRenderer.emitting = true;
        collider.enabled = true;
        this.attack = attack;
        
        Invoke("DestoryOnInit",8f);
    }

    private void OnTriggerEnter(Collider other)
    {
        CancelInvoke("DestoryOnInit");
        StartCoroutine(Destroy());
        // TODO:攻击AI
        if (other.gameObject.CompareTag("Monster"))
        {
            other.gameObject.GetComponent<Monster_Controller>().GetHit(attack);
        }
        
    }

    private void DestoryOnInit()
    {
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        collider.enabled = false;
        rb.velocity = Vector3.zero;
        trailRenderer.emitting = false;
        yield return new WaitForSeconds(2f);
        //销毁自身
        DoDestroy();
    }
    private void DoDestroy()
    {
        this.CKGameObjectPushPool();
    }
}
