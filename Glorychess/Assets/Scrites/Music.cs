using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioClip _audio;//声音片段
    public AudioSource abs;
    public float ss;//控制声音大小
    private void Start()
    {
        playmusic(_audio);
        abs.volume = ss;//设置音量
    }
    public void playmusic(AudioClip abc)
    {
        abs.clip = abc;
    }
    public void kaishi()
    {
        abs.PlayOneShot(_audio);//播放一次
    }
}
