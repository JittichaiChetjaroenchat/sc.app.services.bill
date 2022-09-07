using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SC.App.Services.Bill.Common.Extensions;
using SC.App.Services.Document.Client;

namespace SC.App.Services.Bill.Client.Document
{
    public class DocumentManager : IDocumentManager
    {
        private readonly IDocumentClient _documentClient;

        public DocumentManager(
            IDocumentClient documentClient)
        {
            _documentClient = documentClient;
        }

        public async Task<ResponseOfGetDocumentResponse> GetDocumentByIdAsync(HttpRequest request, Guid id)
        {
            _documentClient.SetAuthorization(request.GetAuthorization());
            _documentClient.SetAcceptLanguage(request.GetAcceptLanguage());

            return await _documentClient.Documents_GetByIdAsync(id);
        }

        public async Task<ResponseOfListOfGetDocumentResponse> GetDocumentByIdsAsync(HttpRequest request, Guid[] ids)
        {
            _documentClient.SetAuthorization(request.GetAuthorization());
            _documentClient.SetAcceptLanguage(request.GetAcceptLanguage());

            return await _documentClient.Documents_GetByIdsAsync(ids);
        }
    }
}