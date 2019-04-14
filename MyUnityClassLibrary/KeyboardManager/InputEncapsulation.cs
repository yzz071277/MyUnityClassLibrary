using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardManager
{
    public static class InputEncapsulation
    {
        //存放按键和轴的容器
        private static Dictionary<string, KeyCode> keyCodeDic = new Dictionary<string, KeyCode>();
        private static Dictionary<string, AxisObject> axisDic = new Dictionary<string, AxisObject>();

        //轴需要用到的数据
        private class AxisObject
        {
            public string positiveButton;
            public string negativeButton;
            public float axisValue;

            public AxisObject(string positiveButton, string negativeButton)
            {
                this.positiveButton = positiveButton;
                this.negativeButton = negativeButton;
                this.axisValue = 0;
            }
        }

        //添加按键
        public static void AddKeyCode(string name, KeyCode keyCode)
        {
            if (!keyCodeDic.ContainsKey(name))
            {
                keyCodeDic.Add(name, keyCode);
            }

        }

        //判断是否按住按键
        public static bool GetKey(string buttonName)
        {
            if (keyCodeDic.ContainsKey(buttonName))
            {
                return Input.GetKey(keyCodeDic[buttonName]);
            }
            return false;
        }

        //判断是否按下按键
        public static bool GetKeyDown(string buttonName)
        {
            if (keyCodeDic.ContainsKey(buttonName))
            {
                return Input.GetKeyDown(keyCodeDic[buttonName]);
            }
            return false;
        }

        //判断按键是否抬起
        public static bool GetKeyUp(string buttonName)
        {
            if (keyCodeDic.ContainsKey(buttonName))
            {
                return Input.GetKeyUp(keyCodeDic[buttonName]);
            }
            return false;
        }

        //添加自定义虚拟轴
        public static void AddAxis(string axisName, string positiveButtonName, string negativeButtonName)
        {
            if ((keyCodeDic.ContainsKey(positiveButtonName) && keyCodeDic.ContainsKey(negativeButtonName)) && !axisDic.ContainsKey(axisName))
            {
                axisDic[axisName] = new AxisObject(positiveButtonName, negativeButtonName);
            }
        }

        //获取轴值
        public static float GetAxis(string axisName, float maxDelta = 0.04f)
        {
            if (Input.GetKey(keyCodeDic[axisDic[axisName].positiveButton]))
            {
                axisDic[axisName].axisValue = Mathf.MoveTowards(axisDic[axisName].axisValue, 1, maxDelta);
            }
            else if (Input.GetKey(keyCodeDic[axisDic[axisName].negativeButton]))
            {
                axisDic[axisName].axisValue = Mathf.MoveTowards(axisDic[axisName].axisValue, -1, maxDelta);
            }
            else
            {
                axisDic[axisName].axisValue = Mathf.MoveTowards(axisDic[axisName].axisValue, 0, maxDelta);
            }
            return axisDic[axisName].axisValue;
        }

        //获取Raw轴值
        public static float GetAxisRaw(string axisName, float maxDelta = 0.04f)
        {
            if (Input.GetKey(keyCodeDic[axisDic[axisName].positiveButton]))
            {
                return 1;
            }
            else if (Input.GetKey(keyCodeDic[axisDic[axisName].negativeButton]))
            {
                return -1;
            }
            return 0;
        }

        //检查并返回重复按键的名字
        public static string CheckRepeatButton(KeyCode key)
        {
            foreach (var item in keyCodeDic)
            {
                if (item.Value == key)
                {
                    return item.Key;
                }
            }
            return string.Empty;
        }

        //更换按键并自动将重复键置空
        public static void ChangeButton(string buttonName, KeyCode key)
        {
            if (keyCodeDic.ContainsKey(buttonName))
            {
                if (CheckRepeatButton(key) != string.Empty)
                {
                    keyCodeDic[CheckRepeatButton(key)] = KeyCode.None;
                }
                keyCodeDic[buttonName] = key;
            }
        }

        //更换按键并手动指定重复按键
        public static void ChangeButton(string buttonName, KeyCode key, string RepeatButton)
        {
            if (keyCodeDic.ContainsKey(buttonName) && keyCodeDic.ContainsKey(RepeatButton))
            {
                keyCodeDic[RepeatButton] = KeyCode.None;
                keyCodeDic[buttonName] = key;
            }
        }

        //获取具体按键
        public static KeyCode GetKeyCode(string keyName)
        {
            return keyCodeDic[keyName];
        }

    }

}

