using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;

public class PlacementSystem : MonoBehaviour
{
    public static PlacementSystem instance;
    public static event Action<NFT> nftSelected;
    public static event Action nftVoid;

    [SerializeField] 
    GameObject mouseIndicator, gridIndicator;

    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private Grid grid;

    [SerializeField]
    public GameObject shoes_shop;

    [SerializeField]
    public GameObject pizza_store;

    [SerializeField]
    public GameObject music_store;

    [SerializeField]
    public GameObject fruit_shop;

    [SerializeField]
    public GameObject fastfood;

    [SerializeField]
    public GameObject building_pink;

    [SerializeField]
    public GameObject building_green;

    [SerializeField]
    public GameObject house_yellow;

    [SerializeField]
    public GameObject house_red;

    [SerializeField]
    public GameObject house_orange;

    [SerializeField]
    public GameObject building_big_red;

    [SerializeField]
    public GameObject building_big_blue;

    Dictionary<Vector3Int, NFT> nftDatabase = new Dictionary<Vector3Int, NFT>();
    bool saleEnabled = false;


    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        BlockchainController.nftsInitialized += InitializeNFTs;
        UI_Data_Binding.hide_sales_action += salesActions;

    }

    public void InitializeNFTs(object sender, EventArgs e)
    {
        NFT[] nfts = BlockchainController.nfts;

        foreach (NFT nft in nfts)
        {
            
            Vector3Int vector = new Vector3Int(nft.getXPosition(),-1, nft.getYPosition());

            if (nftDatabase.ContainsKey(vector))
            {
                nftDatabase.Remove(vector);
                nftDatabase.Add(vector, nft);
            }
            else
            {
                nftDatabase.Add(vector, nft);
            }
        }

        placeAssets();
    }

    public void placeAssets()
    {

        foreach(KeyValuePair<Vector3Int , NFT> nft in nftDatabase)
        {
            
            switch(nft.Value.getAssetType())
            {
                case 1:

                    Instantiate(shoes_shop, new Vector3(nft.Value.getXPosition(), 0, nft.Value.getYPosition()), Quaternion.identity);
                    break;

                case 2:
                    Instantiate(pizza_store, new Vector3(nft.Value.getXPosition(), 0, nft.Value.getYPosition()), Quaternion.identity);
                    break;

                case 3:
                    Instantiate(music_store, new Vector3(nft.Value.getXPosition(), 0, nft.Value.getYPosition()), Quaternion.identity);
                    break;

                case 4:
                    Instantiate(fruit_shop, new Vector3(nft.Value.getXPosition(), 0, nft.Value.getYPosition()), Quaternion.identity);
                    break;

                case 5:
                    Instantiate(fastfood, new Vector3(nft.Value.getXPosition(), 0, nft.Value.getYPosition()), Quaternion.identity);
                    break;

                case 6:
                    Instantiate(building_pink, new Vector3(nft.Value.getXPosition(), 0, nft.Value.getYPosition()), Quaternion.identity);
                    break;

                case 7:
                    Instantiate(building_green, new Vector3(nft.Value.getXPosition(), 0, nft.Value.getYPosition()), Quaternion.identity);
                    break;

                case 8:
                    Instantiate(house_yellow, new Vector3(nft.Value.getXPosition(), 0, nft.Value.getYPosition()), Quaternion.identity);
                    break;

                case 9:
                    Instantiate(house_red, new Vector3(nft.Value.getXPosition(), 0, nft.Value.getYPosition()), Quaternion.identity);
                    break;

                case 10:
                    Instantiate(house_orange, new Vector3(nft.Value.getXPosition(), 0, nft.Value.getYPosition()), Quaternion.identity);
                    break;

                case 11:
                    Instantiate(building_big_red, new Vector3(nft.Value.getXPosition(), 0, nft.Value.getYPosition()), Quaternion.identity);
                    break;

                case 12:
                    Instantiate(building_big_blue, new Vector3(nft.Value.getXPosition(), 0, nft.Value.getYPosition()), Quaternion.identity);
                    break;
            }
        }
    }

    private void Update()
    {
        Vector3? selectedMapPosition = inputManager.GetSelectedMapPosition();
        if (selectedMapPosition != null)
        {
            Vector3 mousePosition = selectedMapPosition.Value;
            Vector3Int gridPosition = grid.WorldToCell(mousePosition);
            mouseIndicator.transform.position = mousePosition;

            Vector3 gridPositionFinal = grid.CellToWorld(gridPosition);
            gridIndicator.transform.position = gridPositionFinal + new Vector3((float)0.5, 1, 0);


            if (Input.GetMouseButtonDown(0))
            {
                selectAsset(gridPosition);
            }
        }
    }

    private void selectAsset(Vector3Int _gridPosition)
    {
        if(nftDatabase.ContainsKey(_gridPosition) && saleEnabled == false)
        {
            Debug.Log("There is an NFT on the grid");

            saleEnabled = true;
            nftSelected(nftDatabase[_gridPosition]);
        }

    }

    private void salesActions(bool action)
    {
        if(action)
        {
            saleEnabled = false;
            nftVoid();
        }
    }



    public bool AddObject(Vector3Int _gridPosition, NFT _nft)
    {
        // Check if a grid is already taken 
        if (!(checkEmptyGrid(_gridPosition)))
        {
            return false;
        }

        // If the grid is empty, add the NFT on it
        nftDatabase.Add(_gridPosition, _nft);
        return true;
    }

    public bool checkEmptyGrid(Vector3Int _gridPosition)
    {
        if (nftDatabase.ContainsKey(_gridPosition))
        {
            Debug.Log($"There is already an NFT on the grid {_gridPosition} ");
            return true;
        }
        else
        {
            return false;
        }
    }

}