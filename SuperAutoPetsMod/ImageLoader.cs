using System;
using UnhollowerBaseLib;
using UnityEngine;

public abstract class ImageLoader
{
    internal delegate bool LoadImageDelegate(IntPtr tex, IntPtr data, bool markNonReadable);
    internal static LoadImageDelegate LoadImageIL2CPP;

    public static bool LoadImage(Texture2D tex, byte[] data, bool markNonReadable)
    {
        if (LoadImageIL2CPP == null)
        {
            LoadImageIL2CPP = IL2CPP.ResolveICall<LoadImageDelegate>("UnityEngine.ImageConversion::LoadImage");
        }

        var il2cppArray = (Il2CppStructArray<byte>)data;

        return LoadImageIL2CPP.Invoke(tex.Pointer, il2cppArray.Pointer, markNonReadable);
    }
}
