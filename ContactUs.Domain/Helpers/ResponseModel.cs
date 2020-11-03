using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace ConnectUs.Domain.Helpers
{
    public class ResponseModel<T> where T : class
    {
        public T Data { get; set; }
        public string Message { get; set; }
        public short ResultCode { get; set; }
        /// <summary>
        /// returns ok
        /// </summary>
        /// <param name="Data"></param>
        public ResponseModel(T Data=null)
        {
            this.Data = Data;
            ResultCode = 0;
        }
        /// <summary>
        /// returns error
        /// </summary>
        /// <param name="message"></param>
        /// <param name="dto"></param>
        public ResponseModel(string message,T dto=null)
        {
            Data = dto;
            ResultCode = 1;
            Message = (message);

        }
    }
}
