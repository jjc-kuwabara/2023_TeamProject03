using UnityEngine;

namespace SaveData_Settings
{
    //音量設定のデータをロード
    public class Load : MonoBehaviour
    {
        public static int bgm, se, voi;          //Audio用

        //音量設定のロード
        public static void Audio()
        {
            Debug.Log("音量設定をロードしました");

            bgm = PlayerPrefs.GetInt("Vol_BG", 8);
            se = PlayerPrefs.GetInt("Vol_SE", 8);
            voi = PlayerPrefs.GetInt("Vol_Voice", 8);

            SoundManager.Instance.VolumeChange_Start(bgm, se, voi);
        }
    }

    //保存データをセーブ
    public class Save : MonoBehaviour
    {
        //音量設定の保存
        public static void Audio(int b, int s, int v)
        {
            Debug.Log("音量設定をセーブしました");

            PlayerPrefs.SetInt("Vol_BG", b);
            PlayerPrefs.SetInt("Vol_SE", s);
            PlayerPrefs.SetInt("Vol_Voice", v);
            PlayerPrefs.Save();

            Load.bgm = b;
            Load.se = s;
            Load.voi = v;
        }
    }

    //保存データの初期化
    public class InitializeSaveData : MonoBehaviour
    {
        public static void All()
        {
            Debug.Log("全データを初期化しました");
            PlayerPrefs.DeleteAll();

            Load.Audio();       //初期状態の音量設定をロード
        }
    }
}