using System.Collections;
//UI 컴포넌트에 접근하기 위해 추가한 네임스페이스
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : MonoBehaviour {

	//Text UI 항목 연결을 위한 변수
	public Text txtScore;
    public Text txtTopScore;

    //누적 점수를 기록하기 위한 변수
    private int nowScore = 0;
    private int topScore = 0;
    

    void Start () {
        //처음 실행 후 저장된 스코어 정보 로드
        topScore = PlayerPrefs.GetInt ("TOP_SCORE", 0);

        DispScore (0);	
     
	}
	
	//점수 누적 및 화면 표시
	public void DispScore(int score){
        nowScore += score;

		txtScore.text = "SCORE <color=0000000>" + nowScore.ToString () + "</color>";
        txtTopScore.text = "TOP <color=0000000>" + topScore.ToString() + "</color>";


        //획득한 점수가 신기록보다 높을경우 신기록 갱신
        if(nowScore >= topScore)
         PlayerPrefs.SetInt("TOP_SCORE", nowScore);


    }

}
