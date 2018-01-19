using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCtrl : MonoBehaviour {

	//스파크  파티클  프리팹 연결할 변수
	public GameObject sparkEffect;

    void OnWall(object[] _params)
    {
        Debug.Log(string.Format("Hit ray {0}", _params[0]));
        //스파클 파티클을 동적으로 생성

        GameObject spark = (GameObject)Instantiate(sparkEffect, (Vector3)_params[0], Quaternion.identity);
        //ParticleSystem 컴포넌트의 수행시간(duration)이 지난 후  삭제 거리
        Destroy(spark, spark.GetComponent<ParticleSystem>().duration + 0.2f);
    }
}
