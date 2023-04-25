using System.Linq;
using UnityEngine;

public class UltimateLoad : MonoBehaviour {
    private float YinFill = 0f;
    private float YangFill = 0f;

    public void ActivateUltimate() {
        if (YinFill != 100 || YangFill != 100) return;
        else {
            EndZone[] WinZones = GameObject.FindObjectsOfType<EndZone>();
            if (WinZones.All(x => x.Activated)) {
                // Put win here
            } else {
                // Display a messsage saying that at least one character isn't in the right place
            }
        }
    }

    public void AddCharge(float amount, bool yin) {
        if (yin && YinFill < 100) {
            YinFill += amount;
            if (YinFill >= 100) {
                //Display message "Yin is charged"
            }
        }
        else if (!yin && YangFill < 100) {
            YangFill += amount;
            if (YangFill > 100) {
                //Display message "Yang is charged"    
            }
        }
    }
}
