﻿#region copyright
// ------------------------------------------------------
// Copyright (C) Dmitriy Yukhanov [https://codestage.net]
// ------------------------------------------------------
#endregion

namespace CodeStage.AntiCheat.ObscuredTypes
{
	using System;
	using UnityEngine;
	using Utils;

	/// <summary>
	/// Use it instead of regular <c>Vector3Int</c> for any cheating-sensitive variables.
	/// </summary>
	/// <strong>\htmlonly<font color="FF4040">WARNING:</font>\endhtmlonly Doesn't mimic regular type API, thus should be used with extra caution.</strong> Cast it to regular, not obscured type to work with regular APIs.<br/>
	/// <strong><em>Regular type is faster and memory wiser comparing to the obscured one!</em></strong><br/>
	/// Feel free to use regular types for all short-term operations and calculations while keeping obscured type only at the long-term declaration (i.e. class field).
	[Serializable]
	public struct ObscuredVector3Int : IObscuredType
	{
		private static readonly Vector3Int Zero = Vector3Int.zero;

		[SerializeField]
		private int currentCryptoKey;

		[SerializeField]
		private RawEncryptedVector3Int hiddenValue;

		[SerializeField]
		private bool inited;

		[SerializeField]
		private Vector3Int fakeValue;

		[SerializeField]
		private bool fakeValueActive;

		private ObscuredVector3Int(Vector3Int value)
		{
			currentCryptoKey = GenerateKey();
			hiddenValue = Encrypt(value, currentCryptoKey);

#if UNITY_EDITOR
			fakeValue = value;
			fakeValueActive = true;
#else
			var detectorRunning = Detectors.ObscuredCheatingDetector.ExistsAndIsRunning;
			fakeValue = detectorRunning ? value : Zero;
			fakeValueActive = detectorRunning;
#endif
			inited = true;
		}

		/// <summary>
		/// Mimics constructor of regular Vector3Int.
		/// </summary>
		/// <param name="x">X component of the vector</param>
		/// <param name="y">Y component of the vector</param>
		/// <param name="z">Z component of the vector</param>
		public ObscuredVector3Int(int x, int y, int z)
		{
			currentCryptoKey = GenerateKey();
			hiddenValue = Encrypt(x, y, z, currentCryptoKey);

			if (Detectors.ObscuredCheatingDetector.ExistsAndIsRunning)
			{
				fakeValue = new Vector3Int
				{
					x = x,
					y = y,
					z = z
				};
				fakeValueActive = true;
			}
			else
			{
				fakeValue = Zero;
				fakeValueActive = false;
			}

			inited = true;
		}

		public int x
		{
			get
			{
				var decrypted = ObscuredInt.Decrypt(hiddenValue.x, currentCryptoKey);
				if (Detectors.ObscuredCheatingDetector.ExistsAndIsRunning && fakeValueActive && Math.Abs(decrypted - fakeValue.x) > 0)
				{
					Detectors.ObscuredCheatingDetector.Instance.OnCheatingDetected();
				}
				return decrypted;
			}

			set
			{
				hiddenValue.x = ObscuredInt.Encrypt(value, currentCryptoKey);
				if (Detectors.ObscuredCheatingDetector.ExistsAndIsRunning)
				{
					fakeValue.x = value;
					fakeValue.y = ObscuredInt.Decrypt(hiddenValue.y, currentCryptoKey);
					fakeValue.z = ObscuredInt.Decrypt(hiddenValue.z, currentCryptoKey);
					fakeValueActive = true;
				}
				else
				{
					fakeValueActive = false;
				}
			}
		}

		public int y
		{
			get
			{
				var decrypted = ObscuredInt.Decrypt(hiddenValue.y, currentCryptoKey);
				if (Detectors.ObscuredCheatingDetector.ExistsAndIsRunning && fakeValueActive && Math.Abs(decrypted - fakeValue.y) > 0)
				{
					Detectors.ObscuredCheatingDetector.Instance.OnCheatingDetected();
				}
				return decrypted;
			}

			set
			{
				hiddenValue.y = ObscuredInt.Encrypt(value, currentCryptoKey);
				if (Detectors.ObscuredCheatingDetector.ExistsAndIsRunning)
				{
					fakeValue.x = ObscuredInt.Decrypt(hiddenValue.x, currentCryptoKey);
					fakeValue.y = value;
					fakeValue.z = ObscuredInt.Decrypt(hiddenValue.z, currentCryptoKey);
					fakeValueActive = true;
				}
				else
				{
					fakeValueActive = false;
				}
			}
		}

		public int z
		{
			get
			{
				var decrypted = ObscuredInt.Decrypt(hiddenValue.z, currentCryptoKey);
				if (Detectors.ObscuredCheatingDetector.ExistsAndIsRunning && fakeValueActive && Math.Abs(decrypted - fakeValue.z) > 0)
				{
					Detectors.ObscuredCheatingDetector.Instance.OnCheatingDetected();
				}
				return decrypted;
			}

			set
			{
				hiddenValue.z = ObscuredInt.Encrypt(value, currentCryptoKey);
				if (Detectors.ObscuredCheatingDetector.ExistsAndIsRunning)
				{
					fakeValue.x = ObscuredInt.Decrypt(hiddenValue.x, currentCryptoKey);
					fakeValue.y = ObscuredInt.Decrypt(hiddenValue.y, currentCryptoKey);
					fakeValue.z = value;
					fakeValueActive = true;
				}
				else
				{
					fakeValueActive = false;
				}
			}
		}

		public int this[int index]
		{
			get
			{
				switch (index)
				{
					case 0:
						return x;
					case 1:
						return y;
					case 2:
						return z;
					default:
						throw new IndexOutOfRangeException("Invalid ObscuredVector3Int index!");
				}
			}
			set
			{
				switch (index)
				{
					case 0:
						x = value;
						break;
					case 1:
						y = value;
						break;
					case 2:
						z = value;
						break;
					default:
						throw new IndexOutOfRangeException("Invalid ObscuredVector3Int index!");
				}
			}
		}

		/// <summary>
		/// Encrypts passed value using passed key.
		/// </summary>
		/// Key can be generated automatically using GenerateKey().
		/// \sa Decrypt(), GenerateKey()
		public static RawEncryptedVector3Int Encrypt(Vector3Int value, int key)
		{
			return Encrypt(value.x, value.y, value.z, key);
		}

		/// <summary>
		/// Encrypts passed components using passed key.
		/// </summary>
		/// Key can be generated automatically using GenerateKey().
		/// \sa Decrypt(), GenerateKey()
		public static RawEncryptedVector3Int Encrypt(int x, int y, int z, int key)
		{
			RawEncryptedVector3Int result;
			result.x = ObscuredInt.Encrypt(x, key);
			result.y = ObscuredInt.Encrypt(y, key);
			result.z = ObscuredInt.Encrypt(z, key);

			return result;
		}

		/// <summary>
		/// Decrypts passed value you got from Encrypt() using same key.
		/// </summary>
		/// \sa Encrypt()
		public static Vector3Int Decrypt(RawEncryptedVector3Int value, int key)
		{
			var result = new Vector3Int
			{
				x = ObscuredInt.Decrypt(value.x, key),
				y = ObscuredInt.Decrypt(value.y, key),
				z = ObscuredInt.Decrypt(value.z, key)
			};

			return result;
		}

		/// <summary>
		/// Creates and fills obscured variable with raw encrypted value previously got from GetEncrypted().
		/// </summary>
		/// Literally does same job as SetEncrypted() but makes new instance instead of filling existing one,
		/// making it easier to initialize new variables from saved encrypted values.
		///
		/// <param name="encrypted">Raw encrypted value you got from GetEncrypted().</param>
		/// <param name="key">Encryption key you've got from GetEncrypted().</param>
		/// <returns>New obscured variable initialized from specified encrypted value.</returns>
		/// \sa GetEncrypted(), SetEncrypted()
		public static ObscuredVector3Int FromEncrypted(RawEncryptedVector3Int encrypted, int key)
		{
			var instance = new ObscuredVector3Int();
			instance.SetEncrypted(encrypted, key);
			return instance;
		}

		/// <summary>
		/// Generates random key. Used internally and can be used to generate key for manual Encrypt() calls.
		/// </summary>
		/// <returns>Key suitable for manual Encrypt() calls.</returns>
		public static int GenerateKey()
		{
			return RandomUtils.GenerateIntKey();
		}

		/// <summary>
		/// Allows to pick current obscured value as is.
		/// </summary>
		/// <param name="key">Encryption key needed to decrypt returned value.</param>
		/// <returns>Encrypted value as is.</returns>
		/// Use it in conjunction with SetEncrypted().<br/>
		/// Useful for saving data in obscured state.
		/// \sa FromEncrypted(), SetEncrypted()
		public RawEncryptedVector3Int GetEncrypted(out int key)
		{
			if (!inited)
				Init();
			
			key = currentCryptoKey;
			return hiddenValue;
		}

		/// <summary>
		/// Allows to explicitly set current obscured value. Crypto key should be same as when encrypted value was got with GetEncrypted().
		/// </summary>
		/// Use it in conjunction with GetEncrypted().<br/>
		/// Useful for loading data stored in obscured state.
		/// \sa FromEncrypted()
		public void SetEncrypted(RawEncryptedVector3Int encrypted, int key)
		{
			inited = true;
			hiddenValue = encrypted;
			currentCryptoKey = key;

			if (Detectors.ObscuredCheatingDetector.ExistsAndIsRunning)
			{
				fakeValueActive = false;
				fakeValue = InternalDecrypt();
				fakeValueActive = true;
			}
			else
			{
				fakeValueActive = false;
			}
		}

		/// <summary>
		/// Alternative to the type cast, use if you wish to get decrypted value
		/// but can't or don't want to use cast to the regular type.
		/// </summary>
		/// <returns>Decrypted value.</returns>
		public Vector3Int GetDecrypted()
		{
			return InternalDecrypt();
		}

		public void RandomizeCryptoKey()
		{
			var decrypted = InternalDecrypt();
			currentCryptoKey = GenerateKey();
			hiddenValue = Encrypt(decrypted, currentCryptoKey);
		}

		private Vector3Int InternalDecrypt()
		{
			if (!inited)
			{
				Init();
				return Zero;
			}

			var decrypted = Decrypt(hiddenValue, currentCryptoKey);

			if (Detectors.ObscuredCheatingDetector.ExistsAndIsRunning && fakeValueActive && decrypted != fakeValue)
			{
				Detectors.ObscuredCheatingDetector.Instance.OnCheatingDetected();
			}

			return decrypted;
		}
		
		private void Init()
		{
			currentCryptoKey = GenerateKey();
			hiddenValue = Encrypt(Zero, currentCryptoKey);
			fakeValue = Zero;
			fakeValueActive = false;
			inited = true;
		}

		#region operators, overrides, interface implementations

		//! @cond
		public static implicit operator ObscuredVector3Int(Vector3Int value)
		{
			return new ObscuredVector3Int(value);
		}

		public static implicit operator Vector3Int(ObscuredVector3Int value)
		{
			return value.InternalDecrypt();
		}

		public static implicit operator Vector3(ObscuredVector3Int value)
		{
			return value.InternalDecrypt();
		}

		public static ObscuredVector3Int operator +(ObscuredVector3Int a, ObscuredVector3Int b)
		{
			return a.InternalDecrypt() + b.InternalDecrypt();
		}

		public static ObscuredVector3Int operator +(Vector3Int a, ObscuredVector3Int b)
		{
			return a + b.InternalDecrypt();
		}

		public static ObscuredVector3Int operator +(ObscuredVector3Int a, Vector3Int b)
		{
			return a.InternalDecrypt() + b;
		}

		public static ObscuredVector3Int operator -(ObscuredVector3Int a, ObscuredVector3Int b)
		{
			return a.InternalDecrypt() - b.InternalDecrypt();
		}

		public static ObscuredVector3Int operator -(Vector3Int a, ObscuredVector3Int b)
		{
			return a - b.InternalDecrypt();
		}

		public static ObscuredVector3Int operator -(ObscuredVector3Int a, Vector3Int b)
		{
			return a.InternalDecrypt() - b;
		}

		public static ObscuredVector3Int operator *(ObscuredVector3Int a, int d)
		{
			return a.InternalDecrypt() * d;
		}

		public static bool operator ==(ObscuredVector3Int lhs, ObscuredVector3Int rhs)
		{
			return lhs.InternalDecrypt() == rhs.InternalDecrypt();
		}

		public static bool operator ==(Vector3Int lhs, ObscuredVector3Int rhs)
		{
			return lhs == rhs.InternalDecrypt();
		}

		public static bool operator ==(ObscuredVector3Int lhs, Vector3Int rhs)
		{
			return lhs.InternalDecrypt() == rhs;
		}

		public static bool operator !=(ObscuredVector3Int lhs, ObscuredVector3Int rhs)
		{
			return lhs.InternalDecrypt() != rhs.InternalDecrypt();
		}

		public static bool operator !=(Vector3Int lhs, ObscuredVector3Int rhs)
		{
			return lhs != rhs.InternalDecrypt();
		}

		public static bool operator !=(ObscuredVector3Int lhs, Vector3Int rhs)
		{
			return lhs.InternalDecrypt() != rhs;
		}

		public override bool Equals(object other)
		{
			return InternalDecrypt().Equals(other);
		}

		public override int GetHashCode()
		{
			return InternalDecrypt().GetHashCode();
		}

		public override string ToString()
		{
			return InternalDecrypt().ToString();
		}

		public string ToString(string format)
		{
			return InternalDecrypt().ToString(format);
		}

		#endregion

		#region obsolete

		[Obsolete("This API is redundant and does not perform any actions. It will be removed in future updates.")]
		public static void SetNewCryptoKey(int newKey) {}

		[Obsolete("This API is redundant and does not perform any actions. It will be removed in future updates.")]
		public void ApplyNewCryptoKey() {}

		[Obsolete("Please use new Encrypt(value, key) API instead.", true)]
		public static RawEncryptedVector3Int Encrypt(Vector3Int value) { throw new Exception(); }

		[Obsolete("Please use new Decrypt(value, key) API instead.", true)]
		public static Vector3Int Decrypt(RawEncryptedVector3Int value) { throw new Exception(); }

		[Obsolete("Please use new GetEncrypted(out key) API instead.", true)]
		public RawEncryptedVector3Int GetEncrypted() { throw new Exception(); }

		[Obsolete("Please use new SetEncrypted(encrypted, key) API instead.", true)]
		public void SetEncrypted(RawEncryptedVector3Int encrypted) {}

		#endregion

		//! @endcond

		/// <summary>
		/// Used to store encrypted Vector3Int.
		/// </summary>
		[Serializable]
		public struct RawEncryptedVector3Int
		{
			/// <summary>
			/// Encrypted value
			/// </summary>
			public int x;

			/// <summary>
			/// Encrypted value
			/// </summary>
			public int y;

			/// <summary>
			/// Encrypted value
			/// </summary>
			public int z;
		}
	}
}