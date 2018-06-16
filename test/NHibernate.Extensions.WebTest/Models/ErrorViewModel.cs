using System;

namespace NHibernate.Extensions.WebTest.Models {

    public class ErrorViewModel {

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    }

}
