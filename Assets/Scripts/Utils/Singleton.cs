using UnityEngine;

/*
 泛型单例
 */
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    //单例实例
    private static T instance;

    //封装
    public static T Instance
    {
        get { return instance; }
    }

    //判断当前单例是否已生成
    public static bool IsInitialized
    {
        get { return instance != null; }
    }

    //抽象方法 可重写
    protected virtual void Awake()
    {
        if (instance != null) //已经有实例
            Destroy(gameObject);
        else //没有实例
            instance = (T)this;
    }

    //生命周期函数，当销毁时调用
    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

}
