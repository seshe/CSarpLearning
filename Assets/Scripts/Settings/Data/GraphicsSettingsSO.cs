using UnityEngine;

[CreateAssetMenu(fileName = "GraphicsSettings", menuName = "Settings/Graphics")]
public class GraphicsSettingsSO : ScriptableObject
{
   public SettingsData Data;

   public void ResetToDefaults()
   {
      Data = new SettingsData();
   }
}
