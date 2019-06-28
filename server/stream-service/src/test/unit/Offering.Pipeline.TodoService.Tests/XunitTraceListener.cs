// ************************************************************************
// *****      COPYRIGHT 2014 - 2018 HONEYWELL INTERNATIONAL SARL      *****
// ************************************************************************

namespace Offering.Pipeline.TodoService.Tests
{
    using System.Diagnostics;
    using Xunit.Abstractions;

    public class XUnitTraceListener : TraceListener
    {
        private readonly ITestOutputHelper _output;

        public XUnitTraceListener(ITestOutputHelper output)
        {
            _output = output;
        }

        public override void WriteLine(string str)
        {
            _output.WriteLine(str);
        }

        public override void Write(string str)
        {
            _output.WriteLine(str);
        }
    }
}