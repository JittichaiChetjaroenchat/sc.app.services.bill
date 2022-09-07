using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SC.App.Services.Document.Client;

namespace SC.App.Services.Bill.Client.Document
{
    public interface IDocumentManager
    {
        Task<ResponseOfGetDocumentResponse> GetDocumentByIdAsync(HttpRequest request, Guid id);

        Task<ResponseOfListOfGetDocumentResponse> GetDocumentByIdsAsync(HttpRequest request, Guid[] ids);
    }
}