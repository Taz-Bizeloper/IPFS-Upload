const { NFTStorage, File } = require('nft.storage'); // link to API documentation https://nft.storage/docs/client/js/
const fs = require('fs');
const args = process.argv.slice(2);
var mime = require('mime');

const filePath = args[0];
const fileName = args[1];
const fileDescr = args[2];
const fileNameFromPath = filePath.split("\\")[filePath.split("\\").length - 1];

const API_KEY = "NFT_STORAGE_API_KEY" //sign up for a free API key at https://nft.storage

async function storeAsset() {
    const client = new NFTStorage({ token: API_KEY })

    //https://nft.storage/docs/client/js/#store---store-erc1155-nft-data
    const metadata = await client.store({
        name: fileName,
        description: fileDescr,
        image: new File(
            [await fs.promises.readFile(filePath)],
            fileNameFromPath,
            { type: mime.getType(filePath) }
        ),
    });
    return metadata.ipnft;
}

storeAsset()
    .then((metadata) => {
        console.log(metadata);
        process.stdout = metadata;
        process.exit(0)
    })
    .catch((error) => {
        console.error(error);
        process.stdout = JSON.stringify(error);
        process.exit(1);
    });