using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;


//사용 방법
// using static HappyRyuMa.GameMaker;


namespace HappyRyuMa
{
    public class GameMaker : MonoBehaviour
    {


        //오브젝트를 봐라본 각도Z 구하는 함수
        public static float GetAngleZ(Vector2 Target, Vector2 MyVec)
        {
            float AngleZ = 0;
            Vector2 vec = Target - MyVec;
            vec.Normalize();
            AngleZ = Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
            //이미지의 0도 중심이 아래일 경우 +90
            //이미지의 0도 중심이 위일 경우 -90
            AngleZ -= 90;
            if (AngleZ < 0)
                AngleZ += 360;

            return AngleZ;

        }

        //테스트 할 때만 사용하는 디버그
        [Conditional("UnityEditor")]
        public static void DebugLog(string _Debug)
        {
            UnityEngine.Debug.Log(_Debug);

        }



        // 0~100까지 랜덤한 값을 리턴
        public static float Random100()
        {
            float Result = Random.Range(0.0f, 100.0f);
            return Result;
        }


        public static bool CheckInternet()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                return false;
            }
            return true;
        }




    }
}