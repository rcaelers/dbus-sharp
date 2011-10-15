// Copyright 2006 Alp Toker <alp@atoker.com>
// This software is made available under the MIT License
// See COPYING for details

using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace DBus
{
	// Subclass obsolete BadAddressException to avoid ABI break
#pragma warning disable 0618
	//public class InvalidAddressException : Exception
	public class InvalidAddressException : BadAddressException
	{
		public InvalidAddressException (string reason) : base (reason) {}
	}
#pragma warning restore 0618

	[Obsolete ("Use InvalidAddressException")]
	public class BadAddressException : Exception
	{
		public BadAddressException (string reason) : base (reason) {}
	}

	static class Address
	{
		enum FileRights : uint          // constants from winbase.h
		{
			Read = 4,
			Write = 2,
			ReadWrite = Read + Write
		}

		[DllImport ("kernel32.dll", SetLastError = true)]
		static extern IntPtr OpenFileMapping (FileRights dwDesiredAccess,
						      bool bInheritHandle,
						      string lpName);
		[DllImport ("kernel32.dll", SetLastError = true)]
		static extern IntPtr MapViewOfFile (IntPtr hFileMappingObject,
						    FileRights dwDesiredAccess,
						    uint dwFileOffsetHigh,
						    uint dwFileOffsetLow,
						    uint dwNumberOfBytesToMap);
		[DllImport ("Kernel32.dll")]
		static extern bool UnmapViewOfFile (IntPtr map);

		[DllImport ("kernel32.dll")]
        static extern int CloseHandle (IntPtr hObject);

		//(unix:(path|abstract)=.*,guid=.*|tcp:host=.*(,port=.*)?);? ...
		public static AddressEntry[] Parse (string addresses)
		{
			if (addresses == null)
				throw new ArgumentNullException (addresses);

			List<AddressEntry> entries = new List<AddressEntry> ();

			foreach (string entryStr in addresses.Split (';'))
				entries.Add (AddressEntry.Parse (entryStr));

			return entries.ToArray ();
		}

		const string SYSTEM_BUS_ADDRESS = "unix:path=/var/run/dbus/system_bus_socket";
		public static string System
		{
			get {
				string addr = Environment.GetEnvironmentVariable ("DBUS_SYSTEM_BUS_ADDRESS");

				if (String.IsNullOrEmpty (addr))
					addr = SYSTEM_BUS_ADDRESS;

				return addr;
			}
		}

		public static string Session
		{
			get {
				string addr = Environment.GetEnvironmentVariable ("DBUS_SESSION_BUS_ADDRESS");
				if (String.IsNullOrEmpty (addr) &&
				    (Environment.OSVersion.Platform == PlatformID.Win32Windows ||
				     Environment.OSVersion.Platform == PlatformID.Win32NT ||
				     Environment.OSVersion.Platform == PlatformID.WinCE)) {
					IntPtr mapping = OpenFileMapping (FileRights.Read, false, "DBusDaemonAddressInfo");
					if (mapping != IntPtr.Zero) {
						IntPtr p = MapViewOfFile (mapping, FileRights.Read, 0, 0, 0);
						if (p != IntPtr.Zero) {
							addr = Marshal.PtrToStringAnsi (p);
							UnmapViewOfFile (p);
						}
						CloseHandle (mapping);
					}
				}
				return addr;
				    
			}
		}

		public static string Starter
		{
			get {
				return Environment.GetEnvironmentVariable ("DBUS_STARTER_ADDRESS");
			}
		}

		public static string StarterBusType
		{
			get {
				return Environment.GetEnvironmentVariable ("DBUS_STARTER_BUS_TYPE");
			}
		}
	}
}
