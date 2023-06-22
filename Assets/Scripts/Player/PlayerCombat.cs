using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;
    [SerializeField] private Transform _weaponPoint;
    [SerializeField] private float _pickUpWeaponRange = 0.5f;
    [SerializeField] private float _reloadSpeed = 1;

    public static event UnityAction<Weapon> WeaponChanged;

    private void Awake()
    {
        var weapon = Instantiate(_weapon);
        EquipWeapon(weapon);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            TryShoot();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            TryDropWeapon();
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            TryEquipWeapon();
        }


        if (Input.GetKeyDown(KeyCode.R))
        {
            TryReloadWeapon();
        }
    }

    private void TryReloadWeapon()
    {
        if (_weapon == null)
            return;

        _weapon.TryReload(_reloadSpeed);
    }

    private void TryEquipWeapon()
    {
        var closestObjects = Physics2D.OverlapCircleAll(transform.position, _pickUpWeaponRange);
        var closestWeapon = closestObjects.FirstOrDefault(obj => obj.transform.parent == null && obj.TryGetComponent(out Weapon weapon));

        if (closestWeapon != null)
            EquipWeapon(closestWeapon.GetComponent<Weapon>());
    }

    private void TryShoot()
    {
        if (_weapon == null)
            return;

        _weapon.TryShoot();
    }

    private void EquipWeapon(Weapon newWeapon)
    {
        TryDropWeapon();

        _weapon = newWeapon;
        WeaponChanged?.Invoke(_weapon);
        _weapon.PickUp(_weaponPoint);
    }

    private void TryDropWeapon()
    {
        if (_weapon == null)
            return;

        _weapon.Drop();
        _weapon = null;

        WeaponChanged?.Invoke(_weapon);
    }
}
