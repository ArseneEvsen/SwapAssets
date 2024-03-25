using Nethereum.Contracts.Standards.ERC20.TokenList;
using System;
using System.Collections;
using System.Collections.Generic;
using Thirdweb;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Data_Binding : MonoBehaviour
{
    public static event Action<bool> hide_sales_action;
    public static event Action<SalesTickets> buy_event;
    public static event Action refresh_event;
    public static event Action<SalesTickets> listToExchange_event;

    Label nft_Id;
    NFT nft;
    public ListView listView;

    public VisualElement SALE_DETAILS;
    public VisualElement SALES_DISPLAY;
    public VisualTreeAsset itemTemplate;
    Button button;

    Label shares_value;
    Label price_value;
    Label ticket_number;
    Label seller;

    Button buy_button;
    Button refresh_button;

    VisualElement sellCreation;

    Button cancel_sale;
    Button create_sale;

    TextField amount_share_sell;
    TextField price;
    void Awake()
    {
        PlacementSystem.nftSelected += displayNftInfo;
        PlacementSystem.nftSelected += UpdateListView;

        PlacementSystem.nftVoid += emptyUI;
        PlacementSystem.nftVoid += emptyList;
        PlacementSystem.nftVoid += hideSale;
    }

    private void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        SALE_DETAILS = root.Q<VisualElement>("SALE_DETAILS");
        SALES_DISPLAY = root.Q<VisualElement>("SALES_DISPLAY");

        button = root.Q<Button>("sales_list_action");
        button.clicked += actionSale;

        listView = root.Query<ListView>("SALES_DISPLAY").First();


        shares_value = root.Q<Label>("shares_value");
        price_value = root.Q<Label>("price_value");
        ticket_number = root.Q<Label>("ticket_number_value");
        seller = root.Q<Label>("seller_address_value");

        refresh_button = root.Q<Button>("refresh_button");
        refresh_button.clicked += refresh;

        sellCreation = root.Q<VisualElement>("SALE_CREATION");
        cancel_sale = root.Q<Button>("cancel_sale");
        cancel_sale.clicked += CancelButton_clicked;
        create_sale = root.Q<Button>("create_sale");
        create_sale.clicked += createTheSale;

        amount_share_sell = root.Q<TextField>("amount_shares");
        price = root.Q<TextField>("price_value");
    }


    private void showSalesList()
    {
        
        SALE_DETAILS.style.display = DisplayStyle.Flex;
        SALES_DISPLAY.style.display = DisplayStyle.Flex;

    }
    private void hideSale()
    {
        SALE_DETAILS.style.display = DisplayStyle.None;
        SALES_DISPLAY.style.display = DisplayStyle.None;
    }

    private void actionSale()
    {
        if (button.text == "SHOW SALES")
        {
            showSalesList();
            button.text = "CLOSE SALES";
        }
        else
        {
            hideSale();
            button.text = "SHOW SALES";
            button.style.display = DisplayStyle.None;
            hide_sales_action(true);
        }
    }

    private async void displayNftInfo(NFT obj)
    {
        nft = obj;

        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        button.style.display = DisplayStyle.Flex;

        Label asset_type = root.Q<Label>("asset_type_label");
        asset_type.text = obj.getAssetDescription();
        nft_Id = root.Q<Label>("token_id_label");

        nft_Id.text = obj.getTokenId().ToString();

        var address = await ThirdwebManager.Instance.SDK.wallet.GetAddress();
        foreach(Owners owner in obj.getOwners())
        {
            if (owner.getOwnerAddress() == address)
            {
                Label isOwner = root.Q<Label>("isOwner");
                isOwner.style.display = DisplayStyle.Flex;

                VisualElement sellArea = root.Q<VisualElement>("SELL_AREA");
                sellArea.style.display = DisplayStyle.Flex;

                Label amout_share_to_sell = root.Q<Label>("amout_share_to_sell");
                amout_share_to_sell.text = owner.getSharesForSelling().ToString();

                Button sale_button = root.Q<Button>("sell_button");
                sale_button.clicked += Sale_button_clicked;
            }
        }
    }

    private void Sale_button_clicked()
    {
        sellCreation.style.display = DisplayStyle.Flex;
    }

    private void CancelButton_clicked()
    {
        sellCreation.style.display = DisplayStyle.None;
    }

    private void createTheSale()
    {

        SalesTickets ticket = new SalesTickets();

        ticket.setSharesToSell(int.Parse(amount_share_sell.value));
        ticket.setPrice(int.Parse(price.value));
        ticket.setTokenId(nft.getTokenId());

        listToExchange_event(ticket);

        sellCreation.style.display = DisplayStyle.None;
    } 

    private void emptyUI()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        nft_Id = root.Q<Label>("token_id_label");
        Label isOwner = root.Q<Label>("isOwner");
        VisualElement sellArea = root.Q<VisualElement>("SELL_AREA");
        Label asset_type = root.Q<Label>("asset_type_label");

        nft_Id.text = "";
        asset_type.text = "";
        isOwner.style.display = DisplayStyle.None;
        sellArea.style.display = DisplayStyle.None;
    }

    void UpdateListView(NFT obj)
    {
        // Erase actual list view content
        listView.hierarchy.Clear();

        if (!(obj.getSalesTickets() == null))
        {
            foreach (SalesTickets ticket in obj.getSalesTickets())
            {
                // Instanciate model
                var element = itemTemplate.CloneTree();

                Button buttonTicket = element.Q<Button>("select_button");
                buttonTicket.clicked += () => selectTicket(ticket);

                // link
                var label = element.Q<Label>("ticket-number");

                if (label != null)
                {
                    label.text = ticket.getTicketNumber().ToString();
                }

                // Add element to the ListView
                listView.hierarchy.Add(element);
            }
        }
    }

    void selectTicket(SalesTickets ticket)
    {
        shares_value.text = ticket.getSharesToSell().ToString();
        price_value.text = ticket.getPrice().ToString();
        ticket_number.text = ticket.getTicketNumber().ToString();
        seller.text = ticket.getSeller().ToString();

        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        buy_button = root.Q<Button>("buy_button");
        buy_button.clicked += () => buyNft(ticket);
    }

    void emptyList() {
        listView.hierarchy.Clear();
    }
    void test() { Debug.Log("buy"); }

    void buyNft(SalesTickets ticket)
    {
        buy_event(ticket);
    }

    private void refresh()
    {
        refresh_event();
    }
}
