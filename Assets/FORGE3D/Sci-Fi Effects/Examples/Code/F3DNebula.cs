/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using System.Collections;

public class F3DNebula : MonoBehaviour {

   

	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {

        transform.position -= Vector3.forward * Time.deltaTime * 1000;

        if (transform.position.z < -2150)
        {
            Vector3 newPos = transform.position;
            newPos.z = 2150;
            transform.position = newPos;
            transform.rotation = Random.rotation;
            transform.localScale = new Vector3(1, 1, 1) * Random.Range(200, 800);
        }
            
	}
}