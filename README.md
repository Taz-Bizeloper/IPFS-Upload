<h1 align="center">
  <a href="https://FreeNFTMint.app"><img width="30%" src="https://freenftmint.app/assets/images/logo.png" alt="Free NFT Mint App logo" /></a>
</h1>

<h3 align="center">C# WebAPI to store ERC1155 NFT data onto IPFS with NFT.Storage NodeJS Module </h3>

<h4 align="center">Made With 🧡 By The <a href="https://Bizelop.app">Bizelop</a> Community </h4>
<p align="center">
  <a href="https://discord.gg/bizelop"><img src="https://img.shields.io/badge/chat-discord?style=for-the-badge&logo=discord&label=discord&logoColor=7389D8&color=ff6501" /></a>
  <a href="https://twitter.com/mmwirelesstech"><img alt="Twitter Follow" src="https://img.shields.io/twitter/follow/mmwirelesstech?color=ff6501&label=twitter&logo=twitter&style=for-the-badge"></a>
</p>



# About
This is a server side C# WebAPI that uses the [NFT.Storage](https://nft.storage) NodeJS API to upload data to IPFS and get back the metadata hash location. 


This integration is done through [NFT.storage](NFT.storage) on a server side.

Feel free to use the API end point or create your own endpoint. 

```
https://freenftmint.app/api/UploadFile
```

The endpoint accepts a FormData that expects to have a filename with the extension, metadata NFT name (appears as the name of the NFT), metadata NFT description (appears as the desciption of the NFT), and the file. The uploaded images are not stored on any centralized servers and are uploaded directly to IPFS. The response will be the IPFS hash. You then need to construct the final URL your liking using either the "ipfs://" protocol or using an IPFS gateway like "https://gateway.ipfs.io/ipfs/"

```
//response will look like this //bafyreieimn4csj24nj6h2txaan3k5iqvjospubony2a2e6ivmpayxvip24

var ipfsHash = "bafyreieimn4csj24nj6h2txaan3k5iqvjospubony2a2e6ivmpayxvip24"

//construct the metadata.json  
var url = "ipfs://" + ipfsHash + "/metadata.json"; 
OR
var url = "https://gateway.ipfs.io/ipfs/" + ipfsHash + "/metadata.json";

```
## Prerequisites

Node version 16.13.0 (This version is what we used) 
 
 - Use Node Version Manager (nvm) to install and use different node versions on the same machine

Windows Server or Windows PC
 - [Visual Studio Community](https://visualstudio.microsoft.com/vs/older-downloads/) or licensed (Not Visual Code) 

## Initial Setup 

Clone the  project to your local machine

```
git clone https://github.com/bizelop/IPFS-Upload.git
```

cd to the C# Project folder (Two levels deep) 

```
cd IPFS-Upload\IPFS-Upload
```

install node dependencies (see package.json)

```
npm install
```

Create an [NFT.Storage](https://nft.storage/) account and get your API key

in 
```
upload-ipfs.js
```
populate the API_KEY variable

```
const API_KEY = "NFT_STORAGE_API_KEY";
```

Open the IPFS-Upload.sln or the IPFS-Upload.csproj in Visual Studio

Build your client side to send FormData to your endpoint.


Run the application using IIS Express "http://localhost:52371/" settings or create your own local IIS "http://localhost/IPFS-Upload" (Right click the project --> Properties --> Web --> Servers)

You can use the [FreeNFTMint-Angular-dApp](https://github.com/bizelop/FreeNFTMint-Angular-dApp) client project and update the UploadFile API link in the client code to point to your local endpoint to test your local code

# Data to send
Below is example code snippet of how to construct the properties needed to send to API end point

```
var ipfsUploadApiUrl = "https://freenftmint.app/api/UploadFile";

var data = new FormData();
data.append("filePath", this.file.name); //contains extension (.jpg,.gif,.png) to temp store
data.append("fileName", this.imgName); //NFT name 
data.append("fileDescr", this.imgDescr); //NFT description
data.append("UploadedImage", this.file);//file blob from files input

var ipfsHash = await this.http.post(this.rootVars.ipfsUploadApiUrl, data).toPromise(); //Angular HTTP Service 

var url = "ipfs://" +ipfsHash + "/metadata.json";

```

# Publish To Server 
The Windows Server must have NodeJS installed via [NVM](https://github.com/coreybutler/nvm-windows) so that it becomes accessible to the IIS service account. 

If you do not have an IIS server you can take the upload-ipfs.js file and use it with your own server code.

Alternatively you can use the upload-ipfs.js file directly in your client dApp if it supports JavaScript. The downside is if you have a website this will expose the NFT.Storage API_KEY but if developing an Android/iOS Native WebApp then you may not be worried about that and so you do not need a WebAPI and can use the NFT.Storage nodeJS module directly in your client code

If you need any help or have issues please join the (Discord)[https://discord.gg/bizelop]

# License

```
MIT License

Copyright (c) 2022 Bizelop Community Code

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

```