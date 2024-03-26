## Ownership tests

#### Context :
In case of an exchange and the fact that an NFT can be owned by multiples owners through "shares owning", the ownership with SwapAssets has two ways of being.

The first one, and most common in the NFT universe, is the way that an wallet address own the NFT. In this case, in the state of the swapAssets smart-contact, the owner have "100"% of the "share".
However, the second way is more complicated : 
	- if the owner, even with 100% of shares, want to sell his NFT by creating a sale ticket, he have to send the NFT to the smart-contract for the trade purpose.
	- if the NFT have more that one owner, no one really own the NFT like the first way we discuss. The NFT is detained by the smart-contract but the all the owners have a certain amount of shares.
	- if one owner get 100% of shares, the NFT is automatically send to the owner's wallet.

#### Test 1:
When an NFT is minted for the first time, it has logically one owner with 100% of shares + he actually detain the NFT in his wallet address.

In this test, we test that :
	- the newly owner hold the NFT in his wallet
	- all his "OwnerInfos" are set correctly :
		- isOwner == true
		- owner == owner's address
		- shares = 100(%)
		- sharesForSelling == 100(%)
		- numTicket = 0

#### Test 2:
When a newly 100% owner list his NFT to the exchange by creating a sale ticket, we test that :
	- The owner doesn't detain the NFT anymore, but the smart-contract does
	- the shares is still at 100% but the sharesForSelling == 100 - the amount of shares the owner want to sell in his sale ticket
	- the sharesForSelling is subtracted by the amount of shares the owner decide to sell for this sale
	- numTicket = 1 since one sale ticket has be created so far

#### Test 3:
When a sale occurs, there is four scenarios :
	- the seller is no longer the owner and the buyer is a new owner 
	- the seller is no longer the owner and the buyer is not a new owner 
	- The seller is still the owner and the buyer is a new owner 
	- the seller is still the owner and the buyer is not a new owner

##### Test 3A :
Case : If the buyer is a new owner of the NFT && If the seller is still an owner of the NFT

We test that : 
	- seller shares minus + sharesSelling
	- seller still isOwner = true
	- buyer shares/sharesSelling = sale ticket stats
	- buyer = isOwner == true
	- OwnerList[] -> buyer added
	- NftOwned[] -> tokenId added

##### Test 3B :
Case : If the buyer is a new owner of the NFT && If the seller is not an owner anymore

We test that :
	- seller shares/sharesSelling = 0
	- seller isOwner = false
	- buyer shares/sharesSelling = sale ticket stats
	- buyer = isOwner == true
	- OwnerList[] -> seller remove 
	- OwnerList[] -> buyer added
	- NftOwned[] -> seller remove
	- NftOwned[] -> tokenId added

##### Test 3C :
Case : If the buyer is already an owner of the NFT && If the seller is not an owner anymore 

We test that : 
	- seller shares/sharesSelling = 0
	- seller isOwner = false
	- buyer shares/sharesSelling = base + sale ticket stats
	- buyer = isOwner == true
	- OwnerList[] -> seller remove
	-  NftOwned[] -> seller remove

##### Test 3D :
Case : If the buyer is already an owner && If the seller is still an owner of the NFT

We test that :
	- seller shares/sharesSelling = minus sale ticket stats
	- seller still isOwner = true
	- buyer shares/sharesSelling = base + sale ticket stats
	- buyer = isOwner == true


