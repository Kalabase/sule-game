using UnityEngine;

public class StatPanelUI : MonoBehaviour
{
    public RadarChartUI rightRadarChart;
    public RadarChartUI leftRadarChart;

    public void Initialize()
    {
        rightRadarChart.values = new float[3]{
        (float)Wallet.Instance.STR / Wallet.Instance.maxSTR,
        (float)Wallet.Instance.RST / Wallet.Instance.maxRST,
        (float)Wallet.Instance.NRG / Wallet.Instance.maxNRG
        };
    }
}
