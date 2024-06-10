﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Soft.Core
{
    public class DictionaryCacheHelper
    {
        //缓存容器 
        private static Dictionary<string, object> CacheDictionary = new Dictionary<string, object>();
        /// <summary>
        /// 添加缓存
        /// </summary>
        public static void Add(string key, object value)
        {
            CacheDictionary.Add(key, value);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        public static T Get<T>(string key)
        {
            return (T)CacheDictionary[key];
        }

        /// <summary>
        /// 判断缓存是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Exsits(string key)
        {
            return CacheDictionary.ContainsKey(key);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Delete(string key)
        {
            return CacheDictionary.Remove(key);
        }
        /// <summary>
        /// 缓存获取方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存字典容器对应key</param>
        /// <param name="func">委托方法 传入操作对象</param>
        /// <returns></returns>
        public static T GetCache<T>(string key, Func<T> func)
        {
            T t = default(T);
            if (DictionaryCacheHelper.Exsits(key))
            {
                //缓存存在，直接获取原数据
                t = DictionaryCacheHelper.Get<T>(key);
            }
            else
            {
                //缓存不存在，去生成缓存，并加入容器
                t = func.Invoke();
                DictionaryCacheHelper.Add(key, t);
            }
            return t;
        }

        /// <summary>
        /// 缓存获取方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存字典容器对应key</param>
        /// <param name="func">委托方法 传入操作对象</param>
        /// <returns></returns>
        public static T UpdateCache<T>(string key, Func<T> func)
        {
            T t = default(T);
            if (DictionaryCacheHelper.Exsits(key))
            {
                //缓存存在，直接获取原数据
                if (DictionaryCacheHelper.Delete(key))
                {
                    t = func.Invoke();
                    DictionaryCacheHelper.Add(key, t);
                }
            }
            else
            {
                //缓存不存在，去生成缓存，并加入容器
                t = func.Invoke();
                DictionaryCacheHelper.Add(key, t);
            }
            return t;
        }
    }
}
