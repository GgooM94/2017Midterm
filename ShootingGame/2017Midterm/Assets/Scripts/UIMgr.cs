using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Scene 병합
using UnityEngine.SceneManagement;

public class UIMgr : MonoBehaviour {

    public void OnClickStartBtn(){
		Debug.Log ("Click Button");
	//	SceneManager.LoadScene ("scMid", LoadSceneMode.Single);
        SceneManager.LoadScene("scMid");

        //LoadSceneMode.Single :  기존에 로드된 씬을 모두 삭제한 후 새로운 씬을 로드한다.
        //LoadSceneMode.Addictive : 기존의 씬을 삭제하지 않고 추가해서 새로운 씬을 로드한다.

    }


    public void OnClickExitBtn()
    {
        Application.Quit();
        Debug.Log("asd");
    }
}
