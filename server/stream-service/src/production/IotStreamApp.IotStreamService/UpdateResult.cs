// ************************************************************************
// *****      COPYRIGHT 2014 - 2018 HONEYWELL INTERNATIONAL SARL      *****
// ************************************************************************

namespace Honeywell.IotStreamApp.IotStreamService
{
    public class UpdateResult
    {
        public bool IsNotFoundError { get; set; }

        public bool IsSuccess { get; set; }

        public TodoItem UpdatedItem { get; set; }
    }
}