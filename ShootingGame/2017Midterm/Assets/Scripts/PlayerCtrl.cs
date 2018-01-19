using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;	//UI항목에 접근하기 위해 반드시 추가
using UnityEngine.SceneManagement;

[System.Serializable]
public class Anim{
	public AnimationClip idle;
	public AnimationClip runForward;
	public AnimationClip runBackward;
	public AnimationClip runRight;
	public AnimationClip runLeft;

}

public class PlayerCtrl : MonoBehaviour {
	private float h = 0.0f;
	private float v = 0.0f;

    public GameObject gameOver;

	//접근해야 하는 컴포넌트는 반드시 변수에 할당한 후 사용
	private Transform tr;
	//Player 이동 속도 변수
	public float moveSpeed = 10.0f;
	//회전 속도 변수
	public float rotSpeed =200.0f;

	//인스펙터뷰에 표시할 애니메이션 클래스 변수
	public Anim anim;
	//아래에 있는 3D 모델의 Animation 컴포넌트에 접근하기 위한 변수
	public Animation _animation;

	//Player의 생명 변수
	public int hp = 100;

	//Player의 생명 변수
	private int initHp;
	//Player의 Health Bar 이미지
	public Image imgHpBar;

	//게임 매니저에 접근하기 위한 변수
	private GameMgr gameMgr;

	public delegate void PlayerDieHandler ();
	public static event PlayerDieHandler OnPlayerDie;

    



	void Start () {
		//생명 초깃값 설정
		initHp =hp;

		//스크립트 처음에 Transform 컴포넌트 할당
		tr = GetComponent<Transform> ();
		//GameMgr 스크립트 할당
		gameMgr = GameObject.Find("GameManager").GetComponent<GameMgr>();

		_animation = GetComponentInChildren<Animation> ();
		_animation.clip = anim.idle;
		_animation.Play ();


    }
	
	// Update is called once per frame
	void Update () {

     
       if(gameOver.active == true)
       {
          if (Input.GetButtonDown("Jump"))
                {
                SceneManager.LoadScene("scMain");

            }
        return;                 
       }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Esc");
            Application.Quit();
        }
        
		h = Input.GetAxis ("Horizontal");
		v = Input.GetAxis ("Vertical");

		//전후좌우 이동 방향 벡터 계산
		Vector3 moveDir = (Vector3.forward *v) + (Vector3.right * h);
		//Translate(이동방향 * 속도 * 변위값 * Time.deltaTime, 기준좌표)
	
		tr.Translate (moveDir.normalized * moveSpeed * Time.deltaTime, Space.Self);
		tr.Rotate (Vector3.up * Time.deltaTime * rotSpeed * Input.GetAxis ("Mouse X"));

		//키보드 입력값을 기준으로 동작할 애니메이션 수행
		//CrossFade(변경할 animation clip name, change time)
		if (v >= 0.1f) {
			//전진 애니메이션
			_animation.CrossFade (anim.runForward.name, 0.3f);	
		} else if (v <= -0.1f) {
			//후진 애니메이션
			_animation.CrossFade (anim.runBackward.name, 0.3f);
		} else if (h >= 0.1f) {
			//오른쪽 이동 애니메이션/
			_animation.CrossFade (anim.runRight.name, 0.3f);
		} else if (h <= -0.1f) {
			//왼쪽 이동 애니메이션
			_animation.CrossFade(anim.runLeft.name, 0.3f);
		}else{
			//기본 상태
			_animation.CrossFade(anim.idle.name, 0.3f);
		
		}
	}

	//충돌한 Collider의 IsTrigger 옵션이 체크됐을때 발생
	void OnTriggerEnter(Collider coll){

		//충돌한 Collider가 몬스터의 PUNCH이면 Player의 HP차감
		if (coll.gameObject.tag == "PUNCH") {
			hp -= 10;

			//Image UI 항목의 fillAmount 속성을 조절해 생명 게이지 값 조절
			imgHpBar.fillAmount = (float)hp / (float)initHp;


			Debug.Log("Player HP =" + hp.ToString());

			//Player의 생명이  0이하이면 사망 처리
			if (hp <= 0) {
				PlayerDie ();
			}
		}
        else if(coll.gameObject.tag == "MonsterFire")
        {
            hp -= 20;
            //Image UI 항목의 fillAmount 속성을 조절해 생명 게이지 값 조절
            imgHpBar.fillAmount = (float)hp / (float)initHp;


            Debug.Log("Player HP =" + hp.ToString());

            //Player의 생명이  0이하이면 사망 처리
            if (hp <= 0)
            {
                PlayerDie();
            }
        }

	}

	//Player의 사망 처리 루틴
	void PlayerDie(){

        int sw = Screen.width;
        int sh = Screen.height;

        gameOver.SetActive(true);


		Debug.Log ("Player Die");
		OnPlayerDie ();

		//gameMgr의 싱글턴 인스턴스를 접근해 isGameOver 변숫값을 변경
		GameMgr.instance.isGameOver = true;

    }



}
