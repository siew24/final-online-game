using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class WeaponSwitching : MonoBehaviour
{

    public int selectedWeapon;
    public InputActionReference scrollWeapon;
    private float scrollValue;

    private void Awake()
    {
        scrollWeapon.action.performed += _x => scrollValue = _x.action.ReadValue<float>();
    }
    // Start is called before the first frame update
    void Start()
    {
        selectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if(scrollValue > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;

            else
                selectedWeapon++;
        }

     

        if(previousSelectedWeapon != selectedWeapon)
        {
            selectWeapon();
        }
    }

    void selectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if( i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
