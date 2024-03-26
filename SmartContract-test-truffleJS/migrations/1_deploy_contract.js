const swapAssets = artifacts.require('swapassets.sol');

module.exports = function (deployer) {
    deployer.deploy(swapAssets);
}