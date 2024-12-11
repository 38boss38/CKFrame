using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CKFrame;

[Serializable]
public class UserSetting
{
    public float GlobalVolume;
    public float BGVolume;
    public float EffectVolume;
    public LanguageType LanguageType;

    public UserSetting(float globalVolume, float bgVolume, float effectVolume, LanguageType languageType)
    {
        GlobalVolume = globalVolume;
        BGVolume = bgVolume;
        EffectVolume = effectVolume;
        LanguageType = languageType;
    }
}
