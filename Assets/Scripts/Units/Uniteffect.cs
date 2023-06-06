using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uniteffect : MonoBehaviour
{
    [SerializeField] ParticleSystem attackspeed_effect;
    [SerializeField] ParticleSystem zombiesynergy_effect;

    public void SynergyEffectPlay()
    {
        attackspeed_effect.Play();
    }

    public void ZombieEffectPlay()
    {
        zombiesynergy_effect.Play();
    }
}
