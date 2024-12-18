using UnityEngine;

/*
 ���͵���
 */
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    //����ʵ��
    private static T instance;

    //��װ
    public static T Instance
    {
        get { return instance; }
    }

    //�жϵ�ǰ�����Ƿ�������
    public static bool IsInitialized
    {
        get { return instance != null; }
    }

    //���󷽷� ����д
    protected virtual void Awake()
    {
        if (instance != null) //�Ѿ���ʵ��
            Destroy(gameObject);
        else //û��ʵ��
            instance = (T)this;
    }

    //�������ں�����������ʱ����
    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

}
