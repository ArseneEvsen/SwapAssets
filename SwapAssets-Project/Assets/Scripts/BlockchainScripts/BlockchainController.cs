using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using Thirdweb;
using System;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using UnityEngine.UIElements;
using Dynamitey.DynamicObjects;

public class BlockchainController : MonoBehaviour
{
    public static BlockchainController instance;
    public static event EventHandler nftsInitialized;

    
    string contractAddress = "0x859a46189261177B036803d4979Bc395FBf37539";
    string ownerAddress = "0xeb28BEDdd1368b0980881c141dbdACeF97593A43";
    string abi = "[\r\n  {\r\n    \"type\": \"constructor\",\r\n    \"name\": \"\",\r\n    \"inputs\": [],\r\n    \"outputs\": [],\r\n    \"stateMutability\": \"nonpayable\"\r\n  },\r\n  {\r\n    \"type\": \"event\",\r\n    \"name\": \"Approval\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"owner\",\r\n        \"indexed\": true,\r\n        \"internalType\": \"address\"\r\n      },\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"approved\",\r\n        \"indexed\": true,\r\n        \"internalType\": \"address\"\r\n      },\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"tokenId\",\r\n        \"indexed\": true,\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"outputs\": [],\r\n    \"anonymous\": false\r\n  },\r\n  {\r\n    \"type\": \"event\",\r\n    \"name\": \"ApprovalForAll\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"owner\",\r\n        \"indexed\": true,\r\n        \"internalType\": \"address\"\r\n      },\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"operator\",\r\n        \"indexed\": true,\r\n        \"internalType\": \"address\"\r\n      },\r\n      {\r\n        \"type\": \"bool\",\r\n        \"name\": \"approved\",\r\n        \"indexed\": false,\r\n        \"internalType\": \"bool\"\r\n      }\r\n    ],\r\n    \"outputs\": [],\r\n    \"anonymous\": false\r\n  },\r\n  {\r\n    \"type\": \"event\",\r\n    \"name\": \"OwnershipTransferred\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"previousOwner\",\r\n        \"indexed\": true,\r\n        \"internalType\": \"address\"\r\n      },\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"newOwner\",\r\n        \"indexed\": true,\r\n        \"internalType\": \"address\"\r\n      }\r\n    ],\r\n    \"outputs\": [],\r\n    \"anonymous\": false\r\n  },\r\n  {\r\n    \"type\": \"event\",\r\n    \"name\": \"Transfer\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"from\",\r\n        \"indexed\": true,\r\n        \"internalType\": \"address\"\r\n      },\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"to\",\r\n        \"indexed\": true,\r\n        \"internalType\": \"address\"\r\n      },\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"tokenId\",\r\n        \"indexed\": true,\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"outputs\": [],\r\n    \"anonymous\": false\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"allTokensInfos\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"outputs\": [\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"tokenId\",\r\n        \"internalType\": \"uint256\"\r\n      },\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"assetType\",\r\n        \"internalType\": \"uint256\"\r\n      },\r\n      {\r\n        \"type\": \"int8\",\r\n        \"name\": \"xPosition\",\r\n        \"internalType\": \"int8\"\r\n      },\r\n      {\r\n        \"type\": \"int8\",\r\n        \"name\": \"yPosition\",\r\n        \"internalType\": \"int8\"\r\n      }\r\n    ],\r\n    \"stateMutability\": \"view\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"approve\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"to\",\r\n        \"internalType\": \"address\"\r\n      },\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"tokenId\",\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"outputs\": [],\r\n    \"stateMutability\": \"nonpayable\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"balanceOf\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"owner\",\r\n        \"internalType\": \"address\"\r\n      }\r\n    ],\r\n    \"outputs\": [\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"stateMutability\": \"view\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"buyNFT\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"_tokenId\",\r\n        \"internalType\": \"uint256\"\r\n      },\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"_sellerAddress\",\r\n        \"internalType\": \"address\"\r\n      },\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"_saleTicket\",\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"outputs\": [],\r\n    \"stateMutability\": \"payable\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"cancelSale\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"_tokenId\",\r\n        \"internalType\": \"uint256\"\r\n      },\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"_saleNumber\",\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"outputs\": [],\r\n    \"stateMutability\": \"nonpayable\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"getAllNFTsInfos\",\r\n    \"inputs\": [],\r\n    \"outputs\": [\r\n      {\r\n        \"type\": \"tuple[]\",\r\n        \"name\": \"\",\r\n        \"components\": [\r\n          {\r\n            \"type\": \"uint256\",\r\n            \"name\": \"tokenId\",\r\n            \"internalType\": \"uint256\"\r\n          },\r\n          {\r\n            \"type\": \"uint256\",\r\n            \"name\": \"assetType\",\r\n            \"internalType\": \"uint256\"\r\n          },\r\n          {\r\n            \"type\": \"int8\",\r\n            \"name\": \"xPosition\",\r\n            \"internalType\": \"int8\"\r\n          },\r\n          {\r\n            \"type\": \"int8\",\r\n            \"name\": \"yPosition\",\r\n            \"internalType\": \"int8\"\r\n          }\r\n        ],\r\n        \"internalType\": \"struct tokenisation.TokenInfos[]\"\r\n      }\r\n    ],\r\n    \"stateMutability\": \"view\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"getApproved\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"tokenId\",\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"outputs\": [\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"address\"\r\n      }\r\n    ],\r\n    \"stateMutability\": \"view\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"getListOfNftsOwned\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"_owner\",\r\n        \"internalType\": \"address\"\r\n      }\r\n    ],\r\n    \"outputs\": [\r\n      {\r\n        \"type\": \"uint256[]\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"uint256[]\"\r\n      }\r\n    ],\r\n    \"stateMutability\": \"view\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"getNftsOwners\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"_tokenId\",\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"outputs\": [\r\n      {\r\n        \"type\": \"tuple[]\",\r\n        \"name\": \"\",\r\n        \"components\": [\r\n          {\r\n            \"type\": \"bool\",\r\n            \"name\": \"isOwner\",\r\n            \"internalType\": \"bool\"\r\n          },\r\n          {\r\n            \"type\": \"address\",\r\n            \"name\": \"owner\",\r\n            \"internalType\": \"address payable\"\r\n          },\r\n          {\r\n            \"type\": \"uint256\",\r\n            \"name\": \"shares\",\r\n            \"internalType\": \"uint256\"\r\n          },\r\n          {\r\n            \"type\": \"uint256\",\r\n            \"name\": \"sharesForSelling\",\r\n            \"internalType\": \"uint256\"\r\n          },\r\n          {\r\n            \"type\": \"uint256\",\r\n            \"name\": \"numTicket\",\r\n            \"internalType\": \"uint256\"\r\n          }\r\n        ],\r\n        \"internalType\": \"struct tokenisation.OwnerInfos[]\"\r\n      }\r\n    ],\r\n    \"stateMutability\": \"view\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"getNumberOfOwners\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"_tokenId\",\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"outputs\": [\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"stateMutability\": \"view\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"getNumberSalesOwner\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"_tokenId\",\r\n        \"internalType\": \"uint256\"\r\n      },\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"_owner\",\r\n        \"internalType\": \"address\"\r\n      }\r\n    ],\r\n    \"outputs\": [\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"stateMutability\": \"view\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"getSalesInfos\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"_tokenId\",\r\n        \"internalType\": \"uint256\"\r\n      },\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"_owner\",\r\n        \"internalType\": \"address\"\r\n      }\r\n    ],\r\n    \"outputs\": [\r\n      {\r\n        \"type\": \"tuple[]\",\r\n        \"name\": \"\",\r\n        \"components\": [\r\n          {\r\n            \"type\": \"address\",\r\n            \"name\": \"owner\",\r\n            \"internalType\": \"address payable\"\r\n          },\r\n          {\r\n            \"type\": \"uint256\",\r\n            \"name\": \"saleTicketNumber\",\r\n            \"internalType\": \"uint256\"\r\n          },\r\n          {\r\n            \"type\": \"uint256\",\r\n            \"name\": \"price\",\r\n            \"internalType\": \"uint256\"\r\n          },\r\n          {\r\n            \"type\": \"uint256\",\r\n            \"name\": \"sharesToSell\",\r\n            \"internalType\": \"uint256\"\r\n          },\r\n          {\r\n            \"type\": \"bool\",\r\n            \"name\": \"currentlyForSale\",\r\n            \"internalType\": \"bool\"\r\n          }\r\n        ],\r\n        \"internalType\": \"struct tokenisation.SaleInfos[]\"\r\n      }\r\n    ],\r\n    \"stateMutability\": \"view\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"getTokenInfoForId\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"_tokenId\",\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"outputs\": [\r\n      {\r\n        \"type\": \"tuple\",\r\n        \"name\": \"\",\r\n        \"components\": [\r\n          {\r\n            \"type\": \"uint256\",\r\n            \"name\": \"tokenId\",\r\n            \"internalType\": \"uint256\"\r\n          },\r\n          {\r\n            \"type\": \"uint256\",\r\n            \"name\": \"assetType\",\r\n            \"internalType\": \"uint256\"\r\n          },\r\n          {\r\n            \"type\": \"int8\",\r\n            \"name\": \"xPosition\",\r\n            \"internalType\": \"int8\"\r\n          },\r\n          {\r\n            \"type\": \"int8\",\r\n            \"name\": \"yPosition\",\r\n            \"internalType\": \"int8\"\r\n          }\r\n        ],\r\n        \"internalType\": \"struct tokenisation.TokenInfos\"\r\n      }\r\n    ],\r\n    \"stateMutability\": \"view\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"getTotalNumberOfNfts\",\r\n    \"inputs\": [],\r\n    \"outputs\": [\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"stateMutability\": \"view\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"getTotalNumberOfTicketsForNft\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"_tokenId\",\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"outputs\": [\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"stateMutability\": \"view\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"isApprovedForAll\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"owner\",\r\n        \"internalType\": \"address\"\r\n      },\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"operator\",\r\n        \"internalType\": \"address\"\r\n      }\r\n    ],\r\n    \"outputs\": [\r\n      {\r\n        \"type\": \"bool\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"bool\"\r\n      }\r\n    ],\r\n    \"stateMutability\": \"view\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"listToExchange\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"_tokenId\",\r\n        \"internalType\": \"uint256\"\r\n      },\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"_price\",\r\n        \"internalType\": \"uint256\"\r\n      },\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"_sharesToSell\",\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"outputs\": [],\r\n    \"stateMutability\": \"nonpayable\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"mint\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"_to\",\r\n        \"internalType\": \"address payable\"\r\n      },\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"_assetType\",\r\n        \"internalType\": \"uint256\"\r\n      },\r\n      {\r\n        \"type\": \"int8\",\r\n        \"name\": \"_xPosition\",\r\n        \"internalType\": \"int8\"\r\n      },\r\n      {\r\n        \"type\": \"int8\",\r\n        \"name\": \"_yPosition\",\r\n        \"internalType\": \"int8\"\r\n      }\r\n    ],\r\n    \"outputs\": [],\r\n    \"stateMutability\": \"nonpayable\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"name\",\r\n    \"inputs\": [],\r\n    \"outputs\": [\r\n      {\r\n        \"type\": \"string\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"string\"\r\n      }\r\n    ],\r\n    \"stateMutability\": \"view\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"nftsOwned\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"address\"\r\n      },\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"outputs\": [\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"stateMutability\": \"view\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"nftsOwners\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"uint256\"\r\n      },\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"address\"\r\n      }\r\n    ],\r\n    \"outputs\": [\r\n      {\r\n        \"type\": \"bool\",\r\n        \"name\": \"isOwner\",\r\n        \"internalType\": \"bool\"\r\n      },\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"owner\",\r\n        \"internalType\": \"address payable\"\r\n      },\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"shares\",\r\n        \"internalType\": \"uint256\"\r\n      },\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"sharesForSelling\",\r\n        \"internalType\": \"uint256\"\r\n      },\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"numTicket\",\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"stateMutability\": \"view\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"owner\",\r\n    \"inputs\": [],\r\n    \"outputs\": [\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"address\"\r\n      }\r\n    ],\r\n    \"stateMutability\": \"view\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"ownerOf\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"tokenId\",\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"outputs\": [\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"address\"\r\n      }\r\n    ],\r\n    \"stateMutability\": \"view\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"ownersList\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"uint256\"\r\n      },\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"outputs\": [\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"address\"\r\n      }\r\n    ],\r\n    \"stateMutability\": \"view\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"ownersSalesTicketsList\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"uint256\"\r\n      },\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"address\"\r\n      },\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"outputs\": [\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"stateMutability\": \"view\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"renounceOwnership\",\r\n    \"inputs\": [],\r\n    \"outputs\": [],\r\n    \"stateMutability\": \"nonpayable\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"safeTransferFrom\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"from\",\r\n        \"internalType\": \"address\"\r\n      },\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"to\",\r\n        \"internalType\": \"address\"\r\n      },\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"tokenId\",\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"outputs\": [],\r\n    \"stateMutability\": \"nonpayable\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"safeTransferFrom\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"from\",\r\n        \"internalType\": \"address\"\r\n      },\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"to\",\r\n        \"internalType\": \"address\"\r\n      },\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"tokenId\",\r\n        \"internalType\": \"uint256\"\r\n      },\r\n      {\r\n        \"type\": \"bytes\",\r\n        \"name\": \"data\",\r\n        \"internalType\": \"bytes\"\r\n      }\r\n    ],\r\n    \"outputs\": [],\r\n    \"stateMutability\": \"nonpayable\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"salesListing\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"uint256\"\r\n      },\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"address\"\r\n      },\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"outputs\": [\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"owner\",\r\n        \"internalType\": \"address payable\"\r\n      },\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"saleTicketNumber\",\r\n        \"internalType\": \"uint256\"\r\n      },\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"price\",\r\n        \"internalType\": \"uint256\"\r\n      },\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"sharesToSell\",\r\n        \"internalType\": \"uint256\"\r\n      },\r\n      {\r\n        \"type\": \"bool\",\r\n        \"name\": \"currentlyForSale\",\r\n        \"internalType\": \"bool\"\r\n      }\r\n    ],\r\n    \"stateMutability\": \"view\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"setApprovalForAll\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"operator\",\r\n        \"internalType\": \"address\"\r\n      },\r\n      {\r\n        \"type\": \"bool\",\r\n        \"name\": \"approved\",\r\n        \"internalType\": \"bool\"\r\n      }\r\n    ],\r\n    \"outputs\": [],\r\n    \"stateMutability\": \"nonpayable\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"supportsInterface\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"bytes4\",\r\n        \"name\": \"interfaceId\",\r\n        \"internalType\": \"bytes4\"\r\n      }\r\n    ],\r\n    \"outputs\": [\r\n      {\r\n        \"type\": \"bool\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"bool\"\r\n      }\r\n    ],\r\n    \"stateMutability\": \"view\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"symbol\",\r\n    \"inputs\": [],\r\n    \"outputs\": [\r\n      {\r\n        \"type\": \"string\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"string\"\r\n      }\r\n    ],\r\n    \"stateMutability\": \"view\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"tokenURI\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"tokenId\",\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"outputs\": [\r\n      {\r\n        \"type\": \"string\",\r\n        \"name\": \"\",\r\n        \"internalType\": \"string\"\r\n      }\r\n    ],\r\n    \"stateMutability\": \"view\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"transferFrom\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"from\",\r\n        \"internalType\": \"address\"\r\n      },\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"to\",\r\n        \"internalType\": \"address\"\r\n      },\r\n      {\r\n        \"type\": \"uint256\",\r\n        \"name\": \"tokenId\",\r\n        \"internalType\": \"uint256\"\r\n      }\r\n    ],\r\n    \"outputs\": [],\r\n    \"stateMutability\": \"nonpayable\"\r\n  },\r\n  {\r\n    \"type\": \"function\",\r\n    \"name\": \"transferOwnership\",\r\n    \"inputs\": [\r\n      {\r\n        \"type\": \"address\",\r\n        \"name\": \"newOwner\",\r\n        \"internalType\": \"address\"\r\n      }\r\n    ],\r\n    \"outputs\": [],\r\n    \"stateMutability\": \"nonpayable\"\r\n  }\r\n]";
    public static NFT[] nfts;


    public void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        UI_Data_Binding.buy_event += async (ticket) => buyNft(ticket);
        UI_Data_Binding.refresh_event += getAllNfts;
        UI_Data_Binding.listToExchange_event += async (ticket) => listToExchangeFromUI(ticket);
    }

    // Function to get all the NFTs related to the map
    public async void getAllNfts()
    {

        Contract contract = ThirdwebManager.Instance.SDK.GetContract(contractAddress, abi);
        
        List<List<object>> allNfts;
    
        int nftCount;

        // GET ALL NFTS INFOS
        allNfts =  await contract.Read<List<List<object>>>("getAllNFTsInfos");

        // GET TOTAL NUMBER OF NFTS
        nftCount = await getTotalNumberOfNfts();


        // INSTANTIATE THE NFTS ARRAY
        nfts = new NFT[nftCount];

        
        // LOOP TO INSTANTIATE ALL NFTS OBJECT FROM RESULTS
        for(int i =0; i<nftCount; i++)
        {
            // Get TokenId
            var json = Convert.ToString(allNfts[i][0]);
            var data = (JObject)JsonConvert.DeserializeObject(json);
            string output = data["Result"].Value<string>();
            int tokenId = int.Parse(output);

            // Get xPosition
            var json2 = Convert.ToString(allNfts[i][2]);
            var data2 = (JObject)JsonConvert.DeserializeObject(json2);
            string output2 = data2["Result"].Value<string>();
            int xPosition = int.Parse(output2);

            // Get yPosition
            var json3 = Convert.ToString(allNfts[i][3]);
            var data3 = (JObject)JsonConvert.DeserializeObject(json3);
            string output3 = data3["Result"].Value<string>();
            int yPosition = int.Parse(output3);

            // Get assetType
            var json4 = Convert.ToString(allNfts[i][1]);
            var data4 = (JObject)JsonConvert.DeserializeObject(json4);
            string output4 = data4["Result"].Value<string>();
            int assetType = int.Parse(output4);

            // Set asset description 
            string assetDescription = getAssetDescription(assetType);

            NFT nft = new NFT(tokenId,assetType, xPosition, yPosition, assetDescription);
            nfts[i] = nft;
        }
        
        // LOOP TO INSTANTIATE THE OWNERLIST OF EACH NFT
        for (int i = 0; i < nftCount; i++)
        {

            List<List<object>> ownersInfos = await contract.Read<List<List<object>>>("getNftsOwners", i);
            int ownersCount = await getTotalNumberOfOwners(i);

            Owners[] owners = new Owners[ownersCount];

            for (int j = 0; j < ownersCount; j++)
            {
                // Get isOwner
                var json = Convert.ToString(ownersInfos[j][0]);
                var data = (JObject)JsonConvert.DeserializeObject(json);
                string output = data["Result"].Value<string>();
                bool isOwner = bool.Parse(output);

                // Get address owner
                var json2 = Convert.ToString(ownersInfos[j][1]);
                var data2 = (JObject)JsonConvert.DeserializeObject(json2);
                string ownerAddress = data2["Result"].Value<string>();

                // Get shares
                var json3 = Convert.ToString(ownersInfos[j][2]);
                var data3 = (JObject)JsonConvert.DeserializeObject(json3);
                string output3 = data3["Result"].Value<string>();
                int shares = int.Parse(output3);

                // Get sharesForSelling
                var json4 = Convert.ToString(ownersInfos[j][3]);
                var data4 = (JObject)JsonConvert.DeserializeObject(json4);
                string output4 = data4["Result"].Value<string>();
                int sharesForSelling = int.Parse(output4);

                Owners owner = new Owners(ownerAddress, shares, sharesForSelling, isOwner);

                owners[j] = owner;
            }

            // Set ownersList in the actual NFT
            nfts[i].setOwnersList(owners);
        }

        // LOOP TO INSTANTIATE THE SALESTICKETLIST OF EACH NFT
        for (int i = 0; i < nftCount; i++)
        {
            int ownersCount = await getTotalNumberOfOwners(i);
            int totalCountOfSales = 0;

            int totalTickets = await getTotalNumberOfSalesTickets(i);
            SalesTickets[] salesTicketsList = new SalesTickets[totalTickets];

            
            // Loop to get all the tickets's sale from an owner of an nft
            for (int j = 0; j < ownersCount; j++)
            {
                List<List<object>> salesInfos = await contract.Read<List<List<object>>>("getSalesInfos", i, nfts[i].getOwners()[j].getOwnerAddress());

                int salesTicketsCount = await getNumberOfSalesTicketsOwner(i, nfts[i].getOwners()[j].getOwnerAddress());

                for (int d = 0; d < salesTicketsCount; d++)
                {
                    // Get seller address
                    var json = Convert.ToString(salesInfos[d][0]);
                    var data = (JObject)JsonConvert.DeserializeObject(json);
                    string seller = data["Result"].Value<string>();

                    // Get ticket number
                    var json2 = Convert.ToString(salesInfos[d][1]);
                    var data2 = (JObject)JsonConvert.DeserializeObject(json2);
                    string output2 = data2["Result"].Value<string>();
                    int ticketNumber = int.Parse(output2);

                    // Get price
                    var json3 = Convert.ToString(salesInfos[d][2]);
                    var data3 = (JObject)JsonConvert.DeserializeObject(json3);
                    string output3 = data3["Result"].Value<string>();
                    int price = int.Parse(output3);

                    // Get sharesToSell
                    var json4 = Convert.ToString(salesInfos[d][3]);
                    var data4 = (JObject)JsonConvert.DeserializeObject(json4);
                    string output4 = data4["Result"].Value<string>();
                    int sharesToSell = int.Parse(output4);

                    // Get currentlyForSale
                    var json5 = Convert.ToString(salesInfos[d][4]);
                    var data5 = (JObject)JsonConvert.DeserializeObject(json5);
                    string output5 = data5["Result"].Value<string>();
                    bool currentlyForSale = bool.Parse(output5);


                    SalesTickets ticket = new SalesTickets(seller, ticketNumber, sharesToSell, price, currentlyForSale, i);

                    salesTicketsList[totalCountOfSales] = ticket;
                    totalCountOfSales++;
                }

            }
            // Set salesTicketList in the actual NFT
            nfts[i].setSalesTickets(salesTicketsList);
            
        }
        
        nftsInitialized?.Invoke(this, EventArgs.Empty);
        
    }
    public void test()
    {
        Debug.Log(nfts[0].getXPosition());
        Debug.Log(nfts[0].getYPosition());
        Debug.Log(nfts[1].getXPosition());
        Debug.Log(nfts[1].getYPosition());
        Debug.Log(nfts[2].getXPosition());
        Debug.Log(nfts[2].getYPosition());
        Debug.Log(nfts[3].getXPosition());
        Debug.Log(nfts[3].getYPosition());
    }
    public async void getTokenInfoForId()
    {
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(contractAddress, abi);

        var obj = await contract.Read<List<object>>("getTokenInfoForId", 0);

    }

    // Get the total number of nfts so far
    public async Task<int> getTotalNumberOfNfts()
    {
        int result = 0;
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(contractAddress, abi);

        // Call the smart contract function
        result = await contract.Read<int>("getTotalNumberOfNfts");

        return result;

    }

    // Get the total number of owners for a given nft
    public async Task<int> getTotalNumberOfOwners(int _tokenId)
    {
        int result = 0;
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(contractAddress, abi);

        // Call the smart contract function
        result = await contract.Read<int>("getNumberOfOwners", _tokenId);

        return result;

    }

    // Get the total number of sales for an owner of an nft
    public async Task<int> getNumberOfSalesTicketsOwner(int _tokenId, string _address)
    {
        int result = 0;
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(contractAddress, abi);

        // Call the smart contract function
        result = await contract.Read<int>("getNumberSalesOwner", _tokenId, _address);

        return result;

    }

    // Get the total number of sales of an nft
    public async Task<int> getTotalNumberOfSalesTickets(int _tokenId)
    {
        int result = 0;
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(contractAddress, abi);

        // Call the smart contract function
        result = await contract.Read<int>("getTotalNumberOfTicketsForNft", _tokenId);

        return result;

    }

    public async Task mint(string _address, int _assetType, int _xPosition, int _yPosition)
    {
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(contractAddress, abi);

        // Call the smart contract function mint
        TransactionResult txRes = await contract.Write("mint", _address, _assetType, _xPosition, _yPosition);

    }

    public async Task listToExchange(int _tokenId, int _price, int _sharesToSell)
    {
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(contractAddress, abi);

        // Call the smart contract function listToExchange
        TransactionResult txRes = await contract.Write("listToExchange", _tokenId, _price, _sharesToSell);
    }

    public async Task listToExchangeFromUI(SalesTickets ticket)
    {
        await listToExchange(ticket.getToken_id(), ticket.getPrice(), ticket.getSharesToSell());
    }

    public async Task buyNft(SalesTickets ticket)
    {
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(contractAddress, abi);

        // Call the smart contract function buyNft
        TransactionResult txRes = await contract.Write("buyNFT",new TransactionRequest() { value = "100", gasLimit = "1000000" }, ticket.getToken_id(), ticket.getSeller(), ticket.getTicketNumber());
    }

    public async Task cancelSale(int _tokenId, int _saleTicket)
    {
        Contract contract = ThirdwebManager.Instance.SDK.GetContract(contractAddress, abi);

        // Call the smart contract function cancelSale
        TransactionResult txRes = await contract.Write("cancelSale", _tokenId, _saleTicket);
    }

    public string getAssetDescription(int _assetType) {
        switch (_assetType)
        {
            case 1:
                return "shoes_shop";
                break;

            case 2:
                return "pizza_store";
                break;

            case 3:
                return "music_store";
                break;

            case 4:
                return "fruit_shop";
                break;

            case 5:
                return "fastfood";
                break;

            case 6:
                return "building_pink";
                break;

            case 7:
                return "building_green";
                break;

            case 8:
                return "house_yellow";
                break;

            case 9:
                return "house_red";
                break;

            case 10:
                return "house_orange";
                break;

            case 11:
                return "building_big_red";
                break;

            case 12:
                return "building_big_blue";
                break;
        }
        return null;
    }
}