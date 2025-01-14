// Copyright 2006 Alp Toker <alp@atoker.com>
// This software is made available under the MIT License
// See COPYING for details

// This AssemblyInfo file is used in builds that aren't driven by autoconf, eg. Visual Studio

using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: AssemblyFileVersion("0.7.0")]
[assembly: AssemblyInformationalVersion("0.7.0")]
[assembly: AssemblyVersion("1.0")]
[assembly: AssemblyTitle ("dbus-sharp")]
[assembly: AssemblyDescription ("D-Bus IPC protocol library and CLR binding")]
[assembly: AssemblyCopyright ("Copyright (C) Alp Toker and others")]

#if STRONG_NAME
[assembly: InternalsVisibleTo ("dbus-sharp-tests, PublicKey=0024000004800000940000000602000000240000525341310004000011000000931ae68c635866ff1dcc22547815bbfd67e3d6e80dbfdc9afe7d079670243b9af245eb9797e0766f8adf6afb5eae1d6716fb46ef25d82c37ac7303fe1d13b90780886e0f7a8208167f16dd4678682d4d793a56ccaf0a233411b7604128ae128e306c959fcd2a8038003b2830a326fda3cbbade2f285a9477f6ff8d194e20a2a5")]
[assembly: InternalsVisibleTo ("dbus-monitor, PublicKey=0024000004800000940000000602000000240000525341310004000011000000931ae68c635866ff1dcc22547815bbfd67e3d6e80dbfdc9afe7d079670243b9af245eb9797e0766f8adf6afb5eae1d6716fb46ef25d82c37ac7303fe1d13b90780886e0f7a8208167f16dd4678682d4d793a56ccaf0a233411b7604128ae128e306c959fcd2a8038003b2830a326fda3cbbade2f285a9477f6ff8d194e20a2a5")]
[assembly: InternalsVisibleTo ("dbus-daemon, PublicKey=0024000004800000940000000602000000240000525341310004000011000000931ae68c635866ff1dcc22547815bbfd67e3d6e80dbfdc9afe7d079670243b9af245eb9797e0766f8adf6afb5eae1d6716fb46ef25d82c37ac7303fe1d13b90780886e0f7a8208167f16dd4678682d4d793a56ccaf0a233411b7604128ae128e306c959fcd2a8038003b2830a326fda3cbbade2f285a9477f6ff8d194e20a2a5")]
[assembly: InternalsVisibleTo ("dbus-sharp-glib, PublicKey=0024000004800000940000000602000000240000525341310004000011000000931ae68c635866ff1dcc22547815bbfd67e3d6e80dbfdc9afe7d079670243b9af245eb9797e0766f8adf6afb5eae1d6716fb46ef25d82c37ac7303fe1d13b90780886e0f7a8208167f16dd4678682d4d793a56ccaf0a233411b7604128ae128e306c959fcd2a8038003b2830a326fda3cbbade2f285a9477f6ff8d194e20a2a5")]
[assembly: InternalsVisibleTo ("dbus-sharp-proxies, PublicKey=0024000004800000940000000602000000240000525341310004000011000000931ae68c635866ff1dcc22547815bbfd67e3d6e80dbfdc9afe7d079670243b9af245eb9797e0766f8adf6afb5eae1d6716fb46ef25d82c37ac7303fe1d13b90780886e0f7a8208167f16dd4678682d4d793a56ccaf0a233411b7604128ae128e306c959fcd2a8038003b2830a326fda3cbbade2f285a9477f6ff8d194e20a2a5")]
#else
[assembly: InternalsVisibleTo ("dbus-sharp-tests")]
[assembly: InternalsVisibleTo ("dbus-monitor")]
[assembly: InternalsVisibleTo ("dbus-daemon")]
[assembly: InternalsVisibleTo ("dbus-sharp-glib")]
[assembly: InternalsVisibleTo ("dbus-sharp-proxies")]
[assembly: InternalsVisibleTo ("DBus.Proxies")]
#endif
