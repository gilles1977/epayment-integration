using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Web.SessionState;

namespace IntegrationWeb.Tests.Mocks
{
    public class MockControllerContext : ControllerContext
    {
        HttpContextBase _fakeHttpContext = new FakeHttpContext();

        public override System.Web.HttpContextBase HttpContext
        {
            get { return _fakeHttpContext; }
            set { _fakeHttpContext = value; }
        }
    }

    class FakeHttpContext : HttpContextBase
    {
        private readonly HttpRequestBase _fakeHttpRequest = new FakeHttpRequest();
        private readonly HttpSessionStateBase _fakeHttpSession = new MockHttpSession();

        public override HttpRequestBase Request
        {
            get { return _fakeHttpRequest; }
        }

        public override HttpSessionStateBase Session
        {
            get { return _fakeHttpSession; }
        }
    }

    class FakeHttpRequest : HttpRequestBase
    {
        public override string this[string key]
        {
            get { return null; }
        }

        public override NameValueCollection Headers
        {
            get { return new NameValueCollection(); }
        }
    }
}
