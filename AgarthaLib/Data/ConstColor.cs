using System;
using UnityEngine;

namespace AgarthaLib.Data
{
    [Serializable] public enum ConstColor
    {
        red,
        white,
        black,
        blue,
        clear,
        cyan,
        gray,
        green,
        grey,
        magenta,
        yellow,
    }

    public static class ConstColorExtensions
    {
        public static Color Resolve(this ConstColor cc)
        {
            return cc switch
            {
                ConstColor.red => Color.red,
                ConstColor.white => Color.white,
                ConstColor.black => Color.black,
                ConstColor.blue => Color.blue,
                ConstColor.clear => Color.clear,
                ConstColor.cyan => Color.cyan,
                ConstColor.gray => Color.gray,
                ConstColor.green => Color.green,
                ConstColor.grey => Color.grey,
                ConstColor.magenta => Color.magenta,
                ConstColor.yellow => Color.yellow,
                _ => default,
            };
        }
    }
}
