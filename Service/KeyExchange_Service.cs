using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace chat_app
{
    internal class KeyExchange_Service
    {
        public static  byte[]?  publicKey;
        public static byte[]? sharedSecret;
        byte[]? privateKeyBlob;
        ECDiffieHellmanCng? key;

        public byte[] generatekeyPublickey()
        {
            key = new ECDiffieHellmanCng();
            //KeyDerivationFunction property specifies how this raw secret is transformed into a usable key.
            key.KeyDerivationFunction = ECDiffieHellmanKeyDerivationFunction.Hash;  //shared secret will be hashed to produce the final key.
            key.HashAlgorithm = CngAlgorithm.Sha256; //defining the hash func to be sha256
            publicKey = key.PublicKey.ToByteArray();
            privateKeyBlob = key.Key.Export(CngKeyBlobFormat.EccPrivateBlob); // Export private key
            return publicKey;
        }

        public byte[] generateSharedSecret(byte[] receivedPublicKey)
        {
            byte[] publicKeyBytes = Convert.FromBase64String(Encoding.ASCII.GetString(receivedPublicKey));
            ECDiffieHellmanCng _key = new ECDiffieHellmanCng(CngKey.Import(privateKeyBlob, CngKeyBlobFormat.EccPrivateBlob));
            CngKey recievedKey = CngKey.Import(publicKeyBytes, CngKeyBlobFormat.EccPublicBlob);
            byte[] sharedSecret = _key.DeriveKeyMaterial(recievedKey);
            return sharedSecret;
        }

    }
}
