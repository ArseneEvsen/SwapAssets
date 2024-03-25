using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;

public class WalletInfos : MonoBehaviour
{
    public async void getAddressWallet()
    {
        var address = await ThirdwebManager.Instance.SDK.wallet.GetAddress();
        Debug.Log(address);
        
    }

}
