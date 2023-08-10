using System;
using CrashUtils.WeaponManager.WeaponSetup;
using ULTRAKILL.Cheats;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000257 RID: 599

namespace WeezerGunUtils
{

    public class Weezerrev : MonoBehaviour
    {




        // Token: 0x06000D76 RID: 3446 RVA: 0x0007A9FC File Offset: 0x00078BFC
        private void Start()
        {
            this.targeter = MonoSingleton<CameraFrustumTargeter>.Instance;
            this.inman = MonoSingleton<InputManager>.Instance;
            this.wid = base.GetComponent<WeaponIdentifier>();
            this.gunReady = false;
            this.cam = MonoSingleton<CameraController>.Instance.GetComponent<Camera>();
            this.camObj = this.cam.gameObject;
            this.cc = MonoSingleton<CameraController>.Instance;
            this.nmov = MonoSingleton<NewMovement>.Instance;
            this.shootCharge = 0f;
            this.pierceShotCharge = 0f;
            this.pierceCharge = 100f;
            this.pierceReady = false;
            this.shootReady = false;
            this.gunAud = base.GetComponent<AudioSource>();
            this.kickBackPos = base.transform.parent.GetChild(0);
            this.superGunAud = this.kickBackPos.GetComponent<AudioSource>();
            if (this.gunVariation == 0)
            {
                this.screenAud = this.screenMR.gameObject.GetComponent<AudioSource>();
            }
            else
            {
                this.screenAud = base.GetComponentInChildren<Canvas>().GetComponent<AudioSource>();
            }
            if (chargeEffect != null)
            {
                for (int i = 0; i < chargeEffect.Length; i++)
                {
                    this.ceaud = chargeEffect[i].GetComponent<AudioSource>();
                    this.celight[i] = chargeEffect[i].GetComponent<Light>();
                }
            }
            if (this.gunVariation == 0)
            {
                this.screenAud.clip = this.chargingSound;
                this.screenAud.loop = true;
                this.screenAud.pitch = 1f;
                this.screenAud.volume = 0.25f;
                this.screenAud.Play();
            }
            this.cylinder = base.GetComponentInChildren<RevolverCylinder>();
            this.gc = base.GetComponentInParent<GunControl>();
            this.beamDirectionSetter = new GameObject();
            this.anim = base.GetComponentInChildren<Animator>();
            this.wc = MonoSingleton<WeaponCharges>.Instance;
            this.wpos = base.GetComponent<WeaponPos>();
            if (this.wid.delay != 0f && this.gunVariation == 0)
            {
                this.pierceCharge = this.wc.rev0charge;
            }
        }

        // Token: 0x06000D77 RID: 3447 RVA: 0x0007ABF0 File Offset: 0x00078DF0
        private void OnDisable()
        {
            if (this.wc == null)
            {
                this.wc = MonoSingleton<WeaponCharges>.Instance;
            }
            if (this.gunVariation == 0)
            {
                this.wc.rev0alt = this.altVersion;
                this.wc.rev0charge = this.pierceCharge;
            }
            this.pierceShotCharge = 0f;
            this.gunReady = false;
        }

        // Token: 0x06000D78 RID: 3448 RVA: 0x0007AC54 File Offset: 0x00078E54
        private void OnEnable()
        {
            if (this.wc == null)
            {
                this.wc = MonoSingleton<WeaponCharges>.Instance;
            }
            this.shootCharge = 100f;
            if (this.gunVariation == 0)
            {
                this.pierceCharge = this.wc.rev0charge;
            }
            else
            {
                this.pierceCharge = 100f;
                this.pierceReady = true;
                this.CheckCoinCharges();
            }
            this.wc.rev2alt = (this.gunVariation == 2 && this.altVersion);
            if (this.altVersion)
            {
                if (!this.anim)
                {
                    this.anim = base.GetComponentInChildren<Animator>();
                }
                if (this.wc.revaltpickupcharges[this.gunVariation] > 0f)
                {
                    this.anim.SetBool("SlowPickup", true);
                }
                else
                {
                    this.anim.SetBool("SlowPickup", false);
                }
            }
            this.gunReady = false;
        }

        // Token: 0x06000D79 RID: 3449 RVA: 0x0007AD3C File Offset: 0x00078F3C
        private void Update()
        {
            if (!this.shootReady)
            {
                if (this.shootCharge + 200f * Time.deltaTime < 100f)
                {
                    this.shootCharge += 200f * Time.deltaTime;
                }
                else
                {
                    this.shootCharge = 100f;
                    this.shootReady = true;
                }
            }
            if (!this.pierceReady)
            {
                if (this.gunVariation == 0)
                {
                    if (NoWeaponCooldown.NoCooldown)
                    {
                        this.pierceCharge = 100f;
                    }
                    float num = 1f;
                    if (this.altVersion)
                    {
                        num = 0.5f;
                    }
                    if (this.pierceCharge + 40f * Time.deltaTime < 100f)
                    {
                        this.pierceCharge += 40f * Time.deltaTime * num;
                    }
                    else
                    {
                        this.pierceCharge = 100f;
                        this.pierceReady = true;
                        this.screenAud.clip = this.chargedSound;
                        this.screenAud.loop = false;
                        this.screenAud.volume = 0.35f;
                        this.screenAud.pitch = UnityEngine.Random.Range(1f, 1.1f);
                        this.screenAud.Play();
                    }
                    if (this.cylinder.spinSpeed > 0f)
                    {
                        this.cylinder.spinSpeed = Mathf.MoveTowards(this.cylinder.spinSpeed, 0f, Time.deltaTime * 50f);
                    }
                    if (this.pierceCharge < 50f)
                    {
                        this.screenMR.material.SetTexture("_MainTex", this.batteryLow);
                        this.screenMR.material.color = Color.red;
                    }
                    else if (this.pierceCharge < 100f)
                    {
                        this.screenMR.material.SetTexture("_MainTex", this.batteryMid);
                        this.screenMR.material.color = Color.yellow;
                    }
                    else
                    {
                        this.screenMR.material.SetTexture("_MainTex", this.batteryFull);
                    }
                }
                else if (this.pierceCharge + 480f * Time.deltaTime < 100f)
                {
                    this.pierceCharge += 480f * Time.deltaTime;
                }
                else
                {
                    this.pierceCharge = 100f;
                    this.pierceReady = true;
                }
            }
            else if (this.gunVariation == 0)
            {
                if (this.pierceShotCharge != 0f)
                {
                    if (this.pierceShotCharge < 50f)
                    {
                        this.screenMR.material.SetTexture("_MainTex", this.batteryCharges[0]);
                    }
                    else if (this.pierceShotCharge < 100f)
                    {
                        this.screenMR.material.SetTexture("_MainTex", this.batteryCharges[1]);
                    }
                    else
                    {
                        this.screenMR.material.SetTexture("_MainTex", this.batteryCharges[2]);
                    }
                    base.transform.localPosition = new Vector3(this.wpos.currentDefault.x + this.pierceShotCharge / 250f * UnityEngine.Random.Range(-0.05f, 0.05f), this.wpos.currentDefault.y + this.pierceShotCharge / 250f * UnityEngine.Random.Range(-0.05f, 0.05f), this.wpos.currentDefault.z + this.pierceShotCharge / 250f * UnityEngine.Random.Range(-0.05f, 0.05f));
                    this.cylinder.spinSpeed = this.pierceShotCharge;
                }
                else
                {
                    if (this.screenMR.material.GetTexture("_MainTex") != this.batteryFull)
                    {
                        this.screenMR.material.SetTexture("_MainTex", this.batteryFull);
                    }
                    if (this.cylinder.spinSpeed != 0f)
                    {
                        this.cylinder.spinSpeed = 0f;
                    }
                }
            }
            if (this.gc.activated)
            {
                if (this.gunVariation != 1 && this.gunReady)
                {
                    float num2 = (float)((this.gunVariation == 0) ? 175 : 75);
                    if ((MonoSingleton<InputManager>.Instance.InputSource.Fire2.WasCanceledThisFrame || (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && !GameStateManager.Instance.PlayerInputLocked && MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed)) && this.shootReady && ((this.gunVariation == 0) ? (this.pierceShotCharge == 100f) : (this.pierceShotCharge >= 25f)))
                    {
                        if (!this.wid || this.wid.delay == 0f)
                        {
                            this.Shoot(2);
                        }
                        else
                        {
                            this.shootReady = false;
                            this.shootCharge = 0f;
                            base.Invoke("DelayedShoot2", this.wid.delay);
                        }
                    }
                    else if (!MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed && this.shootReady && !this.chargingPierce)
                    {
                        if (!this.wid || this.wid.delay == 0f)
                        {
                            this.Shoot(1);
                        }
                        else
                        {
                            this.shootReady = false;
                            this.shootCharge = 0f;
                            base.Invoke("DelayedShoot", this.wid.delay);
                        }
                    }
                    else if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.IsPressed && (this.gunVariation == 2 || this.shootReady) && ((this.gunVariation == 0) ? this.pierceReady : (this.coinCharge >= (float)(this.altVersion ? 300 : 100))))
                    {
                        if (!this.chargingPierce && !this.twirlRecovery)
                        {
                            this.latestTwirlRotation = 0f;
                        }
                        this.chargingPierce = true;
                        if (this.pierceShotCharge + num2 * Time.deltaTime < 100f)
                        {
                            this.pierceShotCharge += num2 * Time.deltaTime;
                        }
                        else
                        {
                            this.pierceShotCharge = 100f;
                        }
                    }
                    else
                    {
                        if (this.chargingPierce)
                        {
                            this.twirlRecovery = true;
                        }
                        this.chargingPierce = false;
                        if (this.pierceShotCharge - num2 * Time.deltaTime > 0f)
                        {
                            this.pierceShotCharge -= num2 * Time.deltaTime;
                        }
                        else
                        {
                            this.pierceShotCharge = 0f;
                        }
                    }
                }
                else if (this.gunVariation == 1)
                {
                    if (MonoSingleton<InputManager>.Instance.InputSource.Fire2.WasPerformedThisFrame && this.pierceReady && this.coinCharge >= 100f)
                    {
                        this.cc.StopShake();
                        if (!this.wid || this.wid.delay == 0f)
                        {
                            this.wc.rev1charge -= 100f;
                        }
                        if (!this.wid || this.wid.delay == 0f)
                        {
                            this.ThrowCoin();
                        }
                        else
                        {
                            base.Invoke("ThrowCoin", this.wid.delay);
                            this.pierceReady = false;
                            this.pierceCharge = 0f;
                        }
                    }
                    else if (this.gunReady && !MonoSingleton<InputManager>.Instance.PerformingCheatMenuCombo() && MonoSingleton<InputManager>.Instance.InputSource.Fire1.IsPressed && this.shootReady)
                    {
                        if (!this.wid || this.wid.delay == 0f)
                        {
                            this.Shoot(1);
                        }
                        else
                        {
                            this.shootReady = false;
                            this.shootCharge = 0f;
                            base.Invoke("DelayedShoot", this.wid.delay);
                        }
                        if (this.ceaud && this.ceaud.volume != 0f)
                        {
                            this.ceaud.volume = 0f;
                        }
                    }
                }
            }
            if (celight != null)
            {
                for (int i = 0; i < celight.Length; i++)
                {
                    if (this.pierceShotCharge == 0f && this.celight[i].enabled)
                    {
                        this.celight[i].enabled = false;
                    }
                    else if (this.pierceShotCharge != 0f)
                    {
                        this.celight[i].enabled = true;
                        this.celight[i].range = this.pierceShotCharge * 0.01f;
                    }
                }
            }
            if (this.gunVariation != 1)
            {
                if (this.gunVariation == 0)
                {
                    for (int i = 0; i < chargeEffect.Length; i++)
                    {
                        this.chargeEffect[i].transform.localScale = Vector3.one * this.pierceShotCharge * 0.02f;
                    }
                    this.ceaud.pitch = this.pierceShotCharge * 0.005f;
                }
                this.ceaud.volume = 0.25f + this.pierceShotCharge * 0.005f;
                MonoSingleton<RumbleManager>.Instance.SetVibrationTracked("rumble.gun.revolver_charge", this.ceaud.gameObject).intensityMultiplier = this.pierceShotCharge / 250f;
            }
            if (this.gunVariation != 0)
            {
                this.CheckCoinCharges();
                return;
            }
            if (this.pierceCharge == 100f && MonoSingleton<ColorBlindSettings>.Instance)
            {
                this.screenMR.material.color = MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.gunVariation];
            }
        }

        // Token: 0x06000D7A RID: 3450 RVA: 0x0007B6C0 File Offset: 0x000798C0
        private void LateUpdate()
        {
            if (this.gunVariation != 2)
            {
                return;
            }
            if (this.chargingPierce || this.twirlRecovery)
            {
                this.anim.SetBool("Spinning", true);
                bool flag = this.latestTwirlRotation < 0f;
                if (this.chargingPierce)
                {
                    this.twirlLevel = Mathf.Min(3f, Mathf.Floor(this.pierceShotCharge / 25f)) + 1f;
                }
                else
                {
                    this.twirlLevel = Mathf.MoveTowards(this.twirlLevel, 0.1f, Time.deltaTime * 100f * this.twirlLevel);
                }
                this.latestTwirlRotation += 1200f * (this.twirlLevel / 3f + 0.5f) * Time.deltaTime;
                if (this.twirlSprite)
                {
                    this.twirlSprite.color = new Color(1f, 1f, 1f, Mathf.Min(2f, Mathf.Floor(this.pierceShotCharge / 25f)) / 3f);
                }
                if (!this.ceaud.isPlaying)
                {
                    this.ceaud.Play();
                }
                this.ceaud.pitch = 0.5f + this.twirlLevel / 2f;
                if (this.twirlRecovery && flag && this.latestTwirlRotation >= 0f)
                {
                    this.latestTwirlRotation = 0f;
                    this.twirlRecovery = false;
                    if (this.twirlSprite)
                    {
                        this.twirlSprite.color = new Color(1f, 1f, 1f, 0f);
                    }
                }
                else
                {
                    while (this.latestTwirlRotation > 180f)
                    {
                        this.latestTwirlRotation -= 360f;
                    }
                    this.twirlBone.localRotation = Quaternion.Euler(this.twirlBone.localRotation.eulerAngles + (this.altVersion ? Vector3.left : Vector3.forward) * this.latestTwirlRotation);
                }
                this.anim.SetFloat("TwirlSpeed", this.twirlLevel / 3f);
                if (this.wid && this.wid.delay != 0f && !MonoSingleton<NewMovement>.Instance.gc.onGround)
                {
                    MonoSingleton<NewMovement>.Instance.rb.AddForce(MonoSingleton<CameraController>.Instance.transform.up * 400f * this.twirlLevel * Time.deltaTime, ForceMode.Acceleration);
                    return;
                }
            }
            else
            {
                this.anim.SetBool("Spinning", false);
                if (this.twirlSprite)
                {
                    this.twirlSprite.color = new Color(1f, 1f, 1f, 0f);
                }
                this.ceaud.Stop();
            }
        }

        // Token: 0x06000D7B RID: 3451 RVA: 0x0007B9B8 File Offset: 0x00079BB8
        private void Shoot(int shotType = 1)
        {
            this.cc.StopShake();
            this.shootReady = false;
            this.shootCharge = 0f;
            if (this.altVersion)
            {
                MonoSingleton<WeaponCharges>.Instance.revaltpickupcharges[this.gunVariation] = 2f;
            }
            if (shotType == 1)
            {
                var Currentrot = this.cc.transform.rotation * Quaternion.Euler(0,-20,0);
                GameObject gameObject1 = Instantiate<GameObject>(this.revolverBeam, this.cc.transform.position, Currentrot);
                if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
                {
                    gameObject1.transform.LookAt(this.targeter.CurrentTarget.bounds.center);
                }
                RevolverBeam component1 = gameObject1.GetComponent<RevolverBeam>();
                component1.sourceWeapon = this.gc.currentWeapon;
                component1.alternateStartPoint = this.gunBarrel.transform.position;
                component1.gunVariation = this.gunVariation;
                if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("PickUp"))
                {
                    component1.quickDraw = true;
                }

                Currentrot = this.cc.transform.rotation * Quaternion.Euler(0, 20, 0);
                GameObject gameObject2 = Instantiate<GameObject>(this.revolverBeam, this.cc.transform.position, Currentrot);
                if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
                {
                    gameObject2.transform.LookAt(this.targeter.CurrentTarget.bounds.center);
                }
                RevolverBeam component2 = gameObject2.GetComponent<RevolverBeam>();
                component2.sourceWeapon = this.gc.currentWeapon;
                component2.alternateStartPoint = this.gunBarrel2.transform.position;
                component2.gunVariation = this.gunVariation;
                if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("PickUp"))
                {
                    component2.quickDraw = true;
                }

                Currentrot = this.cc.transform.rotation * Quaternion.Euler(0, -40, 0);
                GameObject gameObject3 = Instantiate<GameObject>(this.revolverBeam, this.cc.transform.position, Currentrot);
                if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
                {
                    gameObject3.transform.LookAt(this.targeter.CurrentTarget.bounds.center);
                }
                RevolverBeam component3 = gameObject3.GetComponent<RevolverBeam>();
                component3.sourceWeapon = this.gc.currentWeapon;
                component3.alternateStartPoint = this.gunBarrel3.transform.position;
                component3.gunVariation = this.gunVariation;
                if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("PickUp"))
                {
                    component3.quickDraw = true;
                }

                Currentrot = this.cc.transform.rotation * Quaternion.Euler(0, 40, 0);
                GameObject gameObject4 = Instantiate<GameObject>(this.revolverBeam, this.cc.transform.position, Currentrot);
                if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
                {
                    gameObject4.transform.LookAt(this.targeter.CurrentTarget.bounds.center);
                }
                RevolverBeam component4 = gameObject4.GetComponent<RevolverBeam>();
                component4.sourceWeapon = this.gc.currentWeapon;
                component4.alternateStartPoint = this.gunBarrel4.transform.position;
                component4.gunVariation = this.gunVariation;
                if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("PickUp"))
                {
                    component4.quickDraw = true;
                }





                this.currentGunShot = UnityEngine.Random.Range(0, this.gunShots.Length);
                this.gunAud.clip = this.gunShots[this.currentGunShot];
                this.gunAud.volume = 0.55f;
                this.gunAud.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                this.gunAud.Play();
                this.cam.fieldOfView = this.cam.fieldOfView + this.cc.defaultFov / 40f;
                MonoSingleton<RumbleManager>.Instance.SetVibrationTracked("rumble.gun.fire", base.gameObject);
            }
            else if (shotType == 2)
            {
                var Currentrot = this.cc.transform.rotation * Quaternion.Euler(0, -20, 0);
                GameObject gameObject1 = Instantiate<GameObject>(this.revolverBeamSuper, this.cc.transform.position, Currentrot);
                if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
                {
                    gameObject1.transform.LookAt(this.targeter.CurrentTarget.bounds.center);
                }
                RevolverBeam component1 = gameObject1.GetComponent<RevolverBeam>();
                component1.sourceWeapon = this.gc.currentWeapon;
                component1.alternateStartPoint = this.gunBarrel.transform.position;
                component1.gunVariation = this.gunVariation;

                Currentrot = this.cc.transform.rotation * Quaternion.Euler(0, 20, 0);
                GameObject gameObject2 = Instantiate<GameObject>(this.revolverBeamSuper, this.cc.transform.position, Currentrot);
                if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
                {
                    gameObject2.transform.LookAt(this.targeter.CurrentTarget.bounds.center);
                }
                RevolverBeam component2 = gameObject2.GetComponent<RevolverBeam>();
                component2.sourceWeapon = this.gc.currentWeapon;
                component2.alternateStartPoint = this.gunBarrel2.transform.position;
                component2.gunVariation = this.gunVariation;

                Currentrot = this.cc.transform.rotation * Quaternion.Euler(0, -40, 0);
                GameObject gameObject3 = Instantiate<GameObject>(this.revolverBeamSuper, this.cc.transform.position, Currentrot);
                if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
                {
                    gameObject3.transform.LookAt(this.targeter.CurrentTarget.bounds.center);
                }
                RevolverBeam component3 = gameObject3.GetComponent<RevolverBeam>();
                component3.sourceWeapon = this.gc.currentWeapon;
                component3.alternateStartPoint = this.gunBarrel3.transform.position;
                component3.gunVariation = this.gunVariation;

                Currentrot = this.cc.transform.rotation * Quaternion.Euler(0, 40, 0);
                GameObject gameObject4 = Instantiate<GameObject>(this.revolverBeamSuper, this.cc.transform.position, Currentrot);
                if (this.targeter.CurrentTarget && this.targeter.IsAutoAimed)
                {
                    gameObject4.transform.LookAt(this.targeter.CurrentTarget.bounds.center);
                }
                RevolverBeam component4 = gameObject4.GetComponent<RevolverBeam>();
                component4.sourceWeapon = this.gc.currentWeapon;
                component4.alternateStartPoint = this.gunBarrel4.transform.position;
                component4.gunVariation = this.gunVariation;


                if (this.gunVariation == 2)
                {
                    component2.ricochetAmount = Mathf.Min(3, Mathf.FloorToInt(this.pierceShotCharge / 25f));
                }
                this.pierceShotCharge = 0f;
                if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("PickUp"))
                {
                    component2.quickDraw = true;
                }
                this.pierceReady = false;
                this.pierceCharge = 0f;
                if (this.gunVariation == 0)
                {
                    this.screenAud.clip = this.chargingSound;
                    this.screenAud.loop = true;
                    if (this.altVersion)
                    {
                        this.screenAud.pitch = 0.5f;
                    }
                    else
                    {
                        this.screenAud.pitch = 1f;
                    }
                    this.screenAud.volume = 0.55f;
                    this.screenAud.Play();
                }
                else if (!this.wid || this.wid.delay == 0f)
                {
                    this.wc.rev2charge -= (float)(this.altVersion ? 300 : 100);
                }
                if (this.superGunAud)
                {
                    this.currentGunShot = UnityEngine.Random.Range(0, this.superGunShots.Length);
                    this.superGunAud.clip = this.superGunShots[this.currentGunShot];
                    this.superGunAud.volume = 0.5f;
                    this.superGunAud.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
                    this.superGunAud.Play();
                }
                if (this.gunVariation == 2 && this.twirlShotSound)
                {
                    Instantiate<GameObject>(this.twirlShotSound, base.transform.position, Quaternion.identity);
                }
                this.cam.fieldOfView = this.cam.fieldOfView + this.cc.defaultFov / 20f;
                MonoSingleton<RumbleManager>.Instance.SetVibrationTracked("rumble.gun.fire_strong", base.gameObject);
            }
            if (!this.altVersion)
            {
                this.cylinder.DoTurn();
            }
            this.anim.SetFloat("RandomChance", UnityEngine.Random.Range(0f, 1f));
            if (shotType == 1)
            {
                this.anim.SetTrigger("Shoot");
            }
            else
            {
                this.anim.SetTrigger("ChargeShoot");
            }
            this.gunReady = false;
        }

        // Token: 0x06000D7C RID: 3452 RVA: 0x0007BE80 File Offset: 0x0007A080
        private void ThrowCoin()
        {
            if (this.punch == null || !this.punch.gameObject.activeInHierarchy)
            {
                this.punch = MonoSingleton<FistControl>.Instance.currentPunch;
            }
            if (this.punch)
            {
                this.punch.CoinFlip();
            }
            GameObject gameObject = Instantiate<GameObject>(this.coin, this.camObj.transform.position + this.camObj.transform.up * -0.5f, this.camObj.transform.rotation);
            gameObject.GetComponent<Coin>().sourceWeapon = this.gc.currentWeapon;
            MonoSingleton<RumbleManager>.Instance.SetVibration("rumble.coin_toss");
            Vector3 zero = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().AddForce(this.camObj.transform.forward * 20f + Vector3.up * 15f + (MonoSingleton<NewMovement>.Instance.ridingRocket ? MonoSingleton<NewMovement>.Instance.ridingRocket.rb.velocity : MonoSingleton<NewMovement>.Instance.rb.velocity) + zero, ForceMode.VelocityChange);
            this.pierceCharge = 0f;
            this.pierceReady = false;
        }

        // Token: 0x06000D7D RID: 3453 RVA: 0x0007BFD7 File Offset: 0x0007A1D7
        private void ReadyToShoot()
        {
            this.shootReady = true;
        }

        // Token: 0x06000D7E RID: 3454 RVA: 0x0007BFE0 File Offset: 0x0007A1E0
        public void Punch()
        {
            this.gunReady = false;
            this.anim.SetTrigger("ChargeShoot");
        }

        // Token: 0x06000D7F RID: 3455 RVA: 0x0007BFF9 File Offset: 0x0007A1F9
        public void ReadyGun()
        {
            this.gunReady = true;
        }

        // Token: 0x06000D80 RID: 3456 RVA: 0x0007C002 File Offset: 0x0007A202
        public void Click()
        {
            if (this.altVersion)
            {
                MonoSingleton<WeaponCharges>.Instance.revaltpickupcharges[this.gunVariation] = 0f;
            }
            if (this.gunVariation == 2)
            {
                this.chargingPierce = false;
                this.twirlRecovery = false;
            }
        }

        // Token: 0x06000D81 RID: 3457 RVA: 0x0007C039 File Offset: 0x0007A239
        public void MaxCharge()
        {
            if (this.gunVariation == 0)
            {
                this.pierceCharge = 100f;
                return;
            }
            this.CheckCoinCharges();
        }

        // Token: 0x06000D82 RID: 3458 RVA: 0x0007C055 File Offset: 0x0007A255
        private void DelayedShoot()
        {
            this.Shoot(1);
        }

        // Token: 0x06000D83 RID: 3459 RVA: 0x0007C05E File Offset: 0x0007A25E
        private void DelayedShoot2()
        {
            this.Shoot(2);
        }

        // Token: 0x06000D84 RID: 3460 RVA: 0x0007C068 File Offset: 0x0007A268
        private void CheckCoinCharges()
        {
            if (this.coinPanelsCharged == null || this.coinPanelsCharged.Length == 0)
            {
                this.coinPanelsCharged = new bool[this.coinPanels.Length];
            }
            this.coinCharge = ((this.gunVariation == 1) ? this.wc.rev1charge : this.wc.rev2charge);
            for (int i = 0; i < this.coinPanels.Length; i++)
            {
                if (this.altVersion && this.gunVariation == 2)
                {
                    this.coinPanels[i].fillAmount = this.coinCharge / 300f;
                }
                else
                {
                    this.coinPanels[i].fillAmount = this.coinCharge / 100f - (float)i;
                }
                if (this.coinPanels[i].fillAmount < 1f)
                {
                    this.coinPanels[i].color = ((this.gunVariation == 1) ? Color.red : Color.gray);
                    this.coinPanelsCharged[i] = false;
                }
                else
                {
                    if (this.coinPanels[i].color != MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.gunVariation])
                    {
                        this.coinPanels[i].color = MonoSingleton<ColorBlindSettings>.Instance.variationColors[this.gunVariation];
                    }
                    if (!this.coinPanelsCharged[i] && (!this.wid || this.wid.delay == 0f))
                    {
                        if (!this.screenAud)
                        {
                            this.screenAud = base.GetComponentInChildren<Canvas>().GetComponent<AudioSource>();
                        }
                        this.screenAud.pitch = 1f + (float)i / 2f;
                        this.screenAud.Play();
                        this.coinPanelsCharged[i] = true;
                    }
                }
            }
        }

        // Token: 0x04001422 RID: 5154
        private InputManager inman;

        // Token: 0x04001423 RID: 5155
        private WeaponIdentifier wid;

        // Token: 0x04001424 RID: 5156
        public int gunVariation;

        // Token: 0x04001425 RID: 5157
        public bool altVersion;

        // Token: 0x04001426 RID: 5158
        public Transform kickBackPos;

        // Token: 0x04001427 RID: 5159
        private AudioSource gunAud;

        // Token: 0x04001428 RID: 5160
        private AudioSource superGunAud;

        // Token: 0x04001429 RID: 5161
        public AudioClip[] gunShots;

        // Token: 0x0400142A RID: 5162
        public AudioClip[] superGunShots;

        // Token: 0x0400142B RID: 5163
        private int currentGunShot;

        // Token: 0x0400142C RID: 5164
        public GameObject gunBarrel;
        public GameObject gunBarrel2;
        public GameObject gunBarrel3;
        public GameObject gunBarrel4;

        // Token: 0x0400142D RID: 5165
        private bool gunReady;

        // Token: 0x0400142E RID: 5166
        private bool shootReady = true;

        // Token: 0x0400142F RID: 5167
        private bool pierceReady = true;

        // Token: 0x04001430 RID: 5168
        public float shootCharge;

        // Token: 0x04001431 RID: 5169
        public float pierceCharge;

        // Token: 0x04001432 RID: 5170
        private bool chargingPierce;

        // Token: 0x04001433 RID: 5171
        public float pierceShotCharge;

        // Token: 0x04001434 RID: 5172
        public Vector3 shotHitPoint;

        // Token: 0x04001435 RID: 5173
        public GameObject revolverBeam;

        // Token: 0x04001436 RID: 5174
        public GameObject revolverBeamSuper;

        // Token: 0x04001437 RID: 5175
        public RaycastHit hit;

        // Token: 0x04001438 RID: 5176
        public RaycastHit[] allHits;

        // Token: 0x04001439 RID: 5177
        private int currentHit;

        // Token: 0x0400143A RID: 5178
        private int currentHitMultiplier;

        // Token: 0x0400143B RID: 5179
        public float recoilFOV;

        // Token: 0x0400143C RID: 5180
        public GameObject[] chargeEffect;

        // Token: 0x0400143D RID: 5181
        private AudioSource ceaud;

        // Token: 0x0400143E RID: 5182
        private Light[] celight = new Light[4];

        // Token: 0x0400143F RID: 5183
        private GameObject camObj;

        // Token: 0x04001440 RID: 5184
        private Camera cam;

        // Token: 0x04001441 RID: 5185
        private CameraController cc;

        // Token: 0x04001442 RID: 5186
        private Vector3 tempCamPos;

        // Token: 0x04001443 RID: 5187
        public Vector3 beamReflectPos;

        // Token: 0x04001444 RID: 5188
        private GameObject beamDirectionSetter;

        // Token: 0x04001445 RID: 5189
        public MeshRenderer screenMR;

        // Token: 0x04001446 RID: 5190
        public Material batteryMat;

        // Token: 0x04001447 RID: 5191
        public Texture2D batteryFull;

        // Token: 0x04001448 RID: 5192
        public Texture2D batteryMid;

        // Token: 0x04001449 RID: 5193
        public Texture2D batteryLow;

        // Token: 0x0400144A RID: 5194
        public Texture2D[] batteryCharges;

        // Token: 0x0400144B RID: 5195
        private AudioSource screenAud;

        // Token: 0x0400144C RID: 5196
        public AudioClip chargedSound;

        // Token: 0x0400144D RID: 5197
        public AudioClip chargingSound;

        // Token: 0x0400144E RID: 5198
        private int bodiesPierced;

        // Token: 0x0400144F RID: 5199
        private Enemy enemy;

        // Token: 0x04001450 RID: 5200
        private CharacterJoint[] cjs;

        // Token: 0x04001451 RID: 5201
        private CharacterJoint cj;

        // Token: 0x04001452 RID: 5202
        private GameObject limb;

        // Token: 0x04001453 RID: 5203
        private Transform firstChild;

        // Token: 0x04001454 RID: 5204
        private int bulletForce;

        // Token: 0x04001455 RID: 5205
        private bool slowMo;

        // Token: 0x04001456 RID: 5206
        private bool timeStopped;

        // Token: 0x04001457 RID: 5207
        private float untilTimeResume;

        // Token: 0x04001458 RID: 5208
        private int enemiesLeftToHit;

        // Token: 0x04001459 RID: 5209
        private int enemiesPierced;

        // Token: 0x0400145A RID: 5210
        private RaycastHit subHit;

        // Token: 0x0400145B RID: 5211
        private float damageMultiplier = 1f;

        // Token: 0x0400145C RID: 5212
        public Transform twirlBone;

        // Token: 0x0400145D RID: 5213
        private float latestTwirlRotation;

        // Token: 0x0400145E RID: 5214
        private float twirlLevel;

        // Token: 0x0400145F RID: 5215
        public bool twirlRecovery;

        // Token: 0x04001460 RID: 5216
        public SpriteRenderer twirlSprite;

        // Token: 0x04001461 RID: 5217
        public GameObject twirlShotSound;

        // Token: 0x04001462 RID: 5218
        private GameObject currentDrip;

        // Token: 0x04001463 RID: 5219
        public GameObject coin;

        // Token: 0x04001464 RID: 5220
        [HideInInspector]
        public RevolverCylinder cylinder;

        // Token: 0x04001465 RID: 5221
        private SwitchMaterial rimLight;

        // Token: 0x04001466 RID: 5222
        public GunControl gc;

        // Token: 0x04001467 RID: 5223
        private Animator anim;

        // Token: 0x04001468 RID: 5224
        private Punch punch;

        // Token: 0x04001469 RID: 5225
        private NewMovement nmov;

        // Token: 0x0400146A RID: 5226
        private WeaponPos wpos;

        // Token: 0x0400146B RID: 5227
        public Image[] coinPanels;

        // Token: 0x0400146C RID: 5228
        public bool[] coinPanelsCharged;

        // Token: 0x0400146D RID: 5229
        private WeaponCharges wc;

        // Token: 0x0400146E RID: 5230
        private CameraFrustumTargeter targeter;

        // Token: 0x0400146F RID: 5231
        private float coinCharge = 400f;
    }

    public class WeezerGun : Gun
    {
        public static GameObject Asset;


        public static GameObject weaponPrefab;

        public static void LoadAssets()
        {
            weaponPrefab = WeezerGunLoader.Assets.LoadAsset<GameObject>("Weezer Weapon prefab");

        }

        public override GameObject Create(Transform parent)
        {
            base.Create(parent);
            Asset = GameObject.Instantiate(weaponPrefab, parent);
            //CamGunLoader.CamGn = Asset.AddComponent<Weezerrev>();

            return Asset;
        }



        public override int Slot()
        {
            return 0;
        }

        public override int WheelOrder()
        {
            return 7;
        }

        public override string Pref()
        {
            return "wez0";
        }


    }


}