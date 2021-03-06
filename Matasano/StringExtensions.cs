﻿using System;
using System.Collections.Generic;

namespace Matasano
{
    public static class StringExtensions
    {
        public static IEnumerable<string> SplitByLength(this string s, int length)
        {
            for (int i = 0; i < s.Length; i += length)
            {
                if (i + length <= s.Length)
                {
                    yield return s.Substring(i, length);
                }
                else
                {
                    yield return s.Substring(i);
                }
            }
        }

        public static byte[] ToByteArray(this string s)
        {
            byte[] bytes = new byte[s.Length];
            for (int i = 0; i < s.Length; i++)
            {
                bytes[i] = Convert.ToByte(s[i]);
            }
            return bytes;
        }
    }
}
