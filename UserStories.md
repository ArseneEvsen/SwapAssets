# User Story 1

En tant qu'utilisateur,
je veux pouvoir voir les NFTs existant,
afin de pouvoir consulter leurs informations 

Contexte :
Le smart-contract auquel est lié swapAssets agis en tant qu'exchange d'NFT.
Dans ce smart-contract, il est nécessaire de définir les informations qui décrivent chaque NFT.

## Fonctionnalité :

En tant qu'utilisateur, lorsque je consulte un NFT, je veux pouvoir consulter les informations qui le décrivent : son numéro unique, son type d'asset, ses coordonnées spatiales.

## Exigences techniques :

#### R1 : Forme des données

Dans le smart-contract, les données lié à un NFT prennent la forme d'un struct "TokenInfos" composé des variables :
	- "tokenId" de type uint
	- "assetType" de type uint
	- "xPosition" de type int8
	- "yPosition" de type int8

#### R2 : Optimisation de gas et sécurité 

Afin de retrouver chaque struct "TokenInfos", de manière efficiente en terme de computation et donc de consommation de gas, mais aussi pour éviter de rendre le smart-contract inutilisable, un mapping 'allTokensInfos''est utilisé avec : 
	- pour clé : le token id, et pour valeur : le struct "TokenInfos"

#### R3 : Forme des données

Dans le smart-contract, il est nécessaire qu'un NFT ait un, ou plusieurs propriétaire.
Chaque propriétaire ayant un nombre de part de l'NFT et un nombre de part pouvant être mis en vente.
Ces informations prennent la forme du struct "OwnerInfos" composé des variables :
	- "isOwner" de type bool
	- "owner" de type address
	- "shares" de type uint
	- "sharesForSelling" de type uint
	- "numTicket" de type uint

# User Story 2

En tant qu'utilisateur propriétaire,
je veux pouvoir consulter les NFTs dont je suis propriétaire,
afin de pouvoir reconnaître les NFTs dont je suis propriétaire

Contexte :
Le smart-contract auquel est lié swapAssets agis en tant qu'exchange d'NFT.
Dans ce smart-contract, il est nécessaire de définir les informations qui décrivent les propriétaires de chaque NFT.
## Fonctionnalité :

#### 1 :
En tant qu'utilisateur propriétaire, lorsque je consulte un NFT, je veux pouvoir savoir si je suis détenteur de celui-ci, afin de pouvoir reconnaître les NFTs dont je suis propriétaire.

#### 2 :
En tant que propriétaire d'un NFT, 
lorsque je crée une vente, je liste mon NFT partiellement ou totalement dans l'exchange,
ainsi je transfère mon NFT au smart-contract pour qu'il puisse gérer la vente.

## Exigences techniques :

#### R1 : Optimisation de gas et sécurité 

Afin de retrouver chaque struct "OwnerInfos", de manière efficiente en terme de computation et donc de consommation de gas, mais aussi pour éviter de rendre le smart-contract inutilisable, un double mapping "nftsOwners''est utilisé avec : 
	- clé du premier mapping : le token id, valeur : mapping suivant
	- clé du second mapping : adresse du propriétaire, valeur : struct "OwnerInfos"

#### R2 : Retrouver/Itérer les propriétaires d'un NFT

Puisqu'il est nécessaire de connaître en amont la clé d'un mapping pour accéder à sa valeur, dans le cadre du double mapping "nftsOwners", pour accéder au information d'un propriétaire, il faut connaître son adresse.
Pour répondre à cette exigence : création d'un mapping "ownersList" qui contient à tableau listant les adresses propriétaires d'un NFT :
	- clé du mapping : tokenId, valeur : tableau 

#### R3 : Retrouver/Itérer les NFTs détenu par un propriétaire

Utilisation d'un mapping qui contient un tableau listant les tokenId d'NFT détenu :
	- clé du mapping : adresse propriétaire, valeur : tableau

 # User Story 3

En tant qu'utilisateur propriétaire,
je veux pouvoir choisir de mettre en vente une partie d'un NFT que je détient,
afin de ne pouvoir en vendre qu'une partie et rester propriétaire

Contexte :
Le smart-contract auquel est lié swapAssets agis en tant qu'exchange d'NFT.
Dans ce smart-contract, il doit être possible de fractionner un NFT afin qu'il ait plusieurs propriétaires. Lorsqu'un propriétaire veut vendre des parts, il crée un ticket de vente.

## Fonctionnalité :

#### 1 :
En tant qu'utilisateur propriétaire,
je peut choisir combien de part d'un NFT je souhaite vendre,
afin de créer un ticket de vente qui me permettra de rester propriétaire.

Il est possible pour un propriétaire de créer autant de ticket de vente qu'il a de part dans un NFT.

#### 2 :
En tant qu'utilisateur propriétaire,
je peut annuler un ticket de vente,
afin de retirer de la vente les parts.

Lorsqu'un ticket est annulé, les parts théoriques initialement mises en ventes retournent aux propriétaires.

#### 3 :
En tant que propriétaire d'un NFT, 
lorsque je crée une vente, je liste mon NFT partiellement ou totalement dans l'exchange,
ainsi je transfère mon NFT au smart-contract pour qu'il puisse gérer la vente.
## Exigences techniques : 

#### R1 : 

Pour des exigences de sécurités, la somme des parts à vendre des tickets de vente d'un propriétaire, ne peuvent excéder le nombre de part réel qu'il détient d'un NFT.
Pour cela, dans le struct 'OwnerInfos', il existe deux variables liés à ce cas :
	- "shares" : les parts que détient un propriétaire, elle ne changent que lorsqu'une vente ou un achat est conclu
	- "sharesForSelling" : les parts théoriques disponible à la vente, sa valeur change lorsqu'un ticket est créé/annulé, lorsqu'une vente ou un achat est conclu.

#### R2 : Forme des données

Les données qui décrivent un ticket de vente sont présent dans le struct "SaleInfos". Ce struct est composé des variables suivantes :
	- "owner" de type address
	- "saleTicketNumber" de type uint
	- "price" de type uint
	- "sharesToSell" de type uint
	- "currentlyForSale" de type bool

#### R3 : Liaison entre les tickets / son propriétaire / l'NFT

Afin de lier plusieurs tickets à son propriétaire, lui-même lié à son NFT, l'utilisation d'un triple mapping "salesListing" est requis. Ceci aussi pour répondre aux exigences de sécurité et de performance du smart-contract. 

Ce triple mapping se compose de :
	- clé du premier mapping : tokenId, valeur : mapping suivant
	- clé du second mapping : l'adresse du propriétaire, valeur : mapping suivant 
	- clé du troisième mapping : le numéro de ticket, valeur : struct "SaleInfos"

#### R4 : Retrouver/Itérer les tickets de ventes 

Puisqu'il est nécessaire de connaître en amont la clé d'un mapping pour accéder à sa valeur, dans le cadre du tripkle mapping "salesListing", pour accéder au information d'un ticket de vente, il faut connaître son numéro de ticket.
Pour répondre à cette exigence : création d'un double mapping "ownersSalesTicketsList" qui contient à tableau listant les numéros de tickets d'un propriétaires NFT :
	- clé du premier mapping : tokenId, valeur : mapping suivant
	- clé du second mapping : adresse du propriétaire, valeur : tableau contenant les tickets de ventes

#### R5 : Exigences de sécurités
Lorsqu'un utilisateur souhaite lister un NFT pour une vente alors,
le smart-contract doit vérifier :
	- si l'NFT existe
	- si l'utilisateur qui appel la fonction de vente, est bien un des utilisateurs de l'NFT
	- si le nombre de part disponible à la vente que détient le propriétaire est inférieur ou égal au nombre de part qu'il veut vendre dans ce ticket
	- si le prix de vente est supérieur à 0

#### R6 :
Lorsqu'une vente est annulé, les parts "sharesForSelling" du propriétaire : 
	- sont incrémenté par la valeur des parts du tickets "sharesToSell" du struct "SaleInfos"

 # User Story 4

En tant qu'utilisateur acheteur,
je veux pouvoir acheter un NFT disponible en vente,
afin de devenir propriétaire de cet NFT

Contexte :
Le smart-contract auquel est lié swapAssets agis en tant qu'exchange d'NFT.
Dans ce smart-contract, il doit être possible d'acheter un NFT à partir d'un ticket de vente 
afin de devenir propriétaire de cet NFT

## Fonctionnalité :

#### 1 :
En tant qu'utilisateur,
Lorsque j'achète un NFT à partir d'un ticket de vente,
alors je devient propriétaire de cet NFT

#### 2 :
Lorsqu'un achat est effectué,
alors les parts mises en ventes dans le ticket sont transféré depuis les parts du vendeur, vers les parts de l'acheteur

#### 3 :
Lorsqu'un achat est effectué, 
il peut exister quatre scénarios :
	- le vendeur n'est plus propriétaire et l'acheteur est un nouveau propriétaire
	- le vendeur n'est plus propriétaire et l'acheteur n'est pas un nouveau propriétaire
	- le vendeur est toujours propriétaire et l'acheteur est un nouveau propriétaire
	-  le vendeur est toujours propriétaire et l'acheteur n'est pas un nouveau propriétaire
## Exigences : 

#### R1 : Exigence de sécurité 

Lorsqu'un utilisateur souhaite acheter un NFT, il appel un fonction d'achat dans lequel il doit fournir :
	- l'adresse du vendeur
	- le token Id de l'NFT 
	- le numéro de ticket de vente

Il est nécessaire de vérifier en amont :
	- si l'NFT existe
	- si le numéro de ticket existe
	- si l'utilisateur a bien envoyé assez de fond pour acheter les parts de l'NFT

#### R2 :

Lorsqu'une vente ou un achat est conclu les parts en vente "sharesToSell" du struct "SaleInfos" :
	- sont incrémenté aux parts de l'acheteur, dans la variable "shares" du struct "OwnerInfos"
	- sont incrémenté aux parts disponible pour la vente, de l'acheteur, dans la variable "sharesForSelling" du struct "OwnerInfos"
	- sont décrémenté aux parts du vendeur, dans la variable "shares" du struct "OwnerInfos"
	- sont décrémenté aux parts du vendeur, dans la variable "shares" du struct "OwnerInfos"


