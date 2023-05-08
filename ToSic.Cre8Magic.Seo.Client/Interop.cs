using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace ToSic.Cre8magic.Seo
{
    public class Interop
    {
        private readonly IJSRuntime _jsRuntime;

        public Interop(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }
    }
}
