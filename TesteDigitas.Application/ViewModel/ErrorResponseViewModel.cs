using System;
using System.Collections.Generic;

namespace TesteDigitas.Application.ViewModel
{
    public class ErrorResponseViewModel
    {
        public ErrorResponseViewModel()
        {
            TraceId = Guid.NewGuid().ToString();
            Errors = new List<ErrorDetailsViewModel>();
        }

        public ErrorResponseViewModel(string logref, string message)
        {
            TraceId = Guid.NewGuid().ToString();
            Errors = new List<ErrorDetailsViewModel>();
            AddError(logref, message);
        }

        public string TraceId { get; set; }
        public List<ErrorDetailsViewModel> Errors { get; set; }


        public class ErrorDetailsViewModel
        {
            public ErrorDetailsViewModel(string logref, string message)
            {
                Logref = logref;
                Message = message;
            }

            public string Logref { get; set; }
            public string Message { get; set; }
        }

        public void AddError(string logref, string message)
        {
            Errors.Add(new ErrorDetailsViewModel(logref,message));
        }

    }
}
