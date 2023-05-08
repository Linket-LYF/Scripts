using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    //对象列表
    public List<GameObject> poolPrefabs;
    //池子列表，每一个池子都是stack
    private List<ObjectPool<GameObject>> poolEffectList = new List<ObjectPool<GameObject>>();
    private Queue<GameObject> soundQueue = new Queue<GameObject>();

    private void OnEnable()
    {
        EventHandler.InitSoundEffect += InitSoundEffect;
    }

    private void OnDisable()
    {
        EventHandler.InitSoundEffect -= InitSoundEffect;
    }



    private void Start()
    {
        CreatePool();
    }

    /// <summary>
    /// 生成对象池
    /// </summary>
    private void CreatePool()
    {
        foreach (GameObject item in poolPrefabs)
        {
            Transform parent = new GameObject(item.name).transform;
            parent.SetParent(transform);
            //e代表对象池中的每一个物体
            var newPool = new ObjectPool<GameObject>(
                //创建
                () => Instantiate(item, parent),
                //拿出
                e => { e.SetActive(true); },
                //释放
                e => { e.SetActive(false); },
                //销毁
                e => { Destroy(e); }
            );

            poolEffectList.Add(newPool);
        }
    }

    // private void OnParticleEffectEvent(ParticleEffectType effectType, Vector3 pos)
    // {
    //     //WORKFLOW:根据特效补全
    //     ObjectPool<GameObject> objPool = effectType switch
    //     {
    //         ParticleEffectType.LeavesFalling01 => poolEffectList[0],
    //         ParticleEffectType.LeavesFalling02 => poolEffectList[1],
    //         _ => null,
    //     };

    //     GameObject obj = objPool.Get();
    //     obj.transform.position = pos;
    //     StartCoroutine(ReleaseRoutine(objPool, obj));
    // }

    // private IEnumerator ReleaseRoutine(ObjectPool<GameObject> pool, GameObject obj)
    // {
    //     yield return new WaitForSeconds(1.5f);
    //     pool.Release(obj);
    // }


    //取出之后初始化对象池里面的对象
    private void InitSoundEffect(SoundDetails soundDetails)
    {
        GameObject obj = GetPoolObj();
        obj.GetComponent<Sound>().SetSound(soundDetails);
        obj.SetActive(true);
        StartCoroutine(DisablidSound(obj, soundDetails.soundClip.length));

    }
    //创建声音对象池
    private void CreateSoundPool()
    {
        Transform parent = new GameObject(poolPrefabs[0].name).transform;
        parent.SetParent(transform);
        for (int i = 0; i < 20; i++)
        {
            GameObject newobj = Instantiate(poolPrefabs[0], parent);
            newobj.SetActive(false);
            soundQueue.Enqueue(newobj);
        }
    }
    //取出对象
    private GameObject GetPoolObj()
    {
        if (soundQueue.Count < 2)
        {
            CreateSoundPool();

        }
        return soundQueue.Dequeue();
    }
    //释放对象
    private IEnumerator DisablidSound(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
        soundQueue.Enqueue(obj);
    }


}