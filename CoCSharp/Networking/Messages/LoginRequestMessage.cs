﻿namespace CoCSharp.Networking.Messages
{
    /// <summary>
    /// Message that is sent by the client to the server to request
    /// for a login.
    /// </summary>
    public class LoginRequestMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginRequestMessage"/> class.
        /// </summary>
        public LoginRequestMessage()
        {
            // Space
        }

        /// <summary>
        /// User ID needed to login in a specific account.
        /// </summary>
        public long UserID;
        /// <summary>
        /// User token needed to login in a specific account.
        /// </summary>
        public string UserToken;
        /// <summary>
        /// Client major version. If the client version is incorrect
        /// the server will reply with a LoginFailedMessage.
        /// </summary>
        public int ClientMajorVersion;
        /// <summary>
        /// Client content version. If the client version is incorrect
        /// the server will reply with a LoginFailedMessage.
        /// </summary>
        public int ClientContentVersion;
        /// <summary>
        /// Client minor version. If the client version is incorrect
        /// the server will reply with a LoginFailedMessage.
        /// </summary>
        public int ClientMinorVersion;
        /// <summary>
        /// Hash of 'fingerprint.json'. If the fingerprint hash is incorrect
        /// the server will reply with a LoginFailedMessage.
        /// </summary>
        public string FingerprintHash;

        /// <summary>
        /// Unknown string 1.
        /// </summary>
        public string Unknown1;

        /// <summary>
        /// Open UDID.
        /// </summary>
        public string OpenUDID;
        /// <summary>
        /// MAC address of the device. Can be set to null.
        /// </summary>
        public string MacAddress;
        /// <summary>
        /// Model of the device.
        /// </summary>
        public string DeviceModel;
        /// <summary>
        /// Locale key.
        /// </summary>
        public int LocaleKey;
        /// <summary>
        /// Language of the device.
        /// </summary>
        public string Language;
        /// <summary>
        /// Advertising GUID.
        /// </summary>
        public string AdvertisingGUID;
        /// <summary>
        /// Operating system version.
        /// </summary>
        public string OSVersion;

        /// <summary>
        /// Unknown byte 2.
        /// </summary>
        public byte Unknown2;
        /// <summary>
        /// Unknown string 3.
        /// </summary>
        public string Unknown3;

        /// <summary>
        /// Is advertising tracking enabled.
        /// </summary>
        public bool IsAdvertisingTrackingEnabled;
        /// <summary>
        /// Android device ID.
        /// </summary>
        public string AndroidDeviceID;
        /// <summary>
        /// Facebook distribution ID.
        /// </summary>
        public string FacebookDistributionID;
        /// <summary>
        /// Vendor GUID.
        /// </summary>
        public string VendorGUID;
        /// <summary>
        /// Seed that is needed for encryption.
        /// </summary>
        public int Seed;


        /// <summary>
        ///  Gets the ID of the <see cref="LoginRequestMessage"/>.
        /// </summary>
        public override ushort ID { get { return 10101; } }

        /// <summary>
        /// Reads the <see cref="LoginRequestMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="LoginRequestMessage"/>.
        /// </param>
        public override void ReadMessage(MessageReader reader)
        {
            UserID = reader.ReadInt64();
            UserToken = reader.ReadString();
            ClientMajorVersion = reader.ReadInt32();
            ClientContentVersion = reader.ReadInt32();
            ClientMinorVersion = reader.ReadInt32();
            FingerprintHash = reader.ReadString();

            Unknown1 = reader.ReadString();

            OpenUDID = reader.ReadString();
            MacAddress = reader.ReadString();
            DeviceModel = reader.ReadString();
            LocaleKey = reader.ReadInt32();
            Language = reader.ReadString();
            AdvertisingGUID = reader.ReadString();
            OSVersion = reader.ReadString();

            Unknown2 = reader.ReadByte();
            Unknown3 = reader.ReadString();

            AndroidDeviceID = reader.ReadString();
            FacebookDistributionID = reader.ReadString();
            IsAdvertisingTrackingEnabled = reader.ReadBoolean();
            VendorGUID = reader.ReadString();
            Seed = reader.ReadInt32();
        }

        /// <summary>
        /// Writes the <see cref="LoginRequestMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="LoginRequestMessage"/>.
        /// </param>
        public override void WriteMessage(MessageWriter writer)
        {
            writer.Write(UserID);
            writer.Write(UserToken);
            writer.Write(ClientMajorVersion);
            writer.Write(ClientContentVersion);
            writer.Write(ClientMinorVersion);
            writer.Write(FingerprintHash);

            writer.Write(Unknown1);

            writer.Write(OpenUDID);
            writer.Write(MacAddress);
            writer.Write(DeviceModel);
            writer.Write(LocaleKey);
            writer.Write(AdvertisingGUID);
            writer.Write(OSVersion);

            writer.Write(Unknown2);
            writer.Write(Unknown3);

            writer.Write(AndroidDeviceID);
            writer.Write(FacebookDistributionID);
            writer.Write(IsAdvertisingTrackingEnabled);
            writer.Write(VendorGUID);
            writer.Write(Seed);
        }
    }
}