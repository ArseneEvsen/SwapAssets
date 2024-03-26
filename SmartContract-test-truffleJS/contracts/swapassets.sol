// SPDX-License-Identifier: MIT
pragma solidity >=0.4.22 <0.9.0;

import '@openzeppelin/contracts/token/ERC721/ERC721.sol';
import '@openzeppelin/contracts/token/ERC721/extensions/ERC721URIStorage.sol';
import '@openzeppelin/contracts/access/Ownable.sol';

contract swapassets is ERC721, Ownable(msg.sender) {
  
  // This variable will be incremented at every mint
  uint256 totalNumberOfNfts;

  constructor() ERC721('TokenisationMarketplace', 'TMKP') {
    totalNumberOfNfts = 0;
  }

  // Struct to set important data for each NFT
  struct TokenInfos {
    uint256 tokenId;
    uint256 assetType;
    int8 xPosition;
    int8 yPosition;
  }

  // Struct about the infos of the owner of an NFT
  struct OwnerInfos {
    bool isOwner;
    address payable owner;
    uint256 shares;
    uint256 sharesForSelling;
    uint256 numTicket;
  }

  // Struct about the sales infos of an NFT
  struct SaleInfos {
    address payable owner;
    uint256 saleTicketNumber;
    uint256 price;
    uint256 sharesToSell;
    bool currentlyForSale;
  }

  // TOKEN INFOS
  mapping(uint256 => TokenInfos) public allTokensInfos;

  // OWNERS INFOS of an NFT
  mapping(uint256 => mapping(address => OwnerInfos)) public nftsOwners;

  // OWNERS LIST of an NFT
  mapping(uint256 => address[]) public ownersList;

  // SALES INFOS of an owner/nft
  mapping(uint256 => mapping(address => mapping(uint256 => SaleInfos))) public salesListing;

  // ALL ACTUAL SALES of an owner/nft 
  mapping(uint256 => mapping(address => uint256[])) public ownersSalesTicketsList;

  // LIST OF NFTS OWNED BY AN USER
  mapping(address => uint256[]) public nftsOwned;

  // Only owner of the contract can mints tokens
  function mint(address payable _to, uint256 _assetType, int8 _xPosition, int8 _yPosition) external onlyOwner {
    // The newly nft is mint to the given address
    _mint(_to, totalNumberOfNfts);

    // Then NFT's infos are set
    allTokensInfos[totalNumberOfNfts] = TokenInfos(
      totalNumberOfNfts, // Token Id
      _assetType,        // Asset type (house, buidling, store...)
      _xPosition,
      _yPosition
    );

    // The owner of the NFT is set
    nftsOwners[totalNumberOfNfts][_to] = OwnerInfos (
    true,
    _to, // Owner'address
    100, // Number of the share in %, it's 100% by default since there is only one owner at the mint    
    100, // Shares for selling
    0    // Number of sale's ticket so far 
    );

    // The owner address is listed inside the ownersList of the NFT
    ownersList[totalNumberOfNfts].push(_to);
    
    // Update the list of NFT owned by the user
    nftsOwned[_to].push(totalNumberOfNfts);

    // Update the totalNumberOfNfts
    totalNumberOfNfts++ ;
  }


  // Function to transfer NFT to the exchange for selling and create a sale ticket
  function listToExchange(uint _tokenId, uint _price, uint256 _sharesToSell) public {

    // Check that the tokenId exist
    require(_tokenId < totalNumberOfNfts, "The asked NFT doesn't exist.");

    // Verify ownership
    require(nftsOwners[_tokenId][msg.sender].isOwner == true, "You are not ones of the owners of the asked NFT.");

    // Check that the price is greater than 0
    require(_price > 0, "Please choose a price greater than 0.");

    // Check that the seller has enough shares to sell
    require(_sharesToSell <= nftsOwners[_tokenId][msg.sender].sharesForSelling, "The amount of shares to sell you want to sell is greater than the available shares.");

    // Transfer the NFT to the smart contract if the shares of the owner is 100, otherwise, the NFT has been already sent to the smart contract
    if(nftsOwners[_tokenId][msg.sender].sharesForSelling == 100) {
    _transfer(msg.sender, address(this), _tokenId);
    }

    // Create the sale ticket's number
    uint ticketNum = nftsOwners[_tokenId][msg.sender].numTicket;
    ownersSalesTicketsList[_tokenId][msg.sender].push(ticketNum);

    nftsOwners[_tokenId][msg.sender].numTicket += 1;

    // Create the sale
    salesListing[_tokenId][msg.sender][ticketNum] = SaleInfos  (
      payable(msg.sender),
      ticketNum,
      _price,
      _sharesToSell,
      true
    );

    // Update the sharesForSelling of the owner
    nftsOwners[_tokenId][msg.sender].sharesForSelling -= _sharesToSell;

  }

  // Function to buy a listed nft
  function buyNFT(uint256 _tokenId, address _sellerAddress, uint256 _saleTicket ) public payable  {
    // Check that the tokenId exist
    require(_tokenId < totalNumberOfNfts, "The asked NFT doesn't exist.");

    // Check if the sales exist
    require(salesListing[_tokenId][_sellerAddress][_saleTicket].currentlyForSale == true, "The asked sale doesn't exist.");

    // Then we check if the msg.sender send enough funds
    require(salesListing[_tokenId][_sellerAddress][_saleTicket].price == msg.value, "Not enough funds for the desired NFT!");

    // Get the seller address
    address payable seller = salesListing[_tokenId][_sellerAddress][_saleTicket].owner;

    // Update sale's infos
    salesListing[_tokenId][_sellerAddress][_saleTicket].currentlyForSale = false;
    
    // Update the shares of the seller
    uint256 sharesSelling = salesListing[_tokenId][_sellerAddress][_saleTicket].sharesToSell;
    nftsOwners[_tokenId][seller].shares -= sharesSelling;

    // if the seller shares = 0, remove his ownership and his belonging in the ownerList
    if(nftsOwners[_tokenId][seller].shares ==0){
    nftsOwners[_tokenId][seller].isOwner = false;
    }

    // Update the shares of the buyer
    nftsOwners[_tokenId][msg.sender].shares += sharesSelling;
    nftsOwners[_tokenId][msg.sender].sharesForSelling += sharesSelling;

    // Delete the sale ticket from the list of salesTicket
    deleteTicket(_tokenId, seller, _saleTicket );

    // MULTIPLES CASES :

    // If the buyer is a new owner of the NFT
    if(nftsOwners[_tokenId][msg.sender].isOwner == false)
    {
      // If the seller is still an owner of the NFT
      if(nftsOwners[_tokenId][seller].isOwner == true)
      {
        // Add the new owner to the ownersList and updates his infos
        nftsOwners[_tokenId][msg.sender].isOwner = true;
        nftsOwners[_tokenId][msg.sender].owner = payable(msg.sender);

        ownersList[_tokenId].push(msg.sender);
        nftsOwned[msg.sender].push(_tokenId);
      }

      // If the seller is not an owner anymore
      else if(nftsOwners[_tokenId][seller].isOwner == false)
      {

        // Remove him from the ownersList
        deleteOwner(_tokenId,seller);

        // Remove the nft from his nftsOwned list
        deleteNftOwned(_tokenId, seller);

        // Add the new owner to the ownersList and updates his infos
        nftsOwners[_tokenId][msg.sender].isOwner = true;
        nftsOwners[_tokenId][msg.sender].owner = payable(msg.sender);

        ownersList[_tokenId].push(msg.sender);
        nftsOwned[msg.sender].push(_tokenId);
      }
    }

    // If the buyer is already an owner of the NFT
    else if(nftsOwners[_tokenId][msg.sender].isOwner == true)
    {
      // If the seller is an owner too, no changes

      // If the seller is not an owner anymore
      if(nftsOwners[_tokenId][seller].isOwner == false)
      {
        // Remove him from the ownersList
        deleteOwner(_tokenId,seller);

        // Remove the nft from his nftsOwned list
        deleteNftOwned(_tokenId, seller);
      }
    }

    // Pay the seller
    payable(seller).transfer(salesListing[_tokenId][_sellerAddress][_saleTicket].price);
    
    // If the newly owner own 100% of the shares, the nft can be send to him
    if(nftsOwners[_tokenId][msg.sender].shares == 100){
    _transfer(address(this), msg.sender, _tokenId);
    }
    
  }

  // Function to cancel a sale
  function cancelSale(uint256 _tokenId, uint256 _saleNumber) public {

    // Check that the tokenId exist
    require(_tokenId < totalNumberOfNfts, "The asked NFT doesn't exist.");

    // Check the ownership of the sale
    require(salesListing[_tokenId][msg.sender][_saleNumber].owner == msg.sender);

    // Cancel the sale
    salesListing[_tokenId][msg.sender][_saleNumber].currentlyForSale = false;

    // Update the sharesForSelling
    uint sharesSelling = salesListing[_tokenId][msg.sender][_saleNumber].sharesToSell;
    nftsOwners[_tokenId][msg.sender].sharesForSelling += sharesSelling;

    // Delete the sale ticket from the list of salesTicket
    deleteTicket(_tokenId, salesListing[_tokenId][msg.sender][_saleNumber].owner, _saleNumber );

    // Transfer the NFT to the owner
    _transfer(address(this), msg.sender, _tokenId);
  }


  // Delete an owner from array + reorganise if necessary
  function deleteOwner(uint256 _tokenId, address _owner) private {

    // Get the index of the owner
    uint length = ownersList[_tokenId].length;
    uint index;

    // Get the index of the owner
    for(uint256 i = 0; i < length; i++) {
      if(ownersList[_tokenId][i] == _owner) {
        index = i;
      }
    }

    // Check if the owner was the most recent owner, if not reorganise array after delete
    if((length -1) == index)
    {
      ownersList[_tokenId].pop();
    }
    else
    {
      for (uint i = index; i<length-1; i++){
            ownersList[_tokenId][i] = ownersList[_tokenId][i+1];
        }
        ownersList[_tokenId].pop();
    }
  }

  // Function to delete a ticket sale + reorganise array if necessary
  function deleteTicket(uint256 _tokenId, address _owner, uint256 _ticketNumber) private {
    
    // Get the index of the ticket
    uint length = ownersSalesTicketsList[_tokenId][_owner].length;
    uint index;

    // Get the index of the sale ticket
    for(uint256 i = 0; i < length; i++) {
      if(ownersSalesTicketsList[_tokenId][_owner][i] == _ticketNumber) {
        index = i;
      }
    }

    // Check if the ticket was the most recent, if not reorganise array after delete
    if((length -1) == index)
    {
      ownersSalesTicketsList[_tokenId][_owner].pop();
    }
    else
    {
      for (uint i = index; i<length-1; i++){
            ownersSalesTicketsList[_tokenId][_owner][i] = ownersSalesTicketsList[_tokenId][_owner][i+1];
        }
        ownersSalesTicketsList[_tokenId][_owner].pop();
    }

  }

  // Function to delete the list of nfts owned by the an user
  function deleteNftOwned(uint256 _tokenId, address _owner) private {

    // Get the index of the nft
    uint length = nftsOwned[_owner].length;
    uint index;

    // Get the index of the nft
    for(uint256 i = 0; i < length; i++) {
      if(nftsOwned[_owner][i] == _tokenId) {
        index = i;
      }

    // Check if the nft was the most recent, if not reorganise array after delete
    if((length -1) == index)
    {
      nftsOwned[_owner].pop();
    }
    else
    {
      for (uint p = index; p<length-1; p++){
          nftsOwned[_owner][p] = nftsOwned[_owner][p+1];
        }
        nftsOwned[_owner].pop();
    }
    
  }

}

  // FRONTEND FUNCTIONS

  // NFTS INFOS
  function getTotalNumberOfNfts() public view returns (uint){
    return totalNumberOfNfts;
  }
  
  function getAllNFTsInfos() public view returns (TokenInfos[] memory) {
    TokenInfos[] memory tokens = new TokenInfos[](totalNumberOfNfts);
    
    for(uint i=0; i<totalNumberOfNfts; i++) {
      TokenInfos storage currentNft = allTokensInfos[i];
      tokens[i] = currentNft;
    }

    return tokens;
  }

  function getTokenInfoForId (uint256 _tokenId) public view returns(TokenInfos memory) {
    return allTokensInfos[_tokenId];

  }

  function getNftOwnerList(uint _tokenId) public view returns (address[] memory){
    return ownersList[_tokenId];
  }
  
  // OWNERS INFOS
  function getNftsOwners(uint256 _tokenId) public view returns(OwnerInfos[] memory) {

    uint256 numberOfOwners = ownersList[_tokenId].length;

    // Create an OwnerInfos struct array with the size of the number of owners from ownersList
    OwnerInfos[] memory ownersArray = new OwnerInfos[](numberOfOwners);

    // For every owners, get their infos
    for(uint i = 0; i<numberOfOwners; i++) {
      address currentAddress = ownersList[_tokenId][i];

      ownersArray[i] = nftsOwners[_tokenId][currentAddress];
    }

    return ownersArray;
  } 

  function getOwnerInfos(uint _tokenId, address _owner) public view returns (OwnerInfos memory) {
    return nftsOwners[_tokenId][_owner];
  }

  function getNumberOfOwners(uint256 _tokenId) public view returns(uint) {
    return ownersList[_tokenId].length;
  }

  // SALES INFOS
  function getSalesInfos(uint256 _tokenId, address _owner) public view returns(SaleInfos[] memory) {

    uint256 numberOfSales = ownersSalesTicketsList[_tokenId][_owner].length;

    // Create an SaleInfos struct array with the size of the number of sales from ownersSalesTicketsList
    SaleInfos[] memory salesArray = new SaleInfos[](numberOfSales);

    // For every sale, get their infos
    for(uint i = 0; i<numberOfSales; i++) {
      uint256 currentTicketNumber = ownersSalesTicketsList[_tokenId][_owner][i];

      salesArray[i] = salesListing[_tokenId][_owner][currentTicketNumber];
    }

    return salesArray;
  } 

    function getNumberSalesOwner(uint256 _tokenId, address _owner) public view returns(uint) {
    return ownersSalesTicketsList[_tokenId][_owner].length;
  }

  function getTotalNumberOfTicketsForNft(uint _tokenId) public view returns(uint) {
    uint totalCount = 0;

    for(uint i = 0; i < getNumberOfOwners(_tokenId); i++) {

      for(uint j = 0; j < getNumberSalesOwner(_tokenId, ownersList[_tokenId][i]); j++){
        totalCount += 1;
      }
    }
    return totalCount;
  }

    // LIST OF NFTS OWNED BY AN ADDRESS
  function getListOfNftsOwned(address _owner) public view returns(uint256[] memory) {

    uint256 numberOfNfts = nftsOwned[_owner].length;

    // Create the array of nfts owned
    uint256[] memory nftsOwnedArray = new uint256[](numberOfNfts);

    nftsOwnedArray = nftsOwned[_owner];

    return nftsOwnedArray;
  } 
}