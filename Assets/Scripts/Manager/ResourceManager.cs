using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ResourceManager : MonoBehaviour
{
    public static RuntimeAnimatorController Get_Animator(string _path)
    {
        return Resources.Load<RuntimeAnimatorController>("Animators/" + _path);
    }

    public static Sprite Get_Sprite(string _path)
    {
        return Resources.Load<Sprite>(_path);
    }
    public static Sprite GetSprite_to_atlas(string _path, string _name)
    {
        return Resources.Load<SpriteAtlas>($"Atlas/{_path}").GetSprite(_name);
    }
    public static void UnloadAsset()
    {
        Resources.UnloadUnusedAssets();
    }
}
