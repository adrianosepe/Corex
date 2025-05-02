#region License

// Copyright(c) 2023 GrappTec
// 
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

#endregion

using System;
using System.IO;
using System.IO.Ports;
using System.Threading;
using Corex.Library.Events;

namespace Corex.Library.Serial;

public class SerialProvider : ISerialProvider
{
    private readonly MemoryStream _buffer;
    private readonly SerialPort _serial;

    private bool _disposed;
    private long _identityID;

    public SerialProvider()
    {
        _serial = new SerialPort();
        _buffer = new MemoryStream();

        BindingEvents();
    }

    public bool IsOpen => _serial.IsOpen;

    public event EventHandler<DataEventArgs<byte[]>> DataReceived;

    public bool Close()
    {
        if (!IsOpen)
        {
            return false;
        }

        _serial.Close();

        return true;
    }

    public void Configure(string portName, int baudRate, int dataBits, Parity parity, StopBits stopBits)
    {
        _serial.PortName = portName;
        _serial.BaudRate = baudRate;
        _serial.DataBits = dataBits;
        _serial.Parity = parity;
        _serial.StopBits = stopBits;

        _serial.ReadTimeout = 500;
    }

    public long NewIdentity() => Interlocked.Increment(ref _identityID);

    public bool Open()
    {
        if (IsOpen)
        {
            return false;
        }

        _serial.Open();
        return true;
    }

    public void Write(byte[] data)
    {
        _serial.Write(data, 0, data.Length);
    }

    public event EventHandler BeginDelegateDataReceived;

    public event EventHandler EndDelegateDataReceived;

    private void BindingEvents()
    {
        _serial.DataReceived +=
            (sender, args) =>
                {
                    _buffer.SetLength(0);

                    try
                    {
                        var v = _serial.ReadLine();
                        Console.WriteLine(v);

                        int value;
                        while ((value = _serial.ReadByte()) != (byte) '\r')
                        {
                            _buffer.WriteByte((byte) value);
                        }

                        if (_buffer.Length > 0)
                        {
                            XHelper.Flow.Ensure(() => BeginDelegateDataReceived?.Invoke(this, EventArgs.Empty));

                            XHelper.Flow.Ensure(() => DataReceived?.Invoke(this, new DataEventArgs<byte[]>(_buffer.ToArray())));

                            XHelper.Flow.Ensure(() => EndDelegateDataReceived?.Invoke(this, EventArgs.Empty));
                        }
                    }
                    catch (TimeoutException)
                    {
                        DataReceived?.Invoke(this, new DataEventArgs<byte[]>(_buffer.ToArray()));
                    }
                    catch (Exception)
                    {
                        XHelper.Flow.Ensure(() => _serial.Close());
                    }
                };

        _serial.ErrorReceived +=
            (sender, args) => { XHelper.Flow.Ensure(() => _serial.Close()); };
    }

    #region IDisposable

    public void Dispose() => Dispose(true);

    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        _disposed = true;

        if (disposing)
        {
            GC.SuppressFinalize(this);
        }

        XHelper.Flow.Ensure(() => _serial.Close());
        _serial.Dispose();

        Disposed?.Invoke(this, EventArgs.Empty);
    }

    ~SerialProvider()
    {
        Dispose(false);
    }

    public event EventHandler Disposed;

    #endregion
}