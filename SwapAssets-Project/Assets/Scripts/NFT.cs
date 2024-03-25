using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NFT 
{
    private int tokenId;
    private int assetType;
    private string assetDescription;
    private int xPosition;
    private int yPosition;

    private Owners[] ownersList;
    private SalesTickets[] salesTicketsList;

    public NFT(int _tokenId, int _assetType, int _xPosition, int _yPosition, string _assetDescription)
    {
        this.tokenId = _tokenId;
        this.assetType = _assetType;
        this.xPosition = _xPosition;
        this.yPosition = _yPosition;   
        this.assetDescription = _assetDescription;    
    }

    public void setTokenId(int _tokenId)
    {
        this.tokenId = _tokenId;
    }

    public int getTokenId()
    {
        return this.tokenId;
    }

    public void setAssetType (int _assetType) {
        this.assetType = _assetType; 
    }

    public int getAssetType()
    {
        return this.assetType;
    }

    public void setAssetDescription(string _description)
    {
        this.assetDescription = _description;
    }

    public string getAssetDescription()
    {
        return this.assetDescription;
    }

    public void setXPosition(int _xPosition)
    {
        this.xPosition = _xPosition;
    }
    public int getXPosition()
    {
        return this.xPosition;
    }

    public void setYPosition(int _yPosition)
    {
        this.yPosition = _yPosition;
    }
    public int getYPosition()
    {
        return this.yPosition;
    }

    public Owners[] getOwners() { return ownersList; }
    public void setOwnersList(Owners[] ownersList) { this.ownersList = ownersList; }

    public SalesTickets[] getSalesTickets() { return salesTicketsList; }
    public void setSalesTickets(SalesTickets[] salesTicketsList) { this.salesTicketsList = salesTicketsList; }

    public override string ToString()
    {
        return base.ToString();
    }
}


