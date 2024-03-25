using Dynamitey;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Owners
{
    string ownerAddress;
    int shares;
    int sharesForSelling;
    bool isOwner;

    public Owners(string ownerAddress, int shares, int sharesForSelling, bool isOwner)
    {
        this.ownerAddress = ownerAddress;
        this.shares = shares;
        this.sharesForSelling = sharesForSelling;
        this.isOwner = isOwner;
    }

    public string getOwnerAddress() { return ownerAddress; }
    public int getShares() { return shares; }
    public int getSharesForSelling() { return sharesForSelling; }
    public bool getIsOwner() { return isOwner; }

    public void setOwnerAddress(string ownerAddress) {  this.ownerAddress = ownerAddress; }
    public void setShares(int shares) {  this.shares = shares; }
    public void setSharesForSelling(int sharesForSelling) { this.sharesForSelling = sharesForSelling; }
    public void setIsOwner(bool isOwner) { this.isOwner = isOwner; }
}
