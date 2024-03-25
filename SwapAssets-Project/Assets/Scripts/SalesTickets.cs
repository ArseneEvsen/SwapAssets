using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalesTickets 
{
    string seller;
    int ticketNumber;
    int sharesToSell;
    int price;
    bool currentlyForSale;
    int token_id;

    public SalesTickets(string seller, int ticketNumber, int sharesToSell, int price, bool currentlyForSale, int token_id)
    {
        this.seller = seller;
        this.ticketNumber = ticketNumber;
        this.sharesToSell = sharesToSell;
        this.price = price;
        this.currentlyForSale = currentlyForSale;
        this.token_id = token_id;
    }

    public SalesTickets()
    {
    }

    public string getSeller() { return seller; }
    public int getTicketNumber() {  return ticketNumber; }
    public int getSharesToSell() {  return sharesToSell; }
    public int getPrice() { return price; }
    public bool isCurrentlyForSale() {  return currentlyForSale; }
    public int getToken_id() { return token_id;}

    public void setSeller(string seller) { this.seller = seller; }
    public void setTickerNumber(int ticketNumber) { this.ticketNumber = ticketNumber; }
    public void setSharesToSell(int sharesToSell) { this.sharesToSell = sharesToSell;}
    public void setPrice(int price) {  this.price = price; }
    public void setCurrentlyForSale(bool currentlyForSale) { this.currentlyForSale = currentlyForSale;}
    public void setTokenId(int tokenId) { this.token_id = tokenId;}
}
