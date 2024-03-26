const swapassets = artifacts.require("swapassets");

// #TEST : NFT COUNT INCREMENTATION
contract("swapassets", function(accounts){
    it("should increment the total number of nfts by one when an NFT is minted", function(){
        return swapassets.deployed().then(async function(instance){
            await instance.mint(accounts[0],1,2,2);
            return instance.getTotalNumberOfNfts.call();

        }).then(function(result){
            assert.equal(result,1,"The total number of nfts didn't increased");
        })
    }
    )
    });


// #TEST OWNERSHIP :

// TEST 1 : infos are set correctly for a newly owner in the "OwnerInfos" struct

contract("swapassets", function(accounts){
    it("should set correct value to ownerInfos", function(){
        return swapassets.deployed().then(async function(instance){
            await instance.mint(accounts[0],1,2,2);
            return instance.getOwnerInfos(0, accounts[0]);

        }).then(function(result){
            assert.equal(result[0],true,"The value isOwner not set correctly");
            assert.equal(result[1],accounts[0],"The owner address not set correctly");
            assert.equal(result[2],100,"The shares not set correctly");
            assert.equal(result[3],100,"The sharesForSelling not set correctly");
            assert.equal(result[4],0,"The numTicket not set correctly");
        })
    }
    )
    }
);

// TEST 1 : infos are set correctly for a newly owner in the "OwnerInfos" struct

contract("swapassets", function(accounts){
    it("should set correct value to ownerInfos", function(){
        return swapassets.deployed().then(async function(instance){
            await instance.mint(accounts[0],1,2,2);
            return instance.getOwnerInfos(0, accounts[0]);

        }).then(function(result){
            assert.equal(result[0],true,"The value isOwner not set correctly");
            assert.equal(result[1],accounts[0],"The owner address not set correctly");
            assert.equal(result[2],100,"The shares not set correctly");
            assert.equal(result[3],100,"The sharesForSelling not set correctly");
            assert.equal(result[4],0,"The numTicket not set correctly");
        })
    }
    )
    }
);

// TEST 2 : Newly owner should detain the NFT

contract("swapassets", function(accounts){
    it("should transfer the NFT to smart contract", function(){
        return swapassets.deployed().then(async function(instance){
            await instance.mint(accounts[0],1,2,2);
            return instance.ownerOf(0);

        }).then(function(result){
            assert.equal(result, accounts[0], "The owner should detain the NFT");
        })
    }
    )
    }
);

// TEST 3 : When 100% owner create a ticket sale, OwnerInfos values are set correctly

contract("swapassets", function(accounts){
    it("should set correct value to ownerInfos + transfer the NFT to smart contract", function(){
        return swapassets.deployed().then(async function(instance){
            await instance.mint(accounts[0],1,2,2);
            await instance.listToExchange(0, 10, 20);
            return instance.getOwnerInfos(0, accounts[0]);

        }).then(function(result){
            assert.equal(result[0],true,"The value isOwner should stay to true");
            assert.equal(result[1],accounts[0],"The owner address shouldn't change");
            assert.equal(result[2],100,"The shares shouldn't change when a ticket sale is created");
            assert.equal(result[3],80,"The sharesForSelling didn't subtracted correctly");
            assert.equal(result[4],1,"The numTicket didn't increased by one");
        })
    }
    )
    }
);


// TEST 4 : When 100% owner create a ticket sale, the nft is transfered to the smart contract
contract("swapassets", function(accounts){
    it("should transfer the NFT to smart contract", function(){
        return swapassets.deployed().then(async function(instance){
            await instance.mint(accounts[0],1,2,2);
            await instance.listToExchange(0, 10, 20);
            return instance.ownerOf(0);

        }).then(function(result){
            assert.notEqual(result, accounts[0], "The owner shouldn't detain the NFT");
        })
    }
    )
    }
);

// TEST 5 : In case of a sale, if the buyer is a new owner and the seller still an owner of the NFT, the OwnerInfos of the seller is correctly set
contract("swapassets", function(accounts){
    it("should set the OwnerInfos of the seller correctly", function(){
        return swapassets.deployed().then(async function(instance){
            await instance.mint(accounts[0],1,2,2);
            await instance.listToExchange(0, 10, 20);
            await instance.buyNFT(0, accounts[0], 0, {from: accounts[1], value: 10});
            return instance.getOwnerInfos(0, accounts[0]);

        }).then(function(result){
            assert.equal(result[0],true,"The value isOwner should stay to true");
            assert.equal(result[1],accounts[0],"The owner address shouldn't change");
            assert.equal(result[2],80,"Since the sale occured, the shares of the seller have to be updated");
            assert.equal(result[3],80,"The sharesForSelling shouldn't change after a sale");
            assert.equal(result[4],1,"The numTicket shouldn't change after a sale");
        })
    }
    )
    }
);

// TEST 6 : In case of a sale, if the buyer is a new owner and the seller still an owner of the NFT, the OwnerInfos of the buyer is correctly set
contract("swapassets", function(accounts){
    it("should set the OwnerInfos of the buyer correctly", function(){
        return swapassets.deployed().then(async function(instance){
            await instance.mint(accounts[0],1,2,2);
            await instance.listToExchange(0, 10, 20);
            await instance.buyNFT(0, accounts[0], 0, {from: accounts[1], value: 10});
            return instance.getOwnerInfos(0, accounts[1]);

        }).then(function(result){
            assert.equal(result[0],true,"The value isOwner should be true");
            assert.equal(result[1],accounts[1],"The owner address should be correctly set");
            assert.equal(result[2],20,"Since the sale occured, the shares of the buyer have to be updated");
            assert.equal(result[3],20,"The sharesForSelling shouldn't change after a sale");
            assert.equal(result[4],0,"The numTicket shouldn't change after a sale");
        })
    }
    )
    }
);

// TEST 7 : In case of a sale, if the buyer is a new owner and the seller still an owner of the NFT, the OwnerList of the NFT is correctly set
contract("swapassets", function(accounts){
    it("should set the OwnerList of the NFT correctly", function(){
        return swapassets.deployed().then(async function(instance){
            await instance.mint(accounts[0],1,2,2);
            await instance.listToExchange(0, 10, 20);
            await instance.buyNFT(0, accounts[0], 0, {from: accounts[1], value: 10});
            return instance.getNftOwnerList(0);

        }).then(function(result){
            assert.equal(result.length, 2, "The length of the OwnerList should be 2");
            assert.equal(result[0], accounts[0], "The seller address should be in the OwnerList");
            assert.equal(result[1], accounts[1], "The buyer address should be in the OwnerList");
        })
    }
    )
    }
);

// TEST 8 : In case of a sale, if the buyer is a new owner and the seller still an owner of the NFT, the nftOwned of the seller is correctly set
contract("swapassets", function(accounts){
    it("should set the nftOwned of the seller correctly ", function(){
        return swapassets.deployed().then(async function(instance){
            await instance.mint(accounts[0],1,2,2);
            await instance.listToExchange(0, 10, 20);
            await instance.buyNFT(0, accounts[0], 0, {from: accounts[1], value: 10});
            return instance.getListOfNftsOwned(accounts[0]);

        }).then(function(result){
            assert.equal(result[0], 0, "The tokenId of the NFT should still be in the nftOwned array of the seller");
        })
    }
    )
    }
);

// TEST 9 : In case of a sale, if the buyer is a new owner and the seller still an owner of the NFT, the nftOwned of the buyer is correctly set
contract("swapassets", function(accounts){
    it("should set the nftOwned of the buyer correctly ", function(){
        return swapassets.deployed().then(async function(instance){
            await instance.mint(accounts[0],1,2,2);
            await instance.listToExchange(0, 10, 20);
            await instance.buyNFT(0, accounts[0], 0, {from: accounts[1], value: 10});
            return instance.getListOfNftsOwned(accounts[1]);

        }).then(function(result){
            assert.equal(result[0], 0, "The tokenId of the NFT should be in the nftOwned array of the buyer");
        })
    }
    )
    }
);

// TEST 10 : In case of a sale, if the buyer is a new owner and the seller is not an owner of the NFT anymore, the OwnerInfos of the seller is correctly set
contract("swapassets", function(accounts){
    it("should set the OwnerInfos of the seller correctly -2", function(){
        return swapassets.deployed().then(async function(instance){
            await instance.mint(accounts[0],1,2,2);
            await instance.listToExchange(0, 10, 100);
            await instance.buyNFT(0, accounts[0], 0, {from: accounts[1], value: 10});
            return instance.getOwnerInfos(0, accounts[0]);

        }).then(function(result){
            assert.equal(result[0],false,"The value isOwner be false");
            assert.equal(result[1],accounts[0],"The owner address shouldn't change");
            assert.equal(result[2],0,"Since the sale occured, the shares of the seller should be 0");
            assert.equal(result[3],0,"The sharesForSelling shouldn't change after a sale");
            assert.equal(result[4],1,"The numTicket shouldn't change after a sale");
        })
    }
    )
    }
);

// TEST 11 : In case of a sale, if the buyer is a new owner and the seller is not an owner of the NFT anymore, the OwnerInfos of the buyer is correctly set
contract("swapassets", function(accounts){
    it("should set the OwnerInfos of the buyer correctly -2 ", function(){
        return swapassets.deployed().then(async function(instance){
            await instance.mint(accounts[0],1,2,2);
            await instance.listToExchange(0, 10, 100);
            await instance.buyNFT(0, accounts[0], 0, {from: accounts[1], value: 10});
            return instance.getOwnerInfos(0, accounts[1]);

        }).then(function(result){
            assert.equal(result[0],true,"The value isOwner should be true");
            assert.equal(result[1],accounts[1],"The owner address should be set correctly");
            assert.equal(result[2],100,"Since the sale occured, the shares of the buyer should be 100");
            assert.equal(result[3],100,"The sharesForSelling should be 100");
            assert.equal(result[4],0,"The numTicket should be 0");
        })
    }
    )
    }
);

// TEST 12 : In case of a sale, if the buyer is a new owner and the seller is not an owner of the NFT anymore, the OwnerList of the NFT is correctly set
contract("swapassets", function(accounts){
    it("should set the OwnerList of the NFT correctly -2 ", function(){
        return swapassets.deployed().then(async function(instance){
            await instance.mint(accounts[0],1,2,2);
            await instance.listToExchange(0, 10, 100);
            await instance.buyNFT(0, accounts[0], 0, {from: accounts[1], value: 10});
            return instance.getNftOwnerList(0);

        }).then(function(result){
            assert.equal(result.length, 1, "The length of the OwnerList should be 1");
            assert.equal(result[0], accounts[1], "The buyer address should be in the OwnerList");
        })
    }
    )
    }
);

// TEST 13 : In case of a sale, if the buyer is a new owner and the seller is not an owner of the NFT anymore, the nftOwned of the seller is correctly set
contract("swapassets", function(accounts){
    it("should set the nftOwned of the seller correctly -2", function(){
        return swapassets.deployed().then(async function(instance){
            await instance.mint(accounts[0],1,2,2);
            await instance.listToExchange(0, 10, 100);
            await instance.buyNFT(0, accounts[0], 0, {from: accounts[1], value: 10});
            return instance.getListOfNftsOwned(accounts[0]);

        }).then(function(result){
            assert.equal(result.length, 0, "The tokenId of the NFT shouldn't be in the nftOwned array of the seller");
        })
    }
    )
    }
);

// TEST 14 : In case of a sale, if the buyer is a new owner and the seller is not an owner of the NFT anymore, the nftOwned of the buyer is correctly set
contract("swapassets", function(accounts){
    it("should set the nftOwned of the buyer correctly -2", function(){
        return swapassets.deployed().then(async function(instance){
            await instance.mint(accounts[0],1,2,2);
            await instance.listToExchange(0, 10, 100);
            await instance.buyNFT(0, accounts[0], 0, {from: accounts[1], value: 10});
            return instance.getListOfNftsOwned(accounts[1]);

        }).then(function(result){
            assert.equal(result[0], 0, "The tokenId of the NFT should be in the nftOwned array of the buyer");
        })
    }
    )
    }
);

// TEST 15 : In case of a sale, if the buyer is already an owner and the seller is not an owner of the NFT anymore, the OwnerInfos of the seller is correctly set
contract("swapassets", function(accounts){
    it("should set the OwnerInfos of the seller correctly -3", function(){
        return swapassets.deployed().then(async function(instance){
            await instance.mint(accounts[0],1,2,2);
            await instance.listToExchange(0, 10, 50);
            await instance.listToExchange(0, 10, 50);
            await instance.buyNFT(0, accounts[0], 0, {from: accounts[1], value: 10});
            await instance.buyNFT(0, accounts[0], 1, {from: accounts[1], value: 10});
            return instance.getOwnerInfos(0, accounts[0]);

        }).then(function(result){
            assert.equal(result[0],false,"The value isOwner be false");
            assert.equal(result[1],accounts[0],"The owner address shouldn't change");
            assert.equal(result[2],0,"Since the sale occured, the shares of the seller should be 0");
            assert.equal(result[3],0,"The sharesForSelling shouldn't change after a sale");
            assert.equal(result[4],2,"The numTicket shouldn't change after a sale");
        })
    }
    )
    }
);

// TEST 16 : In case of a sale, if the buyer is already an owner and the seller is not an owner of the NFT anymore, the OwnerInfos of the buyer is correctly set
contract("swapassets", function(accounts){
    it("should set the OwnerInfos of the buyer correctly -3 ", function(){
        return swapassets.deployed().then(async function(instance){
            await instance.mint(accounts[0],1,2,2);
            await instance.listToExchange(0, 10, 50);
            await instance.listToExchange(0, 10, 50);
            await instance.buyNFT(0, accounts[0], 0, {from: accounts[1], value: 10});
            await instance.buyNFT(0, accounts[0], 1, {from: accounts[1], value: 10});
            return instance.getOwnerInfos(0, accounts[1]);

        }).then(function(result){
            assert.equal(result[0],true,"The value isOwner shouldn't change");
            assert.equal(result[1],accounts[1],"The owner address shouldn't change");
            assert.equal(result[2],100,"Since the sale occured, the shares of the buyer should be 100");
            assert.equal(result[3],100,"The sharesForSelling should be 100");
            assert.equal(result[4],0,"The numTicket should be 0");
        })
    }
    )
    }
);

// TEST 17 : In case of a sale, if the buyer is already an owner and the seller is not an owner of the NFT anymore, the OwnerList of the NFT is correctly set
contract("swapassets", function(accounts){
    it("should set the OwnerList of the NFT correctly -3 ", function(){
        return swapassets.deployed().then(async function(instance){
            await instance.mint(accounts[0],1,2,2);
            await instance.listToExchange(0, 10, 50);
            await instance.listToExchange(0, 10, 50);
            await instance.buyNFT(0, accounts[0], 0, {from: accounts[1], value: 10});
            await instance.buyNFT(0, accounts[0], 1, {from: accounts[1], value: 10});
            return instance.getNftOwnerList(0);

        }).then(function(result){
            assert.equal(result.length, 1, "The length of the OwnerList should be 1");
            assert.equal(result[0], accounts[1], "The buyer address should be in the OwnerList");
        })
    }
    )
    }
);

// TEST 18 : In case of a sale, if the buyer is already an owner and the seller is not an owner of the NFT anymore, the nftOwned by the seller is correctly set
contract("swapassets", function(accounts){
    it("should set the nftOwned of the seller correctly -3", function(){
        return swapassets.deployed().then(async function(instance){
            await instance.mint(accounts[0],1,2,2);
            await instance.listToExchange(0, 10, 50);
            await instance.listToExchange(0, 10, 50);
            await instance.buyNFT(0, accounts[0], 0, {from: accounts[1], value: 10});
            await instance.buyNFT(0, accounts[0], 1, {from: accounts[1], value: 10});
            return instance.getListOfNftsOwned(accounts[0]);

        }).then(function(result){
            assert.equal(result.length, 0, "The tokenId of the NFT shouldn't be in the nftOwned array of the seller");
        })
    }
    )
    }
);

// TEST 18 : In case of a sale, if the buyer is already an owner and the seller is not an owner of the NFT anymore, the nftOwned by the buyer is correctly set
contract("swapassets", function(accounts){    
    it("should set the nftOwned of the buyer correctly -3", function(){
        return swapassets.deployed().then(async function(instance){
            await instance.mint(accounts[0],1,2,2);
            await instance.listToExchange(0, 10, 50);
            await instance.listToExchange(0, 10, 50);
            await instance.buyNFT(0, accounts[0], 0, {from: accounts[1], value: 10});
            await instance.buyNFT(0, accounts[0], 1, {from: accounts[1], value: 10});
            return instance.getListOfNftsOwned(accounts[1]);

        }).then(function(result){
            assert.equal(result[0], 0, "The tokenId of the NFT should be in the nftOwned array of the buyer");
        })
    }
    )
    }
);

// TEST 19 : In case of a sale, if the buyer is already an owner and the seller is already an owner of the NFT too, the OwnerInfos of the seller is correctly set
contract("swapassets", function(accounts){
    it("should set the OwnerInfos of the seller correctly -4", function(){
        return swapassets.deployed().then(async function(instance){
            await instance.mint(accounts[0],1,2,2);
            await instance.listToExchange(0, 10, 10);
            await instance.listToExchange(0, 10, 50);
            await instance.buyNFT(0, accounts[0], 0, {from: accounts[1], value: 10});
            await instance.buyNFT(0, accounts[0], 1, {from: accounts[1], value: 10});
            return instance.getOwnerInfos(0, accounts[0]);

        }).then(function(result){
            assert.equal(result[0],true,"The value isOwner shouldn't change");
            assert.equal(result[1],accounts[0],"The owner address shouldn't change");
            assert.equal(result[2],40,"Since the sale occured, the shares of the seller should be 40");
            assert.equal(result[3],40,"The sharesForSelling shouldn't change after a sale");
            assert.equal(result[4],2,"The numTicket shouldn't change after a sale");
        })
    }
    )
    }
);

// TEST 20 : In case of a sale, if the buyer is already an owner and the seller is already an owner of the NFT too, the OwnerInfos of the buyer is correctly set
contract("swapassets", function(accounts){
    it("should set the OwnerInfos of the buyer correctly -4 ", function(){
        return swapassets.deployed().then(async function(instance){
            await instance.mint(accounts[0],1,2,2);
            await instance.listToExchange(0, 10, 10);
            await instance.listToExchange(0, 10, 50);
            await instance.buyNFT(0, accounts[0], 0, {from: accounts[1], value: 10});
            await instance.buyNFT(0, accounts[0], 1, {from: accounts[1], value: 10});
            return instance.getOwnerInfos(0, accounts[1]);

        }).then(function(result){
            assert.equal(result[0],true,"The value isOwner shouldn't change");
            assert.equal(result[1],accounts[1],"The owner address shouldn't change");
            assert.equal(result[2],60,"Since the sale occured, the shares of the buyer should be 60");
            assert.equal(result[3],60,"The sharesForSelling should be 60");
            assert.equal(result[4],0,"The numTicket should be 0");
        })
    }
    )
    }
);