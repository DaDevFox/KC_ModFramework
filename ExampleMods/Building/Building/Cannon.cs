using System;
using System.Reflection;
using System.Reflection.Emit;
using Assets;
using Assets.Code;
using I2.Loc;
using UnityEngine;


public class Cannon : WagePayer, IOutputInfo
{
	
	public string GetError()
	{
		string result;
		if (!base.CanPayWage())
		{
			result = ScriptLocalization.ArcherNoGold;
		}
		else if (this.b.GetWorkerPercent() <= 0f)
		{
			result = ScriptLocalization.ArcherNotManning;
		}
		else
		{
			result = null;
		}
		return result;
	}

	public string GetTitle()
	{
		return ScriptLocalization.Damage;
	}

	public string GetNum()
	{
		float timeBetweenShots = this.GetTimeBetweenShots();
		return ((int)(this.pd.projectilePrefab.attackDamage / timeBetweenShots * 10f)).ToString();
	}

	public string GetExplanation(OutputTileUI tileOutput)
	{
		string text = string.Format(ScriptLocalization.ArcherSoldierSkill, (int)(this.b.GetAvgSkill() * 100f));
		if (this.b.GetAvgSkill() > 0.5f)
		{
			text = text + " <color=green>- " + ScriptLocalization.HighlySkilled + " -</color>";
		}
		text += Environment.NewLine;
		float workerPercent = this.b.GetWorkerPercent();
		if (workerPercent < 0.999f)
		{
			text = text + ScriptLocalization.ArcherNeedMoreSoldiers + Environment.NewLine;
		}
		if (workerPercent > 0f)
		{
			float num = (float)base.GetGoldWage() * base.PaydaysPerYear();
			text = text + string.Format(ScriptLocalization.Wages, (int)num) + Environment.NewLine;
		}
		return text;
	}

	public string GetUnit()
	{
		return ScriptLocalization.PerSecond;
	}

	public bool ShowStaffDeficiencyMessage()
	{
		return true;
	}

    private void OnInit()
    {
	    this.fullAttackTime = this.pd.AttackTime;
	    this.useHazardPay = true;
		this.wage = ResourceAmount.Make(FreeResourceType.Gold, 4);

		this.cannon = this.transform.Find("Offset").Find("cannon");
		this.barrel = this.cannon.Find("cannon_top");
		this.hatch = this.barrel.Find("cannon_hatch");

		this.cannonBall = this.barrel.Find("cannon_ball");
		this.cannonBall.gameObject.SetActive(false);

		MaxRangeDisplay MRD = this.transform.GetComponent<MaxRangeDisplay>();
		MRD.position = this.transform.position;
		typeof(MaxRangeDisplay).GetField("b", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(MRD, this.b);

    }

    private float GetSkillBonusPercent()
	{
		return 0.3f * this.b.GetAvgSkill();
	}

    private float GetTimeBetweenShots()
	{
		float skillBonusPercent = this.GetSkillBonusPercent();
		float workerPercent = this.b.GetWorkerPercent();
		float result = Mathf.Lerp(this.LongestAttackTime, this.fullAttackTime * (1f - skillBonusPercent), workerPercent);
		if (workerPercent == 0f)
		{
			result = float.MaxValue;
		}
		return result;
	}

	private void SetRotationY(Transform t, float y)
	{
		Quaternion localRotation = t.localRotation;
		Vector3 eulerAngles = localRotation.eulerAngles;
		eulerAngles.y = y;
		localRotation.eulerAngles = eulerAngles;
		t.localRotation = localRotation;
	}

	private void FireProjectile()
	{
		if (this.state == Cannon.State.Reload)
		{
			if (this.reloadTime >= 1.999f)
			{
				this.state = Cannon.State.Firing;
				this.reloadTime = 2f;
				this.fireTime = 0f;
				SfxSystem.inst.PlayFromBank("DragonFallHit", base.transform.position, null);
				this.cannonBall.gameObject.SetActive(false);
			}
		}
	}

    public override void Tick(float dt)
	{
        try
        {
            this.veteranDecor.gameObject.SetActive(this.b.GetAvgSkill() > 0.5f);
            base.Tick(dt);
            float num = this.b.GetWorkerPercent() * (float)((!this.b.AreWorkersNear(this.b.jobs.Count)) ? 0 : 1);
            this.flag.SetActive(num > 0f);
            if (!this.b.IsBuilt() || num <= 0f)
            {
                this.pd.suppressFiring = true;
            }
            else
            {
                IProjectileHitable projectileHitable = this.pd.attackTarget;
                if (projectileHitable == null || projectileHitable.Equals(null))
                {
                    projectileHitable = this.pd.trackingTarget;
                }
                if (projectileHitable != null && !projectileHitable.Equals(null))
                {
                    Vector3 forward = this.RotateParent.position - projectileHitable.GetPredictedPosition(1f);
                    Quaternion to = Quaternion.LookRotation(forward);

                    float num2 = Vector3.Dot(forward.normalized, this.RotateParent.forward);
                    this.pd.suppressFiring = (num2 < Mathf.Cos(0.017453292f * this.MinTargetAngleForFiring));


					Quaternion baseAtPos = Quaternion.LookRotation(new Vector3(forward.x, 0, forward.z), Vector3.up);
					cannon.rotation = Quaternion.RotateTowards(cannon.rotation, baseAtPos, Time.deltaTime * this.baseRotateSpeed);

					to.y = 0f;
					to.z = 0f;
					
					this.RotateParent.localRotation = Quaternion.Slerp(this.RotateParent.localRotation, to, Time.deltaTime * this.barrelRotateSpeed);

                }
                if (this.state == Cannon.State.Reload)
                {
                    this.reloadTime += dt;
                    if (this.reloadTime > 2f)
                    {
                        this.reloadTime = 2f;
                    }

					if(this.reloadTime <= 1f)
					{
						float t = Mathf.SmoothStep(0f, 1f, this.reloadTime);
						this.SetRotationY(this.hatch, Mathf.Lerp(0f, this.reloadAngle, t));
					}
					else
					{
						float t = Mathf.SmoothStep(0f, 1f, this.reloadTime - 1f);
						this.SetRotationY(this.hatch, Mathf.Lerp(this.reloadAngle, 0f, t));
						this.cannonBall.gameObject.SetActive(true);

						Vector3 localPosition = this.cannonBall.localPosition;
						localPosition.z = Mathf.Lerp(.6f, .2f, t);
						localPosition.y = Mathf.Lerp(0f, .3f, t);
						this.cannonBall.localPosition = localPosition;
					}

                }
                else if (this.state == Cannon.State.Firing)
                {
                    this.fireTime += dt * 5f;
                    if (this.fireTime > 1f)
                    {
                        this.fireTime = 1f;
                        this.state = Cannon.State.Reload;
						SfxSystem.inst.PlayFromBank("BuildingSelectCastleGate", base.transform.position, null);
                        this.reloadTime = 0f;
                    }
                }
            }
        }
        catch(Exception err)
        {
            ModMain.helper.Log(err.ToString());
        }
	}

	public GameObject flag;
	public Transform RotateParent;
	public ProjectileDefense pd;
	public Building b;
    public Transform hatch;
	public Transform cannon;
	public Transform cannonBall;
	public Transform barrel;
	private float fullAttackTime;
	public float LongestAttackTime = 6f;
	private float reloadAngle = 105f;
	private Cannon.State state = Cannon.State.Reload;
	private float reloadTime = 0f;
	private float fireTime = 0f;
	public Transform veteranDecor;
	public float MinTargetAngleForFiring = 15f;
	public float baseRotateSpeed = 20f;
	public float barrelRotateSpeed = 10f;
	private Quaternion rotOffset = Quaternion.Euler(0f, 90f, 0f);

	private enum State
	{
		Reload,
		Firing
	}
}
