// Copyright 2006 Alp Toker <alp@atoker.com>
// This software is made available under the MIT License
// See COPYING for details

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using DBus.Unix;

namespace DBus.Transports
{
	class SocketTransport : Transport
	{
		internal Socket socket;

		public override void Open (AddressEntry entry)
		{
			string host, portStr, family;
			byte[] nonce = {};
			int port;

			if (!entry.Properties.TryGetValue ("host", out host))
				host = "localhost";

			if (!entry.Properties.TryGetValue ("port", out portStr))
				throw new Exception ("No port specified");

			if (entry.Method == "nonce-tcp") {
				string nonceFile;

				if (!entry.Properties.TryGetValue ("noncefile", out nonceFile))
					throw new Exception ("No noncefile specified");

				nonce = File.ReadAllBytes (nonceFile);
				if (nonce.Length != 16)
					throw new Exception ("Nonce should be 16 bytes");
			}

			if (!Int32.TryParse (portStr, out port))
				throw new Exception ("Invalid port: \"" + port + "\"");

			if (!entry.Properties.TryGetValue ("family", out family))
				family = null;

			Open (host, port, family, nonce);
		}

		public void Open (string host, int port, string family, byte[] nonce)
		{
			//TODO: use Socket directly
			TcpClient client = new TcpClient (host, port);
			/*
			client.NoDelay = true;
			client.ReceiveBufferSize = (int)Protocol.MaxMessageLength;
			client.SendBufferSize = (int)Protocol.MaxMessageLength;
			*/
			this.socket = client.Client;
			SocketHandle = (long)client.Client.Handle;
			Stream = client.GetStream ();
			if (nonce.Length > 0)
				Stream.Write (nonce, 0, nonce.Length);
		}

		public void Open (Socket socket)
		{
			this.socket = socket;

			socket.Blocking = true;
			SocketHandle = (long)socket.Handle;
			//Stream = new UnixStream ((int)socket.Handle);
			Stream = new NetworkStream (socket);
		}

		public override void WriteCred ()
		{
			Stream.WriteByte (0);
		}

		public const int TOKEN_QUERY = 0X00000008;

		enum TOKEN_INFORMATION_CLASS {
			TokenUser = 1,
			TokenGroups,
			TokenPrivileges,
			TokenOwner,
			TokenPrimaryGroup,
			TokenDefaultDacl,
			TokenSource,
			TokenType,
			TokenImpersonationLevel,
			TokenStatistics,
			TokenRestrictedSids,
			TokenSessionId
		}

		public struct TOKEN_USER {
			public SID_AND_ATTRIBUTES User;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SID_AND_ATTRIBUTES {
			public IntPtr Sid;
			public int Attributes;
		}

		[DllImport("advapi32.dll")]
		static extern bool OpenProcessToken (IntPtr ProcessHandle,
						     int DesiredAccess,
						     ref IntPtr TokenHandle);

		[DllImport("kernel32.dll")]
		static extern IntPtr GetCurrentProcess ();

		[DllImport("kernel32.dll")]
		static extern bool CloseHandle(IntPtr handle);

		[DllImport("advapi32.dll", SetLastError=true)]
		static extern bool GetTokenInformation (IntPtr TokenHandle,
							TOKEN_INFORMATION_CLASS TokenInformationClass,
							IntPtr TokenInformation,
							int TokenInformationLength,
							ref int ReturnLength);

                [DllImport("advapi32", CharSet=CharSet.Auto, SetLastError=true)]
                static extern bool ConvertSidToStringSid(IntPtr pSID,
                                                         out IntPtr ptrSid);

		public override string AuthString ()
		{
			if (Environment.OSVersion.Platform == PlatformID.Win32Windows ||
			    Environment.OSVersion.Platform == PlatformID.Win32NT ||
			    Environment.OSVersion.Platform == PlatformID.WinCE) {
				IntPtr procToken = IntPtr.Zero;

				if (!OpenProcessToken (GetCurrentProcess (), TOKEN_QUERY,
						       ref procToken))
					return String.Empty;

				TOKEN_USER tokUser;
				const int bufLength = 256;
				IntPtr tu = Marshal.AllocHGlobal (bufLength);
				int cb = bufLength;

				if (!GetTokenInformation (procToken,
							 TOKEN_INFORMATION_CLASS.TokenUser,
							 tu, cb, ref cb)) {
					Marshal.FreeHGlobal (tu);
					return String.Empty;
				}

				tokUser = (TOKEN_USER)Marshal.PtrToStructure (tu, typeof(TOKEN_USER));
				CloseHandle (procToken);

				IntPtr ptrSid;
				string StringSid;
				ConvertSidToStringSid (tokUser.User.Sid, out ptrSid);
				StringSid = Marshal.PtrToStringAuto(ptrSid);

				Marshal.FreeHGlobal (tu);

				return StringSid;
			} else {
				long uid = UnixUid.GetEUID ();
				return uid.ToString ();
			}
		}
	}
}
