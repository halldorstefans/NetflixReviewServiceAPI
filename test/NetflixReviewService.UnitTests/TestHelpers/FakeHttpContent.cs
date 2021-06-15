using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NetflixReviewService.UnitTests.TestHelpers
{
    public class FakeHttpContent : HttpContent
    {
        public string Content { get; set; }

        public FakeHttpContent(string content)
        {
            this.Content = content;
        }

        protected override async Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            var byteArray = Encoding.ASCII.GetBytes(Content);
            await stream.WriteAsync(byteArray, 0, Content.Length);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = Content.Length;
            return true;
        }    
    }
}