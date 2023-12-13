using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ryu
{
    [System.Serializable]
    public class GameObjectPoolData
    {
        public int PrevCount = 10;
        public GameObject PrevObject = null;

    }


    public class ObjectPoolManager : MonoBehaviour
    {
        static ObjectPoolManager m_instance = null;
        public static ObjectPoolManager Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindObjectOfType<ObjectPoolManager>();
                    if (m_instance == null)
                    {
                        GameObject obj = new GameObject("ObjectPoolManager", typeof(ObjectPoolManager));
                        m_instance = obj.GetComponent<ObjectPoolManager>();
                    }
                }

                return m_instance;
            }
        }



        public List<GameObjectPoolData> m_PrevObjectList = new List<GameObjectPoolData>();
        protected void Init()
        {
            foreach (var item in m_PrevObjectList)
            {
                Stack<GameObject> tempstack = new Stack<GameObject>();
                m_PoolManager.Add(item.PrevObject, tempstack);

                for (int i = 0; i < item.PrevCount; ++i)
                {
                    GameObject copyobj = GameObject.Instantiate(item.PrevObject);
                    m_PoolManager.Add(copyobj, tempstack);
                }
            }
        }


        private void Awake()
        {
            Init();
        }

        protected Dictionary<GameObject, Stack<GameObject>> m_PoolManager = new Dictionary<GameObject, Stack<GameObject>>();

        public T CreateObject<T>(T p_obj, Transform p_parent = null) where T : Component
        {
            GameObject obj = CreateObject(p_obj.gameObject);
            if (p_parent)
                obj.transform.SetParent(p_parent);
            return obj.GetComponent<T>();
        }

        public GameObject CreateObject(GameObject p_obj)
        {

            Stack<GameObject> tempstack = null;
            if (m_PoolManager.ContainsKey(p_obj))
            {
                tempstack = m_PoolManager[p_obj];
            }
            else
            {
                tempstack = new Stack<GameObject>();
                m_PoolManager.Add(p_obj, tempstack);
            }


            GameObject outobj = null;
            if (tempstack.Count <= 0)
            {
                outobj = GameObject.Instantiate(p_obj);
                m_PoolManager.Add(outobj, tempstack);
            }
            else
            {
                outobj = tempstack.Pop();
                outobj.SetActive(true);
            }

            return outobj;
        }

        public void DestroyObj(GameObject p_obj)
        {
            Stack<GameObject> statck;
            if (m_PoolManager.TryGetValue(p_obj, out statck))
            {
                statck.Push(p_obj);
                p_obj.SetActive(false);
            }
            else
            {
                Debug.LogError("풀에 없는데이터 지우기려고함 ");
            }
        }
    }
}