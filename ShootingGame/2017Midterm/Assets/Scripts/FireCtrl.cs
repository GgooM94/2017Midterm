﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//반드시 필요한 컴포넌트를 명시해 해당 컴포넌트가 삭제되는 것을 방지하는 Attribute
[RequireComponent(typeof(AudioSource))]

public class FireCtrl : MonoBehaviour {
	//총알 프리팹
	public GameObject bullet;
	//총알 발사 좌표
	public Transform firepos;
	//총알 발사 사운드
	public AudioClip fireSFx;
	//AudioSource 컴포넌트를 추출한 후 변수에 할당
	private AudioSource source = null;
    
	//MuzzleFlash 의 MeshRenderer 컴포넌트 연결 변수
	public MeshRenderer muzzleFlash;

    public GameObject gameOver;
    void Start(){
		//AudioSource 컴포넌트를 추출한 후 변수에 할당
		source = GetComponent<AudioSource>();

		muzzleFlash.enabled = false;

	}
		

	// Update is called once per frame
	void Update () {

        if (gameOver.active == true)
        {
            return;
        }


        //Ray를 시각적으로 표시하기 위해 사용
        Debug.DrawRay(firepos.position, firepos.forward*20.0f, Color.green);

        //마우스 왼쪽 버튼을 클릭했때 Fire 함수 호출
        //GetMouseButton 0: 왼쪽 버튼 , 1: 오른쪽 버튼, 2:가운데 

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
            //Ray에 맞은 게임오브젝트의 정보를 받아올 변수
            RaycastHit hit;
            if (Physics.Raycast(firepos.position, firepos.forward, out hit, 20.0f))
            {
                //Ray에 맞은 게임오브젝트의 Tag 값을 비교해 몬스터 여부 체크
                if (hit.collider.tag == "MONSTER")
                {
                    //SendMessage를 이용해 전달한 인자를 배열에 담음
                    object[] _params = new object[2];
                    _params[0] = hit.point; //Ray에 맞은 정확한 위치값(Vector3)
                    _params[1] = 20;        //몬스터에 입힐 데미지값

                    //몬스터에 데미지 입히는 함수 호출
                    hit.collider.gameObject.SendMessage("OnDamage", _params, SendMessageOptions.DontRequireReceiver);
                }  
                else if(hit.collider.tag == "OBJECT")
                {
                    object[] _params = new object[1];
                    _params[0] = hit.point; //Ray에 맞은 정확한 위치값(Vector3)
                    hit.collider.gameObject.SendMessage("OnWall", _params, SendMessageOptions.DontRequireReceiver);
                }

            }
        }
		
    }
 
    void Fire(){
		//사운드 발생 함수
		GameMgr.instance.PlaySfx(firepos.position, fireSFx);

		//잠시 기다리는 루틴을 위해 코루틴 함수로 호출
		StartCoroutine(this.ShowMuzzleFlash());
	}

	//MuzzleFlash 활성/ 비활성화를 짧은 시간 동안 반복
	IEnumerator ShowMuzzleFlash()
	{
		//MuzzleFlash Scale을 불규칙하게 변경
		float scale = Random.Range(1.0f, 2.0f);
		muzzleFlash.transform.localScale = Vector3.one * scale;

		//MuzzleFlash를 Z축을 기준으로 불규칙하게  회전시킴
		Quaternion rot = Quaternion.Euler(0, 0, Random.Range(0,360));
		muzzleFlash.transform.localRotation = rot;

		//활성화해서 보이게 함
		muzzleFlash.enabled = true;

		//불규칙적인 시간 동안 Delay한 다음 MeshRenderer를 비활성화
		yield return new WaitForSeconds(Random.Range(0.05f,0.3f));

		//비활성화해서 보이지 않게 함
		muzzleFlash.enabled = false;
	
	}
}
	