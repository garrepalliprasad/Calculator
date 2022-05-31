using Microsoft.Research.SEAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Calculator.Common.Utilities
{
    public  class Utilities
    {
        private readonly Evaluator evaluator;
        private readonly Encryptor encryptor;
        private readonly Decryptor decryptor;
        private readonly SEALContext context;
        public Evaluator Evaluator 
        {
            get
            {
                return evaluator;
            } 
        }
        public Encryptor Encryptor
        {
            get
            {
                return encryptor;
            }
        }
        public Decryptor Decryptor 
        {
            get
            {
                return decryptor;
            }
        }
        public SEALContext SEALContext
        {
            get
            {
                return context;
            }
        }
        public Utilities()
        {
            EncryptionParameters parms = new EncryptionParameters(SchemeType.BFV);
            ulong polyModulusDegree = 4096;
            parms.PolyModulusDegree = polyModulusDegree;
            parms.CoeffModulus = CoeffModulus.BFVDefault(polyModulusDegree);
            parms.PlainModulus = new Modulus(1024);
            context = new SEALContext(parms);
            KeyGenerator keygen = new KeyGenerator(context);
            SecretKey secretKey = keygen.SecretKey;
            keygen.CreatePublicKey(out PublicKey publicKey);
            encryptor = new Encryptor(context, publicKey);
            evaluator = new Evaluator(context);
            decryptor = new Decryptor(context, secretKey);
        }
        public  string ULongToString(ulong value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            var r = BitConverter.ToString(bytes).Replace("-", "");
            return r;
        }
        public  string CiphertextToBase64String(Ciphertext ciphertext)
        {
            using (var ms = new MemoryStream())
            {
                ciphertext.Save(ms);
                return Convert.ToBase64String(ms.ToArray());
            }
        }
        public  Ciphertext BuildCiphertextFromBase64String(string base64, SEALContext context)
        {
            var payload = Convert.FromBase64String(base64);

            using (var ms = new MemoryStream(payload))
            {
                var ciphertext = new Ciphertext();
                ciphertext.Load(context, ms);

                return ciphertext;
            }
        }
    }
}
