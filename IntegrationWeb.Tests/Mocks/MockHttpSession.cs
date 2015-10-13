using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace IntegrationWeb.Tests.Mocks
{
    internal class MockHttpSession : HttpSessionStateBase
    {
        private readonly Dictionary<string, object> _sessionDictionary = new Dictionary<string, object>();
        public override object this[string name]
        {
            get
            {
                return !_sessionDictionary.ContainsKey(name) ? null : _sessionDictionary[name];
            }

            set
            {
                _sessionDictionary[name] = value;
            }
        }
    }
}
