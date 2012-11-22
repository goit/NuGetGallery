﻿using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace NuGetGallery
{
    public class CloudBlobContainerWrapper : ICloudBlobContainer
    {
        private readonly CloudBlobContainer _blobContainer;

        public CloudBlobContainerWrapper(CloudBlobContainer blobContainer)
        {
            _blobContainer = blobContainer;
        }

        public Task CreateIfNotExistAsync()
        {
            return Task.Factory.FromAsync<bool>(
                _blobContainer.BeginCreateIfNotExists(null, null), 
                _blobContainer.EndCreateIfNotExists);
        }

        public Task SetPermissionsAsync(BlobContainerPermissions permissions)
        {
            return Task.Factory.FromAsync(
                _blobContainer.BeginSetPermissions(permissions, null, null),
                _blobContainer.EndSetPermissions);
        }

        public async Task<ISimpleCloudBlob> GetBlobReferenceAsync(string blobAddressUri)
        {
            Task<ICloudBlob> task = Task.Factory.FromAsync<ICloudBlob>(
                _blobContainer.BeginGetBlobReferenceFromServer(blobAddressUri, null, null),
                _blobContainer.EndGetBlobReferenceFromServer);

            return new CloudBlobWrapper(await task);
        }
    }
}