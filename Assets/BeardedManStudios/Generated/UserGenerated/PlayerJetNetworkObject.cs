using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0.15,0.15,0,0,0]")]
	public partial class PlayerJetNetworkObject : NetworkObject
	{
		public const int IDENTITY = 8;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		[ForgeGeneratedField]
		private Vector3 _position;
		public event FieldEvent<Vector3> positionChanged;
		public InterpolateVector3 positionInterpolation = new InterpolateVector3() { LerpT = 0.15f, Enabled = true };
		public Vector3 position
		{
			get { return _position; }
			set
			{
				// Don't do anything if the value is the same
				if (_position == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_position = value;
				hasDirtyFields = true;
			}
		}

		public void SetpositionDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_position(ulong timestep)
		{
			if (positionChanged != null) positionChanged(_position, timestep);
			if (fieldAltered != null) fieldAltered("position", _position, timestep);
		}
		[ForgeGeneratedField]
		private Quaternion _rotation;
		public event FieldEvent<Quaternion> rotationChanged;
		public InterpolateQuaternion rotationInterpolation = new InterpolateQuaternion() { LerpT = 0.15f, Enabled = true };
		public Quaternion rotation
		{
			get { return _rotation; }
			set
			{
				// Don't do anything if the value is the same
				if (_rotation == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_rotation = value;
				hasDirtyFields = true;
			}
		}

		public void SetrotationDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_rotation(ulong timestep)
		{
			if (rotationChanged != null) rotationChanged(_rotation, timestep);
			if (fieldAltered != null) fieldAltered("rotation", _rotation, timestep);
		}
		[ForgeGeneratedField]
		private int _currentHealth;
		public event FieldEvent<int> currentHealthChanged;
		public Interpolated<int> currentHealthInterpolation = new Interpolated<int>() { LerpT = 0f, Enabled = false };
		public int currentHealth
		{
			get { return _currentHealth; }
			set
			{
				// Don't do anything if the value is the same
				if (_currentHealth == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x4;
				_currentHealth = value;
				hasDirtyFields = true;
			}
		}

		public void SetcurrentHealthDirty()
		{
			_dirtyFields[0] |= 0x4;
			hasDirtyFields = true;
		}

		private void RunChange_currentHealth(ulong timestep)
		{
			if (currentHealthChanged != null) currentHealthChanged(_currentHealth, timestep);
			if (fieldAltered != null) fieldAltered("currentHealth", _currentHealth, timestep);
		}
		[ForgeGeneratedField]
		private int _currentAmmo;
		public event FieldEvent<int> currentAmmoChanged;
		public Interpolated<int> currentAmmoInterpolation = new Interpolated<int>() { LerpT = 0f, Enabled = false };
		public int currentAmmo
		{
			get { return _currentAmmo; }
			set
			{
				// Don't do anything if the value is the same
				if (_currentAmmo == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x8;
				_currentAmmo = value;
				hasDirtyFields = true;
			}
		}

		public void SetcurrentAmmoDirty()
		{
			_dirtyFields[0] |= 0x8;
			hasDirtyFields = true;
		}

		private void RunChange_currentAmmo(ulong timestep)
		{
			if (currentAmmoChanged != null) currentAmmoChanged(_currentAmmo, timestep);
			if (fieldAltered != null) fieldAltered("currentAmmo", _currentAmmo, timestep);
		}
		[ForgeGeneratedField]
		private bool _reloading;
		public event FieldEvent<bool> reloadingChanged;
		public Interpolated<bool> reloadingInterpolation = new Interpolated<bool>() { LerpT = 0f, Enabled = false };
		public bool reloading
		{
			get { return _reloading; }
			set
			{
				// Don't do anything if the value is the same
				if (_reloading == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x10;
				_reloading = value;
				hasDirtyFields = true;
			}
		}

		public void SetreloadingDirty()
		{
			_dirtyFields[0] |= 0x10;
			hasDirtyFields = true;
		}

		private void RunChange_reloading(ulong timestep)
		{
			if (reloadingChanged != null) reloadingChanged(_reloading, timestep);
			if (fieldAltered != null) fieldAltered("reloading", _reloading, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			positionInterpolation.current = positionInterpolation.target;
			rotationInterpolation.current = rotationInterpolation.target;
			currentHealthInterpolation.current = currentHealthInterpolation.target;
			currentAmmoInterpolation.current = currentAmmoInterpolation.target;
			reloadingInterpolation.current = reloadingInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _position);
			UnityObjectMapper.Instance.MapBytes(data, _rotation);
			UnityObjectMapper.Instance.MapBytes(data, _currentHealth);
			UnityObjectMapper.Instance.MapBytes(data, _currentAmmo);
			UnityObjectMapper.Instance.MapBytes(data, _reloading);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_position = UnityObjectMapper.Instance.Map<Vector3>(payload);
			positionInterpolation.current = _position;
			positionInterpolation.target = _position;
			RunChange_position(timestep);
			_rotation = UnityObjectMapper.Instance.Map<Quaternion>(payload);
			rotationInterpolation.current = _rotation;
			rotationInterpolation.target = _rotation;
			RunChange_rotation(timestep);
			_currentHealth = UnityObjectMapper.Instance.Map<int>(payload);
			currentHealthInterpolation.current = _currentHealth;
			currentHealthInterpolation.target = _currentHealth;
			RunChange_currentHealth(timestep);
			_currentAmmo = UnityObjectMapper.Instance.Map<int>(payload);
			currentAmmoInterpolation.current = _currentAmmo;
			currentAmmoInterpolation.target = _currentAmmo;
			RunChange_currentAmmo(timestep);
			_reloading = UnityObjectMapper.Instance.Map<bool>(payload);
			reloadingInterpolation.current = _reloading;
			reloadingInterpolation.target = _reloading;
			RunChange_reloading(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _position);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _rotation);
			if ((0x4 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _currentHealth);
			if ((0x8 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _currentAmmo);
			if ((0x10 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _reloading);

			// Reset all the dirty fields
			for (int i = 0; i < _dirtyFields.Length; i++)
				_dirtyFields[i] = 0;

			return dirtyFieldsData;
		}

		protected override void ReadDirtyFields(BMSByte data, ulong timestep)
		{
			if (readDirtyFlags == null)
				Initialize();

			Buffer.BlockCopy(data.byteArr, data.StartIndex(), readDirtyFlags, 0, readDirtyFlags.Length);
			data.MoveStartIndex(readDirtyFlags.Length);

			if ((0x1 & readDirtyFlags[0]) != 0)
			{
				if (positionInterpolation.Enabled)
				{
					positionInterpolation.target = UnityObjectMapper.Instance.Map<Vector3>(data);
					positionInterpolation.Timestep = timestep;
				}
				else
				{
					_position = UnityObjectMapper.Instance.Map<Vector3>(data);
					RunChange_position(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (rotationInterpolation.Enabled)
				{
					rotationInterpolation.target = UnityObjectMapper.Instance.Map<Quaternion>(data);
					rotationInterpolation.Timestep = timestep;
				}
				else
				{
					_rotation = UnityObjectMapper.Instance.Map<Quaternion>(data);
					RunChange_rotation(timestep);
				}
			}
			if ((0x4 & readDirtyFlags[0]) != 0)
			{
				if (currentHealthInterpolation.Enabled)
				{
					currentHealthInterpolation.target = UnityObjectMapper.Instance.Map<int>(data);
					currentHealthInterpolation.Timestep = timestep;
				}
				else
				{
					_currentHealth = UnityObjectMapper.Instance.Map<int>(data);
					RunChange_currentHealth(timestep);
				}
			}
			if ((0x8 & readDirtyFlags[0]) != 0)
			{
				if (currentAmmoInterpolation.Enabled)
				{
					currentAmmoInterpolation.target = UnityObjectMapper.Instance.Map<int>(data);
					currentAmmoInterpolation.Timestep = timestep;
				}
				else
				{
					_currentAmmo = UnityObjectMapper.Instance.Map<int>(data);
					RunChange_currentAmmo(timestep);
				}
			}
			if ((0x10 & readDirtyFlags[0]) != 0)
			{
				if (reloadingInterpolation.Enabled)
				{
					reloadingInterpolation.target = UnityObjectMapper.Instance.Map<bool>(data);
					reloadingInterpolation.Timestep = timestep;
				}
				else
				{
					_reloading = UnityObjectMapper.Instance.Map<bool>(data);
					RunChange_reloading(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (positionInterpolation.Enabled && !positionInterpolation.current.UnityNear(positionInterpolation.target, 0.0015f))
			{
				_position = (Vector3)positionInterpolation.Interpolate();
				//RunChange_position(positionInterpolation.Timestep);
			}
			if (rotationInterpolation.Enabled && !rotationInterpolation.current.UnityNear(rotationInterpolation.target, 0.0015f))
			{
				_rotation = (Quaternion)rotationInterpolation.Interpolate();
				//RunChange_rotation(rotationInterpolation.Timestep);
			}
			if (currentHealthInterpolation.Enabled && !currentHealthInterpolation.current.UnityNear(currentHealthInterpolation.target, 0.0015f))
			{
				_currentHealth = (int)currentHealthInterpolation.Interpolate();
				//RunChange_currentHealth(currentHealthInterpolation.Timestep);
			}
			if (currentAmmoInterpolation.Enabled && !currentAmmoInterpolation.current.UnityNear(currentAmmoInterpolation.target, 0.0015f))
			{
				_currentAmmo = (int)currentAmmoInterpolation.Interpolate();
				//RunChange_currentAmmo(currentAmmoInterpolation.Timestep);
			}
			if (reloadingInterpolation.Enabled && !reloadingInterpolation.current.UnityNear(reloadingInterpolation.target, 0.0015f))
			{
				_reloading = (bool)reloadingInterpolation.Interpolate();
				//RunChange_reloading(reloadingInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public PlayerJetNetworkObject() : base() { Initialize(); }
		public PlayerJetNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public PlayerJetNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
