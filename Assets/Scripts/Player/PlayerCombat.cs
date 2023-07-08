using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCombat : MonoBehaviour
{
    private Transform _weaponPoint;
    private Transform _punchPoint;
    private float _handsLength = 0.5f;
    private Weapon _currentWeapon;
    private LayerMask _enemyMask;
    private ParticleSystem _punchVFX;

    public Weapon CurrentWeapon => _currentWeapon;

    public static event UnityAction<int> BulletsChanged;
    public static event UnityAction Shooted;

    public void Init(Weapon startWeapon, Transform weaponPoint, float handsLength, Transform punchPoint, LayerMask enemyMask, ParticleSystem punchVFX)
    {
        _weaponPoint = weaponPoint;
        _punchPoint = punchPoint;
        _handsLength = handsLength;
        _enemyMask = enemyMask;
        _punchVFX = punchVFX;

        var weapon = Instantiate(startWeapon);
        EquipWeapon(weapon);
    }

    public void TryPunch()
    {
        print("Punch");
        AudioManager.Instance.PlayPunchSFX(false);

        var hit = Physics2D.OverlapCircle(_punchPoint.position, _handsLength, _enemyMask);

        if (!hit)
            return;

        print(hit.name);

        if (hit.TryGetComponent(out Health health))
        {
            health.ApplyDamage(ignoreArmor: true);
            _punchVFX.Play();
            AudioManager.Instance.PlayPunchSFX(true);
        }
    }

    public bool TryShoot()
    {
        if (_currentWeapon == null)
            return false;

        if (_currentWeapon.TryShoot())
        {
            Shooted?.Invoke();
            BulletsChanged?.Invoke(_currentWeapon.CurrentBulletsCount);
        }
        else
        {
            return false;
        }

        return true;
    }

    public bool TryDropWeapon()
    {
        if (_currentWeapon == null)
            return false;

        _currentWeapon.Throw();
        _currentWeapon = null;

        BulletsChanged?.Invoke(0);

        return true;
    }

    public bool TryPickUpClosestWeapon()
    {
        var closestObjects = Physics2D.OverlapCircleAll(transform.position, _handsLength);
        var closestWeapon = closestObjects.FirstOrDefault(obj => obj.transform.parent == null && obj.TryGetComponent(out Weapon weapon));

        foreach (var weapon in closestObjects)
            print(weapon.name);


        if (closestWeapon != null)
            EquipWeapon(closestWeapon.GetComponent<Weapon>());

        return closestWeapon != null;
    }

    private void EquipWeapon(Weapon newWeapon)
    {
        TryDropWeapon();

        _currentWeapon = newWeapon;
        _currentWeapon.PickUp(_weaponPoint);

        BulletsChanged?.Invoke(_currentWeapon.CurrentBulletsCount);
    }
}
