using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Calzolari.Grpc.AspNetCore.Validation.SampleRpc;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Calzolari.Grpc.AspNetCore.Validation.Test
{
    public static class WebApplicationFactoryHelper
    {
        public static GrpcChannel CreateGrpcChannel(this WebApplicationFactory<Startup> factory)
        {
            var client = factory.CreateDefaultClient(new ResponseVersionHandler());
            return GrpcChannel.ForAddress(client.BaseAddress, new GrpcChannelOptions
            {
                HttpClient = client
            });
        }

        private class ResponseVersionHandler : DelegatingHandler
        {
            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                CancellationToken cancellationToken)
            {
                var response = await base.SendAsync(request, cancellationToken);
                response.Version = request.Version;

                return response;
            }
        }
    }
}