using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DegerlendirmeSoruları
{
    class Program
    {
        static void Main(string[] args)
        {
            int musteriNumarasi = 15000000;

            CalistirmaMotoru.KomutCalistir("MuhasebeModulu", "MaasYatir", new object[] { musteriNumarasi });
            CalistirmaMotoru.KomutCalistir("MuhasebeModulu", "YillikUcretTahsilEt", new object[] { musteriNumarasi });
            CalistirmaMotoru.KomutCalistir("MuhasebeModulu", "OtomatikOdemeleriGerceklestir", new object[] { musteriNumarasi });
            CalistirmaMotoru.BekleyenIslemleriGerceklestir();

        }
    }
    public class CalistirmaMotoru
    {
        public static object[] KomutCalistir(string modulSinifAdi, string methodAdi, object[] inputs)
        {
            bool islemBekleyebilirMi = VeritabaniIslemleri.IslemBekleyebilirMi(methodAdi);
            if (islemBekleyebilirMi == true)
            {
                VeritabaniIslemleri.BekleyenIslemlerListesineEkle(modulSinifAdi, methodAdi, inputs);
            }
            else
            {
                AnindaCalistir(modulSinifAdi, methodAdi, inputs);
            }
            return null;
        }

        public static void BekleyenIslemleriGerceklestir()
        {
            List<BekleyenIslem> bekleyenIslemler = VeritabaniIslemleri.BekleyenIslemleriGetir();
            foreach (var islem in bekleyenIslemler)
            {
                AnindaCalistir(islem.ModulSinifAdi, islem.MethodAdi, islem.Inputs);
            }

        }

        public static void AnindaCalistir(string modulSinifAdi, string methodAdi, object[] inputs)
        {

            Type t = Type.GetType("DegerlendirmeSoruları." + modulSinifAdi);
            ConstructorInfo constructor = t.GetConstructor(Type.EmptyTypes);
            object classObject = constructor.Invoke(new object[] { });
            MethodInfo theMethod = t.GetMethod(methodAdi, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            theMethod.Invoke(classObject, inputs);
        }
    }

    class MuhasebeModulu
    {
        private void MaasYatir(int musteriNumarasi)
        {
            // gerekli işlemler gerçekleştirilir.
            Console.WriteLine(string.Format("{0} numaralı müşterinin maaşı yatırıldı.", musteriNumarasi));
        }

        private void YillikUcretTahsilEt(int musteriNumarasi)
        {
            // gerekli işlemler gerçekleştirilir.
            Console.WriteLine("{0} numaralı müşteriden yıllık kart ücreti tahsil edildi.", musteriNumarasi);
        }

        private void OtomatikOdemeleriGerceklestir(int musteriNumarasi)
        {
            // gerekli işlemler gerçekleştirilir.
            Console.WriteLine("{0} numaralı müşterinin otomatik ödemeleri gerçekleştirildi.", musteriNumarasi);
        }
    }

    public class VeritabaniIslemleri
    {
        static List<BekleyenIslem> bekleyenIslemlerListesi = new List<BekleyenIslem>();

        public static void BekleyenIslemlerListesineEkle(string modulSinifAdi, string methodAdi, object[] inputs)
        {
            bekleyenIslemlerListesi.Add(new BekleyenIslem
            {
                ModulSinifAdi = modulSinifAdi,
                MethodAdi = methodAdi,
                Inputs = inputs
            });
        }

        //
        public static bool IslemBekleyebilirMi(string methodAdı)
        {

            if (methodAdı == "YillikUcretTahsilEt")
            {
                return true;
            }
            if (methodAdı == "OtomatikOdemeleriGerceklestir")
            {
                return true;
            }
            return false;
        }

        public static List<BekleyenIslem> BekleyenIslemleriGetir()
        {
            return bekleyenIslemlerListesi;
        }

    }


    public class BekleyenIslem
    {
        public string ModulSinifAdi { get; set; }
        public string MethodAdi { get; set; }
        public object[] Inputs { get; set; }
    }

}

