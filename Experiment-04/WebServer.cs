using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Hosting;

public class WebServer
{
    private static HttpListener _listener;
    private static WebServerHost _host;
    private static int _port = 8080;

    public static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Starting Online Student Event Registration Portal Server...");

            // Ensure bin directory exists and copy WebServer.exe to bin\WebServer.exe
            // ASP.NET ApplicationHost requires the host assembly to be in bin directory
            string appDir = AppDomain.CurrentDomain.BaseDirectory;
            string binDir = Path.Combine(appDir, "bin");
            if (!Directory.Exists(binDir))
            {
                Directory.CreateDirectory(binDir);
            }

            string exeName = Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string destExePath = Path.Combine(binDir, exeName);

            // Copy self to bin directory if running from base directory
            if (!string.Equals(System.Reflection.Assembly.GetExecutingAssembly().Location, destExePath, StringComparison.OrdinalIgnoreCase))
            {
                File.Copy(System.Reflection.Assembly.GetExecutingAssembly().Location, destExePath, true);
                // Also copy the pdb if it exists
                string pdbPath = Path.ChangeExtension(System.Reflection.Assembly.GetExecutingAssembly().Location, ".pdb");
                if (File.Exists(pdbPath))
                {
                    File.Copy(pdbPath, Path.Combine(binDir, Path.GetFileName(pdbPath)), true);
                }
            }

            // Create Application Host
            _host = (WebServerHost)ApplicationHost.CreateApplicationHost(
                typeof(WebServerHost),
                "/",
                appDir
            );

            // Start the server in the host AppDomain
            _host.Start(_port);

            Console.WriteLine("Server started and listening at http://localhost:" + _port + "/Default.aspx");
            Console.WriteLine("Opening default browser...");

            // Open the browser
            System.Diagnostics.Process.Start("http://localhost:" + _port + "/Default.aspx");

            Console.WriteLine("Press Ctrl+C or close this window to stop the server.");
            
            // Keep running
            var keepAlive = new AutoResetEvent(false);
            Console.CancelKeyPress += (sender, e) => {
                e.Cancel = true;
                keepAlive.Set();
            };
            keepAlive.WaitOne();

            Console.WriteLine("Stopping server...");
            _host.Stop();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fatal Error starting server: " + ex.ToString());
            Console.ReadLine();
        }
    }
}

public class WebServerHost : MarshalByRefObject
{
    private HttpListener _listener;
    private bool _running;
    private string _physicalDir;

    public void Start(int port)
    {
        _physicalDir = AppDomain.CurrentDomain.BaseDirectory;
        _listener = new HttpListener();
        _listener.Prefixes.Add("http://localhost:" + port + "/");
        _listener.Start();
        _running = true;

        ThreadPool.QueueUserWorkItem(o => {
            while (_running)
            {
                try
                {
                    HttpListenerContext context = _listener.GetContext();
                    ThreadPool.QueueUserWorkItem(c => ProcessRequest((HttpListenerContext)c), context);
                }
                catch (HttpListenerException)
                {
                    // Server stopped
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error accepting connection: " + ex.Message);
                }
            }
        });
    }

    public void Stop()
    {
        _running = false;
        if (_listener != null && _listener.IsListening)
        {
            _listener.Stop();
            _listener.Close();
        }
    }

    private void ProcessRequest(HttpListenerContext context)
    {
        string rawUrl = context.Request.RawUrl;
        Console.WriteLine("[Server] Incoming request: " + context.Request.HttpMethod + " " + rawUrl);
        try
        {
            var request = new ListenerWorkerRequest(context, "/", _physicalDir);
            HttpRuntime.ProcessRequest(request);
            Console.WriteLine("[Server] ProcessRequest completed for: " + rawUrl);
        }
        catch (Exception ex)
        {
            Console.WriteLine("[Server] Error processing request " + rawUrl + ": " + ex.ToString());
            try
            {
                context.Response.StatusCode = 500;
                using (var sw = new StreamWriter(context.Response.OutputStream))
                {
                    sw.WriteLine("<html><body><h1>500 Internal Server Error</h1><pre>" + ex.ToString() + "</pre></body></html>");
                }
                context.Response.Close();
            }
            catch { }
        }
    }

    public override object InitializeLifetimeService()
    {
        // Return null to allow the object to live forever
        return null;
    }
}

public class ListenerWorkerRequest : HttpWorkerRequest
{
    private readonly HttpListenerContext _context;
    private readonly string _virtualDir;
    private readonly string _physicalDir;
    private byte[] _preloadedBody;

    public ListenerWorkerRequest(HttpListenerContext context, string virtualDir, string physicalDir)
    {
        _context = context;
        _virtualDir = virtualDir;
        _physicalDir = physicalDir;
        ReadPreloadedBody();
    }

    private void ReadPreloadedBody()
    {
        if (_context.Request.HasEntityBody)
        {
            using (var ms = new MemoryStream())
            {
                byte[] buffer = new byte[8192];
                int read;
                while ((read = _context.Request.InputStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                _preloadedBody = ms.ToArray();
            }
        }
    }

    public override string GetUriPath()
    {
        return _context.Request.Url.AbsolutePath;
    }

    public override string GetQueryString()
    {
        string rawUrl = _context.Request.RawUrl;
        int idx = rawUrl.IndexOf('?');
        return idx >= 0 ? rawUrl.Substring(idx + 1) : "";
    }

    public override string GetRawUrl()
    {
        return _context.Request.RawUrl;
    }

    public override string GetHttpVerbName()
    {
        return _context.Request.HttpMethod;
    }

    public override string GetHttpVersion()
    {
        return "HTTP/" + _context.Request.ProtocolVersion.ToString();
    }

    public override string GetRemoteAddress()
    {
        return _context.Request.RemoteEndPoint.Address.ToString();
    }

    public override int GetRemotePort()
    {
        return _context.Request.RemoteEndPoint.Port;
    }

    public override string GetLocalAddress()
    {
        return _context.Request.LocalEndPoint.Address.ToString();
    }

    public override int GetLocalPort()
    {
        return _context.Request.LocalEndPoint.Port;
    }

    public override void SendStatus(int statusCode, string statusDescription)
    {
        _context.Response.StatusCode = statusCode;
        _context.Response.StatusDescription = statusDescription;
    }

    public override void SendKnownResponseHeader(int index, string value)
    {
        string headerName = GetKnownResponseHeaderName(index);
        _context.Response.Headers[headerName] = value;
    }

    public override void SendUnknownResponseHeader(string name, string value)
    {
        _context.Response.Headers[name] = value;
    }

    public override void SendResponseFromMemory(byte[] data, int length)
    {
        Console.WriteLine("[Server] SendResponseFromMemory: " + length + " bytes");
        _context.Response.OutputStream.Write(data, 0, length);
    }

    public override void SendResponseFromMemory(IntPtr data, int length)
    {
        Console.WriteLine("[Server] SendResponseFromMemory (IntPtr): " + length + " bytes");
        if (length <= 0) return;
        byte[] buffer = new byte[length];
        System.Runtime.InteropServices.Marshal.Copy(data, buffer, 0, length);
        _context.Response.OutputStream.Write(buffer, 0, length);
    }

    public override void SendResponseFromFile(string filename, long offset, long length)
    {
        Console.WriteLine("[Server] SendResponseFromFile (name): " + filename + ", length: " + length);
        using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            byte[] buffer = new byte[8192];
            fs.Seek(offset, SeekOrigin.Begin);
            long remaining = length;
            while (remaining > 0)
            {
                int toRead = (int)Math.Min(buffer.Length, remaining);
                int read = fs.Read(buffer, 0, toRead);
                if (read <= 0) break;
                _context.Response.OutputStream.Write(buffer, 0, read);
                remaining -= read;
            }
        }
    }

    public override void SendResponseFromFile(IntPtr handle, long offset, long length)
    {
        Console.WriteLine("[Server] SendResponseFromFile (handle), length: " + length);
        if (length <= 0) return;
        
        // Use the handle to read the file content
        var safeHandle = new Microsoft.Win32.SafeHandles.SafeFileHandle(handle, false);
        using (FileStream fs = new FileStream(safeHandle, FileAccess.Read))
        {
            byte[] buffer = new byte[8192];
            fs.Seek(offset, SeekOrigin.Begin);
            long remaining = length;
            while (remaining > 0)
            {
                int toRead = (int)Math.Min(buffer.Length, remaining);
                int read = fs.Read(buffer, 0, toRead);
                if (read <= 0) break;
                _context.Response.OutputStream.Write(buffer, 0, read);
                remaining -= read;
            }
        }
    }

    public override void FlushResponse(bool finalFlush)
    {
        try
        {
            _context.Response.OutputStream.Flush();
            if (finalFlush)
            {
                _context.Response.Close();
            }
        }
        catch { }
    }

    public override void EndOfRequest()
    {
        try
        {
            _context.Response.OutputStream.Close();
            _context.Response.Close();
        }
        catch { }
    }

    public override byte[] GetPreloadedEntityBody()
    {
        return _preloadedBody;
    }

    public override bool IsEntireEntityBodyIsPreloaded()
    {
        return true;
    }

    public override int ReadEntityBody(byte[] buffer, int size)
    {
        return 0;
    }

    public override string GetKnownRequestHeader(int index)
    {
        string name = GetKnownRequestHeaderName(index);
        return _context.Request.Headers[name];
    }

    public override string GetUnknownRequestHeader(string name)
    {
        return _context.Request.Headers[name];
    }

    public override string[][] GetUnknownRequestHeaders()
    {
        var headers = _context.Request.Headers;
        var list = new System.Collections.Generic.List<string[]>();
        for (int i = 0; i < headers.Count; i++)
        {
            string name = headers.GetKey(i);
            if (GetKnownRequestHeaderIndex(name) < 0)
            {
                list.Add(new string[] { name, headers.Get(i) });
            }
        }
        return list.ToArray();
    }

    public override string GetFilePath()
    {
        return _context.Request.Url.AbsolutePath;
    }

    public override string GetFilePathTranslated()
    {
        string path = _context.Request.Url.AbsolutePath;
        return Path.Combine(_physicalDir, path.TrimStart('/').Replace('/', '\\'));
    }

    public override string MapPath(string virtualPath)
    {
        return Path.Combine(_physicalDir, virtualPath.TrimStart('/').Replace('/', '\\'));
    }

    public override string GetAppPath()
    {
        return _virtualDir;
    }

    public override string GetAppPathTranslated()
    {
        return _physicalDir;
    }
}
