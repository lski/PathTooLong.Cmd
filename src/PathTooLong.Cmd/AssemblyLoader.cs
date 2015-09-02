using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PathTooLong.Cmd {

	/// <summary>
	/// Registers a function that will attempt to load the assembly for the domain, can NOT be called in the same method as any method calls that reside in another assembly. This is
	/// because when a method is called it loads all the dependancies it needs for that method before running it. So if there is code in the same method as a the AsssemblyLoader.Register
	/// it will try to load the assembly before registration, which is annoying to get round this, simply wrap it in another next method in the same assembly as the loader and call that.
	/// </summary>
	public class AssemblyLoader {

		readonly Assembly _assembly;

		public AssemblyLoader(AppDomain domain, Assembly assembly) {

			_assembly = assembly;

			domain.AssemblyResolve += OnResolveAssembly;
		}

		public static AssemblyLoader Register() {

			return new AssemblyLoader(AppDomain.CurrentDomain, Assembly.GetEntryAssembly());
		}

		public static AssemblyLoader Register(AppDomain domain, Assembly assembly) {

			return new AssemblyLoader(domain, Assembly.GetEntryAssembly());
		}

		/// <summary>
		/// Runs when the application requires an assembly. It attempts to load the dll files from inside the exe as a resource, rather than files in the same folder.
		/// Dlls can be added as a resource manually or it can be done in the csproj file e.g.
		///
		/// <code>
		/// <Import Project="$(MSBuildToolsPath)\Microsoft.CSharp.targets" />
		/// <Target Name="AfterResolveReferences">
		///		<ItemGroup>
		///		<EmbeddedResource Include = "@(ReferenceCopyLocalPaths)" Condition="'%(ReferenceCopyLocalPaths.Extension)' == '.dll'">
		///			<LogicalName>%(ReferenceCopyLocalPaths.DestinationSubDirectory)%(ReferenceCopyLocalPaths.Filename)%(ReferenceCopyLocalPaths.Extension)</LogicalName>
		///		</EmbeddedResource>
		///		</ItemGroup>
		/// </Target>
		/// </code>
		///
		/// Adapted from the code by Paul Rohde http://www.paulrohde.com/merging-a-wpf-application-into-a-single-exe/
		/// </summary>
		private Assembly OnResolveAssembly(object sender, ResolveEventArgs e) {

			// Get the Name of the AssemblyFile
			var assemblyName = new AssemblyName(e.Name);
			var dllName = assemblyName.Name + ".dll";

			// Load from Embedded Resources - This function is not called if the Assembly is already in the same folder as the app.
			var resources = _assembly.GetManifestResourceNames().Where(s => s.EndsWith(dllName, StringComparison.OrdinalIgnoreCase));

			if (resources.Any()) {

				// 99% of cases will only have one matching item, but if you don't,
				// you will have to change the logic to handle those cases.
				var resourceName = resources.First();

				using (var stream = _assembly.GetManifestResourceStream(resourceName)) {

					if (stream == null) {
						return null;
					}

					var block = new byte[stream.Length];

					// Safely try to load the assembly.
					try {
						stream.Read(block, 0, block.Length);
						return Assembly.Load(block);
					}
					catch (IOException) {
						return null;
					}
					catch (BadImageFormatException) {
						return null;
					}
				}
			}

			// in the case the resource doesn't exist, return null.
			return null;
		}
	}
}