using UnityEngine;
using System.Collections;

public class WeaponScript : MonoBehaviour {

	public int ammo = 5;

	public void setAmmo(string flag) {
		if (flag == "d") {
			ammo--;
			//ammoText.text = "Ammo: " + ammo.ToString ();	
		} else if (flag == "r") {
			ammo = 5;
			//ammoText.text = "Ammo: " + ammo.ToString ();	
		}
	}

}
