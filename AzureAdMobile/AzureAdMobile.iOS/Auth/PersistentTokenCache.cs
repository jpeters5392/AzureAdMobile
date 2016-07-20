using System;
using System.IO;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace AzureAdMobile.iOS.Auth
{
	public class PersistentTokenCache : TokenCache
	{
		private readonly string persistentFileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "..", "Library");
		private readonly string persistentFileName = "token.cache";
		private static readonly object fileLock = new object();

		public PersistentTokenCache()
		{
			this.AfterAccess = AfterAccessNotification;
			this.BeforeAccess = BeforeAccessNotification;
			this.BeforeWrite = BeforeWriteNotification;

			// place the cached value in memory
			this.Deserialize(ReadBytesFromFile());
		}

		// clean the persistent store of all tokens associated with the user.
		public override void Clear()
		{
			base.Clear();

			// persist the cache item with your persistent store
			ClearFile();
		}

		// Notification raised before ADAL accesses the cache.
		// This is your chance to update the in-memory copy from the persistent store if the in-memory version is stale
		void BeforeAccessNotification(TokenCacheNotificationArgs args)
		{
			// look up the current cache and store it in-memory for the current access attempt
			this.Deserialize(ReadBytesFromFile());
		}

		// Notification raised after ADAL accessed the cache.
		// If the HasStateChanged flag is set, ADAL changed the content of the cache
		void AfterAccessNotification(TokenCacheNotificationArgs args)
		{
			// if state changed
			if (this.HasStateChanged)
			{
				// update the persistent store
				WriteBytesToFile(this.Serialize());
				this.HasStateChanged = false;
			}
		}

		void BeforeWriteNotification(TokenCacheNotificationArgs args)
		{
			// if you want to ensure that no concurrent write take place, use this notification to place a lock on the entry
		}

		byte[] ReadBytesFromFile()
		{
			string filePath = Path.Combine(persistentFileLocation, persistentFileName);
			lock (fileLock)
			{
				if (!File.Exists(filePath))
				{
					return null;
				}

				// realistically you should be encrypting/decrypting this data
				return File.ReadAllBytes(filePath);
			}
		}

		void WriteBytesToFile(byte[] data)
		{
			string filePath = Path.Combine(persistentFileLocation, persistentFileName);
			lock (fileLock)
			{
				// realistically you should be encrypting/decrypting this data
				File.WriteAllBytes(filePath, data);
			}
		}

		void ClearFile()
		{
			string filePath = Path.Combine(persistentFileLocation, persistentFileName);
			lock (fileLock)
			{
				File.Delete(filePath);
			}
		}
	}
}

